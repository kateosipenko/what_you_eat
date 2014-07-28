using System;
using System.Device.Location;
using System.Windows;
using System.Windows.Threading;

namespace Core.Helpers.Location
{
	/// <summary>
	/// Gps location helper
	/// </summary>
	public class DeviceLocation
	{
		/// <summary>
		/// private constructor
		/// </summary>
		private DeviceLocation()
		{
		}

		protected static object lockObj = new object();

		private static volatile DeviceLocation instance;

		public GeoPosition<GeoCoordinate> CurrentPosition { get; set; }

		public static DeviceLocation Instance
		{
			get
			{
				if (instance == null)
				{
					lock (lockObj)
					{
						if (instance == null)
						{
							instance = new DeviceLocation();
						}
					}
				}

				return instance;
			}
		}

		private static volatile GeoCoordinateWatcher geoWather;

		/// <summary>
		/// Starts location service
		/// </summary>
		/// <param name="accuracy"></param>
		/// <param name="movementThreshold"></param>
		public void Start(uint reportInterval = 10000)
		{
			lock (lockObj)
			{
				if (geoWather == null)
				{
                    geoWather = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
					geoWather.MovementThreshold = 0;
					geoWather.PositionChanged += this.GeolocatorPositionChanged;
					geoWather.StatusChanged += this.GeolocatorStatusChanged;
				}
			}
		}

		public GeoPositionStatus GetLocationStatus()
		{
            var result = GeoPositionStatus.Disabled;
			if (geoWather != null)
			{
				result = geoWather.Status;
			}
			else
			{
				this.Start();
				result = geoWather.Status;
			}

			return result;
		}

        private void GeolocatorStatusChanged(object sender, GeoPositionStatusChangedEventArgs args)
		{
			EventHandler<GeoPositionStatusChangedEventArgs> gpsStatusChanged = GpsStatusChanged;
			if (gpsStatusChanged != null)
			{
				gpsStatusChanged(sender, args);
			}
		}

        private void GeolocatorPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> args)
		{
			var currentLocationChanged = CurrentLocationChanged;
			if (currentLocationChanged != null)
			{
				var eargs = new DeviceLocationArgs();
				eargs.Position = args.Position;
				currentLocationChanged(sender, eargs);
			}

			this.CurrentPosition = args.Position;
		}

		/// <summary>
		/// Stop location service
		/// </summary>
		public void Stop()
		{
			lock (lockObj)
			{
				if (geoWather != null)
				{
					geoWather.PositionChanged -= GeolocatorPositionChanged;
					geoWather.StatusChanged -= GeolocatorStatusChanged;
					geoWather = null;
					this.CurrentPosition = null;
				}
			}
		}

		/// <summary>
		/// Position changed event
		/// </summary>
		public static event EventHandler<DeviceLocationArgs> CurrentLocationChanged;

		/// <summary>
		/// Status changed event
		/// </summary>
		public static event EventHandler<GeoPositionStatusChangedEventArgs> GpsStatusChanged;
	}

	public class DeviceLocationArgs : EventArgs
	{
		public GeoPosition<GeoCoordinate> Position { get; set; }
	}
}
