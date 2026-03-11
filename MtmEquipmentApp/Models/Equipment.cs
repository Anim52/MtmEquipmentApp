using System;
using System.Collections.Generic;
using System.Text;

namespace MtmEquipmentApp.Models
{
    public class Equipment
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string InventoryNumber { get; set; } = null!;

        public EquipmentStatus Status { get; set; } = EquipmentStatus.Normal;

        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        public ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
    }
    public enum EquipmentStatus
    {
        Normal = 0,
        NeedsInspection = 1,
        UnderRepair = 2,
        Decommissioned = 3
    }
}
