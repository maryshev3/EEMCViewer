using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRollback
{
    [RunInstaller(false)]
    public class MyRollback : System.Configuration.Install.Installer
    {
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }
    }
}
