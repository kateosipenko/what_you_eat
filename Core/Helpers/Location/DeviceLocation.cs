using System;
using System.Device.Location;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Windows.Devices.Geolocation;

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

		public Geoposition CurrentPosition { get; set; }

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

		private static volatile Geolocator geolocator;

		/// <summary>
		/// Starts location service
		/// </summary>
		/// <param name="accuracy"></param>
		/// <param name="movementThreshold"></param>
		public void Start(uint reportInterval = 10000)
		{
			lock (lockObj)
			{
				if (geolocator == null)
				{
					geolocator = new Geolocator();
					geolocator.DesiredAccuracy = PositionAccuracy.High;
					geolocator.MovementThreshold = 0;
					geolocator.ReportInterval = reportInterval;
					geolocator.PositionChanged += this.GeolocatorPositionChanged;
					geolocator.StatusChanged += this.GeolocatorStatusChanged;
				}
			}
		}

		public PositionStatus GetLocationStatus()
		{
			var result = PositionStatus.Disabled;
			if (geolocator != null)
			{
				result = geolocator.LocationStatus;
			}
			else
			{
				this.Start();
				result = geolocator.LocationStatus;
			}

			return result;
		}

		private void GeolocatorStatusChanged(Geolocator sender, StatusChangedEventArgs args)
		{
			EventHandler<StatusChangedEventArgs> gpsStatusChanged = GpsStatusChanged;
			if (gpsStatusChanged != null)
			{
				gpsStatusChanged(sender, args);
			}
		}

		private void GeolocatorPositionChanged(Geolocator sender, PositionChangedEventArgs args)
		{
			EventHandler<DeviceLocationArgs> currentLocationChanged = CurrentLocationChanged;
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
				if (geolocator != null)
				{
					geolocator.PositionChanged -= GeolocatorPositionChanged;
					geolocator.StatusChanged -= GeolocatorStatusChanged;
					geolocator = null;
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
		public static event EventHandler<StatusChangedEventArgs> GpsStatusChanged;
	}

	public class DeviceLocationArgs
	{
		public Geoposition Position { get; set; }
	}
}
