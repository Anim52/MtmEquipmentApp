using System;
using System.Collections.Generic;
using System.Text;

namespace MtmEquipmentApp.Models
{
    // 4. Таблица: Журнал проверок технического состояния
    public class Inspection
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Remarks { get; set; } = string.Empty; // Заметки о состоянии
        public bool IsDefective { get; set; } // Выявлен ли дефект при проверке

        // Внешний ключ: какое оборудование проверяли
        public int EquipmentId { get; set; }
        public virtual Equipment? Equipment { get; set; }

        // Внешний ключ: кто проводил проверку
        public int UserId { get; set; }
        public virtual User? Inspector { get; set; }
    }
}
