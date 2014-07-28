using Microsoft.Phone.Net.NetworkInformation;

namespace Core.Helpers.Connection
{
	public static class ConnectionChecker
	{
		public static bool IsConnectionAvailable()
		{
			//return false;
			return NetworkInterface.GetIsNetworkAvailable();
		}
	}
}
