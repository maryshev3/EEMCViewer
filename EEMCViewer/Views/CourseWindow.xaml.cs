﻿using EEMC.Models;
using EEMC.ViewBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EEMC.Views
{
    /// <summary>
    /// Логика взаимодействия для CourseWindow.xaml
    /// </summary>
    public partial class CourseWindow : Page
    {
        public CourseWindow()
        {
            InitializeComponent();
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape)
                return;

            var item = CourseTreeView.ItemContainerGenerator.ContainerFromItem(CourseTreeView.SelectedItem) as TreeViewItem;

            if (item != null)
                item.IsSelected = false;
        }
    }
}
