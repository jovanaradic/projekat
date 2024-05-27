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
        private Jedinica jedinica;

        public WindowEdit2(ObservableCollection<Garnizon> Garnizoni, Garnizon g, Jedinica j)
        {
            InitializeComponent();
            garnizoni = Garnizoni;
            garnizon = g;
            jedinica = j;

            tbNaziv.Text = jedinica.Naziv.ToString();
            tbAdresa.Text = jedinica.Adresa.ToString();
            string putanja = jedinica.Putanja.ToString();
            slikaJedinice.Source = new BitmapImage(new Uri((jedinica is Jedinica je) ? je.Putanja : ((Jedinica)jedinica).Putanja, UriKind.RelativeOrAbsolute));
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
            if (tbNaziv.Text != null && tbAdresa.Text != null && slikaJedinice.Source != null)
            {
                foreach (var ga in garnizoni)
                {
                    foreach(var je in ga.jedinice)
                    {

                        if (je.Naziv == tbNaziv.Text && je.Naziv != jedinica.Naziv)
                        {
                            MessageBox.Show("Vec postoji jedinica sa tim nazivom!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
                Jedinica j = new Jedinica();
                j.Naziv = tbNaziv.Text;
                j.Adresa = tbAdresa.Text;
                j.Putanja = (slikaJedinice.Source as BitmapImage)?.UriSource.OriginalString;
                garnizon.jedinice.Add(j);
                garnizon.jedinice.Remove(jedinica);
                MessageBox.Show("Uspjesna izmjena jedinice!", "Uspjesna validacija!", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Morate da popunite sva polja prije izmjene!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Zatvori_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
