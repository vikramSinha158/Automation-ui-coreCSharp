using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.Net;
using System.Net.Sockets;
using System.Text;
using OpenQA.Selenium.Remote;

namespace R1.Automation.UI.core.Commons
{
    public class CommonUtility
    {
        private static readonly Random _random = new Random();
        public static IConfigurationRoot GetConfig(string path)
        {
            var config = new ConfigurationBuilder()
                 .AddJsonFile(path)
                 .Build();
            return config;
        }
        public static IConfigurationRoot AppConfig
        {
            get
            {
                return GetConfig("appsettings.json");
            }
        }

        public static IConfigurationRoot TestData(string path)
        {
            return GetConfig(path);
        }

        public string GetFolderPath(string appFolderName)
        {
            var folderName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return Path.Combine(folderName.Substring(0, folderName.LastIndexOf("\\bin")), appFolderName + "\\");
        }

        public string LoadJson(string testDataPath)
        {
            using (StreamReader r = new StreamReader(testDataPath))
            {
                var json = r.ReadToEnd();
                return JObject.Parse(json).ToString();

            }
        }

        public string GetTestData(String key)
        {
            JObject obs = JObject.Parse(LoadJson(GetFolderPath(AppConfig["Connection:TestDataFolderName"]) + AppConfig["Connection:TestDataFileName"]));
            return obs[key].ToString();
        }

        public string GetQueryData(String key)
        {
            JObject obs = JObject.Parse(LoadJson(GetFolderPath(AppConfig["Connection:TestQueryFolderName"]) + AppConfig["Connection:TestQueryFileName"]));
            return obs[key].ToString();
        }

        public string LoadJsonReplace(string testDataPath, Dictionary<string, string> dict)
        {
            string jasonString = null;
            using (StreamReader r = new StreamReader(testDataPath))
            {
                var json = r.ReadToEnd();
                jasonString = JObject.Parse(json).ToString();

            }

            foreach (KeyValuePair<string, string> ele in dict)
            {
                jasonString = jasonString.Replace(ele.Key, ele.Value);
            }

            return jasonString;
        }

        public string TakeScreenshot(RemoteWebDriver driver, string path)
        {
            int min = 1000;
            int max = 99999999;
            Random rdm = new Random();
            string pathScreen = path + "\\" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_tt") + "_" + rdm.Next(min, max) + ".png";
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(pathScreen, ScreenshotImageFormat.Png);
            return pathScreen;
        }

        public static string CreateFolder(String path)
        {
            string folderName = "ScreenShotsFor_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_tt");
            if (!Directory.Exists(path + folderName))
            {
                Directory.CreateDirectory(path + folderName);
            }

            return path + folderName;


        }

        public static string DeleteOldFolders(string appFolderName, string noOfDays)
        {
            int num = Int32.Parse(noOfDays);

            var folderName = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(folderName.Substring(0, folderName.LastIndexOf("\\bin")), appFolderName + "\\");

            string[] subdirectoryEntries = Directory.GetDirectories(path);
            foreach (string subdirectory in subdirectoryEntries)
            {
                DirectoryInfo d = new DirectoryInfo(subdirectory);
                if (d.CreationTime < DateTime.Now.AddDays(-num))
                    d.Delete(true);
            }

            return path;
        }

        /// <summary>This method is used getIP.</summary>
        /// <returns>string</returns>
        public static string GetIP()
        {
            string myIP = string.Empty;
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    myIP = ip.ToString();
                }
            }
            return myIP;
        }

        /// <summary>This method is used for return current date.</summary>
        /// <param name="format"></param>
        /// <returns></returns>

        public static string GetCurrentDate(string format)
        {
            return DateTime.Now.ToString(format);
        }

        /// This method is used for Generate a random number between two numbers.

        public static int GetRandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        /// This method is used for return random string with requested size in lower or upper case
        public static string RandomString(int size, bool lowerCase)
        {
            if (size == 0) return null;
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}