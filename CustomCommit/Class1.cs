using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommit
{
    [RunInstaller(true)]
    public class MyCommit : System.Configuration.Install.Installer
    {
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);

            StringBuilder sb = new StringBuilder();

            if (Context.Parameters.Count > 0)
            {
                foreach (string myString in Context.Parameters.Keys)
                {
                    sb.AppendLine($"{myString} -> {Context.Parameters[myString]}");
                }
            }

            foreach (var s in savedState)
            {
                
            }

            File.WriteAllText("C:\\ttt.txt", sb.ToString());
        }
    }
}
