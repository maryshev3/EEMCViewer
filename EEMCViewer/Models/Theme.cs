using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public class Theme
    {
        public string ThemeName { get; set; }
        [JsonIgnore]
        public string NameWithNumber { get => $"{ThemeNumber}. {ThemeName}"; }
        public int ThemeNumber { get; set; }
        public string ThemeDescription { get; set; }
        public ObservableCollection<ThemeFile>? Files { get; set; }
        public Boolean IsHiden { get; set; }
        //Воспринимать как внешний ключ к Course
        public string CourseName { get; set; }

        public static Theme[] ReadAllThemes()
        {
            string json = File.ReadAllText("./themes.json");

            var themes = JsonConvert.DeserializeObject<Theme[]>(json);

            return themes;
        }

        public static void RewriteAllThemes(Theme[] themes)
        {
            string json = JsonConvert.SerializeObject(themes.OrderBy(x => x.ThemeNumber));

            File.WriteAllText("./themes.json", json);

            //Иммитируем пересбор курсов
            string tmpDir = Path.Combine(Environment.CurrentDirectory, "Курсы", "tmpf");

            if (!Directory.Exists(tmpDir))
            {
                Directory.CreateDirectory(tmpDir);
                Directory.Delete(tmpDir);
            }
            else
            {
                Directory.Delete(tmpDir, true);
            }
        }
    }
}
