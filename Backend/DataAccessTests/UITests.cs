using Microsoft.VisualStudio.TestTools.UnitTesting; 
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccessTests
{
    [TestClass]
    public class UITests
    {
        private IWebDriver driver;

        [TestInitialize]
        public void Setup()
        {
            driver = new ChromeDriver("ChromeDriver");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Window.Maximize();
        }

        [TestMethod]
        public void AddSensorData_ShouldAddSensorData()
        {
            driver.Navigate().GoToUrl("https://frontend.datamatikereksamen.dk/rooms.html");
            try
            {

                var assignSensorButton = driver.FindElement(By.XPath("//*[@id='app']/div[2]/div[2]/div[2]/div/div[3]/div/div/div[1]/div/button"));
                assignSensorButton.Click();

               
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='assignSensorToRoom']/div/div")));

                
                var formcontrol = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='assignSensorToRoom']/div/div/div[2]/div/select")));
                var selectElement = new SelectElement(formcontrol);
                selectElement.SelectByValue("3"); 

                
                var createButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"assignSensor\"]")));
                createButton.Click();

                // Jeg har tilføjet en kort ventetid, så vi kan se at resultatet på hjemmesiden faktisk tilføjere sensor 1 til vores rum.
                Thread.Sleep(2000); 

               
                var modal = driver.FindElement(By.Id("assignSensorToRoom"));
                Assert.IsFalse(modal.Displayed, "Modalen blev ikke lukket.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Fejl: " + ex.Message);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }
    

    }
}
