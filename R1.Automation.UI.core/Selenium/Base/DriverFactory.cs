using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using R1.Automation.UI.core.Commons;
using System;
using System.Collections.Generic;

namespace R1.Automation.UI.core.Selenium.Base
{
    public class DriverFactory
    {
        private readonly IDictionary<string, RemoteWebDriver> Drivers = new Dictionary<string, RemoteWebDriver>();
        private RemoteWebDriver driver;
        private int implicitWait, waitTime;
        private bool remoteFlag = false, headlessFlag = false, windows10 = true;
        private string platformName, platform, hubUrl;

        private readonly IConfigurationRoot config = CommonUtility.AppConfig;

        public RemoteWebDriver Driver
        {
            get
            {
                if (driver == null)
                    throw new NullReferenceException("The WebDriver browser instance was not initialized. You should first call the method InitDriver.");
                return driver;
            }
            private set
            {
                driver = value;
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(int.Parse(config["Connection:ImplicitWait"]));
            }
        }

        //Initialize driver based upon browserName being passed on as parameter by user
        public RemoteWebDriver InitDriver(string browserName)
        {
            try
            {
                if (config != null)
                {
                    headlessFlag = bool.Parse(config["Connection:HeadlessFlag"].Trim());
                    remoteFlag = bool.Parse(config["Connection:RemoteFlag"].Trim());
                    platformName = config["Connection:RemotePlatformName"].Trim();
                    platform = config["Connection:RemoteCapabilityPlatformName"].Trim();
                    waitTime = int.Parse(config["Connection:RemoteDriverWaitTime"].Trim());
                    hubUrl = config["Connection:HubUrl"].Trim();
                    windows10 = bool.Parse(config["Connection:Window10Flag"].Trim());
                }
            }
            catch (Exception e)
            {
                throw new NullReferenceException(e.Message + " Mandatory field values not defined in AppSettings");
            }
            if (driver == null)
            {
                if (browserName.ToUpper().Equals("CHROME") && !remoteFlag)
                    InitChrome();
                else if (browserName.ToUpper().Equals("CHROME") && remoteFlag)
                    InitChromeRemote();
                else if (browserName.ToUpper().Equals("IE11") && !remoteFlag && windows10)
                    InitIECapability();
                else if (browserName.ToUpper().Equals("IE11") && !remoteFlag && !windows10)
                    InitIE();
                else if (browserName.ToUpper().Equals("IE11") && remoteFlag)
                    InitIERemote();
            }

            if (driver != null)
                return driver;
            else
                throw new NullReferenceException("Driver could not be initialized");

        }

        //Initialize Chrome browser locally, no remote capabilities
        private void InitChrome()
        {
            driver = new ChromeDriver(CheckHeadless());
            Drivers.Add("Chrome", Driver);
        }

        //Initialize Chrome driver as remote with defined capabilities
        private void InitChromeRemote()
        {
            ChromeOptions option = CheckHeadless();
            option.PlatformName = platformName;
            option.AddAdditionalCapability("platform", platform, true);
            option.AddArgument("no-sandbox");
            driver = new RemoteWebDriver(new Uri(hubUrl), option.ToCapabilities(), TimeSpan.FromSeconds(waitTime));
            Drivers.Add("Chrome", Driver);
        }

        //Check for headless flag in settings and apply chrome options accordingly
        private ChromeOptions CheckHeadless()
        {
            ChromeOptions option = new ChromeOptions();
            if (headlessFlag)
                option.AddArgument("--headless");
            return option;
        }
        //Headless support not there for IE11 yet
        private void InitIERemote()
        {
            //T0-Do implement remote driver for Internet Explorer

        }

        //Initialize IE11 driver locally, no remote capabilities
        private void InitIE()
        {
            driver = new InternetExplorerDriver();
            Drivers.Add("IE", Driver);
        }

        //Initialize IE11 driver as remote with defined capabilities 
        private void InitIECapability()
        {
            string UrlToSet = config["Connection:URL"].Trim();
            InternetExplorerOptions options = new InternetExplorerOptions();
            options.InitialBrowserUrl = UrlToSet;
            driver = new InternetExplorerDriver(options);
            Drivers.Add("IE", Driver);
        }

        //Cleanup of all drivers, to be called in implementation code for cleanup post every test run
        public void CloseAllDrivers()
        {
            foreach (var key in Drivers.Keys)
            {
                Drivers[key].Close();
                Drivers[key].Quit();
            }
            DisposeDriverService.FinishHim();
        }
    }
}