using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace WPFzad5
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Film> Films { get; } = new ObservableCollection<Film>();
        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
                OnPropertyChanged("ItemSelected");
                NotifyPreviewWindow();
            }
        }
        public bool ItemSelected { get { return SelectedIndex != -1; } }

        private PreviewWindow previewWindow = null;
        public bool PreviewHidden { get { return previewWindow == null; } }

        private void NotifyPreviewWindow()
        {
            if (previewWindow != null)
                previewWindow.Film = selectedIndex != -1 ? Films[selectedIndex] : null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            SelectedIndex = -1;
            FilmListBox.ItemsSource = Films;
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            var dialog = new EditorWindow();
            dialog.Film.Release = DateTime.Today;
            if (dialog.ShowDialog() == true)
                Films.Add(dialog.Film);
            SelectedIndex = Films.Count - 1;
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz usunąć wybrany film?", "Usuń", MessageBoxButton.YesNo,
                MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            int index = SelectedIndex;
            int newIndex;
            if (Films.Count == 0)
                newIndex = -1;
            else
            {
                if (index == Films.Count - 1)
                    newIndex = index - 1;
                else
                    newIndex = index;
            }
            Films.RemoveAt(SelectedIndex);
            SelectedIndex = newIndex;
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            var dialog = new EditorWindow();
            var film = Films[SelectedIndex];
            dialog.Film.Title = film.Title;
            dialog.Film.Release = film.Release;
            dialog.Film.Description = film.Description;
            if (dialog.ShowDialog() == true)
            {
                film.Title = dialog.Film.Title;
                film.Release = dialog.Film.Release;
                film.Description = dialog.Film.Description;
                FilmListBox.Items.Refresh(); // bez tego ObservableCollection nie powiadamia ListBoxa, że jakiś element został zmieniony; powiadamia tylko o dodaniu lub usunięciu elementu
                NotifyPreviewWindow();
            }
        }

        private void PreviewClick(object sender, RoutedEventArgs e)
        {
            previewWindow = new PreviewWindow();
            OnPropertyChanged("PreviewHidden");
            previewWindow.Closed += (ss, ee) =>
            {
                previewWindow = null;
                OnPropertyChanged("PreviewHidden");
            };
            NotifyPreviewWindow();
            previewWindow.Show();
        }

        private void RandomizeClick(object sender, RoutedEventArgs e)
        {
            var rng = new Random();
            DateTime start = new DateTime(1900, 1, 1);
            DateTime end = new DateTime(2100, 1, 1);
            int range = (end - start).Days;
            foreach (var f in Films)
            {
                f.Title = RandomString(rng);
                f.Release = start.AddDays(rng.Next(range));
                f.Description = RandomString(rng);
            }
            FilmListBox.Items.Refresh();
            NotifyPreviewWindow();
        }

        private string RandomString(Random rng)
        {
            var sb = new StringBuilder();
            int length = rng.Next(1, 21);
            for (int i = 0; i < length; ++i)
                sb.Append((char)rng.Next('a', 'z' + 1));
            return sb.ToString();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (previewWindow != null)
                previewWindow.Close();
        }
    }
}
