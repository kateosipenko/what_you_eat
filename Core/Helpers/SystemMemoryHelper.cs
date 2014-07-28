using Microsoft.Phone.Info;
using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace Core
{
	public class SystemMemoryHelper
	{
		private SystemMemoryHelper()
		{
		}

		private static volatile DispatcherTimer timer;

		private static volatile object lockObject = new object();

		/// <summary>
		/// Start memory monitoring
		/// </summary>
		/// <param name="interval"></param>
		public static void Start(TimeSpan interval)
		{
#if DEBUG
			if(timer == null)
			{
				lock(lockObject)
				{
					if(timer == null)
					{
						timer = new DispatcherTimer();
						timer.Tick += MemoryTimerTick;
						timer.Interval = interval;
						timer.Start();
					}
				}
			}
#endif
		}

		/// <summary>
		/// Stop memory monitoring
		/// </summary>
		public static void Stop()
		{
#if DEBUG
			if(timer != null)
			{
				lock (lockObject)
				{
					timer.Stop();
					timer.Tick -= MemoryTimerTick;
					timer = null;
				}
			}
#endif
		}

		/// <summary>
		/// Display memory status
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		static void MemoryTimerTick(object sender, EventArgs e)
		{
#if DEBUG
			GC.Collect();
			const string total = "DeviceTotalMemory";
			const string current = "ApplicationCurrentMemoryUsage";
			const string peak = "ApplicationPeakMemoryUsage";

			long totalBytes = (long)DeviceExtendedProperties.GetValue(total);
			long currentBytes = (long)DeviceExtendedProperties.GetValue(current);
			long peakBytes = (long)DeviceExtendedProperties.GetValue(peak);

			Debug.WriteLine("\n------------- Memory status " + DateTime.Now + " --------------");
			Debug.WriteLine("totalMemory: " + (totalBytes / 1024) + " Kb");
			Debug.WriteLine("currentMemory: " + (currentBytes / 1024) + " Kb");
			Debug.WriteLine("peakMemory: " + (peakBytes / 1024) + " Kb");
			Debug.WriteLine("------------- Memory status --------------\n");
#endif
		}
	}
}
