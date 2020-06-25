using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NonoGramm
{
    /// <summary>
    /// Логика взаимодействия для LevelSelect.xaml
    /// </summary>
    public partial class LevelSelect : Window
    {
        public LevelSelect()
        {
            InitializeComponent();
            BitmapImage theImage = new BitmapImage(new Uri(@"images\nonogram.jpg", UriKind.RelativeOrAbsolute)); //Фон окна
            newGame.Background = new ImageBrush(theImage);
            
        }


        private void ListBox_Loaded(object sender, RoutedEventArgs e) //ListBox со списком уровней
        {
            Random rnd = new Random();
            int i = 0;
            using (StreamReader sr = File.OpenText(@"levels.txt"))
            {
                string line = null;
                do
                {
                    line = sr.ReadLine();
                    if (i == 0){listBox.Items.Add(line); i++;}
                    else if (i < 2) i++;
                    else i = 0;
                } while (line != null);
                sr.Close();
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e) //кнопка выбора уровня
        {
            string lev = (string)listBox.SelectedItem;
            MainWindow main = new MainWindow(lev);
            main.Show();
            this.Close();
            
        }
    }
}
