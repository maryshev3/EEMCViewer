using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EEMC.Models
{
    public class Test
    {
        [JsonIgnore]
        public int SelectedCount { get; set; }
        public string TestName { get; set; }
        public ObservableCollection<Question> Questions { get; set; }
    }
}
