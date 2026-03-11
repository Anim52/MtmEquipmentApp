using System;
using System.Collections.Generic;
using System.Text;

namespace MtmEquipmentApp.Models
{
    // 2. Таблица: Подразделения (Цеха)
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Например: "Механический цех №1"
        public string Location { get; set; } = string.Empty;

        // Связь 1-ко-многим: в одном цехе много оборудования
        public virtual ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
    }
}
