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
            if (!db.Users.Any())
            {
                db.Users.Add(new User
                {
                    FullName = "Системный администратор",
                    Login = "admin",
                    PasswordHash = PasswordHasher.HashPassword("admin"),
                    Role = UserRole.Admin,
                });
            }

            if (!db.Departments.Any())
            {
                db.Departments.AddRange(
                    new Department { Name = "Производственный цех" },
                    new Department { Name = "Склад" },
                    new Department { Name = "Отдел контроля" }
                );
            }

            db.SaveChanges();

            if (!db.Equipment.Any())
            {
                var departments = db.Departments.ToList();

                db.Equipment.AddRange(
                    new Equipment
                    {
                        Name = "Токарный станок 1К62",
                        InventoryNumber = "INV-0001",
                        Status = EquipmentStatus.Normal,
                        DepartmentId = departments[0].Id
                    },
                    new Equipment
                    {
                        Name = "Компрессор ПК-5",
                        InventoryNumber = "INV-0002",
                        Status = EquipmentStatus.NeedsInspection,
                        DepartmentId = departments[1].Id
                    }
                );

                db.SaveChanges();
            }
        }
    }
}
