using MtmEquipmentApp.Models;
using MtmEquipmentApp.ViewModel.Departments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MtmEquipmentApp.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для DepartmentEditWindow.xaml
    /// </summary>
    public partial class DepartmentEditWindow : Window
    {
        public DepartmentEditWindow(int? departmentId = null)
        {
            InitializeComponent();
            DataContext = new DepartmentEditViewModel(this, departmentId);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
