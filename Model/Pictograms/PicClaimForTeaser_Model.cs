using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Task.Model.Pictograms
{
    public class PicClaimForTeaserModel
    {
        public IWebDriver driver;
        public string LocatorSites = "#sites > option";
        public string LocatorPriority = "#priority > option";

        #region Required fields
            protected string FieldLoginManager;
            public string LoginManager
            {
                get { return FieldLoginManager; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("curator"));
                    webelement.SendKeys(value);
                    FieldLoginManager = value;
                }
            }

            protected string FieldCampaign;
            public string Campaign
            {
                get { return FieldCampaign; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("g_partner_id"));
                    webelement.SendKeys(value);
                    FieldCampaign = value;
                }
            }

            public string ChosenSite = null;
            public int QuantityofSites = 0;
            protected int SelectSite;
            public int Site
            {
                get { return SelectSite; }
                set
                {
                    SelectSite = value;
                    IWebElement select = driver.FindElement(By.Id("sites"));
                    SelectElement listSites = new SelectElement(select);
                    listSites.SelectByIndex(SelectSite);
                    ChosenSite = listSites.SelectedOption.Text;
                }
            }

            protected string FieldNumberOfTeasers;
            public string NumberOfTeasers
            {
                get { return FieldNumberOfTeasers; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("amount"));
                    webelement.SendKeys(value);
                    FieldNumberOfTeasers = value;
                }
            }

            public string ChosenPriority = null;
            public int QuantityofPriorities = 0;
            protected int SelectPriority;
            public int Priority
            {
                get { return SelectPriority; }
                set
                {
                    SelectPriority = value;
                    IWebElement select = driver.FindElement(By.Id("priority"));
                    SelectElement listPriorities = new SelectElement(select);
                    listPriorities.SelectByIndex(SelectPriority);
                    ChosenPriority = listPriorities.SelectedOption.Text;
                }
            }

            public string chosenActionWhenSaveClaim = null;
            protected int RadioActionWhenSaveClaim;
            public int ActionWhenSaveClaim
            {
                get { return RadioActionWhenSaveClaim; }
                set
                {
                    RadioActionWhenSaveClaim = value;
                    switch (value)
                    {
                        case 0:
                            {
                                IWebElement sendOnExecution = driver.FindElement(By.Id("whatNeedToDo-1"));
                                chosenActionWhenSaveClaim = "Отправить на выполнение";
                                sendOnExecution.Click();
                                break;
                            }
                        case 1:
                            {
                                IWebElement assignTaskToEditors = driver.FindElement(By.Id("whatNeedToDo-2"));
                                chosenActionWhenSaveClaim = "Назначить задачу редакторам";
                                assignTaskToEditors.Click();
                                break;
                            }
                        case 2:
                            {
                                IWebElement sendOnModeration = driver.FindElement(By.Id("whatNeedToDo-3"));
                                chosenActionWhenSaveClaim = "Отправить на модерацию";
                                sendOnModeration.Click();
                                break;
                            }
                    }
                }
            }

            #region Всплывающее окно "Назначить задачу"
                protected bool ClickTestCreativist;
                public bool TestCreativist
                {
                    get { return ClickTestCreativist; }
                    set
                    {
                        ClickTestCreativist = value;
                        IWebElement webelement = driver.FindElement(By.CssSelector("li[title='Тестовый креативщик'] > a"));
                        webelement.Click();
                    }
                }

                protected string FieldNumberOfTeasersPopUpWindow;
                public string NumberOfTeasersPopUpWindow
                {
                    get { return FieldNumberOfTeasersPopUpWindow; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("giveTask_TisersCount"));
                        webelement.SendKeys(value);
                        FieldNumberOfTeasersPopUpWindow = value;
                    }
                }

                protected bool ClickButtonApply;
                public bool ButtonApply
                {
                    get { return ClickButtonApply; }
                    set
                    {
                        ClickButtonApply = value;
                        IWebElement webelement = driver.FindElement(By.Id("giveTask_OkButton"));
                        webelement.Click();
                    }
                }
            #endregion
        #endregion

        #region Unrequired fields
            protected string FieldTerms;
            public string Terms
            {
                get { return FieldTerms; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("conditions"));
                    webelement.SendKeys(value);
                    FieldTerms = value;
                }
            }

            protected bool CheckboxAutoclaim;
            public bool Autoclaim
            {
                get { return CheckboxAutoclaim; }
                set
                {
                    CheckboxAutoclaim = value;
                    IWebElement webelement = driver.FindElement(By.Id("auto_creative"));
                    webelement.Click();
                }
            }

            protected string FieldPricePerClick;
            public string PricePerClick
            {
                get { return FieldPricePerClick; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("all"));
                    webelement.SendKeys(value);
                    FieldPricePerClick = value;
                }
            }

            protected bool CheckboxOnlyForEditorsWagers;
            public bool OnlyForEditorsWagers
            {
                get { return CheckboxOnlyForEditorsWagers; }
                set
                {
                    CheckboxOnlyForEditorsWagers = value;
                    IWebElement webelement = driver.FindElement(By.Id("only_editor_stuffer"));
                    webelement.Click();
                }
            }
        #endregion

        public void Submit()
        {
            try
            {
                IWebElement webelement = driver.FindElement(By.Id("submit"));
                webelement.Click();
            }
            catch (Exception)
            { }
        }

        public int QuantityItemsInList(string findByCssSelector)
        {
            List<IWebElement> listOfTags = driver.FindElements(By.CssSelector(findByCssSelector)).ToList();
            return listOfTags.Count;
        }

        public List<string> GetErrors()
        {
            List<IWebElement> list = driver.FindElements(By.CssSelector(".errors > li")).ToList();//проверка, есть ли на странице ошибки заполнения полей
            List<string> result = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                result.Add(list[i].Text);
            }
            return result;
        }
    }
}
