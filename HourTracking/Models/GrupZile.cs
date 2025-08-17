using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HourTracking.Models
{
    public class GrupZile : List<ZiLucru>
    {
        public string Titlu { get; set; }
        public List<ZiLucru> Zile
        {
            get => this;
            set
            {
                Clear();
                AddRange(value);
            }
        }
    }
}
