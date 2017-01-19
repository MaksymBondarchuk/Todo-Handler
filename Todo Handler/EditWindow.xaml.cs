using System.Windows;

namespace Todo_Handler
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow
    {
        public string TodoTitle { get; private set; }
        public string Note { get; private set; }

        public EditWindow()
        {
            InitializeComponent();
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            TodoTitle = TextBoxTitle.Text;
            Note = TextBoxNote.Text;
            Close();
        }
    }
}
