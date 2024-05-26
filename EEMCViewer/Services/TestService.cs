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
