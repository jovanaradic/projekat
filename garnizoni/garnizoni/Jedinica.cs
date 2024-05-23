using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace garnizoni
{
    public class Jedinica : INotifyPropertyChanged
    {
        private string naziv;
        private string adresa;
        private string putanja_ikonica;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Jedinica(string naziv, string adresa, string putanja_ikonica)
        {
            this.naziv = naziv;
            this.adresa = adresa;
            this.putanja_ikonica = putanja_ikonica;
        }

        public Jedinica()
        {

        }

        public string Naziv
        {
            get { return this.naziv; }
            set
            {
                if (this.naziv != value)
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
                if (this.adresa != value)
                {
                    this.adresa = value;
                    this.NotifyPropertyChanged("Adresa");
                }
            }
        }

        public string Putanja
        {
            get { return this.putanja_ikonica; }
            set
            {
                if (this.putanja_ikonica != value)
                {
                    this.putanja_ikonica = value;
                    this.NotifyPropertyChanged("Putanja");
                }
            }
        }

        private void NotifyPropertyChanged(string v)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }
    }
}
