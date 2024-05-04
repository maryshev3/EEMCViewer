using EEMC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps.Packaging;
using Ionic.Zip;

namespace EEMC.Services
{
    public static class ImportExportService
    {
        private static void RemoveOldFiles()
        {
            if (Directory.Exists("./Файлы тем конвертированные"))
                Directory.Delete("./Файлы тем конвертированные", true);

            if (Directory.Exists("./Курсы конвертированные"))
                Directory.Delete("./Курсы конвертированные", true);

            if (Directory.Exists("./Курсы"))
                Directory.Delete("./Курсы", true);

            if (Directory.Exists("./Файлы тем"))
                Directory.Delete("./Файлы тем", true);
        }

        public static void Import(string cePath)
        {
            if (!File.Exists(cePath))
                throw new Exception("Не удаётся найти переданный файл с курсами");

            if (Path.GetExtension(cePath) != ".ce")
                throw new Exception("Переданный файл имеет неверный формат");

            RemoveOldFiles();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (ZipFile zip = ZipFile.Read(cePath, options: new ReadOptions() { Encoding = Encoding.UTF8 }))
            {
                zip.ExtractAll(Environment.CurrentDirectory, ExtractExistingFileAction.OverwriteSilently);
            }
        }
    }
}
