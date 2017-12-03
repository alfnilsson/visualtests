using System;
using System.Drawing;
using Applitools.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;

namespace EpiCloseUp.Tests
{
    [TestClass]
    public class VisualTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestAlloy()
        {
            using (IWebDriver driver = new PhantomJSDriver())
            {
                driver.Manage().Window.Size = new Size(1280, 980);
                var eyes = new Eyes
                {
                    ApiKey = "--APIKEY--"
                };

                try
                {
                    driver.Url = "http://epicloseup2017.azurewebsites.net/";
                    eyes.CheckWindow("Start Page");

                    driver.Url = "http://epicloseup2017.azurewebsites.net/en/about-us/contact-us/";
                    eyes.CheckWindow("Contact us - Forms");

                    driver.FindElement(FormPage.SubmitButton).Click();
                    eyes.CheckWindow("Contact us - Missing fields");

                    driver.FindElement(FormPage.NameField).SendKeys("Alf Nilsson");
                    driver.FindElement(FormPage.EmailField).SendKeys("alf.nilsson");
                    driver.FindElement(FormPage.SubmitButton).Click();
                    eyes.CheckWindow("Contact us - Invalid e-mail");

                    driver.FindElement(FormPage.EmailField).SendKeys("@netrelations.com");
                    driver.FindElement(FormPage.AlloyMeetCheckbox).Click();
                    driver.FindElement(FormPage.PricingCheckbox).Click();
                    driver.FindElement(FormPage.MessageField).SendKeys("Oh sweet mama!");

                    eyes.CheckWindow("Contact us - Form entered");

                    driver.FindElement(FormPage.SubmitButton).Click();
                    eyes.CheckWindow("Contact us - Confirmation");
                    eyes.Close();
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.Message);
                }
                finally
                {
                    eyes.AbortIfNotClosed();
                }
            }
        }
    }

    public static class FormPage
    {
        public static By SubmitButton { get; } = By.XPath("//input[@name='XFormsSubmit_run68XpSP4HHFwPPbtDwSoLowJFfcqSLO4vdDKV0Q']");
        public static By NameField { get; } = By.XPath("//input[@name='Name']");
        public static By EmailField { get; } = By.XPath("//input[@name='Email']");
        public static By AlloyMeetCheckbox { get; } = By.XPath("//input[@name='AreaofInterestAlloy Meet']");
        public static By PricingCheckbox { get; } = By.XPath("//input[@name='AreaofInterestPricing']");
        public static By MessageField { get; } = By.XPath("//textarea[@name='Message']");
    }
}
