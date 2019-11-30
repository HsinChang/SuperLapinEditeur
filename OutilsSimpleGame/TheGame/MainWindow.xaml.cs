using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TheGame
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GameViewModel gameViewModel;
        public MainWindow()
        {
            InitializeComponent();
            gameViewModel = new GameViewModel();
            DataContext = gameViewModel;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (Canvas.GetBottom(imagePlayer) > 30)
            {
                Canvas.SetBottom(imagePlayer, Canvas.GetBottom(imagePlayer) - gameViewModel.GetGravity());
            }
            else if (gameViewModel.GetGravity() < 0)
            {
                Canvas.SetBottom(imagePlayer, Canvas.GetBottom(imagePlayer) - gameViewModel.GetGravity());
            }

            if(gameViewModel.GetShoot()==true)
            {
                Bullet bullet = new Bullet();
                Image bulletImg = new Image();
                bulletImg.Source = bullet.bulletImage;
                bulletImg.Width = 45;
                bulletImg.Height = 45;
                bulletImg.Tag = "bullet";
                Canvas.SetLeft(bulletImg, Canvas.GetLeft(imagePlayer) + 70);
                Canvas.SetBottom(bulletImg, Canvas.GetBottom(imagePlayer) + 10);
                canvas.Children.Add(bulletImg);
            }

            foreach (var x in canvas.Children.OfType<Image>())
            {
                if (x.Tag == "bullet")
                {
                    if (Canvas.GetLeft(x) < 777) {
                        Bullet bullet = new Bullet();
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + bullet.speed);
                    }

                }
            }
        }

        void PlayerJump(object sender, MouseButtonEventArgs e)
        {
            gameViewModel.PlayerJump();
        }

        void PlayerJumpEnd(object sender, MouseButtonEventArgs e)
        {
            gameViewModel.EndOfJump();
        }


        void PlayerShoot(object sender, MouseEventArgs e)
        {
           
                gameViewModel.SetShoot(true);
       
        }

        void EndShoot(object sender, MouseEventArgs e)
        {
           
                gameViewModel.SetShoot(false);
           
        }
    }
}
