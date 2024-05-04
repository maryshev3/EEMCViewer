using EEMC.ViewModels;
using EEMC.Models;
using Microsoft.Extensions.DependencyInjection;
using EEMC.Services;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using System;
using Newtonsoft.Json;
using System.Windows.Documents;
using System.Collections.Generic;
using System.IO;

namespace EEMC
{
    public class ViewModelLocator
    {
        private static ServiceProvider _provider;

        public MainWindowVM MainWindowVM => _provider.GetRequiredService<MainWindowVM>();
        public CourseWindowVM CourseWindowVM => _provider.GetRequiredService<CourseWindowVM>();
        public CoursesListVM CoursesListVM => _provider.GetRequiredService<CoursesListVM>();
        public SwitcherCourseViewVM SwitcherCourseViewVM => _provider.GetRequiredService<SwitcherCourseViewVM>();
        public ThemesWindowVM ThemesWindowVM => _provider.GetRequiredService<ThemesWindowVM>();
        public DocumentViewVM DocumentViewVM => _provider.GetRequiredService<DocumentViewVM>();
        public VideoViewVM VideoViewVM => _provider.GetRequiredService<VideoViewVM>();
        public ImageViewVM ImageViewVM => _provider.GetRequiredService<ImageViewVM>();

        public static void AddVMs(ServiceCollection services)
        {
            services.AddTransient<CourseWindowVM>();
            services.AddSingleton<MainWindowVM>();
            services.AddTransient<CoursesListVM>();
            services.AddTransient<SwitcherCourseViewVM>();
            services.AddTransient<ThemesWindowVM>();
            services.AddTransient<DocumentViewVM>();
            services.AddTransient<VideoViewVM>();
            services.AddTransient<ImageViewVM>();
        }

        public static void Init() 
        {
            var services = new ServiceCollection();

            AddVMs(services);

            services.AddSingleton<Course>();

            services.AddSingleton<IWindowService, WindowService>();

            services.AddSingleton<MessageBus>();

            services.AddSingleton<IServiceProvider>(sp => sp);

            _provider = services.BuildServiceProvider();

            foreach (var service in services) 
            {
                _provider.GetRequiredService(service.ServiceType);
            }
        }
    }
}
