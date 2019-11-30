using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TheGame
{
    class Player
    {
        public BitmapImage PlayerImage;
        public int gravity;
        public bool jumping;
        public bool shooting;

        public Player(string PlayerImagePath, int gravity)
        {
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(PlayerImagePath, UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();

            this.PlayerImage = src;
            this.gravity = gravity;
            this.jumping = false;
            shooting = false;
        }

    }
}
