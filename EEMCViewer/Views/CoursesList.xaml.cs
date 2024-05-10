﻿using EEMC.Models;
using EEMC.ViewBases;
using EEMC.ViewModels;
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
    /// Interaction logic for CoursesList.xaml
    /// </summary>
    public partial class CoursesList : Page, ITextHover
    {
        public Button _oldHoveredButton { get; set; }

        public CoursesList()
        {
            InitializeComponent();
        }

        private void Export_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (this as ITextHover).ConfirmHoverEffect(sender, ButtonType.CourseButton);

            Cursor = Cursors.Hand;
        }

        private void Export_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_oldHoveredButton != default)
            {
                (this as ITextHover).ResetButtonStyle(_oldHoveredButton);
            }

            Cursor = Cursors.Arrow;
        }

        private void Versions_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Export_Button_MouseEnter(sender, e);
        }

        private void Versions_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Export_Button_MouseLeave(sender, e);
        }

        private void Import_Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Export_Button_MouseEnter(sender, e);
        }

        private void Import_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Export_Button_MouseLeave(sender, e);
        }
    }
}
