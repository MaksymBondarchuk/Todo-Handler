using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Todo_Handler
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const int Offset = 10;
		private const int GridHeight = 56;

		private int CurrentVerticalOffset { get; set; } = 0;

		public MainWindow() {
			InitializeComponent();
		}

		private void ButtonAdd_Click(object sender, RoutedEventArgs e) {

			//var children = MainGrid.Children;
			//children.Add(MainGrid.Children[MainGrid.Children.Count - 1]);

			var grid = new Grid {
				Margin = new Thickness(10, CurrentVerticalOffset, 0, 0),
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				Width = 249,
				Height = GridHeight,
				Background = Brushes.LightGray
			};
			grid.Children.Add(new Button {
				Margin = new Thickness(164, 10, 0, 0),
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				Width = 75,
				Content = "Delete"
			});

			var btn = new Button();
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
			//var byk = higger.Margin;
			//higger.Margin = new Thickness(byk.Left, byk.Top + higger.Height + 50, byk.Right, byk.Bottom);
			CurrentVerticalOffset += GridHeight + Offset;
		}
	}
}
