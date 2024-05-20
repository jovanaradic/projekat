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
    class Jedinica : INotifyPropertyChanged
    {
        string naziv;
        private string adresa;
        private Image ikonica;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Jedinica(string naziv, string adresa, string putanja)
        {
            this.naziv = naziv;
            this.adresa = adresa;
            this.ikonica = new Image();
            ikonica.Source = new BitmapImage(new Uri(putanja));
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

        public Image Ikonica
        {
            get { return this.ikonica; }
            set
            {
                if (this.ikonica != value)
                {
                    this.ikonica = value;
                    this.NotifyPropertyChanged("Ikonica");
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
