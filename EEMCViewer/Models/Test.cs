using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public class Test
    {
        public string TestName { get; set; }
        public ObservableCollection<Question> Questions { get; set; }
    }
}
