using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RakletTest
{
    public static class SeleniumExtensions
    {
        /*Wait until the linkText appears and click on it */
        public static void click_text(this IWebDriver driver, string linkText)
        {
            try
            {
                WebDriverWait w = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                w.Until(ExpectedConditions.ElementExists(By.LinkText(linkText)));
                driver.FindElement(By.LinkText(linkText)).Click();
            }
            catch (NoSuchElementException)
            {
                throw new NoSuchElementException();
            }
            catch (TimeoutException)
            {
                throw new TimeoutException();
            }
        }

        /* Wait for the element to be clickable and click on it */
        public static void click_element(this IWebDriver driver, IWebElement element)
        {
            try
            {
                WebDriverWait w = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                w.Until(ExpectedConditions.ElementToBeClickable(element));
                element.Click();
            }
            catch (NoSuchElementException)
            {
                throw new NoSuchElementException();
            }
            catch (TimeoutException)
            {
                throw new TimeoutException();
            }
        }

        /*Go to url and wait for the className element to appear and return the element */
        public static IWebElement GoToUrl(this IWebDriver driver, string url, string className)
        {
            try
            {
                driver.Navigate().GoToUrl(@url);
                WebDriverWait w = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                w.Until(ExpectedConditions.ElementExists(By.ClassName(className)));
                return driver.FindElement(By.ClassName(className));
            }
            catch (NoSuchElementException)
            {
                throw new NoSuchElementException();
            }
            catch (TimeoutException)
            {
                throw new TimeoutException();
            }

        }
    }
}
