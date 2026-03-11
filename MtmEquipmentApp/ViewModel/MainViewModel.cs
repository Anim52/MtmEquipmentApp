using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MtmEquipmentApp.Models;
using MtmEquipmentApp.Services;
using MtmEquipmentApp.Views.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MtmEquipmentApp.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly UserRole currentRole = SessionService.CurrentUser?.Role ?? UserRole.Viewer;

        [ObservableProperty]
        private string title = "Учет оборудования МТМ";

        [ObservableProperty]
        private string currentUserName = SessionService.CurrentUser?.FullName ?? "Гость";

        [ObservableProperty]
        private string currentUserRole = SessionService.CurrentUser?.Role.ToString() ?? "-";

        [ObservableProperty]
        private UserControl? currentView;

        [ObservableProperty]
        private bool canOpenDepartments;

        [ObservableProperty]
        private bool canOpenEquipment;

        [ObservableProperty]
        private bool canOpenUsers;

        [ObservableProperty]
        private bool canOpenReports;

        [ObservableProperty]
        private bool canOpenInspections;

        public MainViewModel()
        {
            ConfigureAccess();
            OpenStartPage();
        }

        private void ConfigureAccess()
        {
            CanOpenDepartments = currentRole == UserRole.Admin;
            CanOpenUsers = currentRole == UserRole.Admin;

            CanOpenEquipment = true;
            CanOpenReports = true;

            CanOpenInspections = currentRole == UserRole.Admin || currentRole == UserRole.Inspector;
        }

        private void OpenStartPage()
        {
            if (CanOpenDepartments)
            {
                CurrentView = new DepartmentsPage();
                return;
            }

            if (CanOpenEquipment)
            {
                CurrentView = new EquipmentPage();
                return;
            }

            if (CanOpenReports)
            {
                CurrentView = new ReportsUserControl();
            }
        }

        [RelayCommand]
        private void OpenDepartments()
        {
            if (!CanOpenDepartments)
                return;

            CurrentView = new DepartmentsPage();
        }

        [RelayCommand]
        private void OpenEquipment()
        {
            if (!CanOpenEquipment)
                return;

            CurrentView = new EquipmentPage();
        }

        [RelayCommand]
        private void OpenUsers()
        {
            if (!CanOpenUsers)
                return;

            CurrentView = new UsersPage();
        }

        [RelayCommand]
        private void OpenReports()
        {
            if (!CanOpenReports)
                return;

            CurrentView = new ReportsUserControl();
        }

        [RelayCommand]
        private void OpenInspections()
        {
            if (!CanOpenInspections)
                return;

          CurrentView = new InspectionsPage();
        }
    }
}
