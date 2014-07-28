using System.Windows;
using System.Windows.Controls;

namespace Core.Helpers
{
	public class CustomListBox : ListBox
	{
		#region FIELDS

		private ContentPresenter presenter;

		#endregion FIELDS

		#region CONSTRUCTOR

		public CustomListBox()
		{
			this.presenter = new ContentPresenter { Visibility = Visibility.Collapsed };
			this.Loaded += this.CustomListBoxLoaded;
		}

		#endregion CONSTRUCTOR

		#region DEPENDENCY_PROPERTIES

		#region EmptyTemplate

		public DataTemplate EmptyTemplate
		{
			get { return (DataTemplate)GetValue(EmptyTemplateProperty); }
			set { SetValue(EmptyTemplateProperty, value); }
		}

		public static readonly DependencyProperty EmptyTemplateProperty =
			DependencyProperty.Register("EmptyTemplate", typeof(DataTemplate), typeof(CustomListBox), new PropertyMetadata(null));

		#endregion EmptyTemplate

		#region IsBusy

		public bool IsBusy
		{
			get { return (bool)GetValue(IsBusyProperty); }
			set { SetValue(IsBusyProperty, value); }
		}

		public static readonly DependencyProperty IsBusyProperty =
			DependencyProperty.Register("IsBusy", typeof(bool), typeof(CustomListBox), new PropertyMetadata(OnIsBusyChanged));

		public static void OnIsBusyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var lb = o as CustomListBox;
			CheckVisibility(lb);
		}

		#endregion IsBusy

		#endregion DEPENDENCY_PROPERTIES

		#region METHODS

		protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			CheckVisibility(this);
		}

		private static void CheckVisibility(CustomListBox lb)
		{
			if (lb.IsBusy)
			{
				lb.presenter.Visibility = Visibility.Collapsed;
				lb.Visibility = Visibility.Visible;
			}
			else
			{
				if (lb.Items == null ||
					lb.Items.Count == 0)
				{
					lb.presenter.Visibility = Visibility.Visible;
					lb.Visibility = Visibility.Collapsed;
				}
				else
				{
					lb.presenter.Visibility = Visibility.Collapsed;
					lb.Visibility = Visibility.Visible;
				}
			}
		}

		private void CustomListBoxLoaded(object sender, RoutedEventArgs e)
		{
			this.Loaded -= this.CustomListBoxLoaded;
			var parent = this.Parent as Panel;
			parent.Children.Remove(this);

			var mainGrid = new Grid();
			Grid.SetColumn(mainGrid, Grid.GetColumn(this));
			Grid.SetColumnSpan(mainGrid, Grid.GetColumnSpan(this));
			Grid.SetRow(mainGrid, Grid.GetRow(this));
			Grid.SetRowSpan(mainGrid, Grid.GetRowSpan(this));
			parent.Children.Add(mainGrid);

			this.presenter.ContentTemplate = this.EmptyTemplate;
			mainGrid.Children.Add(this);
			mainGrid.Children.Add(this.presenter);
		}

		#endregion METHODS
	}
}
