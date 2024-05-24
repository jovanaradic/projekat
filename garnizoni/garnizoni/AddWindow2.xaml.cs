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
    /// Interaction logic for AddWindow2.xaml
    /// </summary>
    public partial class AddWindow2 : Window
    {
        private ObservableCollection<Garnizon> garnizoni;
        private Garnizon garnizon;
        public AddWindow2(ObservableCollection<Garnizon> Garnizoni, Garnizon g)
        {
            InitializeComponent();
            garnizoni = Garnizoni;
            garnizon = g;
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

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            Jedinica j = new Jedinica();
            if(tbNaziv.Text != null && tbAdresa.Text != null && slikaJedinice.Source != null)
            {
                    foreach(var ja in garnizon.jedinice)
                    {
                        if(ja.Naziv == tbNaziv.Text)
                        {
                            MessageBox.Show("Vec postoji jedinica sa tim nazivom!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    j.Naziv = tbNaziv.Text;
                    j.Adresa = tbAdresa.Text;
                    j.Putanja = (slikaJedinice.Source as BitmapImage)?.UriSource.AbsolutePath;
                    garnizon.jedinice.Add(j);
                    MessageBox.Show("Uspjesno dodavanje nove jedinice!", "Uspjesna validacija!", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
            }
            else
            {
                MessageBox.Show("Morate da popunite sva polja prije dodavanja!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Zatvori_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
