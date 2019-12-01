using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    [Serializable]
    public class Wave
    {
        public string type { set; get; }
        public int number { set; get; }
        public int id { set; get; }

        public Wave(string type, int number, int id)
        {
            this.type = type;
            this.number = number;
            this.id = id;
        }
        public Wave()
        {
        }
    }
}
