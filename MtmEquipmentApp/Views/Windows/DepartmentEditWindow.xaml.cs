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
        private readonly Department _department;

        public DepartmentEditWindow(Department department)
        {
            InitializeComponent();

            if (department == null)
            {
                DataContext = new DepartmentEditViewModel(null); // Новое подразделение
            }
            else
            {
                DataContext = new DepartmentEditViewModel(department); // Редактирование существующего
            }
        }
    }
}
