using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlurryWP8SDK;
using FlurryWP8SDK.Models;


namespace Core.Helpers
{
	public static class FlurryHelper
	{
		public static void PageViewed(string name, bool isFromTile = false)
		{
			Api.LogEvent("Page viewed", new List<Parameter> { new Parameter("Page name", name), new Parameter("Is from tile", isFromTile.ToString()) });
		}
				
		public static void SwitchOffCameras()
		{
			Api.LogEvent("Switch off cameras");
		}

		public static void AlertView()
		{
			Api.LogEvent("Alert view");
		}

		public static void LocationSelected(string locationName)
		{
			Api.LogEvent("Location selected", new List<Parameter> { new Parameter("Location name", locationName) });
		}

		public static void AddToFavorites(string itemType, string id)
		{
			Api.LogEvent("Add to favorites", new List<Parameter> { new Parameter("Type", itemType), new Parameter("Id", id) });
		}

		public static void ShowCamera(string id, string url)
		{
			Api.LogEvent("Show camera", new List<Parameter> { new Parameter("Camera id", id), new Parameter("Image url", url) });
		}

		public static void PinToStart(string source)
		{
			Api.LogEvent("Pin to start", new List<Parameter> { new Parameter("Source", source)});
		}

		public static void StartSession()
		{
#if DEBUG
			FlurryWP8SDK.Api.StartSession("X3XZ7GS7QRGC5WB2DPWG");
#else
			FlurryWP8SDK.Api.StartSession("JWZYKVCBPTP4JFZ7DTS4");
#endif
		}
	}
}
