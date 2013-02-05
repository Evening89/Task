using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Task.Model
{
    public class GoodsEditTeaser_Model
    {
        public IWebDriver driver;

        #region Редактирование полей
            #region Required fields
            protected string FieldLink;
            public string Link
            {
                get { return FieldLink; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("sunriseField_url"));
                    webelement.Clear();
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
                    webelement.Clear();
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
                    IWebElement webelement = driver.FindElement(By.Id("sunriseField_id_category"));
                    SelectElement listCategory = new SelectElement(webelement);

                    #region Выпадающий список
                        switch (value)
                        {
                            case 1:
                                {
                                    listCategory.SelectByIndex(1);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 2:
                                {
                                    listCategory.SelectByIndex(2);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 3:
                                {
                                    listCategory.SelectByIndex(3);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 4:
                                {
                                    listCategory.SelectByIndex(4);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 5:
                                {
                                    listCategory.SelectByIndex(5);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 6:
                                {
                                    listCategory.SelectByIndex(6);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 7:
                                {
                                    listCategory.SelectByIndex(7);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 8:
                                {
                                    listCategory.SelectByIndex(8);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 9:
                                {
                                    listCategory.SelectByIndex(9);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 10:
                                {
                                    listCategory.SelectByIndex(10);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 11:
                                {
                                    listCategory.SelectByIndex(11);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 12:
                                {
                                    listCategory.SelectByIndex(12);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 13:
                                {
                                    listCategory.SelectByIndex(13);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 14:
                                {
                                    listCategory.SelectByIndex(14);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 15:
                                {
                                    listCategory.SelectByIndex(15);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 16:
                                {
                                    listCategory.SelectByIndex(16);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 17:
                                {
                                    listCategory.SelectByIndex(17);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 18:
                                {
                                    listCategory.SelectByIndex(18);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 19:
                                {
                                    listCategory.SelectByIndex(19);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 20:
                                {
                                    listCategory.SelectByIndex(20);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 21:
                                {
                                    listCategory.SelectByIndex(21);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 22:
                                {
                                    listCategory.SelectByIndex(22);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 23:
                                {
                                    listCategory.SelectByIndex(23);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 24:
                                {
                                    listCategory.SelectByIndex(24);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 25:
                                {
                                    listCategory.SelectByIndex(25);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 26:
                                {
                                    listCategory.SelectByIndex(26);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 27:
                                {
                                    listCategory.SelectByIndex(27);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 28:
                                {
                                    listCategory.SelectByIndex(28);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 29:
                                {
                                    listCategory.SelectByIndex(29);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                            case 30:
                                {
                                    listCategory.SelectByIndex(30);
                                    chosenCategory = listCategory.SelectedOption.Text;
                                    break;
                                }
                        }
                    #endregion
                }
            }

            protected string FieldAdvertText;
            public string AdvertText
            {
                get { return FieldAdvertText; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("sunriseField_advert_text"));
                    webelement.Clear();
                    webelement.SendKeys(value);
                    FieldAdvertText = value;
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
                    //webelement.Clear();
                    webelement.SendKeys(value);
                }
            }
            #endregion

            #region Unrequired fields
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
                        IWebElement webelement = driver.FindElement(By.Id("sunriseField_currencyid"));
                        SelectElement listCurrency = new SelectElement(webelement);
                        switch (value)
                        {
                            case 0:
                                {
                                    listCurrency.SelectByIndex(0);
                                    chosenCurrency = listCurrency.SelectedOption.Text;
                                    break;
                                }
                            case 1:
                                {
                                    listCurrency.SelectByIndex(1);
                                    chosenCurrency = listCurrency.SelectedOption.Text;
                                    break;
                                }
                            case 2:
                                {
                                    listCurrency.SelectByIndex(2);
                                    chosenCurrency = listCurrency.SelectedOption.Text;
                                    break;
                                }
                            case 3:
                                {
                                    listCurrency.SelectByIndex(3);
                                    chosenCurrency = listCurrency.SelectedOption.Text;
                                    break;
                                }
                            case 4:
                                {
                                    listCurrency.SelectByIndex(4);
                                    chosenCurrency = listCurrency.SelectedOption.Text;
                                    break;
                                }
                            case 5:
                                {
                                    listCurrency.SelectByIndex(5);
                                    chosenCurrency = listCurrency.SelectedOption.Text;
                                    break;
                                }
                        }
                    }
                }

                protected string FieldPriceForGoodsService;
                public string PriceForGoodsService
                {
                    get { return FieldPriceForGoodsService; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("sunriseField_price"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldPriceForGoodsService = value;
                    }
                }
            #endregion
        #endregion

        #region Узнать значения полей
            #region Get Required fields
                protected string GetFieldLink;
                public string GetLink
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("sunriseField_url"));
                        GetFieldLink = webelement.GetAttribute("value");
                        return GetFieldLink;
                    }
                    set
                    {
                        GetFieldLink = value;
                    }
                }

                protected string GetFieldTitle;
                public string GetTitle
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("sunriseField_title"));
                        GetFieldTitle = webelement.GetAttribute("value");
                        return GetFieldTitle;
                    }
                    set
                    {
                        GetFieldTitle = value;
                    }
                }

                protected string GetSelectCategory;
                public string GetCategory
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("sunriseField_id_category"));
                        SelectElement listCategory = new SelectElement(webelement);
                        GetSelectCategory = listCategory.SelectedOption.Text;
                        return GetSelectCategory;
                    }
                    set
                    {
                        GetSelectCategory = value;
                    }
                }

                protected string GetFieldAdvertText;
                public string GetAdvertText
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("sunriseField_advert_text"));
                        GetFieldAdvertText = webelement.GetAttribute("value");
                        return GetFieldAdvertText;
                    }
                    set
                    {
                        GetFieldAdvertText = value;
                    }
                }

                protected string GetFieldPriceForClick;
                public string GetPriceForClick
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("sunriseField_price_of_click"));
                        GetFieldPriceForClick = webelement.GetAttribute("value");
                        return GetFieldPriceForClick;
                    }
                    set
                    {
                        GetFieldPriceForClick = value;
                    }
                }

                protected string GetActionAttachFile; 
                public string GetAttachFile
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("imageLink"));
                        GetActionAttachFile = webelement.GetAttribute("value");
                        return GetActionAttachFile;
                    }
                    set
                    {
                        GetActionAttachFile = value;
                    }
                }
            #endregion

            #region Get Unrequired fields
                protected bool GetCheckboxTeaserWomen;
                public bool GetTeaserWomen
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("sunriseField_woman"));
                        try
                        {
                            string _checked = webelement.GetAttribute("checked");
                            return true; //есть атрибут checked--значит чекбокс выбран
                        }
                        catch (Exception) { return false; } //нет атрибута checked--значит чекбокс не выбран
                    }
                    set { GetCheckboxTeaserWomen = value; }
                }

                protected string GetSelectCurrency;
                public string GetCurrency
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("sunriseField_currencyid"));
                        SelectElement listCategory = new SelectElement(webelement);
                        GetSelectCurrency = listCategory.SelectedOption.Text;
                        return GetSelectCurrency;
                    }
                    set
                    {
                        GetSelectCurrency = value;
                    }
                }

                protected string GetFieldPriceForGoodsService;
                public string GetPriceForGoodsService
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("sunriseField_price"));
                        GetFieldPriceForGoodsService = webelement.GetAttribute("value");
                        return GetFieldPriceForGoodsService;
                    }
                    set
                    {
                        GetFieldPriceForGoodsService = value;
                    }
                }
            #endregion
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
    }
}
