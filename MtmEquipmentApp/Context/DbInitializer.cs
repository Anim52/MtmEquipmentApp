using MtmEquipmentApp.Models;
using MtmEquipmentApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MtmEquipmentApp.Context
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext db)
        {
            var rnd = new Random();

            // Пользователи
            if (!db.Users.Any())
            {
                db.Users.Add(new User
                {
                    FullName = "Системный администратор",
                    Login = "admin",
                    PasswordHash = PasswordHasher.HashPassword("admin"),
                    Role = UserRole.Admin
                });

                // Несколько инспекторов и зрителей
                for (int i = 1; i <= 5; i++)
                {
                    db.Users.Add(new User
                    {
                        FullName = $"Инспектор {i}",
                        Login = $"inspector{i}",
                        PasswordHash = PasswordHasher.HashPassword("123"),
                        Role = UserRole.Inspector
                    });

                    db.Users.Add(new User
                    {
                        FullName = $"Зритель {i}",
                        Login = $"viewer{i}",
                        PasswordHash = PasswordHasher.HashPassword("123"),
                        Role = UserRole.Viewer
                    });
                }
            }

            // Отделы
            if (!db.Departments.Any())
            {
                db.Departments.AddRange(
                    new Department { Name = "Производственный цех", Location = "Здание 1" },
                    new Department { Name = "Склад", Location = "Здание 2" },
                    new Department { Name = "Отдел контроля", Location = "Здание 3" }
                );
            }

            db.SaveChanges();

            var departments = db.Departments.ToList();
            var users = db.Users.ToList();

            // Оборудование
            if (!db.Equipment.Any())
            {
                string[] equipmentNames = { "Токарный станок", "Компрессор", "Станок фрезерный", "Сварочный аппарат", "Кран-балка" };

                for (int i = 1; i <= 25; i++)
                {
                    var eq = new Equipment
                    {
                        Name = $"{equipmentNames[rnd.Next(equipmentNames.Length)]} {i}",
                        InventoryNumber = $"INV-{1000 + i}",
                        Status = (EquipmentStatus)rnd.Next(0, 4),
                        DepartmentId = departments[rnd.Next(departments.Count)].Id
                    };

                    db.Equipment.Add(eq);
                    db.SaveChanges();

                    // Создаем 1–3 инспекции для оборудования
                    int inspectionsCount = rnd.Next(1, 4);
                    for (int j = 0; j < inspectionsCount; j++)
                    {
                        db.Inspections.Add(new Inspection
                        {
                            EquipmentId = eq.Id,
                            UserId = users[rnd.Next(users.Count)].Id,
                            Date = DateTime.Now.AddDays(-rnd.Next(0, 30)),
                            Remarks = $"Инспекция {j + 1} для {eq.Name}",
                            IsDefective = rnd.Next(0, 2) == 1
                        });
                    }

                    db.SaveChanges();
                }
            }
        }
    }
}
