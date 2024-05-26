using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace EEMC.Models
{
    public enum ContentType
    {
        File,
        Folder
    }

    public class Explorer
    {
        private string _name;
        public string Name 
        { 
            get => _name;
            set => _name = value;
        }

        public string NameWithoutExtension
        {
            get => Path.GetFileNameWithoutExtension(Name);
        }

        /// <summary>
        /// Заполнено для курсов. Является ссылкой на изображение в CoursesList
        /// </summary>
        public string? ImagePath { get; set; }

        public string NameWithPath;

        public ContentType Type;

        public ObservableCollection<Explorer>? Content { get; set; }

        public ObservableCollection<Theme>? Themes { get; set; }

        public Explorer(string name, string nameWithPath, ContentType type, ObservableCollection<Explorer>? content) 
        {
            this._name = name;
            this.NameWithPath = nameWithPath;
            this.Type = type;
            this.Content = content;
        }

        public bool IsText()
        {
            string extension = Path.GetExtension(Name).ToLower();

            return extension is ".docx" or ".doc" or ".txt" or ".cpp" or ".h" or ".py"
                or ".cs" or ".json" or ".xml" or ".html" or ".css" or ".ppt" or ".pptx";
        }

        public bool IsPdf()
        {
            string extension = Path.GetExtension(Name).ToLower();

            return extension == ".pdf";
        }

        public void Remove()
        {
            string courseDirectory = Environment.CurrentDirectory + NameWithPath;

            if (Type == ContentType.File)
            {
                if (!File.Exists(courseDirectory))
                    throw new Exception("Файл не существует");

                File.Delete(courseDirectory);
            }
            else
            {
                if (!Directory.Exists(courseDirectory))
                    throw new Exception("Раздел не существует");

                //Переформировываем список тем (удаляем все темы, связанные с курсом) (если список тем для данного курса пуст - то нет смысла переформировывать json тем)
                if (Themes != default && Themes.Any())
                {
                    var allThemesWithoutThis = Theme.ReadAllThemes().Where(x => x.CourseName != Name).ToArray();

                    Theme.RewriteAllThemes(allThemesWithoutThis);
                }

                Directory.Delete(courseDirectory, true);
            }

            //Будет автоматически вызван пересбор класса Course
        }
    }
}
