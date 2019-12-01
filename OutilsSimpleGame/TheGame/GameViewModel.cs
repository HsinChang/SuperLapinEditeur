using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace TheGame
{
    class GameViewModel : INotifyPropertyChanged
    {
        private Player player;
        private bool shoot;
        private int currentWave;
        public Configuration configuration;


        public GameViewModel()
        {
            player = new Player("Icons/Rabbid.png", 5);
            shoot = false;
            currentWave = 1;
            //Get configuration from default config file
            XmlSerializer xs = new XmlSerializer(typeof(Configuration));
            using (var sr = new StreamReader("Icons/config.xml"))
            {
                configuration = (Configuration)xs.Deserialize(sr);
            }
            /*
            configuration = new Configuration();
            configuration.waves.Add(new Wave("vivendi", 4));
            configuration.waves.Add(new Wave("EA", 3));
            configuration.enemies.Add(new Enemy(5, "vivendi", "Icons/640px-Vivendi_logo.svg.png"));
            configuration.enemies.Add(new Enemy(7, "EA", "Icons/EA_logo_Electronic_Arts.png"));
            XmlSerializer xs = new XmlSerializer(typeof(Configuration));
            TextWriter tw = new StreamWriter(@"c:\temp\garage.xml");
            xs.Serialize(tw, configuration);
            */
        }

        public GameViewModel(Player player, Configuration configuration)
        {
            shoot = false;
            currentWave = 1;
            this.player = player;
            this.configuration = configuration;
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

        public int GetWave()
        {
            return currentWave;
        }

        public void NextWave()
        {
            currentWave += 1;
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
