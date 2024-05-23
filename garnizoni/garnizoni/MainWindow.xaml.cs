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
        Point startPoint = new Point();

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

<<<<<<< tab2
        private Garnizon selektovaniGarnizon_lijevi;

        public Garnizon SelektovaniGarnizon_lijevi
        {
            get
            {
                return selektovaniGarnizon_lijevi;
            }
            set
            {
                selektovaniGarnizon_lijevi = value;
                this.NotifyPropertyChanged("SelektovaniGarnizon_lijevi");
            }
        }

        private Garnizon selektovaniGarnizon_desni;

        public Garnizon SelektovaniGarnizon_desni
        {
            get
            {
                return selektovaniGarnizon_desni;
            }
            set
            {
                selektovaniGarnizon_desni = value;
                this.NotifyPropertyChanged("SelektovaniGarnizon_desni");
            }
        }


=======
>>>>>>> main
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            garnizoni = new ObservableCollection<Garnizon>();
            ucitajGarnizone("garnizoni.txt", garnizoni);
            ucitajJedinice("jedinice.txt", garnizoni);


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

        private void ucitajJedinice(string putanja, ObservableCollection<Garnizon> g)
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

                    foreach(var ga in g)
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

        

        private void lw_desna_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);

                if (listViewItem == null) return;

                Jedinica jedinica = (Jedinica)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                if (jedinica == null) return;

                DataObject dragData = new DataObject("myFormat", jedinica);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
            }
        }

        private static T FindAnchestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }

                current = VisualTreeHelper.GetParent(current);

            } while (current != null);
            return null;
        }

        private void lw_desna_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void lw_desna_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void lw_desna_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Jedinica jedinica = e.Data.GetData("myFormat") as Jedinica;
                if (jedinica != null)
                {
                    SelektovaniGarnizon_lijevi.jedinice.Remove(jedinica);
                    SelektovaniGarnizon_desni.jedinice.Add(jedinica);
                }

                   

            }
           
        }

        

        private void lw_lijeva_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void lw_lijeva_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Jedinica jedinica = e.Data.GetData("myFormat") as Jedinica;
                if(jedinica != null)
                {

                    SelektovaniGarnizon_desni.jedinice.Remove(jedinica);
                    SelektovaniGarnizon_lijevi.jedinice.Add(jedinica);
                    
                }


            }
            
        }

        private void lw_lijeva_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void lw_lijeva_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);

                if (listViewItem == null) return;

                Jedinica jedinica = (Jedinica)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                if (jedinica == null) return;

                DataObject dragData = new DataObject("myFormat", jedinica);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
            }
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

<<<<<<< tab2
        private void cbLijevi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selektovaniGarnizon_lijevi = cbLijevi.SelectedItem as Garnizon;
            if (selektovaniGarnizon_lijevi != null)
            {
                lw_lijeva.ItemsSource = selektovaniGarnizon_lijevi.jedinice;
            }
        }

        private void cbDesni_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selektovaniGarnizon_desni = cbDesni.SelectedItem as Garnizon;
            if (selektovaniGarnizon_desni != null)
            {
                lw_desna.ItemsSource = selektovaniGarnizon_desni.jedinice;
=======
        private void lvJedinice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvJedinice.SelectedItem != null)
            {
                Jedinica j = (Jedinica)lvJedinice.SelectedItem;
                if(j != null)
                {
                    lvSelektovanaJedinica.Items.Clear();
                    lvSelektovanaJedinica.Items.Add(j);
                }
>>>>>>> main
            }
        }
    }
}