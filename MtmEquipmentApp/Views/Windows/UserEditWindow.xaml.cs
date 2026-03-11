using MtmEquipmentApp.Models;
using MtmEquipmentApp.ViewModel.Users;
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
    /// Логика взаимодействия для UserEditWindow.xaml
    /// </summary>
    public partial class UserEditWindow : Window
    {
        public UserEditWindow(int? userId = null)
        {
            InitializeComponent();
            DataContext = new UserEditViewModel(this, userId);
        }

        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserEditViewModel vm && sender is PasswordBox passwordBox)
                vm.Password = passwordBox.Password;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
