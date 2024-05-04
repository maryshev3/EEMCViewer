using DevExpress.Mvvm;
using EEMC.Messages;
using EEMC.Models;
using EEMC.Services;
using EEMC.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace EEMC.ViewModels
{
    public class ThemesWindowVM : ViewModelBase
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

        public ThemesWindowVM(MessageBus messageBus)
        {
            _messageBus = messageBus;

            _messageBus.Receive<CourseMessage>(this, async (message) =>
            {
                CurrentCourse = message.Course;
            });
        }

        public ICommand ShowFile_Click
        {
            get => new Commands.DelegateCommand(async (currentFile) =>
            {
                ThemeFile currentFileConverted = currentFile as ThemeFile;

                Window window = default;

                if (currentFileConverted.IsVideoOrAudio())
                {
                    window = new VideoView();

                    await _messageBus.SendTo<VideoViewVM>(new ThemeFileMessage(currentFileConverted));
                }
                else
                {
                    if (currentFileConverted.IsImage())
                    {
                        window = new ImageView();

                        await _messageBus.SendTo<ImageViewVM>(new ThemeFileMessage(currentFileConverted));
                    }
                    else
                    {
                        window = new DocumentView();

                        await _messageBus.SendTo<DocumentViewVM>(new ThemeFileMessage(currentFileConverted));
                    }
                }

                window?.ShowDialog();
            }
            );
        }

        public ICommand DownloadFile_Click
        {
            get => new Commands.DelegateCommand(async (currentFile) =>
            {
                ThemeFile currentFileConverted = currentFile as ThemeFile;

                var fileDialog = new SaveFileDialog();
                fileDialog.Filter = currentFileConverted.GetSaveFilter();
                
                if (fileDialog.ShowDialog() == true)
                {
                    var filePath = fileDialog.FileName;

                    currentFileConverted.SaveFile(filePath);
                }
            }
            );
        }
    }
}
