using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace R1.Automation.UI.core.Selenium.Extensions
{
    public static class IWebElementExtensions
    {
        /// <summary>Selects the random value from dropdown.</summary>
        /// <returns>Return selected value as string for reference later</returns>

        public static string SelectRandomValuefromDropdown(this IWebElement webElement)
        {
            if (webElement == null)
                throw new NullReferenceException("Null Element passed");
            SelectElement element = new SelectElement(webElement);
            if (element.Options.Count == 0) return null; //if there are no elements in dropdown, return null
            Random random = new Random();
            element.SelectByIndex(random.Next(1, element.Options.Count));
            return element.SelectedOption.Text.ToString().Trim();
        }

        /// <summary>Gets the dropdown value list.</summary>
        /// <returns>Returns list of values in a dropdown</returns>
        /// <exception cref="NullReferenceException">Null Element passed</exception>
        public static List<string> GetDropdownValueList(this IWebElement webElement)
        {
            if (webElement == null)
                throw new NullReferenceException("Null Element passed");
            SelectElement element = new SelectElement(webElement);
            if (element.Options.Count == 0) return null; //if there are no elements in dropdown, return null
            List<string> valueList = new List<string>();
            foreach (var option in element.Options)
                valueList.Add(option.Text.Trim());
            return valueList;
        }

        /// <summary>Gets the dropdown list total count.</summary>
        /// <returns>List count as integer</returns>
        /// <exception cref="NullReferenceException">Null Element passed</exception>
        public static int GetDropdownListTotalCount(this IWebElement webElement)
        {
            if (webElement == null)
                throw new NullReferenceException("Null Element passed");
            else
            {
                SelectElement element = new SelectElement(webElement);
                return element.Options.Count;
            }
        }

        /// <summary>Clicks the drop down value by containing text.</summary>
        /// <param name="text">Text for which dropvalue to be searched.</param>
        /// <returns>returns bool value for success or failure of the method</returns>
        /// <exception cref="NullReferenceException">Null Element passed or Null or Empty String passed</exception>
        public static bool ClickDropDownValuebyContainingText(this IWebElement webElement, string text)
        {
            if (webElement == null)
                throw new NullReferenceException("Null Element passed");
            var itemText = text.Trim();
            if (string.IsNullOrEmpty(text))
                throw new NullReferenceException("Null or Empty String passed");
            bool counter = false;
            SelectElement element = new SelectElement(webElement);
            foreach (var option in element.Options)
            {
                if (option.Text.Trim().Contains(itemText))
                {
                    option.Click();
                    counter = true;
                    break;
                }
            }
            return counter;//if value is found, it will return true otherwise false
        }

        /// <summary>Gets the value by Index for a dropdown</summary>
        /// <param name="index">The index.</param>
        /// <returns>returns value as string</returns>
        public static string GetValuedropdownByIndex(this IWebElement webElement, int index)
        {
            if (webElement == null)
                throw new NullReferenceException("Null Element passed");
            SelectElement element = new SelectElement(webElement);
            element.SelectByIndex(index);
            string value = element.SelectedOption.Text.ToString().Trim();
            return value;
        }

        /// <summary>This method is used to verify whether the element is displayed.</summary>
        /// <param name="webElement">Element to be verified</param>
        /// <returns>true/false</returns>
        public static bool VerifyElementDisplay(this IWebElement webElement)
        {
            try
            {
                return webElement.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>Gets the color of the cssvalues like background.</summary>
        /// <param name="webElement">WebElement.</param>
        /// <param name="cssValue">The CSS value whose color is to be fetched.</param>
        /// <returns>Color in Hex format</returns>
        public static string GetColor(this IWebElement webElement, string cssValue)
        {
            var value = webElement.GetCssValue(cssValue);
            string[] colours = value.Replace("rgba(", "").Replace(")", "").Split(",");
            int r = int.Parse(colours[0]);
            int g = int.Parse(colours[1]);
            int b = int.Parse(colours[2]);
            int a = int.Parse(colours[3]);
            Color myColor = Color.FromArgb(a, r, g, b);
            string hex = "#" + myColor.R.ToString("X")[0] + myColor.G.ToString("X")[0] + myColor.B.ToString("X")[0];
            string hexlower = hex.ToLower();
            return hexlower;
        }
        /// <summary>Selects the filter value having equal value.</summary>
        /// <param name="webElement">The web element.</param>
        /// <param name="text">The text.</param>
        /// <exception cref="NullReferenceException">Null Element passed or Null or Empty String passed</exception>
        public static void SelectFilterValueHavingEqualValue(this IWebElement webElement, string text)
        {
            if (webElement == null)
                throw new NullReferenceException("Null Element passed");
            if (string.IsNullOrEmpty(text))
                throw new NullReferenceException("Null or Empty String passed");
            SelectElement element = new SelectElement(webElement);
            foreach (var option in element.Options)
            {
                if (option.Text.Trim().Equals(text))
                {
                    option.Click();
                }
            }
        }
    }
} 