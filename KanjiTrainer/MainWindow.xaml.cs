using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KanjiTrainer
{
    public partial class MainWindow : Window
    {
        string filename;
        List<int> list = new List<int>();
        int counter = 0;
        int listCount;
        string kanji = string.Empty;
        //string kana = string.Empty;
        //string translate = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
        }
        public void Cleaner()
        {
            bKanjiChanger.Visibility = Visibility.Hidden;
            tbTranslate.Text = "Translate";
            tbKanji.Text = "Kanji";
            tbKana.Text = "Kana";
            kanji = string.Empty;
            lName.Content = string.Empty;
            counter = 0;
            lRemains.Content = string.Empty;
            lCounter.Content = string.Empty;
            listCount = 0;
            list.Clear();
            filename = string.Empty;
        }
        private void KanjiChanger(object sender, RoutedEventArgs e)
        {
            if (counter == 0)
            {
                bKanjiChanger.Content = "--->";
            }
            if (counter == listCount)
            {
                Cleaner();
                System.Windows.MessageBox.Show("Конец");
            }
            else
            {
                IEnumerable<string> result = File.ReadLines(filename).Skip(list[counter]).Take(1);
                foreach (string str in result)
                {
                    string[] line = str.Split('－');
                    tbTranslate.Text = line[0];
                    tbKanji.Text = "*********";
                    kanji = line[1];
                    tbKana.Text = line[2];
                }
                lRemains.Content = listCount - counter;
                lCounter.Content = counter;
                counter++;
            }
        }

        void Opening()
        {
            if (kanji != string.Empty)
            {
                tbKanji.Text = kanji;
            }
        }

        private void FindFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Text documents (*.wkj)|*.wkj";
            dialog.FilterIndex = 2;

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                Cleaner();
                filename = dialog.FileName;
                lName.Content = dialog.SafeFileName;
                bKanjiChanger.Visibility = Visibility.Visible;
                bKanjiChanger.Content = "Начать";

                Random rnd = new Random();
                TextReader reader = new StreamReader(filename);
                while (reader.ReadLine() != null)
                {
                    listCount++;
                }
                reader.Close();

                for (int i = 0; i < listCount; i++)
                {
                    list.Add(i);
                }

                for (int i = list.Count - 1; i >= 1; i--)
                {
                    int j = rnd.Next(i + 1);
                    var temp = list[j];
                    list[j] = list[i];
                    list[i] = temp;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Вы не выбрали файл", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
        
        private void tbKanji_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Opening();
        }

        private void tbKanji_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (counter != 0)
            {
                tbKanji.Text = "*********";
            }
        }
    }
}
