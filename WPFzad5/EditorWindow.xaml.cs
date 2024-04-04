using System.Windows;

namespace WPFzad5
{
    public partial class EditorWindow : Window
    {
        public Film Film { get; set; } = new Film();

        public EditorWindow()
        {
            InitializeComponent();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
