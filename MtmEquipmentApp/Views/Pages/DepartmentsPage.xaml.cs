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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MtmEquipmentApp.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для DepartmentsPage.xaml
    /// </summary>
    public partial class DepartmentsPage : UserControl
    {
        public DepartmentsPage()
        {
            InitializeComponent();
            DataContext = new DepartmentViewModel();
        } 
    }
}
