using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MtmEquipmentApp.Services;
using MtmEquipmentApp.Views.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MtmEquipmentApp.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [RelayCommand]
        public void Navigate(string pageName)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;

            switch (pageName)
            {
                case "EquipmentsPage":
                    mainWindow.MainFrame.Navigate(new EquipmentPage());
                    break;
                case "DepartmentsPage":
                    mainWindow.MainFrame.Navigate(new DepartmentsPage());
                    break;
                case "UsersPage":
                    mainWindow.MainFrame.Navigate(new UsersPage());
                    break;
                case "InspectionsPage":
                    mainWindow.MainFrame.Navigate(new InspectionsPage());
                    break;
                default:
                    break;
            }
        }
    }
}
