using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace garnizoni
{
    /// <summary>
    /// Interaction logic for WindowEdit2.xaml
    /// </summary>
    public partial class WindowEdit2 : Window
    {
        private ObservableCollection<Garnizon> garnizoni;
        private Garnizon garnizon;

        public WindowEdit2(ObservableCollection<Garnizon> Garnizoni, Garnizon g)
        {
            InitializeComponent();
            garnizoni = Garnizoni;



        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                string putanja = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(putanja);
                bitmap.EndInit();
                slikaJedinice.Source = bitmap;
            }
        }

        private void Izmijeni_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Zatvori_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
