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
    /// Interaction logic for WindowEdit1.xaml
    /// </summary>
    public partial class WindowEdit1 : Window
    {
        private Garnizon garnizon;
        private ObservableCollection<Garnizon> garnizoni;

        public WindowEdit1(ObservableCollection<Garnizon> Garnizoni, Garnizon g)
        {
            InitializeComponent();
            garnizon = g;
            garnizoni = Garnizoni;

            tbID.Text = garnizon.Id.ToString();
            tbNaziv.Text = garnizon.Naziv.ToString();
            tbAdresa.Text = garnizon.Adresa.ToString();
            string putanja = garnizon.Putanja.ToString();
            slikaGarnizona.Source = new BitmapImage(new Uri((garnizon is Garnizon ga) ? ga.Putanja : ((Garnizon)garnizon).Putanja, UriKind.RelativeOrAbsolute));
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
                slikaGarnizona.Source = bitmap;
            }
        }

        private void Izmijeni_Click(object sender, RoutedEventArgs e)
        {
            int id;
            if (tbID.Text != null && tbNaziv.Text != null && tbAdresa.Text != null && slikaGarnizona.Source != null)
            {
                if (Int32.TryParse(tbID.Text, out id))
                {
                    foreach (var ga in garnizoni)
                    {
                        if (ga.Id == id && id != garnizon.Id)
                        {
                            MessageBox.Show("Uneseni ID je trenutno zauzet!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    Garnizon izmijenjen = new Garnizon();
                    izmijenjen.Id = id;
                    izmijenjen.Naziv = tbNaziv.Text;
                    izmijenjen.Adresa = tbAdresa.Text;
                    izmijenjen.Putanja = (slikaGarnizona.Source as BitmapImage)?.UriSource.OriginalString;
                    izmijenjen.jedinice = garnizon.jedinice;
                    garnizoni.Add(izmijenjen);
                    garnizoni.Remove(garnizon);
                    MessageBox.Show("Uspjesno izmijena garnizona!", "Uspjesna validacija!", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Unijeli ste ID u pogresnom formatu!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Morate da popunite sva polja prije izmjene!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Zatvori_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
