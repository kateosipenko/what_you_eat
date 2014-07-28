using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Core.Helpers.Messages
{
	public delegate void UIPopupButtonHandler(UIPopup sender, object state);

	/// <summary>
	/// Represents dialog box popup with custom content
	/// </summary>
	public class UIPopup
	{
		/// <summary>
		/// Fires when IsOpen property's value changed
		/// </summary>
		public event EventHandler VisibilityChanged;

		public static readonly TimeSpan DefaultVibrationDuration = TimeSpan.FromMilliseconds(75);

		private const double SystemTrayHeight = 32;

		private readonly Popup _popup = new Popup();
		private readonly UIPopupContent _content = new UIPopupContent();
		private IApplicationBar _appBarToShowOnClose;

		public UIPopup()
			: base()
		{
			VisualStateManager.GoToState(_content, _content.HiddenState.Name, false);
			_popup.Child = _content;
			_content.Tap += this.ContentTap;
			VibrationDuration = DefaultVibrationDuration;
		}

		private void ContentTap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			this.Close();
		}

		private bool isOpen;
		/// <summary>
		/// Gets the value that indicates if popup is currently shown on screen
		/// </summary>
		public bool IsOpen
		{
			get { return isOpen; }
			set
			{
				if (isOpen != value)
				{
					isOpen = value;

					var handler = VisibilityChanged;
					if (handler != null)
					{
						handler(this, EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the content.
		/// </summary>
		/// <value>
		/// The content.
		/// </value>
		public object Content
		{
			get
			{
				return _content.UIPresenter.Content;
			}
			set
			{
				_content.UIPresenter.Content = value;
			}
		}

		/// <summary>
		/// Gets or sets the content template.
		/// </summary>
		/// <value>
		/// The content template.
		/// </value>
		public DataTemplate ContentTemplate
		{
			get
			{
				return _content.UIPresenter.ContentTemplate;
			}
			set
			{
				_content.UIPresenter.ContentTemplate = value;
			}
		}

		/// <summary>
		/// Gets or sets the duration of the vibration.
		/// </summary>
		/// <value>
		/// The duration of the vibration.
		/// </value>
		public TimeSpan VibrationDuration
		{
			get;
			set;
		}

		/// <summary>
		/// Shows the content on the screen
		/// </summary>
		public void Open()
		{
			if (IsOpen)
			{
				return;
			}
			IsOpen = true;
			_appBarToShowOnClose = null;
			PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
			if (frame != null)
			{
				frame.OrientationChanged += FrameOrientationChanged;
				PhoneApplicationPage currentPage = frame.Content as PhoneApplicationPage;
				if (currentPage != null)
				{
					(currentPage.Content as Panel).Children.Add(_popup);
					IApplicationBar appBar = currentPage.ApplicationBar;
					if (appBar != null && appBar.IsVisible)
					{
						_appBarToShowOnClose = appBar;
						appBar.IsVisible = false;
					}

					currentPage.BackKeyPress += OnBackKeyPress;
				}
			}

			AdjustLayout();

			_popup.IsOpen = true;
			VisualStateManager.GoToState(_content, _content.VisibleState.Name, true);

			if (VibrationDuration.TotalMilliseconds > 0)
			{
				VibrateController.Default.Start(VibrationDuration);
			}
		}

		private void FrameOrientationChanged(object sender, OrientationChangedEventArgs e)
		{
			this.SetContentSize(e.Orientation);
		}

		private void SetContentSize(PageOrientation orientation)
		{
			if (orientation == PageOrientation.Landscape ||
				orientation == PageOrientation.LandscapeLeft ||
				orientation == PageOrientation.LandscapeRight)
			{
				_content.Height = Application.Current.Host.Content.ActualWidth;
				_content.Width = Application.Current.Host.Content.ActualHeight - _popup.VerticalOffset;
				_content.MessageGrid.MaxHeight = Application.Current.Host.Content.ActualWidth;
			}
			else
			{
				_content.Width = Application.Current.Host.Content.ActualWidth;
				_content.Height = Application.Current.Host.Content.ActualHeight - _popup.VerticalOffset;
				_content.MessageGrid.MaxHeight = Application.Current.Host.Content.ActualHeight - _popup.VerticalOffset;
			}
		}

		/// <summary>
		/// Hides popup
		/// </summary>
		public void Close()
		{
			if (!IsOpen)
			{
				return;
			}
			IsOpen = false;
			if (_appBarToShowOnClose != null)
			{
				_appBarToShowOnClose.IsVisible = true;
			}
			PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
			if (frame != null)
			{
				frame.OrientationChanged -= this.FrameOrientationChanged;
				PhoneApplicationPage currentPage = frame.Content as PhoneApplicationPage;
				if (currentPage != null)
				{
					(currentPage.Content as Panel).Children.Remove(_popup);
					currentPage.BackKeyPress -= OnBackKeyPress;
				}
			}
			_content.HiddenState.Storyboard.Completed += OnHiddenStateStoryboardCompleted;
			VisualStateManager.GoToState(_content, _content.HiddenState.Name, true);
		}


		private void OnHiddenStateStoryboardCompleted(object sender, EventArgs e)
		{
			_content.HiddenState.Storyboard.Completed -= OnHiddenStateStoryboardCompleted;
			_popup.IsOpen = false;
		}

		private void AdjustLayout()
		{
			if (SystemTray.IsVisible)
			{
				_popup.VerticalOffset = SystemTrayHeight;
			}

			PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
			this.SetContentSize(frame.Orientation);
		}

		private void OnBackKeyPress(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			Close();
		}
	}
}
