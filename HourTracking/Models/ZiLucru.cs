using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace HourTracking.Models
{
    public class ZiLucru
    {
        public int Id { get; set; }
        public DateTime Data { get; set; } = DateTime.Today;
        public double OreLucrate { get; set; } = 0;
        public string Comentariu { get; set; } = string.Empty;
        public bool Platit { get; set; } = false;
        public string PlatitText => Platit ? "Da" : "Nu";
    }
}
