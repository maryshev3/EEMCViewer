using EEMC.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EEMC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void InitCatalogesFromCe()
        {
            string[] files = Directory.GetFiles(Environment.CurrentDirectory);

            string ceFile = files.FirstOrDefault(x => Path.GetExtension(x) == ".ce");

            if (ceFile == default)
                return;

            ImportExportService.Import(ceFile);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            InitCatalogesFromCe();

            ViewModelLocator.Init();

            base.OnStartup(e);
        }
    }
}
