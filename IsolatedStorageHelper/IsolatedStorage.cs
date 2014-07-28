namespace IsolatedStorageHelper
{
	using System;
	using System.Collections.Generic;
    using System.IO.IsolatedStorage;
    using System.Threading;

	/// <summary>
	/// Class for working with isolated storage
	/// </summary>
	public static class IsolatedStorage
	{
		#region FIELDS

		/// <summary>
		/// Lock object for correct async writing 
		/// </summary>
		private static object lockObject = new object();

		#endregion FIELDS

		#region PROPERTIES

		/// <summary>
		/// Gets value of the settings property
		/// </summary>
		private static IsolatedStorageSettings Storage
		{
			get
			{
				return IsolatedStorageSettings.ApplicationSettings;
			}
		}

		#endregion PROPERTIES

		#region METHODS

		/// <summary>
		/// Reads the value form application state or isolated storage.
		/// </summary>
		/// <typeparam name="T">Type of the result</typeparam>
		/// <param name="key">The key for reading.</param>
		/// <returns>Readed value</returns>
		public static T ReadValue<T>(string key)
		{
			T result = default(T);
			if (!string.IsNullOrEmpty(key))
			{
				lock (lockObject)
				{
					if (Storage.Contains(key))
					{
						if (Storage[key] is T)
						{
							try
							{
								result = (T)Storage[key];
							}
							catch
							{
							}
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Writes the value to application state and isolated storage.
		/// </summary>
		/// <param name="key">The key for writing.</param>
		/// <param name="value">The value for writing.</param>
		public static void WriteValue(string key, object value)
		{
			if (!string.IsNullOrEmpty(key) && value != null)
			{
				lock (lockObject)
				{
					ThreadPool.QueueUserWorkItem(ThreadPoolCallback, new KeyValuePair<string, object>(key, value));
				}
			}
		}

		/// <summary>
		/// Writes walue to isolated storage.
		/// </summary>
		/// <param name="keyValue">Key and value.</param>
		private static void ThreadPoolCallback(object keyValue)
		{
			KeyValuePair<string, object> data = (KeyValuePair<string, object>)keyValue;
			lock (lockObject)
			{
				try
				{
					Storage[data.Key] = data.Value;
					Storage.Save();
				}
				catch (Exception)
				{

				}
			}
		}

		#endregion METHODS
	}
}
