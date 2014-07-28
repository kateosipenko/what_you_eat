using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Navigation;

namespace Core.Helpers.Navigation
{
	public class NavigationService : INavigationService
	{
		private PhoneApplicationFrame _mainFrame;

		public event NavigatingCancelEventHandler Navigating;

		public void NavigateTo(Uri pageUri)
		{
			if (EnsureMainFrame())
			{
				_mainFrame.Navigate(pageUri);
			}
		}

		public void GoBack()
		{
			if (EnsureMainFrame()
				&& _mainFrame.CanGoBack)
			{
				_mainFrame.GoBack();
			}
		}

		private bool EnsureMainFrame()
		{
			if (_mainFrame != null)
			{
				return true;
			}

			_mainFrame = Application.Current.RootVisual as PhoneApplicationFrame;

			if (_mainFrame != null)
			{
				_mainFrame.Navigating += (s, e) =>
				{
					if (Navigating != null)
					{
						Navigating(s, e);
					}
				};

				return true;
			}

			return false;
		}

		public string CurrentPage
		{
			get
			{
				string result = string.Empty;
				if (EnsureMainFrame())
				{
					result = _mainFrame.CurrentSource.OriginalString;
				}

				return result;
			}
		}

		public Dictionary<string, string> QueryString
		{
			get
			{
				var uri = new Uri(this.CurrentPage, UriKind.Relative);
				if (this.CurrentPage.Contains('?'))
				{
					return this.ParseParams(this.CurrentPage);
				}
				else
				{
					return new Dictionary<string, string>();
				}
			}
		}

		private Dictionary<string, string> ParseParams(string url)
		{
			var queryStringRegex = new Regex(@"[\?&](?<name>[^&=]+)=(?<value>[^&=]+)");
			var matches = queryStringRegex.Matches(url);
			var dictionary = new Dictionary<string, string>();
			for (int i = 0; i < matches.Count; i++)
			{
				var match = matches[i];
				dictionary.Add(match.Groups["name"].Value, match.Groups["value"].Value);
			}
			return dictionary;
		}
	}
}
