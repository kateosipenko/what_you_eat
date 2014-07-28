namespace Core.Helpers
{
    using Microsoft.Phone.Controls;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;

	public class OrientationHelper : INotifyPropertyChanged
	{
		#region CONSTRUCTOR

		public OrientationHelper()
		{
			this.Orientation = PageOrientation.PortraitUp;
			this.OnPropertyChanged("Orientation");
			this.SetVisibilityProperties(PageOrientation.PortraitUp);
			this.Initialize();
		}

		#endregion CONSTRUCTOR

		#region PROPERTIES

		public PageOrientation Orientation { get; private set; }
		public Visibility PortraitVisibility { get; private set; }
		public Visibility LandscapeVisibility { get; private set; }

		#endregion PROPERTIES

		#region METHODS

		private void Initialize()
		{
			while (Application.Current.RootVisual == null)
			{
                Thread.Sleep(50);
			}

			var frame = Application.Current.RootVisual as PhoneApplicationFrame;
			frame.OrientationChanged += this.FrameOrientationChanged;
			this.Orientation = frame.Orientation;
			this.OnPropertyChanged("Orientation");
			this.SetVisibilityProperties(frame.Orientation);
		}

		private void FrameOrientationChanged(object sender, OrientationChangedEventArgs e)
		{
			Deployment.Current.Dispatcher.BeginInvoke(() =>
			{
				this.Orientation = e.Orientation;
				this.OnPropertyChanged("Orientation");
				this.SetVisibilityProperties(e.Orientation);
			});
		}

		private void SetVisibilityProperties(PageOrientation orientation)
		{
			switch (orientation)
			{
				case PageOrientation.Landscape:
				case PageOrientation.LandscapeLeft:
				case PageOrientation.LandscapeRight:
					this.PortraitVisibility = Visibility.Collapsed;
					this.LandscapeVisibility = Visibility.Visible;
					break;

				case PageOrientation.None:
				case PageOrientation.Portrait:
				case PageOrientation.PortraitDown:
				case PageOrientation.PortraitUp:
					this.PortraitVisibility = Visibility.Visible;
					this.LandscapeVisibility = Visibility.Collapsed;
					break;
			}

			this.OnPropertyChanged("PortraitVisibility");
			this.OnPropertyChanged("LandscapeVisibility");
		}

		#endregion METHODS

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}

		#endregion INotifyPropertyChanged
	}
}
