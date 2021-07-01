using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RakletTest
{
    public static class SeleniumExtensions
    {
        /*Wait until the linkText appears and click on it */
        public static void ClickText(this IWebDriver driver, string linkText)
        {
            try
            {
                WebDriverWait w = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                w.Until(ExpectedConditions.ElementExists(By.LinkText(linkText)));
                driver.FindElement(By.LinkText(linkText)).Click();
                CheckForPopup(driver);
            }
            catch (NoSuchElementException)
            {
                Assert.Fail("NoSuchElementException");
            }
            catch (TimeoutException)
            {
                Assert.Fail("TimeoutException");
            }
        }

        /* Wait for the element to be clickable and click on it */
        public static void ClickElement(this IWebDriver driver, IWebElement element)
        {
            try
            {
                WebDriverWait w = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                w.Until(ExpectedConditions.ElementToBeClickable(element));
                element.Click();
                CheckForPopup(driver);
            }
            catch (NoSuchElementException)
            {
                Assert.Fail("NoSuchElementException");
            }
            catch (TimeoutException)
            {
                Assert.Fail("TimeoutException");
            }
        }

        /*Go to url and wait for the className element to appear and return the element */
        public static IWebElement GoToUrl(this IWebDriver driver, string url, string className)
        {
            IWebElement body = null;
            try
            {
                driver.Navigate().GoToUrl(@url);
                WebDriverWait w = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                w.Until(ExpectedConditions.ElementExists(By.ClassName(className)));
                body = driver.FindElement(By.ClassName(className));
            }
            catch (NoSuchElementException)
            {
                Assert.Fail("NoSuchElementException");
            }
            catch (TimeoutException)
            {
                Assert.Fail("TimeoutException");
            }
            return body;
        }

        public static void CheckForPopup(IWebDriver driver)
        {
            try
            {
                string xpath = "//*[@class=\"modal-body\"]//a";
                var e = driver.FindElement(By.XPath(xpath));
                if (e.Text.Length < 1) return;
                WebDriverWait w = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                w.Until(ExpectedConditions.ElementToBeClickable(e));
                e.Click();
            }
            catch (NoSuchElementException)
            {
                return;
            }
            catch (TimeoutException)
            {
                Assert.Fail("TimeoutException");
            }
        }

        public static IWebElement CheckSiteLoaded(this IWebDriver driver, string verifyClass, int time = 10)
        {
            IWebElement element = null;
            try
            {
                WebDriverWait w = new WebDriverWait(driver, TimeSpan.FromSeconds(time));
                w.Until(ExpectedConditions.ElementExists(By.ClassName(verifyClass)));
                element = driver.FindElement(By.ClassName(verifyClass));
            }
            catch (NoSuchElementException)
            {
                Assert.Fail("NoSuchElementException");
            }
            catch (TimeoutException)
            {
                Assert.Fail("TimeoutException");
            }
            return element;

        }

    }
}
