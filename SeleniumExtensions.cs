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
        /*Wait until the link/Text appears and click on it */
        public static void ClickText(this IWebDriver driver, string linkText)
        {
            try
            {
                Wait(driver, 20, "ElementExists", By.LinkText(linkText), null);
                driver.CheckElementExist(By.LinkText(linkText)).Click();
                CheckForPopup(driver);
            }
            catch (NoSuchElementException e)
            {
                Assert.Fail("NoSuchElementException" + e.Message + e.StackTrace);
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException e)
            {
                Assert.Fail("TimeoutException" + e.Message + e.StackTrace);
            }
        }

        /* Wait for the element to be clickable and click on it */
        public static void ClickElement(this IWebDriver driver, IWebElement element)
        {
            try
            {
                Wait(driver, 20, "ElementToBeClickable", null, element);
                element.Click();
                CheckForPopup(driver);
            }
            catch (NoSuchElementException e)
            {
                Assert.Fail("NoSuchElementException" + e.Message + e.StackTrace);
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException e)
            {
                Assert.Fail("TimeoutException" + e.Message + e.StackTrace);
            }
        }

        /*Go to url and wait for the className element to appear and return the element */
        public static IWebElement GoToUrl(this IWebDriver driver, string url, string className)
        {
            IWebElement body = null;
            try
            {
                driver.Navigate().GoToUrl(url);
                Wait(driver, 20, "ElementExists", By.ClassName(className), null);
                body = driver.CheckElementExist(By.ClassName(className));
            }
            catch (NoSuchElementException e)
            {
                Assert.Fail("NoSuchElementException" + e.Message + e.StackTrace);
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException e)
            {
                Assert.Fail("TimeoutException" + e.Message + e.StackTrace);
            }
            return body;
        }

        public static void CheckForPopup(IWebDriver driver)
        {
            try
            {
                string xpath = "//*[@class=\"modal-body\"]//a";
                var e = driver.CheckElementExist(By.XPath(xpath), null, true);
                if (e == null || e.Text.Length < 1) return;
                Wait(driver, 10, "ElementToBeClickable", null, e);
                e.Click();
            }
            catch (NoSuchElementException)
            {
                return;
            }
            catch (TimeoutException e)
            {
                Assert.Fail("TimeoutException" + e.Message + e.StackTrace);
            }
        }

        public static IWebElement CheckSiteLoaded(this IWebDriver driver, string verifyClass, int time = 10, bool timeOut = true)
        {
            IWebElement element = null;
            try
            {
                Wait(driver, time, "ElementExists", By.ClassName(verifyClass), null);
                element = driver.CheckElementExist(By.ClassName(verifyClass));
            }
            catch (NoSuchElementException e)
            {
                if (timeOut)
                {
                    Assert.Fail("NoSuchElementException" + e.Message + e.StackTrace);
                }
                else
                {
                    return element;
                }
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException e)
            {
                if (timeOut)
                {
                    Assert.Fail("TimeoutException - Site is not loaded - element not found: " + verifyClass + e.Message + e.StackTrace);
                }
                else
                {
                    return element;
                }
            }
            return element;
        }

        public static IWebElement CheckElementExist(this IWebDriver driver, By by, IWebElement body = null, bool popup = false)
        {
            IWebElement element = null;
            try
            {
                if (body == null)
                {
                    element = driver.FindElement(by);
                }
                else
                {
                    element = body.FindElement(by);
                }
            }
            catch (NoSuchElementException e)
            {
                if (popup) return element;
                Assert.Fail("Element not loaded" + e.Message + e.StackTrace);
            }
            return element;
        }

        public static IList<IWebElement> CheckElementsExist(this IWebDriver driver, By by, IWebElement body = null)
        {
            IList<IWebElement> element = null;
            try
            {
                if (body == null)
                {
                    element = driver.FindElements(by);
                }
                else
                {
                    element = body.FindElements(by);
                }
            }
            catch (NoSuchElementException e)
            {
                Assert.Fail("Element not loaded" + e.Message + e.StackTrace);
            }
            return element;
        }

        public static void Wait(this IWebDriver driver, int time, string method, By by, IWebElement element)
        {
            try
            {
                WebDriverWait w = new WebDriverWait(driver, TimeSpan.FromSeconds(time));
                if (method.Equals("ElementExists"))
                {
                    w.Until(ExpectedConditions.ElementExists(by));
                }
                else if (method == "ElementToBeClickable")
                    w.Until(ExpectedConditions.ElementToBeClickable(element));
                else Assert.Fail("Wait is not defined");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
