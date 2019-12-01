using Microsoft.Win32;
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
        private Player player;
        private BitmapImage src_temp;
        private string enemy_img_tmp;
        private int WaveCount;

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
            DataContext = configuration;
            player = new Player("Icons/Rabbid.png", 5);
            this.RefreshList();
            src_temp = null;
            enemy_img_tmp = null;
            WaveCount = configuration.waves.Count;
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            if (this.configuration != null && this.player != null)
            {
                mw = new TheGame.MainWindow(player, configuration);
                mw.Show();
            } else
            {
                MessageBox.Show("Please check the info onglet to make sure that there is a configuration loaded");
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            mw.Close();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ((sender as ListBox).SelectedItem as string);
            //It's in this way because Char.GetNumericValue() returns a double
            if (item != null)
            {
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
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            //TODO: reject XML files that is not in desired structure
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML - File | *.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                XmlSerializer xs = new XmlSerializer(typeof(Configuration));
                using (var sr = new StreamReader(openFileDialog.FileName))
                {
                    configuration = (Configuration)xs.Deserialize(sr);
                }
            }
            this.RefreshList();
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

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "XML - File | *.xml";
            if (saveFileDialog1.ShowDialog()==true)
            {
                XmlSerializer xs = new XmlSerializer(typeof(Configuration));
                TextWriter tw = new StreamWriter(saveFileDialog1.FileName);
                xs.Serialize(tw, configuration);
            }
            MessageBox.Show("Configuration saved");
        }

        private void ButtonPlayerPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                //display the selected file
                string enemyImagePath = openFileDialog.FileName;
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(enemyImagePath, UriKind.Relative);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
                imagePlayer.Source = src;
                src_temp = src;
            }
            this.RefreshList();
        }

        private void ButtonSavePlayer_Click(object sender, RoutedEventArgs e)
        {
            player.PlayerImage = src_temp;
        }

        private void ButtonEnemyPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                //display the selected file
                enemy_img_tmp = openFileDialog.FileName;
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(enemy_img_tmp, UriKind.Relative);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
                imageEnemy.Source = src;
            }
            this.RefreshList();
        }

        private void ButtonSaveEnemy_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxEnemyEdit.SelectedItem != null)
            {
                if (textBoxName.Text != "")
                {
                    string name_old = (comboBoxEnemyEdit.SelectedItem as Enemy).name;
                    string name_new = textBoxName.Text;
                    configuration.enemies.Find(x => x.name == name_old).name = name_new;
                }
                if (IsDigitsOnly(textBoxSpeed.Text) && textBoxSpeed.Text != "")
                {
                    int speed = Convert.ToInt32(textBoxSpeed.Text);
                    string name_old = (comboBoxEnemyEdit.SelectedItem as Enemy).name;
                    configuration.enemies.Find(x => x.name == name_old).speed = speed;
                }
                else
                {
                    MessageBox.Show("Please enter digits for speed");
                }
                if (enemy_img_tmp != null)
                {
                    string name_old = (comboBoxEnemyEdit.SelectedItem as Enemy).name;
                    configuration.enemies.Find(x => x.name == name_old).EnemyImagePath = enemy_img_tmp;
                }
                MessageBox.Show("Enemy data saved");
            } else
            {
                MessageBox.Show("Please select an enemy to edit");
            }
            this.RefreshList();
        }

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private void ComboBoxEnemyEdit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ComboBox).SelectedItem as Enemy;
            if (item!=null)
            {
                enemy_img_tmp = item.EnemyImagePath;
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(enemy_img_tmp, UriKind.Relative);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
                imageEnemy.Source = src;
                textBoxName.Text = item.name;
                textBoxSpeed.Text = item.speed.ToString();
            }
        }

        private void ButtonAddEnemy_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxName.Text != "" && IsDigitsOnly(textBoxSpeed.Text) && textBoxSpeed.Text != "" && enemy_img_tmp != null)
            {
                string name_new = textBoxName.Text;
                if (configuration.enemies.Find(x=>x.name == name_new)==null) {
                    int speed = Convert.ToInt32(textBoxSpeed.Text);
                    configuration.enemies.Add(new Enemy(speed, name_new, enemy_img_tmp));
                    MessageBox.Show("New Enemy Added");
                } else
                {
                    MessageBox.Show("An enemy with this name already exists");
                }
            }
            else
            {
                MessageBox.Show("A photo, a name and a speed (int only) are needed to add an enemy");
            }
            this.RefreshList();
        }

        private void ButtonDeleteEnemy_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxEnemyEdit.SelectedItem != null)
            {
                string name_old = (comboBoxEnemyEdit.SelectedItem as Enemy).name;
                configuration.enemies.RemoveAll(x => x.name == name_old);
                MessageBox.Show("Enemy removed");
            } else
            {
                MessageBox.Show("Please select an enemy to delete");
            }
            this.RefreshList();
        }

        private void ButtonSaveWave_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxSelectWave.SelectedItem != null && comboBoxType.SelectedItem!= null && slider.Value!=0)
            {
                string type = (comboBoxType.SelectedItem as Enemy).name;
                int id = (comboBoxSelectWave.SelectedItem as Wave).id;

                configuration.waves.Find(x => x.id == id).type = type;
                configuration.waves.Find(x => x.id == id).number = Convert.ToInt32(slider.Value);
            } else
            {
                MessageBox.Show("Please make sure that the wave and desired enemy type is selected, and number of the slider is not 0.");
            }
            this.RefreshList();
        }

        private void ComboBoxSelectWave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ComboBox).SelectedItem as Wave;
            if (item != null)
            {
                slider.Value = item.number;
            }
        }

        private void ButtonAddWave_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxType.SelectedItem != null && slider.Value != 0)
            {
                string type = (comboBoxType.SelectedItem as Enemy).name;
                int id = WaveCount + 1;
                WaveCount += 1;
                configuration.waves.Add(new Wave(type, Convert.ToInt32(slider.Value), id));
                MessageBox.Show("Wave added");
            } else
            {
                MessageBox.Show("Please make sure that the desired enemy type is selected, and number of the slider is not 0.");
            }
            this.RefreshList();
        }

        private void ButtonDeleteWave_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxSelectWave.SelectedItem != null)
            {
                int id = (comboBoxSelectWave.SelectedItem as Wave).id;
                configuration.waves.RemoveAll(x => x.id == id);
                MessageBox.Show("Wave deleted");
            }
            else
            {
                MessageBox.Show("Please select a wave to delete");
            }
            this.RefreshList();
        }
    }
}
