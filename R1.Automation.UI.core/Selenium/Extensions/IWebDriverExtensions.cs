using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace R1.Automation.UI.core.Selenium.Extensions
{
    public static class IWebDriverExtensions
    {
        public static object Configapp { get; private set; }

        /// <summary>Switches to new window based on Window Number</summary>
        /// <param name="windowNumber">The window number.</param>
        public static void SwitchToNewWindow(this RemoteWebDriver driver, int windowNumber)
        {
            var counter = 0;
            foreach (var handle in driver.WindowHandles)
            {
                if (counter == windowNumber)
                {
                    driver.SwitchTo().Window(handle);
                    break;
                }

                counter++;
            }
        }

        /// <summary>Switches to new window and get title.</summary>
        /// <param name="windowNumber">The window number.</param>
        /// <returns>Retruns Title of the new window</returns>
        public static string SwitchToNewWindowAndGetTitle(this RemoteWebDriver driver, int windowNumber)
        {
            driver.SwitchToNewWindow(windowNumber);
            return driver.Title;
        }

        /// <summary>Get number of iframes in a page.</summary>
        /// <returns>count of iframes as integer</returns>
        public static int NoOfFrameInPage(this RemoteWebDriver driver)
        {
            IList<IWebElement> element = driver.FindElements(By.TagName("iframe"));
            return element.Count;
        }

        /// <summary>This method is used for return open window's count.</summary>
        /// <returns>integer count of open windows</returns>
        public static int GetWindowCount(this RemoteWebDriver driver)
        {
            return driver.WindowHandles.Count;
        }

        /// <summary>This method is used for Click on OK Button of JS Alert.</summary>
        public static void AcceptAlert(this RemoteWebDriver driver)
        {
            IAlert alert = driver.SwitchTo().Alert();
            alert.Accept();
        }

        /// <summary>This method is used for Click on Cancel Button Of JS Alert./// </summary>

        public static void CancelAlert(this RemoteWebDriver driver)
        {
            IAlert alert = driver.SwitchTo().Alert();
            alert.Dismiss();
        }

        /// <summary>This method is used for Return text Of JS Alert./// </summary>
        /// <returns>Returns JS Alert text as String</returns>

        public static string GetAlertText(this RemoteWebDriver driver)
        {
            IAlert alert = driver.SwitchTo().Alert();
            return alert.Text;
        }

        /// <summary>This method is used for Enter Value in JS Alert/// </summary>
        /// <param name="value"></param>
        public static void EnterValueInAlert(this RemoteWebDriver driver, string value)
        {
            IAlert alert = driver.SwitchTo().Alert();
            alert.SendKeys(value);
        }

        /// <summary>This method is used for Click on Element using JS.</summary>
        /// <param name="element"></param>
        public static void ClickOnElement(this RemoteWebDriver driver, IWebElement element)
        {
            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
            executor.ExecuteScript("arguments[0].click();", element);
        }

        /// <summary>This method is used for Scroll Up the page.</summary>
        public static void PageScrollUp(this RemoteWebDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, 0)");
        }

        /// <summary>This method is used for Page Scroll Down.</summary>
        public static void PageScrollDown(this RemoteWebDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
        }

        /// <summary>This method is used to drag and drop Element.</summary>
        /// <param name="source"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        public static void DragDropToOffset(this RemoteWebDriver driver, IWebElement source, int offsetX, int offsetY)
        {
            Actions action = new Actions(driver);
            action.DragAndDropToOffset(source, offsetX, offsetY).Build().Perform();
        }


        /// <summary>Highlights the control by scrolling it in view.</summary>
        /// <param name="element">The element.</param>
        public static void ScrollInView(this RemoteWebDriver driver, IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        /// <summary>This method is used for Mouse hover to Element.</summary>
        /// <param name="element"></param>
        public static void MouseHover(this RemoteWebDriver driver, IWebElement element)
        {
            Actions action = new Actions(driver);
            action.MoveToElement(element).Build().Perform();
        }

        /// <summary>This method is used for Wait until element Visible with timeout.</summary>
        /// <param name="time">Timeout.</param>
        /// <param name="locator">The locator.</param>
        public static void WaitForVisibility(this RemoteWebDriver driver, int time, By locator)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(time));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }

        /// <summary>This method is used for Wait till element Click-able with timeout.</summary>
        /// <param name="time">Timeout.</param>
        /// <param name="element">The element.</param>
        public static void WaitForClickable(this RemoteWebDriver driver, int time, IWebElement element)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(time));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element));
        }

        /// <summary>This method is used for Wait till Text Present In Element with timeout.</summary>
        /// <param name="time">Timeout.</param>
        /// <param name="element">The element.</param>
        /// <param name="text">Text to wait for.</param>
        public static void WaitForTextPresentInElement(this RemoteWebDriver driver, int time, IWebElement element, string text)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(time));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElement(element, text));
        }

        /// <summary>Explicit Wait Logic for invisibility of element located</summary>
        /// <param name="elementPath"></param>
        /// <param name="driver"></param>
        public static void WaitForInvisibility(this RemoteWebDriver driver, string elementPath, int explicitWaitTime)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(explicitWaitTime));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath(elementPath)));
        }
    }
}