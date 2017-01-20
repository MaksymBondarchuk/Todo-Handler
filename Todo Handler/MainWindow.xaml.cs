using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Web.Script.Serialization;

namespace Todo_Handler
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private const int Offset = 10;
		private const int GridHeight = 63;
		private const int GridWidth = 249;
		private const string FileName = "Data.json";

		private int CurrentHorizontalOffset { get; set; } = 10;
		private int CurrentVerticalOffset { get; set; } = 10;

		// ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
		private JsonObject Items { get; set; } = new JsonObject();

		public MainWindow() {
			InitializeComponent();

			#region Load from JSON
			try {
				using (var file = new StreamReader(FileName)) {
					var json = file.ReadToEnd();
					var deserializer = new JavaScriptSerializer();
					Items = deserializer.Deserialize<JsonObject>(json);
					AddAll();
				}
			} catch (Exception) {
				// ignored
			}
			#endregion
		}

		#region Create
		private void ButtonAdd_Click(object sender, RoutedEventArgs e) {
			var window = new EditWindow(null, null);
			window.ShowDialog();

			if (string.IsNullOrEmpty(window.TodoTitle))
				return;
			var id = Guid.NewGuid();
			AddItem(window.TodoTitle, window.Note, id);
			Items.Items.Add(new TodoItem { Title = window.TodoTitle, Note = window.Note, Id = id });
		}

		private bool CheckCannotAdd() {
			return MainGrid.ActualHeight < CurrentVerticalOffset + GridHeight + 2 * Offset;
		}

		private void AddItem(string title, string note, Guid id) {
			if (CheckCannotAdd())
				return;

			var grid = new Grid {
				Margin = new Thickness(CurrentHorizontalOffset, CurrentVerticalOffset, 0, 0),
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				Width = GridWidth,
				Height = GridHeight,
				Background = Brushes.LightGray,
				Tag = id,
				Style = FindResource("TodoGridStyle") as Style
			};

			var buttonDelete = new Button {
				Margin = new Thickness(224, 6, 0, 0),
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				Content = "x",
				Style = FindResource("DeleteButtonStyle") as Style
			};
			buttonDelete.Click += ButtonDelete_Click;
			grid.Children.Add(buttonDelete);

			var buttonEdit = new Button {
				Margin = new Thickness(224, 35, 0, 0),
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				Content = "🖉",
				Style = FindResource("EditButtonStyle") as Style
			};
			buttonEdit.Click += ButtonEdit_Click;
			grid.Children.Add(buttonEdit);

			grid.Children.Add(new Label() {
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				FontSize = 14,
				FontWeight = FontWeights.Bold,
				Width = 159,
				HorizontalContentAlignment = HorizontalAlignment.Center,
				Content = title
			});
			grid.Children.Add(new Label() {
				Margin = new Thickness(0, 31, 0, 0),
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				Width = 159,
				Content = note
			});
			MainGrid.Children.Add(grid);

			if (MainGrid.ActualWidth < CurrentHorizontalOffset + 2 * GridWidth + 2 * Offset) {
				CurrentHorizontalOffset = Offset;
				CurrentVerticalOffset += GridHeight + Offset;
			} else
				CurrentHorizontalOffset += GridWidth + Offset;

			ButtonAdd.IsEnabled = !CheckCannotAdd();
		}

		private void AddAll() {
			foreach (var item in Items.Items)
				AddItem(item.Title, item.Note, item.Id);
		}
		#endregion

		#region Delete
		private void ButtonDelete_Click(object sender, RoutedEventArgs e) {
			var btn = sender as Button;
			if (btn == null)
				return;
			var parentGrid = btn.Parent as Grid;
			Items.Items.Remove(Items.Items.Select(t => t)
				.FirstOrDefault(t => parentGrid != null && t.Id == (Guid)parentGrid.Tag));

			//MainGrid.Children.Remove(parentGrid);
			DeleteAll();
			AddAll();
		}

		private void DeleteAll() {
			MainGrid.Children.Clear();
			CurrentHorizontalOffset = Offset;
			CurrentVerticalOffset = Offset;
		}
		#endregion

		#region Update
		private void ButtonEdit_Click(object sender, RoutedEventArgs e) {
			var btn = sender as Button;
			var parentGrid = btn?.Parent as Grid;
			if (parentGrid == null)
				return;

			var item = Items.Items.Select(t => t).FirstOrDefault(t => t.Id == (Guid)parentGrid.Tag);
			if (item == null)
				return;

			var window = new EditWindow(item.Title, item.Note);
			window.ShowDialog();

			if (!string.IsNullOrEmpty(window.TodoTitle)) {
				item.Title = window.TodoTitle;
				var titleLabel = parentGrid.Children[2] as Label;
				if (titleLabel != null)
					titleLabel.Content = window.TodoTitle;
			}
			if (!string.IsNullOrEmpty(window.Note)) {
				item.Note = window.Note;
				var noteLabel = parentGrid.Children[3] as Label;
				if (noteLabel != null)
					noteLabel.Content = window.Note;
			}
		}
		#endregion

		#region Additional
		private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
			DeleteAll();
			AddAll();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			var json = new JavaScriptSerializer().Serialize(Items);
			using (var file = new StreamWriter(FileName)) {
				file.WriteLine(json);
			}
		}
		#endregion
	}
}
