using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using TheGame;

namespace OutilsSimpleGame
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TheGame.MainWindow mw;
        private Configuration configuration;

        public MainWindow()
        {
            InitializeComponent();
            mw = new TheGame.MainWindow();
            mw.Show();
            XmlSerializer xs = new XmlSerializer(typeof(Configuration));
            using (var sr = new StreamReader("Icons/config.xml"))
            {
                configuration = (Configuration)xs.Deserialize(sr);
            }
            this.RefreshList();
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            mw.Close();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ((sender as ListBox).SelectedItem as string);
            //It's in this way because Char.GetNumericValue() returns a double
            int WaveNum = Convert.ToInt32(Char.GetNumericValue(item[4]));
            Wave wave = configuration.waves.ElementAt(WaveNum - 1);
            labelNumber.Content = wave.number;
            labelType.Content = wave.type;
            string enemyImagePath = configuration.enemies.Find(i => i.name == wave.type).EnemyImagePath;
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(enemyImagePath, UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            imageEnemyInfo.Source = src;
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshList()
        {
            int WaveNum = configuration.waves.Count;
            listBox.Items.Clear();
            for(int i= 1; i<=WaveNum; i++)
            {
                listBox.Items.Add("Wave"+i);
            }
        }
    }
}
