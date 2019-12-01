using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    class Wave
    {
        public string type { set; get; }
        public int number { set; get; }

        public Wave(string type, int number)
        {
            this.type = type;
            this.number = number;
        }
    }
}
