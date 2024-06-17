using EEMC.Models;
using Ionic.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace EEMC.Services
{
    public class ValidateResponse
    {
        public bool IsValid { get; set; }
        /// <summary>
        /// Текст ошибки валидации
        /// </summary>
        public string ValidErrorText { get; set; }
    }

    public static class TestService
    {
        private static int ConvertPointsForQuestion(int thisCountForQuestion, int totalCountForQuestions)
        {
            double weight = (double)thisCountForQuestion / totalCountForQuestions;

            if (weight <= 0.25)
                return 1;

            if (weight <= 0.5)
                return 2;

            if (weight <= 0.75)
                return 3;

            return 4;
        }

        private static Random rng = new Random();

        private static IList<T> Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

        public static Test TestFromTotalTest(TotalTestItem[] items, string originFileName)
        {
            var test = new Test
            {
                Questions = new(),
                TestName = "Итоговый тест"
            };

            //Считываем тесты с тем
            var themes = Theme.ReadAllThemes();

            var groupedTests = themes
                .Where(x => x.Files != null)
                .Select(
                    x => new ThemeToTests
                    {
                        Theme = x,
                        Tests = x.Files
                            .Where(y => y.IsTest())
                            .Select(y => TestService.Load(Environment.CurrentDirectory + y.NameWithPath))
                            .ToArray()
                    }
                )
                .Where(x => x.Tests.Any())
                .ToArray();

            //Возможно в items QuestionsCount не соотносится с актуальным максимальным количеством вопросов по теме
            //Надо их переписать, если это так
            bool isNeedOverrite = false;

            foreach (var item in items)
            {
                var groupedTest = groupedTests.First(x => x.Theme.ThemeName == item.ThemeName);

                if (item.QuestionsCount > groupedTest.QuestionsCount)
                {
                    item.QuestionsCount = groupedTest.QuestionsCount;

                    isNeedOverrite = true;
                }
            }

            if (isNeedOverrite)
            {
                //Перезаписываем исходный файл
                string json = JsonConvert.SerializeObject(items);

                File.WriteAllText(originFileName, json);
            }

            //Формируем тест
            var countMap = items.ToDictionary(x => x.ThemeName, x => x.QuestionsCount);

            int questionNumber = 1;

            foreach (var groupedTest in groupedTests)
            {
                if (!countMap.ContainsKey(groupedTest.Theme.ThemeName))
                    continue;

                var thisCount = countMap[groupedTest.Theme.ThemeName];
                var flatList = groupedTest.Tests.SelectMany(x => x.Questions).ToList();

                var shuffledQuestions = Shuffle(flatList);

                var slice = shuffledQuestions.Take(thisCount);

                foreach (var item in slice)
                {
                    item.QuestionWeight = ConvertPointsForQuestion(item.QuestionWeight.Value, groupedTest.QuestionsCount);
                    item.QuestionNumber = questionNumber;

                    questionNumber++;

                    test.Questions.Add(item);
                }
            }

            return test;
        }

        public static Test Load(string pathTest)
        {
            string savePath = Path.Combine(Environment.CurrentDirectory, "tmp_test");

            //Пересоздаём временную папку
            if (Directory.Exists(savePath))
                Directory.Delete(savePath, true);

            Directory.CreateDirectory(savePath);

            //Временно разархивируем архив
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (ZipFile zip = ZipFile.Read(pathTest, options: new ReadOptions() { Encoding = Encoding.UTF8 }))
            {
                zip.ExtractAll(savePath, ExtractExistingFileAction.OverwriteSilently);
            }

            //Заполняем объект Test
            string json = File.ReadAllText(Path.Combine(savePath, "test.json"));

            Test test = JsonConvert.DeserializeObject<Test>(json);

            foreach (var question in test.Questions)
            {
                //question.QuestionText;
                using (FileStream fileStream = new FileStream(Path.Combine(savePath, question.TextFileName), FileMode.Open))
                {
                    question.QuestionText = new FlowDocument();

                    var content = new TextRange(question.QuestionText.ContentStart, question.QuestionText.ContentEnd);

                    if (content.CanLoad(DataFormats.Rtf))
                    {
                        content.Load(fileStream, DataFormats.Rtf);
                    }
                    else
                    {
                        throw new Exception($"Не удаётся загрузить содержимое теста");
                    }
                }
            }

            //Удаляем временную папку
            Directory.Delete(savePath, true);

            return test;
        }
    }
}
