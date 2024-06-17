using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomInstaller
{
    [RunInstaller(true)]
    public class MyInstaller : System.Configuration.Install.Installer
    {
        public override void Install(IDictionary stateSaver)
        {
            //CustomInstaller.InstallState

            base.Install(stateSaver);

            string currentPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string programPath = Path.GetFullPath(Path.Combine(currentPath, @"..\..\"));

            string statePath = Path.Combine(programPath, "for_install", "CustomInstaller.InstallState");

            File.WriteAllText("C:\\statepath.txt", Path.Combine(programPath, "for_install", "CustomInstaller.InstallState"));

            var bytes = File.ReadAllBytes(statePath);

            string rollbackPath = Path.Combine(programPath, "for_rollback", "CustomRollback.InstallState");
            string commitPath = Path.Combine(programPath, "for_commit", "CustomCommit.InstallState");

            File.WriteAllBytes(rollbackPath, bytes);
            File.WriteAllBytes(commitPath, bytes);
        }
    }
}
