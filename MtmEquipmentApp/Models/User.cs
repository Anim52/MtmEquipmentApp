using System;
using System.Collections.Generic;
using System.Text;

namespace MtmEquipmentApp.Models
{
    public enum UserRole
    {
        Admin,      // Администратор (настройка справочников)
        Inspector,  // Инспектор (проводит осмотры)
        Viewer      // Наблюдатель (только чтение)
    }

    // 1. Таблица: Пользователи
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Пароли лучше хранить в виде хэша
        public string FullName { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Viewer;

        // Связь 1-ко-многим: один пользователь может провести много проверок
        public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
    }
}
