using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Valloon.Selenium
{
    class Robot : IDisposable
    {
        public static void Print(String text = null, ConsoleColor? color = null)
        {
            if (text == null)
            {
                Console.WriteLine();
            }
            else if (color == null)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.ForegroundColor = (ConsoleColor)color;
                Console.WriteLine(text);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static Boolean IsDeveloper
        {
            get
            {
                String username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                if (username.StartsWith(@"DESKTOP-2KTBPSE\")) return true;
                return false;
            }
        }

        public static String GetNumbers(String input)
        {
            return new String(input.Where(c => char.IsDigit(c)).ToArray());
        }

        public static String GetNowDateTimeString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss");
        }

        public static Func<IWebDriver, bool> UrlContains(string fraction)
        {
            return (driver) => { return driver.Url.ToLowerInvariant().Contains(fraction.ToLowerInvariant()); };
        }

        public static Func<IWebDriver, bool> UrlNotContains(string fraction)
        {
            return (driver) => { return !driver.Url.ToLowerInvariant().Contains(fraction.ToLowerInvariant()); };
        }

        private const int DEFAULT_TIMEOUT_PAGELOAD = 180;
        private const String SUFFIX_AUTO = "    (AUTO)";
        public const String LOG_FILENAME = "log.txt";
        private RandomGenerator Random = new RandomGenerator();
        private IWebDriver FirefoxDriver;
        private WebDriverWait Wait;
        private IJavaScriptExecutor JSE;

        public Robot()
        {
            Init();
        }

        private void Init()
        {
            FirefoxOptions options = new FirefoxOptions();
            var driverService = FirefoxDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            //options.AddArgument("--start-maximized");
            //options.AddArgument("--auth-server-whitelist");
            //options.AddArguments("--disable-extensions");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--ignore-ssl-errors");
            options.AddArgument("--system-developer-mode");
            options.AddArgument("--no-first-run");
            options.SetLoggingPreference(LogType.Driver, LogLevel.All);
            //options.AddAdditionalCapability("useAutomationExtension", false);
            //chromeOptions.AddArguments("--disk-cache-size=0");
            //options.AddArgument("--user-data-dir=" + m_chr_user_data_dir);
#if !DEBUG
            options.AddArguments("--headless");
            options.AddArguments("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36");
            options.AddArguments("--disable-plugins-discovery");
            //options.AddArguments("--profile-directory=Default");
            //options.AddArguments("--no-sandbox");
            //options.AddArguments("--incognito");
            //options.AddArguments("--disable-gpu");
            //options.AddArguments("--no-first-run");
            //options.AddArguments("--ignore-certificate-errors");
            //options.AddArguments("--start-maximized");
            //options.AddArguments("disable-infobars");

            //options.AddAdditionalCapability("acceptInsecureCerts", true, true);
#endif
            FirefoxDriver = new FirefoxDriver(driverService, options, TimeSpan.FromSeconds(DEFAULT_TIMEOUT_PAGELOAD));
            FirefoxDriver.Manage().Window.Position = new Point(0, 0);
            FirefoxDriver.Manage().Window.Size = new Size(1200, 900);
            FirefoxDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            JSE = (IJavaScriptExecutor)FirefoxDriver;
            Wait = new WebDriverWait(FirefoxDriver, TimeSpan.FromSeconds(180));
            Wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            Wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
        }

        public void StartPublishingThread(String rootDirectoryPath, String dataFilename, String email, String password)
        {
            Thread thread = new Thread(() => StartPublishing(rootDirectoryPath, dataFilename, email, password));
            thread.Start();
        }

        public void StartPublishing(String rootDirectoryPath, String dataFilename, String email, String password)
        {
            Print($"Root directory selected : \"{rootDirectoryPath}\"\r\n");
            Print($"Reading data from \"{dataFilename}\"\r\n");
            Car[] cars = Car.ReadData(dataFilename);
            int carCount = cars.Length;
            int carCountSuccess = 0, carCountFailed = 0;
        line_login:
            {
                FirefoxDriver.Navigate().GoToUrl("https://auth.leboncoin.fr");
                Print($"Login with {email} ...    [{GetNowDateTimeString()}]", ConsoleColor.Green);
                {
                    var el = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("[data-qa-id='profilarea-login']")));
                    el.Click();
                }
                var inputEmail = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("email")));
                inputEmail.SendKeys(email);
                var inputPassword = FirefoxDriver.FindElement(By.Id("password"));
                inputPassword.SendKeys(password);
                var by = By.CssSelector("button[type=submit]");
                Wait.Until(d => d.FindElement(by).GetAttribute("disabled") == null);
                var submit = FirefoxDriver.FindElement(by);
                //JSE.ExecuteScript("arguments[0].click();", submit);
                submit.Click();
                Wait.Until(UrlContains("/oauth2callback"));
                Wait.Until(UrlNotContains("/oauth2callback"));
                Thread.Sleep(3000);
                Print();
            }
            for (int carIndex = 0; carIndex < carCount; carIndex++)
            {
                try
                {
                    Car car = cars[carIndex];
                    String directoryName = car.DirectoryName;
                    Print($"( {carIndex + 1} / {carCount} )    {directoryName}    [{GetNowDateTimeString()}]", ConsoleColor.Green);
                    String directoryPath = Path.Combine(rootDirectoryPath, directoryName);
                    DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                    if (!directoryInfo.Exists)
                    {
                        Print($"Directory not found : {directoryPath}", ConsoleColor.Red);
                        continue;
                    }
                    FirefoxDriver.Navigate().GoToUrl("https://www.leboncoin.fr/deposer-une-annonce/");
                    try
                    {
                        var el = FirefoxDriver.FindElement(By.CssSelector("[data-qa-id='newad-categorylist_cat_1']"));
                        el.Click();
                    }
                    catch
                    {
                        Print($"Login problem. Try again...", ConsoleColor.Red);
                        goto line_login;
                    }
                    {
                        var el = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("[data-qa-id='newad-categorylist_cat_2']")));
                        el.Click();
                    }
                    {
                        var el = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("[data-qa-id='newad-button-next-categories']")));
                        el.Click();
                    }
                    {
                        var el = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("brand")));
                        var selectElement = new SelectElement(el);
                        selectElement.SelectByText(car.Brand);
                        Print($"\t\tBrand = {car.Brand}");
                    }
                    {
                        var el = FirefoxDriver.FindElement(By.Name("model"));
                        var selectElement = new SelectElement(el);
                        selectElement.SelectByText(car.Model);
                        Print($"\t\tModel = {car.Model}");
                    }
                    {
                        var el = FirefoxDriver.FindElement(By.Name("regdate"));
                        var selectElement = new SelectElement(el);
                        selectElement.SelectByText(car.Year);
                        Print($"\t\tYear = {car.Year}");
                    }
                    {
                        var el = FirefoxDriver.FindElement(By.Name("mileage"));
                        el.SendKeys(car.Mileage);
                        Print($"\t\tMileage = {car.Mileage}");
                    }
                    {
                        var el = FirefoxDriver.FindElement(By.CssSelector("[data-qa-id='newad-input_fuel']"));
                        var array = el.FindElements(By.TagName("div"));
                        foreach (var div in array)
                        {
                            var text = div.Text;
                            if (String.Equals(text, car.Fuel, StringComparison.OrdinalIgnoreCase))
                            {
                                div.Click();
                                Print($"\t\tFuel = {car.Fuel}");
                                break;
                            }
                        }
                    }
                    {
                        var el = FirefoxDriver.FindElement(By.CssSelector("[data-qa-id='newad-input_gearbox']"));
                        var array = el.FindElements(By.ClassName("_2tk-4"));
                        if (car.Gearbox == "1")
                        {
                            array[1].Click();
                            Print($"\t\tGearbox = Automatic");
                        }
                        else
                        {
                            array[0].Click();
                            Print($"\t\tGearbox = Manual");
                        }
                    }
                    {
                        var by = By.CssSelector("[data-qa-id='newad-button-next-ad_params']");
                        Wait.Until(d => d.FindElement(by).GetAttribute("disabled") == null);
                        var el = FirefoxDriver.FindElement(by);
                        JSE.ExecuteScript("arguments[0].click();", el);
                        //el.Click();
                    }
                    {
                        var el = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("subject")));
                        el.SendKeys(car.Subject);
                        Print($"\t\tSubject = {car.Subject}");
                    }
                    {
                        var el = FirefoxDriver.FindElement(By.Name("body"));
                        el.SendKeys(car.Description);
                        Print($"\t\tDescription = (...)");
                    }
                    {
                        var by = By.CssSelector("[data-qa-id='newad-button-next-description']");
                        Wait.Until(d => d.FindElement(by).GetAttribute("disabled") == null);
                        var el = FirefoxDriver.FindElement(by);
                        el.Click();
                    }
                    {
                        var el = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("price")));
                        el.SendKeys(car.Price);
                        Print($"\t\tPrice = {car.Price}");
                    }
                    {
                        var by = By.CssSelector("[data-qa-id='newad-button-next-price']");
                        Wait.Until(d => d.FindElement(by).GetAttribute("disabled") == null);
                        var el = FirefoxDriver.FindElement(by);
                        el.Click();
                    }
                    {
                        var el = FirefoxDriver.FindElement(By.CssSelector("input[type=file]"));
                        String[] filenames = car.Pictures.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        String fullValue = "";
                        int pictureCount = 0;
                        foreach (var filename in filenames)
                        {
                            String jpgFilename = filename.Trim();
                            if (!jpgFilename.EndsWith(".jpg")) jpgFilename += ".jpg";
                            String fullName = Path.Combine(directoryPath, jpgFilename);
                            FileInfo fileInfo = new FileInfo(fullName);
                            if (fileInfo.Exists)
                            {
                                fullValue += fileInfo.FullName + " \n ";
                                pictureCount++;
                            }
                            else
                            {
                                Print($"\t\t\tPicture not found : {fullName}", ConsoleColor.Red);
                            }
                        }
                        el.SendKeys(fullValue.Trim());
                        Print($"\t\tPictures = {car.Pictures}    ({pictureCount} pictures added.)");
                    }
                    {
                        var by = By.CssSelector("[data-qa-id='newad-button-next-pictures']");
                        Wait.Until(d => d.FindElement(by).GetAttribute("disabled") == null);
                        var el = FirefoxDriver.FindElement(by);
                        el.Click();
                    }
                    {
                        var el = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("address")));
                        el.SendKeys(car.Address);
                        el.SendKeys(Keys.Return);
                        Print($"\t\tAddress = {car.Address}");
                    }
                    {
                        var by = By.CssSelector("[data-qa-id='newad-button-next-undefined']");
                        Wait.Until(d => d.FindElement(by).GetAttribute("disabled") == null);
                        var el = FirefoxDriver.FindElement(by);
                        el.Click();
                    }
                    {
                        var el = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name("phone")));
                        el.SendKeys(car.Tel);
                        Print($"\t\tTel = {car.Tel}");
                    }
                    {
                        var el = FirefoxDriver.FindElement(By.CssSelector("[data-qa-id='newad-input_phone_hidden']"));
                        el.Click();
                    }
                    {
                        var by = By.CssSelector("[data-qa-id='newad-button-next-contact']");
                        Wait.Until(d => d.FindElement(by).GetAttribute("disabled") == null);
                        var el = FirefoxDriver.FindElement(by);
                        el.Click();
                    }
                }
                catch (Exception ex)
                {
                    Print(ex.ToString(), ConsoleColor.Red);
                    carCountFailed++;
                }
                Print();
            }
            Print($"\r\nAll completed : {carCount} cars\r\n{carCountSuccess} succeed, {carCountFailed} failed.\r\n[{GetNowDateTimeString()}]\r\n\r\n\r\n", ConsoleColor.Green);
        }

        private void NavigatePageWithTimeout(String url, int timeoutSeconds = 5)
        {
            var timeout = FirefoxDriver.Manage().Timeouts();
            timeout.PageLoad = TimeSpan.FromSeconds(timeoutSeconds);
            timeout.AsynchronousJavaScript = TimeSpan.FromSeconds(timeoutSeconds);
            try
            {
                FirefoxDriver.Navigate().GoToUrl(url);
            }
            catch { }
            timeout.PageLoad = TimeSpan.FromSeconds(DEFAULT_TIMEOUT_PAGELOAD);
            timeout.AsynchronousJavaScript = TimeSpan.FromSeconds(DEFAULT_TIMEOUT_PAGELOAD);
        }

        private void ClearCache()
        {
            FirefoxDriver.Close();
            Init();
        }

        private IWebElement expandRootElement(IWebElement element)
        {
            return (IWebElement)((IJavaScriptExecutor)FirefoxDriver).ExecuteScript("return arguments[0].shadowRoot", element);
        }

        private String GetStringFromCssSelector(String cssSelector, String defaultValue = null)
        {
            try
            {
                IWebElement e = FirefoxDriver.FindElement(By.CssSelector(cssSelector));
                return e.Text;
            }
            catch { return defaultValue; }
        }

        private Decimal? GetDecimalFromCssSelector(String cssSelector, Decimal? defaultValue = null)
        {
            try
            {
                IWebElement e = FirefoxDriver.FindElement(By.CssSelector(cssSelector));
                return Convert.ToDecimal(GetNumbers(GetStringFromCssSelector(cssSelector)));
            }
            catch { return defaultValue; }
        }

        private static String JoinForCSV(IEnumerable<String> values)
        {
            List<String> list = new List<string>();
            foreach (String s in values)
            {
                list.Add("\"" + s + "\"");
            }
            return String.Join(",", list);
        }

        public void Dispose()
        {
            FirefoxDriver.Close();
            FirefoxDriver.Quit();
        }
    }
}
