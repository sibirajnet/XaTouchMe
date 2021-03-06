﻿using System;
using System.Net;
using System.IO;
using System.Xml;

namespace testXaDroid
{
	public class ColorHandler
	{
		public static  ColorPattern FetchColor ()
		{
			// Create an HTTP web request using the URL:
			string url = "http://www.colourlovers.com/api/colors/random";
			ColorPattern ReturnColorPattern = new ColorPattern ();
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
			request.ContentType = "application/json";
			request.Method = "GET";
			try {
				// Send the request to the server and get the response:
				using (WebResponse response = request.GetResponse ()) {
					// Get a stream representation of the HTTP web response:
					using (Stream stream = response.GetResponseStream ()) {
						// Read the content.
						StreamReader reader = new StreamReader (stream);
						string responseFromServer = reader.ReadToEnd ();

						//Load XML
						XmlDocument doc = new XmlDocument ();
						doc.LoadXml (responseFromServer);
						XmlNodeList colorsList = doc.SelectNodes ("colors");

						//Get Color Elements
						foreach (XmlNode colors in colorsList) {
							XmlNodeList ColorNodeList = colors.SelectNodes ("color");
							foreach (XmlNode color in ColorNodeList) {
								XmlNodeList RGBNodeList = color.SelectNodes ("rgb");
								foreach (XmlNode RGB in RGBNodeList) {
									ReturnColorPattern.R = Convert.ToInt16 (RGB ["red"].InnerText);
									ReturnColorPattern.G = Convert.ToInt16 (RGB ["green"].InnerText);
									ReturnColorPattern.B = Convert.ToInt16 (RGB ["blue"].InnerText);
								}
							}
						}
					}
				}
				if (ReturnColorPattern.R == 0 && ReturnColorPattern.G == 0 && ReturnColorPattern.G == 0) {
					//No internet connection or invalid xml
					//Fill Random colors
					Random randonGen = new Random ();
					ReturnColorPattern.R = randonGen.Next (256);
					ReturnColorPattern.G = randonGen.Next (256);
					ReturnColorPattern.B = randonGen.Next (256);
				}
			} catch {
				    //Something went worng
				    //Fill Random colors
					Random randonGen = new Random ();
					ReturnColorPattern.R = randonGen.Next (256);
					ReturnColorPattern.G = randonGen.Next (256);
					ReturnColorPattern.B = randonGen.Next (256);

			}
			return ReturnColorPattern;
		}
	}
	public class ColorPattern
	{
		public int R { get; set; }
		public int G { get; set; }
		public int B { get; set; }
	}
}

