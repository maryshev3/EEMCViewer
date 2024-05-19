using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using EEMC.Views;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Packaging;

namespace EEMC.ViewModels
{
    public class CourseWindowVM : ViewerBase
    {
        private Explorer? _currentCourse;

        public Explorer? CurrentCourse
        {
            get => _currentCourse;
            set
            {
                _currentCourse = value;
                RaisePropertyChanged(() => CurrentCourse);
            }
        }

        private readonly MessageBus _messageBus;

        public CourseWindowVM(
            MessageBus messageBus
        )
        {
            _messageBus = messageBus;

            _messageBus.Receive<CourseMessage>(this, async (message) =>
                {
                    CurrentCourse = message.Course;
                });
        }

        private static readonly string _defaultUriString = "about:blank";
        private Uri _pdfPath = new Uri(_defaultUriString);
        public Uri PdfPath
        {
            get => _pdfPath;
            set
            {
                _pdfPath = value;
                RaisePropertyChanged(() => PdfPath);
                RaisePropertyChanged(() => PdfViewVisibility);
                RaisePropertyChanged(() => XpsViewVisibility);
            }
        }

        public Visibility PdfViewVisibility
        {
            get => _pdfPath.OriginalString != _defaultUriString ? Visibility.Visible : Visibility.Collapsed;
        }

        public Visibility XpsViewVisibility
        {
            get => _pdfPath.OriginalString == _defaultUriString ? Visibility.Visible : Visibility.Collapsed;
        }

        public ICommand ShowFile_Click
        {
            get => new Commands.DelegateCommand(async (chosenFile) =>
            {
                Explorer chosenFileConverted = chosenFile as Explorer;
                if (chosenFileConverted.IsText())
                {
                    PdfPath = new Uri(_defaultUriString);
                    await this.ShowDocument.ExecuteAsync(chosenFile);
                }
                if (chosenFileConverted.IsPdf())
                {
                    PdfPath = new Uri(Environment.CurrentDirectory + chosenFileConverted.NameWithPath);
                }
            }
            );
        }
    }
}
