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
            this.Name = "Game";
            gameViewModel = new GameViewModel();
            DataContext = gameViewModel;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += timer_Tick;
            timer.Start();
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
        #region Timer and Game logic
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

            if (gameViewModel.GetShoot() == true)
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

            //When there is no enemy in the Canvas, generate the next wave
            if (!canvas.Children.Cast<FrameworkElement>()
                      .Any(x => x.Tag != null && x.Tag.ToString() == "enemy"))
            {
                int waveNum = gameViewModel.GetWave()-1;
                if(waveNum == gameViewModel.configuration.waves.Count) {
                    if (Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "Game") != null)
                    {
                        //Check if there is a game window opened
                        MessageBox.Show("You have passed all enemies!");
                        Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "Game");
                        win.Close();
                        return;
                    }
                }
                string enemyName = gameViewModel.configuration.waves.ElementAt(waveNum).type;
                int enemyNum = gameViewModel.configuration.waves.ElementAt(waveNum).number;
                string enemyImagePath = gameViewModel.configuration.enemies.Find(i => i.name == enemyName).EnemyImagePath;
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(enemyImagePath, UriKind.Relative);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
                int pos = 15;
                for (int i=0; i<enemyNum; i++)
                {
                    
                    Image enemyImg = new Image();
                    enemyImg.Source = src;
                    enemyImg.Height = 60;
                    enemyImg.Tag = "enemy";
                    Canvas.SetTop(enemyImg, pos);
                    Canvas.SetRight(enemyImg, 10);
                    canvas.Children.Add(enemyImg);
                    pos += 77;
                }
                if (waveNum <= gameViewModel.configuration.waves.Count-1)
                {
                    gameViewModel.NextWave();
                } 
                
            }

            foreach (var x in canvas.Children.OfType<Image>())
            {
                //move the bullet
                if (x.Tag == "bullet")
                {
                    if (Canvas.GetLeft(x) < 777)
                    {
                        Bullet bullet = new Bullet();
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + bullet.speed);
                    }
                    if (Canvas.GetLeft(x) >= 777)
                    {
                        x.Tag = "invalid";
                    }

                }
                //move the enemy
                if(x.Tag == "enemy")
                {
                    double val = Canvas.GetRight(x);
                    if (Canvas.GetRight(x) < 670)
                    {
                        int waveNum = gameViewModel.GetWave()-2;
                        string enemyName = gameViewModel.configuration.waves.ElementAt(waveNum).type;
                        int enemyNum = gameViewModel.configuration.waves.ElementAt(waveNum).number;
                        int speed = gameViewModel.configuration.enemies.Find(i => i.name == enemyName).speed;
                        Canvas.SetRight(x, Canvas.GetRight(x) + speed);
                    }
                    if(val + x.ActualWidth>= 670)
                    {
                        //Check if there is a game window opened
                        if (Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "Game") != null)
                        {
                            MessageBox.Show("Enemies crossed your defense!");
                            Window win = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.Name == "Game");
                            win.Close();
                            return;
                        }
                    }

                }
                //enemy get hit
                foreach (var y in canvas.Children.OfType<Image>())
                {
                    if((y.Tag == "bullet") && (x.Tag == "enemy"))
                    {
                        //Some values are not initialized, TranslatePoint() needed for exact actual position
                        Rect rect1 = new Rect();
                        Point p1 = y.TranslatePoint(new Point(0, 0), canvas);
                        rect1.X = p1.X;
                        rect1.Y = p1.Y;
                        rect1.Width = (float)y.ActualWidth;
                        rect1.Height = (float)y.ActualHeight;

                        Rect rect2 = new Rect();
                        Point p2 = x.TranslatePoint(new Point(0, 0), canvas);
                        rect2.X = p2.X;
                        rect2.Y = p2.Y;
                        rect2.Width = (float)x.ActualWidth;
                        rect2.Height = (float)x.ActualHeight;

                        if (rect1.IntersectsWith(rect2))
                        {
                            y.Tag = "invalid";
                            x.Tag = "invalid";
                        }
                    }
                }
            }

            //Delete elements out of border and enemies eleminated
            if (canvas.Children.Cast<FrameworkElement>()
                      .Any(x => x.Tag != null && x.Tag.ToString() == "invalid")) {
                var child = (from c in canvas.Children.OfType<Image>()
                             where "invalid".Equals(c.Tag)
                             select c);
                for (int i = 0; i < child.Count(); i++)
                {
                    canvas.Children.Remove(child.ElementAt(i));
                }
            }
            
        }
        #endregion
    }
}
