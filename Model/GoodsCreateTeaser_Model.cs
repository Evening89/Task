using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Task.Model
{
    public class GoodsCreateTeaser_Model
    {
        public IWebDriver driver;
        public string locatorCategory = "#sunriseField_id_category > option";
        public string locatorCurrency = "#sunriseField_currencyid > option";

        #region Required fields
            protected string FieldLink;
            public string Link
            {
                get { return FieldLink; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("sunriseField_url"));
                    webelement.SendKeys(value);
                    FieldLink = value;
                }
            }

            protected string FieldTitle;
            public string Title
            {
                get { return FieldTitle; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("sunriseField_title"));
                    webelement.SendKeys(value);
                    FieldTitle = value;
                }
            }

            public string chosenCategory = null;
            protected int SelectCategory;
            public int Category
            {
                get { return SelectCategory; }
                set
                {
                    SelectCategory = value;
                    IWebElement select = driver.FindElement(By.Id("sunriseField_id_category"));
                    SelectElement selectCategory = new SelectElement(select);
                    selectCategory.SelectByIndex(SelectCategory);
                    chosenCategory = selectCategory.SelectedOption.Text;
                }
            }

            protected string FieldAdvertText;
            public string AdvertText
            {
                get { return FieldAdvertText; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("sunriseField_advert_text"));
                    webelement.SendKeys(value);
                    FieldAdvertText = value;
                }
            }

            protected string FieldPriceForClick;
            public string PriceForClick
            {
                get { return FieldPriceForClick; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("sunriseField_price_of_click"));
                    webelement.SendKeys(value);
                    FieldPriceForClick = value;
                }
            }

            protected string ActionAttachFile; //???????
            public string AttachFile
            {
                get { return ActionAttachFile; }
                set
                {
                    ActionAttachFile = value;
                    IWebElement webelement = driver.FindElement(By.Id("imageFile"));
                    webelement.SendKeys(value);
                }
            }
        #endregion

        #region Unrequired fields
            protected bool ClickScrewBeginsAt;
            public bool ScrewBeginsAt
            {
                get { return ClickScrewBeginsAt; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("when_start"));
                    webelement.Click();
                    ClickScrewBeginsAt = value;
                }
            }

            protected string FieldDate;
            public string Date
            {
                get { return FieldDate; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("auto-date_start"));
                    webelement.SendKeys(value);
                    FieldDate = value;
                }
            }

            protected string FieldTime;
            public string Time
            {
                get { return FieldTime; }
                set
                {
                    //IWebElement webelement1 = driver.FindElement(By.Id("auto_div"));
                    //webelement1.Click();
                    IWebElement webelement2 = driver.FindElement(By.Id("auto-time_start"));
                    //webelement2.Click();
                    //Thread.Sleep(3000);
                    webelement2.SendKeys(value);
                    FieldTime = value;
                }
            }

            protected bool ClickButtonApply;
            public bool ButtonApply
            {
                get { return ClickButtonApply; }
                set
                {
                    List<IWebElement> listButtons = driver.FindElements(By.CssSelector("div[class = 'ui-dialog-buttonset'] > button")).ToList();
                    for (int i = 0; i < listButtons.Count; i++)
                    {
                        if(listButtons[i].Text == "Применить")
                        {
                            IWebElement webElement = listButtons[i];
                            ClickButtonApply = value;
                            webElement.Click();
                            break;
                        }
                    }
                }
            }

            protected bool CheckboxTeaserWomen;
            public bool TeaserWomen
            {
                get { return CheckboxTeaserWomen; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("sunriseField_woman"));
                    webelement.Click();
                    CheckboxTeaserWomen = value;
                }
            }

            public string chosenCurrency = null;
            protected int SelectCurrency;
            public int Currency
            {
                get { return SelectCurrency; }
                set
                {
                    SelectCurrency = value;
                    IWebElement select = driver.FindElement(By.Id("sunriseField_currencyid"));
                    SelectElement selectCurrency = new SelectElement(select);
                    selectCurrency.SelectByIndex(SelectCurrency);
                    chosenCurrency = selectCurrency.SelectedOption.Text;
                }
            }

            protected string FieldPriceForGoodsService;
            public string PriceForGoodsService
            {
                get { return FieldPriceForGoodsService; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("sunriseField_price"));
                    webelement.SendKeys(value);
                    FieldPriceForGoodsService = value;
                }
            }
        #endregion

        public List<string> GetErrors()
        {
            List<IWebElement> list = driver.FindElements(By.CssSelector(".error > li")).ToList(); //проверка, есть ли на странице ошибки заполнения _обязательных_ полей
            List<string> result = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                result.Add(list[i].Text);
            }
            return result;
        }

        public void Submit()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5)); //явное ожидание
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("document.getElementById('sendButton').style.display='block';"); //делаем блок кнопок "Сохранить" и "Сохранить в черновик" видимым
            //Thread.Sleep(5000);
            IWebElement save = wait.Until(ExpectedConditions.ElementExists(By.Id("save")));
            save = driver.FindElement(By.Id("save")); //клик по "Сохранить"
            //Thread.Sleep(5000);
            save.Click();
        }

        public int QuantityItemsInList(string findByCss)
        {
            List<IWebElement> listOfTags = driver.FindElements(By.CssSelector(findByCss)).ToList();
            return listOfTags.Count;
        }
    }
}
