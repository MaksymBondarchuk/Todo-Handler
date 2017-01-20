using System.Reflection;
using System.Windows;

namespace Todo_Handler
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow
    {
        public string TodoTitle { get; set; }
        public string Note { get; set; }

        public EditWindow(string title, string note)
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(title))
                TextBoxTitle.Text = title;
            if (!string.IsNullOrEmpty(note))
                TextBoxNote.Text = note;

            TextBoxTitle.Focus();
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            TodoTitle = TextBoxTitle.Text;
            Note = TextBoxNote.Text;
            Close();
        }
    }
}
