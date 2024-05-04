using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using EEMC.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EEMC.ViewModels
{
    public abstract class CoursesListVMBase : ViewModelBase
    {
        protected Course _courses;
        public abstract Course Courses { get; set; }

        protected MessageBus _messageBus;
        protected Explorer? _chosenCourse = null;

        protected abstract void OnDirectoryChanged(object sender, FileSystemEventArgs e);

        public abstract ICommand OpenCourse_Click { get; }
    }
}
