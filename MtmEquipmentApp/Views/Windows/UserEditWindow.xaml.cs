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
        private readonly User _user;

        public UserEditWindow(User user)
        {
            InitializeComponent();

            if (user == null)
            {
                DataContext = new UserEditViewModel(null); // Новый пользователь
            }
            else
            {
                DataContext = new UserEditViewModel(user); // Редактируем существующего
            }
        }
    }
}
