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
        public InspectionEditWindow(Inspection inspection)
        {
            InitializeComponent();

            if (inspection == null)
            {
                DataContext = new InspectionEditViewModel(null); // Новая инспекция
            }
            else
            {
                DataContext = new InspectionEditViewModel(inspection); // Редактируем существующую
            }
        }
    }
}
