using System;
using System.Collections.Generic;
using System.Text;

namespace MtmEquipmentApp.Models
{
    public class ReportRow
    {
        public string EquipmentName { get; set; } = string.Empty;
        public string InventoryNumber { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int InspectionCount { get; set; }
        public bool HasDefects { get; set; }
    }
}
