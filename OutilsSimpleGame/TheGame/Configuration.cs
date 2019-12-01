using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    [Serializable]
    public class Configuration
    {
        public List<Enemy> enemies { get; set; }
        public List<Wave> waves { get; set; }

        public Configuration(){
            enemies = new List<Enemy>();
            waves = new List<Wave>();
        }
    }
}
