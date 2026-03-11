using MtmEquipmentApp.Models;
using MtmEquipmentApp.ViewModel.Inspections;
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
    /// Логика взаимодействия для InspectionEditWindow.xaml
    /// </summary>
    public partial class InspectionEditWindow : Window
    {
        public InspectionEditWindow(Inspection? inspection = null)
        {
            InitializeComponent();
            DataContext = new InspectionEditViewModel(inspection, this);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
