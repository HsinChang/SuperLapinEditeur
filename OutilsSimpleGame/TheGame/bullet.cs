using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TheGame
{
    class Bullet
    {
        public int speed;
        public BitmapImage bulletImage;

        public Bullet()
        {
            speed = 30;

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri("Icons/ubisoft.png", UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();

            bulletImage = src;
        }
    }
}
