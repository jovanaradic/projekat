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
    /// Interaction logic for AddWindow1.xaml
    /// </summary>
    public partial class AddWindow1 : Window
    {
        private ObservableCollection<Garnizon> garnizoni;
        public AddWindow1(ObservableCollection<Garnizon> Garnizoni)
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
                slikaGarnizona.Source = bitmap;
            }

        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            Garnizon g = new Garnizon();
            int id;
            if(tbID.Text != null && tbNaziv.Text != null && tbAdresa.Text != null && slikaGarnizona.Source !=null)
            {
                if(Int32.TryParse(tbID.Text, out id))
                {
                    foreach(var ga in garnizoni)
                    {
                        if(ga.Id == id)
                        {
                            MessageBox.Show("Uneseni ID je trenutno zauzet!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    g.Id = id;
                    g.Naziv = tbNaziv.Text;
                    g.Adresa = tbAdresa.Text;
                    g.Putanja = (slikaGarnizona.Source as BitmapImage)?.UriSource.OriginalString;
                    garnizoni.Add(g);
                    MessageBox.Show("Uspjesno dodavanje novog garnizona!", "Uspjesna validacija!", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Unijeli ste ID u pogresnom formatu!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Morate da popunite sva polja prije dodavanja!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void Zatvori_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
