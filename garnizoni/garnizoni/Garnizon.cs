using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace garnizoni
{
    public class Garnizon : INotifyPropertyChanged
    {
        private int id;
        private string naziv;
        private string adresa;
        private string putanja_ikonica;

        public ObservableCollection<Jedinica> jedinice
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler? PropertyChanged;


        public Garnizon(int id, string naziv, string adresa, string putanja_ikonica)
        {
            this.id = id;
            this.naziv = naziv;
            this.adresa = adresa;
            this.putanja_ikonica = putanja_ikonica;
            this.jedinice = new ObservableCollection<Jedinica>();
        }

        public Garnizon()
        {
            jedinice = new ObservableCollection<Jedinica>();
        }

        public int Id
        {
            get { return this.id; }
            set {
                if(this.id != value)
                {
                    this.id = value;
                    this.NotifyPropertyChanged("Id");
                }
            }
        }

        public string Naziv
        {
            get { return this.naziv; }
            set
            {
                if(this.naziv != value)
                {
                    this.naziv = value;
                    this.NotifyPropertyChanged("Naziv");
                }
            }
        }

        public string Adresa
        {
            get { return this.adresa; }
            set
            {
                if(this.adresa != value)
                {
                    this.adresa= value;
                    this.NotifyPropertyChanged("Adresa");
                }
            }
        }

        public string Putanja
        {
            get { return this.putanja_ikonica; }
            set
            {
                if(this.putanja_ikonica != value)
                {
                    this.putanja_ikonica = value;
                    this.NotifyPropertyChanged("Putanja");
                }
            }
        }



        private void NotifyPropertyChanged(string v)
        {
            if(this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }
    }
}
