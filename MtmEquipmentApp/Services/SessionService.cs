using MtmEquipmentApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MtmEquipmentApp.Services
{
    public static class SessionService
    {
        public static User? CurrentUser { get; set; }
    }
}
