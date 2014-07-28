using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Core.Helpers.Serialization
{
	public static class JsonSerializer
	{
		#region Fields

		/// <summary>
		/// instance of StringReader
		/// </summary>
		private static StringReader stringReader;

		/// <summary>
		/// instance of StringWriter
		/// </summary>
		private static StringWriter writer;

		/// <summary>
		/// instance of Newtonsoft.Json.JsonTextReader
		/// </summary>
		private static Newtonsoft.Json.JsonTextReader reader;

		/// <summary>
		/// instance of Newtonsoft.Json.JsonSerializer
		/// </summary>
		private static Newtonsoft.Json.JsonSerializer serializer;

		#endregion Fields

		#region Properties

		/// <summary>
		/// Gets value of serializer (initializes if it equals null)
		/// </summary>
		public static Newtonsoft.Json.JsonSerializer Serializer
		{
			get
			{
				if (serializer == null)
				{
					serializer = new Newtonsoft.Json.JsonSerializer();
					serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
				}

				return serializer;
			}
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Method for deserialization json
		/// </summary>
		/// <typeparam name="T">type of returned data</typeparam>
		/// <param name="jsonData">data for deserializing</param>
		/// <returns>deserialized data</returns>
		public static T Deserialize<T>(string jsonData)
		{
			T result;

			stringReader = new StringReader(jsonData);
			reader = new Newtonsoft.Json.JsonTextReader(stringReader);


			try
			{
				result = Serializer.Deserialize<T>(reader);
			}
			catch (Exception ex)
			{
				ex.ToString();
				throw;
			}

			return result;
		}

		/// <summary>
		/// Method for serialization data to json string
		/// </summary>
		/// <param name="value">data for serialization</param>
		/// <returns>serialized json as string</returns>
		public static string Serialize(object value)
		{
			string result = string.Empty;
			writer = new StringWriter();
			try
			{
				Serializer.Serialize(writer, value);
				result = writer.ToString();
			}
			catch (Exception ex)
			{
				ex.Message.ToString();
				//throw;
			}

			return result;
		}

		public static string FormatCollectionField(string response)
		{
			var responseWithReplacedCounter = Regex.Replace(response, "\":\\[[0-9,]+", "\":[");
			var responseWithReplacedEmptyCollection = Regex.Replace(responseWithReplacedCounter, "\":{}", "\":[]");
			return responseWithReplacedEmptyCollection;
		}

		#endregion Methods
	}
}
