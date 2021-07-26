using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RakletTest
{
    public class Driver
    {
        public ChromeOptions ChromeOptions = new ChromeOptions();
        public ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();

        public IWebDriver DriverOptions()
        {
#if DEBUG
            DebugOptions();
#else
            HeadlessDriver();
#endif
            IWebDriver driver = new ChromeDriver(chromeDriverService, ChromeOptions);
            return driver;
        }

        public void DebugOptions()
        {
            ChromeOptions.AddArgument("--incognito");
            ChromeOptions.AddArgument("--disable-gpu");
            ChromeOptions.AddArgument("--window-size=1920,1200");
        }

        public void HeadlessDriver()
        {
            DebugOptions();
            ChromeOptions.AddArgument("--headless");
        }
    }
}
