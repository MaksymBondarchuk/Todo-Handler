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

		public MainWindow() {
			InitializeComponent();
		}

		private void ButtonAdd_Click(object sender, RoutedEventArgs e) {

			//var children = MainGrid.Children;
			//children.Add(MainGrid.Children[MainGrid.Children.Count - 1]);

			var grid = new Grid {
				Margin = new Thickness(CurrentHorizontalOffset, CurrentVerticalOffset, 0, 0),
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				Width = GridWidth,
				Height = GridHeight,
				Background = Brushes.LightGray
			};
			//grid.Children.Add(new Button {
			//	Margin = new Thickness(164, 10, 0, 0),
			//	HorizontalAlignment = HorizontalAlignment.Left,
			//	VerticalAlignment = VerticalAlignment.Top,
			//	Width = 75,
			//	Content = "Delete"
			//});

			var btn = new Button {
				Margin = new Thickness(164, 10, 0, 0),
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				Width = 75,
				Content = "Delete"
			};
			btn.Click += ButtonDelete_Click;
			grid.Children.Add(btn);
			grid.Children.Add(new TextBox {
				Margin = new Thickness(10, 10, 0, 0),
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				Height = 23,
				Width = 120,
				TextWrapping = TextWrapping.Wrap,
				Text = "Test text"
			});
			MainGrid.Children.Add(grid);

			if (MainGrid.ActualWidth < CurrentHorizontalOffset + 2 * GridWidth + 2 * Offset) {
				CurrentHorizontalOffset = Offset;
				CurrentVerticalOffset += GridHeight + Offset;
			} else
				CurrentHorizontalOffset += GridWidth + Offset;
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
			MainGrid.Children.Clear();
			CurrentHorizontalOffset = Offset;
			CurrentVerticalOffset = Offset;
		}

		private void ButtonDelete_Click(object sender, RoutedEventArgs e) {
			var btn = sender as Button;
			if (btn == null)
				return;
			var parentGrid = btn.Parent as Grid;
			MainGrid.Children.Remove(parentGrid);
		}
	}
}
