using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TheGame
{
    class GameViewModel : INotifyPropertyChanged
    {
        private Player player;
        private bool shoot;

        public GameViewModel()
        {
            player = new Player("Icons/Rabbid.png", 5);
        }

        public int GetGravity()
        {
            return player.gravity;
        }

        public void PlayerJump()
        {
            player.jumping = true;
            player.gravity = -20;
        }

        public void EndOfJump()
        {
            player.jumping = false;
            player.gravity = 20;
        }

        public bool GetShoot()
        {
            return this.shoot;
        }

        public void SetShoot(bool st)
        {
            this.shoot = st;
        }

        public BitmapImage PlayerImagePlay
        {
            get { return this.player.PlayerImage; }
            set { this.player.PlayerImage = value; this.OnPropertyChange("PlayerImagePlay"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
