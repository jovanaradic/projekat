using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
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

        Point dropPosition = new Point();

        Point startPoint3 = new Point();
        private List<int> garnizoniNaCanvasu;
        private List<string> jediniceNaCanvasu;


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



        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            garnizoni = new ObservableCollection<Garnizon>();
            ucitajGarnizone("garnizoni.txt", garnizoni);
            ucitajJedinice("jedinice.txt", garnizoni);
            garnizoniNaCanvasu = new List<int>();
            jediniceNaCanvasu = new List<string>();


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
                    if(SelektovaniGarnizon_desni != null)
                    {

                        SelektovaniGarnizon_lijevi.jedinice.Remove(jedinica);
                        SelektovaniGarnizon_desni.jedinice.Add(jedinica);
                    }
                    else
                    {
                        MessageBox.Show("Oba garnizona moraju biti selektovana!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
                    if(SelektovaniGarnizon_lijevi == null)
                    {
                        MessageBox.Show("Oba garnizona moraju biti selektovana!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {

                        SelektovaniGarnizon_desni.jedinice.Remove(jedinica);
                        SelektovaniGarnizon_lijevi.jedinice.Add(jedinica);

                    }
                    
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
            AddWindow1 addWindow1 = new AddWindow1(garnizoni);
            addWindow1.ShowDialog();
        }

        private void btnIzmijeniGarnizon_Click(object sender, RoutedEventArgs e)
        {
            if (SelektovaniGarnizon != null)
            {
                WindowEdit1 windowEdit1 = new WindowEdit1(garnizoni, SelektovaniGarnizon);
                windowEdit1.ShowDialog();
            }
            else
            {
                MessageBox.Show("Garnizon nije selektovan!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnObrisiGarnizon_Click(object sender, RoutedEventArgs e)
        {
            Garnizon g = SelektovaniGarnizon;
            if(g != null)
            {
                if (System.Windows.MessageBox.Show("Da li zaizsta zelite da obrisete garnizon?", "Potvrda o brisanju garnizona", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    garnizoni.Remove(g);
                    if(lvSelektovanaJedinica.Items != null)
                    {
                        lvSelektovanaJedinica.Items.Clear();
                    }
                    MessageBox.Show("Uspjesno brisanje!", "Uspjesna validacija!", MessageBoxButton.OK, MessageBoxImage.Information);
                    foreach(var ga in garnizoni)
                    {
                        if(ga.Naziv == "SAMOSTALNE")
                        {
                            foreach(var j in g.jedinice)
                            {
                                ga.jedinice.Add(j);
                            }
                        }
                    }
                }   
            }
            else
            {
                MessageBox.Show("Garnizon nije selektovan!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDodajJedinicu_Click(object sender, RoutedEventArgs e)
        {
            if (SelektovaniGarnizon != null)
            {
                AddWindow2 addWindow2 = new AddWindow2(garnizoni, SelektovaniGarnizon);
                addWindow2.ShowDialog();
            }
            else
            {
                MessageBox.Show("Jedinica nije selektovana!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnIzmijeniJedinicu_Click(object sender, RoutedEventArgs e)
        {
            Jedinica j = lvJedinice.SelectedItem as Jedinica;
            if (j != null)
            {
                WindowEdit2 windowEdit2 = new WindowEdit2(garnizoni, SelektovaniGarnizon);
                windowEdit2.ShowDialog();
            }
            else
            {
                MessageBox.Show("Jedinica nije selektovana!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnObrisiJedinicu_Click(object sender, RoutedEventArgs e)
        {
            Jedinica j = lvJedinice.SelectedItem as Jedinica;
            if (j != null)
            {
                if (System.Windows.MessageBox.Show("Da li zaizsta zelite da obrisete jedinicu?", "Potvrda o brisanju jedinice", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    SelektovaniGarnizon.jedinice.Remove(j);
                    lvSelektovanaJedinica.Items.Clear();
                    MessageBox.Show("Uspjesno brisanje!", "Uspjesna validacija!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Jedinica nije selektovana!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

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
            }
        }

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

            }
        }

        private void selekcijaGarnizona_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(selekcijaGarnizona.SelectedItem != null)
            {
                lvSelektovanaJedinica.Items.Clear();
            }
        }

        private void lwGarnizoniTab3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lwGarnizoniTab3.SelectedItem != null)
            {
                Garnizon g = lwGarnizoniTab3.SelectedItem as Garnizon;
                lwJediniceTab3.ItemsSource = g.jedinice;
            }
        }



        private void slikaCanvas_Drop(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(slikaCanvas);
            if (dropPosition != null)
            {
                if (e.Data.GetDataPresent("garnizonFormat"))
                {
                    Garnizon garnizon = e.Data.GetData("garnizonFormat") as Garnizon;
                    
                    Image ikonica = new Image
                    {
                        Source = new BitmapImage(new Uri((garnizon is Garnizon g) ? g.Putanja : ((Garnizon)garnizon).Putanja, UriKind.Relative)),
                        Width = 30,
                        Height = 30
                    };

                    // Postavljanje slike na određene koordinate u Canvas-u
                    Canvas.SetLeft(ikonica, dropPosition.X);
                    Canvas.SetTop(ikonica, dropPosition.Y);

                    slikaCanvas.Children.Add(ikonica);
                    ikonica.Tag = garnizon;
                    garnizoniNaCanvasu.Add(garnizon.Id);
                }
                else if (e.Data.GetDataPresent("jedinicaFormat"))
                {
                    Jedinica jedinica = e.Data.GetData("jedinicaFormat") as Jedinica;
                    Image ikonica = new Image
                    {
                        Source = new BitmapImage(new Uri((jedinica is Jedinica j) ? j.Putanja : ((Jedinica)jedinica).Putanja, UriKind.Relative)),
                        Width = 30,
                        Height = 30
                    };

                    // Postavljanje slike na određene koordinate u Canvas-u
                    Canvas.SetLeft(ikonica, dropPosition.X);
                    Canvas.SetTop(ikonica, dropPosition.Y);

                    slikaCanvas.Children.Add(ikonica);
                    ikonica.Tag = jedinica;
                    jediniceNaCanvasu.Add(jedinica.Naziv);
                }
            }
        }

        private void lw_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("garnizonFormat") && !e.Data.GetDataPresent("jedinicaFormat"))
            {
                e.Effects = DragDropEffects.None;
            }
            if(e.Data.GetDataPresent("garnizonFormat"))
            {
                Garnizon garnizon = e.Data.GetData("garnizonFormat") as Garnizon;
                if(garnizoniNaCanvasu.Contains(garnizon.Id))
                {
                    e.Effects = DragDropEffects.None;
                    return;
                }
            }
            else if(e.Data.GetDataPresent("jedinicaFormat"))
            {
                Jedinica jedinica = e.Data.GetData("jedinicaFormat") as Jedinica;

                if (jediniceNaCanvasu.Contains(jedinica.Naziv))
                {
                    e.Effects = DragDropEffects.None;
                    return;
                }
            }
        }

        private void lw_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint3 - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);

                if (listViewItem == null) return;

                if (listViewItem.Content is Garnizon garnizon)
                {
                    DataObject dragData = new DataObject("garnizonFormat", garnizon);
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                }
                else if (listViewItem.Content is Jedinica jedinica)
                {
                    DataObject dragData = new DataObject("jedinicaFormat", jedinica);
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                }
            }
        }

        private void lw_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint3 = e.GetPosition(null);
        }

        private void slikaCanvas_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            DependencyObject izvor = (DependencyObject)e.OriginalSource;
            if(izvor is Image ikonica)
            {
                ContextMenu meni = new ContextMenu();

                MenuItem ukloniSaCanvasa = new MenuItem();
                ukloniSaCanvasa.Header = "Izbrisi sa mape";
                ukloniSaCanvasa.Click += (sender, args) =>
                {
                    slikaCanvas.Children.Remove(ikonica);
                    if(ikonica.Tag is Garnizon garnizon)
                    {
                        garnizoniNaCanvasu.Remove(garnizon.Id);
                    }
                    if(ikonica.Tag is Jedinica jedinica)
                    {
                        jediniceNaCanvasu.Remove(jedinica.Naziv); 
                    }
                    
                };
                meni.Items.Add(ukloniSaCanvasa);

                MenuItem ukloni = new MenuItem();
                ukloni.Header = "Izbrisi";
                ukloni.Click += (sender, args) =>
                {
                    if (ikonica.Tag is Garnizon garnizon)
                    {
                        garnizoni.Remove(garnizon);
                        slikaCanvas.Children.Remove(ikonica);
                        garnizoniNaCanvasu.Remove(garnizon.Id);
                    }
                    else if(ikonica.Tag is Jedinica jedinica)
                    {
                        foreach (var g in garnizoni)
                        {
                            foreach(var j in g.jedinice)
                            {
                                if(j == jedinica)
                                {
                                    g.jedinice.Remove(jedinica);
                                    slikaCanvas.Children.Remove(ikonica);
                                    jediniceNaCanvasu.Remove(jedinica.Naziv);
                                }
                            }
                        }
                    }
                };
                meni.Items.Add(ukloni);

                ContextMenuService.SetContextMenu(ikonica, meni);
            }
            else
            {
                ((FrameworkElement)sender).ContextMenu = null;
            }
            
        }
    }
}