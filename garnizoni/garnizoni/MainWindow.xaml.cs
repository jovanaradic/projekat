using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace garnizoni
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public ObservableCollection<Garnizon> garnizoni { get; set; }

        private Garnizon selektovaniGarnizon;

        public Garnizon SelektovaniGarnizon
        {
            get
            {
                return selektovaniGarnizon;
            }
            set
            {
                selektovaniGarnizon = value;
                this.NotifyPropertyChanged("SelektovaniGarnizon");
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            garnizoni = new ObservableCollection<Garnizon>();
            ucitajGarnizone("garnizoni.txt", garnizoni);
            ucitajJedinice("jedinice.txt");


        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(string v)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }

        private void ucitajGarnizone(string putanja, ObservableCollection<Garnizon> g)
        {
            StreamReader sr = null;
            string linija="";
            int id;
            string naziv, adresa, putanja_ikonice;
            try
            {
                sr = new StreamReader(putanja);
                while((linija = sr.ReadLine()) != null)
                {
                    string[] dijeloviLinije = linija.Split('|');
                    id = Int32.Parse(dijeloviLinije[0]);
                    naziv = dijeloviLinije[1];
                    adresa = dijeloviLinije[2];
                    putanja_ikonice = dijeloviLinije[3];
                    g.Add(new Garnizon(id, naziv, adresa, putanja_ikonice));
                }
                sr.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Greska pri otvaranju fajla." + e.Message);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }

        private void ucitajJedinice(string putanja)
        {
            StreamReader sr = null;
            string linija = "";
            string naziv, adresa, putanja_ikonice;
            try
            {
                sr = new StreamReader(putanja);
                while((linija = sr.ReadLine()) != null)
                {
                    string[] dijelovi = linija.Split('|');
                    string garnizon = dijelovi[0];

                    foreach(var ga in garnizoni)
                    {
                        if(ga.Naziv == garnizon)
                        {
                            naziv = dijelovi[1];
                            adresa = dijelovi[2];
                            putanja_ikonice = dijelovi[3];
                            Jedinica j = new Jedinica(naziv, adresa, putanja_ikonice);
                            ga.jedinice.Add(j);
                        }
                    }

                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Neuspjesno ucitavanje fajla jedinice.txt \n" + e.Message);
            }
            finally
            {
                if(sr != null)
                {
                    sr.Close();
                }    
            }
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ListView_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {

        }

        private void btnDodajGarnizon_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnIzmijeniGarnizon_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnObrisiGarnizon_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDodajJedinicu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnIzmijeniJedinicu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnObrisiJedinicu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Garnizoni_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void selecijaGarnizona_selectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}