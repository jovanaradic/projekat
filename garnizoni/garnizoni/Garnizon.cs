using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace garnizoni
{
    class Garnizon : INotifyPropertyChanged
    {
        private int id;
        private string naziv;
        private string adresa;
        private Image ikonica;


        public event PropertyChangedEventHandler? PropertyChanged;

        public Garnizon(int id, string naziv, string adresa, string putanja)
        {
            this.id = id;
            this.naziv = naziv;
            this.adresa = adresa;
            this.ikonica = new Image();
            ikonica.Source = new BitmapImage(new Uri(putanja));

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

        public Image Ikonica
        {
            get { return this.ikonica; }
            set
            {
                if(this.ikonica != value)
                {
                    this.ikonica = value;
                    this.NotifyPropertyChanged("Ikonica");
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
