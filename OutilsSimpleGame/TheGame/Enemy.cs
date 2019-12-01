using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TheGame
{
    class Enemy
    {
        public int speed { get; set; }
        public string name { get; set; }
        public string EnemyImagePath { get; set; }

        public Enemy(int speed, string name, string EnemyImagePath)
        {
            this.speed = speed;
            this.name = name;
            this.EnemyImagePath = EnemyImagePath;
        }
    }
}
