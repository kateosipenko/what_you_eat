using System;
using System.Collections.Generic;
using System.Windows.Navigation;

namespace Core.Helpers.Navigation
{
	public interface INavigationService
	{
		event NavigatingCancelEventHandler Navigating;
		void NavigateTo(Uri pageUri);
		void GoBack();
		string CurrentPage { get; }
		Dictionary<string, string> QueryString { get; }
	}
}
