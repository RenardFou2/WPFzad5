using System;
using System.ComponentModel;
using System.Windows;

namespace WPFzad5
{
    public partial class PreviewWindow : Window, INotifyPropertyChanged
    {
        private Film film = new Film();
        public Film Film
        {
            get { return film; }
            set
            {
                if (value == null)
                {
                    Error = "Nie wybrano filmu.";
                    film.Title = "";
                    film.Release = DateTime.Today;
                    FormattedRelease = "";
                    film.Description = "";
                }
                else
                {
                    Error = "";
                    film.Title = value.Title;
                    film.Release = value.Release;
                    FormattedRelease = film.Release.ToString("d");
                    film.Description = value.Description;
                }
            }
        }

        private string release;
        public string FormattedRelease
        {
            get { return release; }
            private set { release = value; OnPropertyChanged("FormattedRelease"); }
        }

        private string error;
        public string Error
        {
            get { return error; }
            private set { error = value; OnPropertyChanged("Error"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public PreviewWindow()
        {
            InitializeComponent();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
