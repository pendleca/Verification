using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using OpenQA.Selenium.Remote;
using System.Diagnostics;
using System.Timers;

namespace Verification
{
    [TestClass]
    public class VerificationTest
    {
        public static IWebDriver WebDriver;
        string url = "http://cgross.github.io/angular-busy/demo/";

        //Initialize the Chrome Driver
        [TestInitialize]
        public void TestInitialize()
        {
            WebDriver = new ChromeDriver();
            WebDriver.Navigate().GoToUrl(url);
        }

        //Quit the driver
        [TestCleanup]
        public void TestCleanup()
        {
            WebDriver.Quit();
        }

        //Delay input test
        [TestMethod]
        public void TestOne()
        {
            //Initialize the expected value
            var expectedValue = 1000 + "";

            //Locate the delay textbox and fill in the expected value
            var delayTextBox = WebDriver.FindElement(By.Id("delayInput"));
            delayTextBox.Clear();
            delayTextBox.SendKeys(expectedValue);

            //Find the busy spinner
            var spinner = WebDriver.FindElement(By.ClassName("cg-busy-default-wrapper"));

            //Find the demo button
            var demoButton = WebDriver.FindElement(By.TagName("button"));

            //Initialize a stopwatch
            Stopwatch timer = new Stopwatch();

            //Click the demo button, and time the time it takes for the busy spinner to appear
            demoButton.Click();
            timer.Start();
            while (!spinner.Displayed);
            timer.Stop();
            
            //get the elapsed time and round it
            var time = timer.ElapsedMilliseconds;
            int roundedTime = ((int)Math.Round((double)time / 1000)) * 1000;

            //Check if values are equal
            Assert.AreEqual(Convert.ToInt32(expectedValue), roundedTime);
        }

        //Min duration test
        [TestMethod]
        public void TestTwo()
        {
            //Initialize expected value
            var expectedValue = 5000 + "";

            //Locate the duration text box and fill in the expected value
            var durationTextBox = WebDriver.FindElement(By.Id("durationInput"));
            durationTextBox.Clear();
            durationTextBox.SendKeys(expectedValue);

            //Find the busy spinner and demo button
            var spinner = WebDriver.FindElement(By.ClassName("cg-busy-default-wrapper"));
            var demoButton = WebDriver.FindElement(By.TagName("button"));

            //Initialize timer
            Stopwatch timer = new Stopwatch();

            //Click demo button and time the duration the busy spinner is displayed
            demoButton.Click();
            while(!spinner.Displayed);
            timer.Start();
            while(spinner.Displayed);
            timer.Stop();

            //Get the elapsed time and round
            var time = timer.ElapsedMilliseconds;
            int roundedTime = ((int)Math.Round((double)time / 1000)) * 1000;

            //Confirm that the time the busy spinner was displayed is greater than the minimum duration.
            Assert.IsTrue(Convert.ToInt32(expectedValue) <= roundedTime);
        }

        //Test in which the following occurs:
        // 1. Demo click
        // 2. Demo with "Waiting" message
        // 3. Set minimum duration to 1000ms. Demo click with "Waiting".
        [TestMethod]
        public void TestThree()
        {
            //Find demo button and click
            var demoButton = WebDriver.FindElement(By.TagName("button"));
            demoButton.Click();

            //Stop execution for viewing purposes
            Thread.Sleep(4000);

            //Initialize value for the message textbox
            var expectedValue = "Waiting.";

            //Locate and fill in the message text box with the expected value
            var messageTextBox = WebDriver.FindElement(By.Id("message"));
            messageTextBox.Clear();
            messageTextBox.SendKeys(expectedValue);

            //Click demo button

            demoButton.Click();
            //Stop execution for viewing purposes
            Thread.Sleep(4000);

            //set expected value for the duration textbox
            expectedValue = 1000 + "";

            //Find and fill in text box with expected value
            var mindurationTextBox = WebDriver.FindElement(By.Id("durationInput"));
            mindurationTextBox.Clear();
            mindurationTextBox.SendKeys(expectedValue);

            //Click demo button
            demoButton.Click();

            //Stop execution for viewing purposes
            Thread.Sleep(4000);
        }

        //Test in which standard is selected, then the custom template is selected
        [TestMethod]
        public void TestFour()
        {
            //Locate the template dropdown and select the standard option
            var templateDropDown = WebDriver.FindElement(By.Id("template"));
            SelectElement templateSelected = new SelectElement(templateDropDown);
            templateSelected.SelectByText("Standard");
            
            //Find and click the demo button
            var demoButton = WebDriver.FindElement(By.TagName("button"));
            demoButton.Click();

            //Stop execution for viewing purposes
            Thread.Sleep(4000);

            //Select custom template and click the demo button
            templateSelected.SelectByText("custom-template.html");
            demoButton.Click();

            //Stop execution for viewing purposes
            Thread.Sleep(4000);

        }

        //Selecting the custom template displays the dancing wizard test
        [TestMethod]
        public void TestFive()
        {
            //Locate the template dropdown and select the custom template option
            var templateDropDown = WebDriver.FindElement(By.Id("template"));
            SelectElement templateSelected = new SelectElement(templateDropDown);
            templateSelected.SelectByText("custom-template.html");
            
            //Find and click demo button
            var demoButton = WebDriver.FindElement(By.TagName("button"));
            demoButton.Click();

            //Find the wizard busy indicator
            var wizardBusyIndicator = WebDriver.FindElement(By.CssSelector("div[ng-show='$cgBusyIsActive()']"));

            //Check if the wizard busy indicator was found
            Assert.IsFalse(wizardBusyIndicator==null);
        }

        //Test to validate that the message textbox is the message that displays in the busy spinner
        [TestMethod]
        public void TestSix()
        {
            //Initialize expected value
            var expectedValue = "Waiting...";

            //Retrieve message text box and fill it with the expected value
            var messageTextBox = WebDriver.FindElement(By.Id("message"));
            messageTextBox.Clear();
            messageTextBox.SendKeys(expectedValue);
            
            //Find and click the demo button
            var demoButton = WebDriver.FindElement(By.TagName("button"));
            demoButton.Click();

            //Find the busy spinner and the message it is displaying
            var spinner = WebDriver.FindElement(By.ClassName("cg-busy-default-wrapper"));
            string spinnerMessage = spinner.Text;

            //Test if the values are equal
            Assert.AreEqual(expectedValue, spinnerMessage);

        }
    }
}
