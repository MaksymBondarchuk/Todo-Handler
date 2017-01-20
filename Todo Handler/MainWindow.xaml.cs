using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Todo_Handler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const int Offset = 10;
        private const int GridHeight = 56;
        private const int GridWidth = 249;

        private int CurrentHorizontalOffset { get; set; } = 10;
        private int CurrentVerticalOffset { get; set; } = 10;

        private List<TodoItem> Items { get; set; } = new List<TodoItem>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {

            //var children = MainGrid.Children;
            //children.Add(MainGrid.Children[MainGrid.Children.Count - 1]);

            var window = new EditWindow(null, null);
            window.ShowDialog();

            if (string.IsNullOrEmpty(window.TodoTitle)) return;
            var id = Guid.NewGuid();
            AddItem(window.TodoTitle, window.Note, id);
            Items.Add(new TodoItem { Title = window.TodoTitle, Note = window.Note, Id = id });
        }

        private void AddItem(string title, string note, Guid id)
        {
            var grid = new Grid
            {
                Margin = new Thickness(CurrentHorizontalOffset, CurrentVerticalOffset, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = GridWidth,
                Height = GridHeight,
                Background = Brushes.LightGray,
                Tag = id
            };

            var buttonDelete = new Button
            {
                Margin = new Thickness(164, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 75,
                Content = "Delete"
            };
            buttonDelete.Click += ButtonDelete_Click;
            grid.Children.Add(buttonDelete);

            var buttonEdit = new Button
            {
                Margin = new Thickness(164, 35, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 75,
                Content = "Edit"
            };
            buttonEdit.Click += ButtonEdit_Click;
            grid.Children.Add(buttonEdit);
            /*
                <Button x:Name="button" Content="Delete" HorizontalAlignment="Left" Margin="164,10,0,0" VerticalAlignment="Top" Width="75"/>
                <Label x:Name="label" Content="Label" HorizontalAlignment="Left" VerticalAlignment="Top" FontSize="14" FontWeight="Bold" Width="159" HorizontalContentAlignment="Center"/>
                <Label x:Name="label1" Content="Label" HorizontalAlignment="Left" Margin="0,31,0,0" VerticalAlignment="Top" Width="159"/>
                <Button x:Name="ButtonEdit" Content="Edit" HorizontalAlignment="Left" Margin="164,35,0,0" VerticalAlignment="Top" Width="75"/>
               */
            //grid.Children.Add(new TextBox
            //{
            //    Margin = new Thickness(10, 10, 0, 0),
            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    VerticalAlignment = VerticalAlignment.Top,
            //    Height = 23,
            //    Width = 120,
            //    TextWrapping = TextWrapping.Wrap,
            //    Text = title
            //});
            grid.Children.Add(new Label()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Width = 159,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Content = title
            });
            grid.Children.Add(new Label()
            {
                Margin = new Thickness(0, 31, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 159,
                Content = note
            });
            MainGrid.Children.Add(grid);

            if (MainGrid.ActualWidth < CurrentHorizontalOffset + 2 * GridWidth + 2 * Offset)
            {
                CurrentHorizontalOffset = Offset;
                CurrentVerticalOffset += GridHeight + Offset;
            }
            else
                CurrentHorizontalOffset += GridWidth + Offset;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DeleteAll();
            AddAll();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
                return;
            var parentGrid = btn.Parent as Grid;
            Items.Remove(Items.Select(t => t).FirstOrDefault(t => parentGrid != null && t.Id == (Guid)parentGrid.Tag));

            //MainGrid.Children.Remove(parentGrid);
            DeleteAll();
            AddAll();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var parentGrid = btn?.Parent as Grid;
            if (parentGrid == null) return;

            var item = Items.Select(t => t).FirstOrDefault(t => t.Id == (Guid)parentGrid.Tag);
            if (item == null) return;

            var window = new EditWindow(item.Title, item.Note);
            window.ShowDialog();

            if (!string.IsNullOrEmpty(window.TodoTitle))
            {
                item.Title = window.TodoTitle;
                var titleLabel = parentGrid.Children[2] as Label;
                if (titleLabel != null)
                    titleLabel.Content = window.TodoTitle;
            }
            if (!string.IsNullOrEmpty(window.Note))
            {
                item.Note = window.Note;
                var noteLabel = parentGrid.Children[3] as Label;
                if (noteLabel != null) noteLabel.Content = window.Note;
            }
        }

        private void AddAll()
        {
            foreach (var item in Items)
                AddItem(item.Title, item.Note, item.Id);
        }

        private void DeleteAll()
        {
            MainGrid.Children.Clear();
            CurrentHorizontalOffset = Offset;
            CurrentVerticalOffset = Offset;
        }
    }
}
