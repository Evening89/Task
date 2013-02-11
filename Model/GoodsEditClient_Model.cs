using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Task.Model
{
    public class GoodsEditClient_Model
    {
        public IWebDriver driver;

        public string locatorExtCurator = "optgroup[label='Внешние кураторы'] > option"; //локатор поиска выпадающего списка "Внешний" (новый куратор)
        public string locatorIntCurator = "#curators-inner_curator optgroup[label='Внутренние кураторы'] > option";  //локатор поиска выпадающего списка "Внутренний" (новый куратор)
        public string locatorAttractor = "#curators-inviter optgroup[label='Внутренние кураторы'] > option";  //локатор поиска выпадающего списка "Привлекатель"

        public string locatorStartSection = "#start_section option";

        #region Редактирование полей

            #region Контакты по направлению

                protected string FieldContactsByDirectionEmail;
                public string ContactsByDirectionEmail
                {
                    get { return FieldContactsByDirectionEmail; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("email"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldContactsByDirectionEmail = value;
                    }
                }

                protected string FieldContactsByDirectionEmailAdditional;
                public string ContactsByDirectionEmailAdditional
                {
                    get { return FieldContactsByDirectionEmailAdditional; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("additional_email"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldContactsByDirectionEmailAdditional = value;
                    }
                }

                protected string FieldContactsByDirectionPhone;
                public string ContactsByDirectionPhone
                {
                    get { return FieldContactsByDirectionPhone; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("phoneTextArea"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldContactsByDirectionPhone = value;
                    }
                }
                protected string FieldContactsByDirectionSkype;
                public string ContactsByDirectionSkype
                {
                    get { return FieldContactsByDirectionSkype; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("skype"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldContactsByDirectionSkype = value;
                    }
                }
                protected string FieldContactsByDirectionIcq;
                public string ContactsByDirectionIcq
                {
                    get { return FieldContactsByDirectionIcq; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("icq"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldContactsByDirectionIcq = value;
                    }
                }
                protected string FieldContactsByDirectionName;
                public string ContactsByDirectionName
                {
                    get { return FieldContactsByDirectionName; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("name"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldContactsByDirectionName = value;
                    }
                }
            #endregion
       
            #region Основные контакты

                protected string FieldContactsCommonEmail;
                public string ContactsCommonEmail
                {
                    get { return FieldContactsCommonEmail; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("email_2"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldContactsCommonEmail = value;
                    }
                }

                protected string FieldContactsCommonEmailAdditional;
                public string ContactsCommonEmailAdditional
                {
                    get { return FieldContactsCommonEmailAdditional; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("additional_email_2"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldContactsCommonEmailAdditional = value;
                    }
                }

                protected string FieldContactsCommonPhone;
                public string ContactsCommonPhone
                {
                    get { return FieldContactsCommonPhone; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("phone_2"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldContactsCommonPhone = value;
                    }
                }

                protected string FieldContactsCommonSkype;
                public string ContactsCommonSkype
                {
                    get { return FieldContactsCommonSkype; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("skype_2"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldContactsCommonSkype = value;
                    }
                }

                protected string FieldContactsCommonIcq;
                public string ContactsCommonIcq
                {
                    get { return FieldContactsCommonIcq; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("icq_2"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldContactsCommonIcq = value;
                    }
                }

                protected string FieldContactsCommonName;
                public string ContactsCommonName
                {
                    get { return FieldContactsCommonName; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("name_2"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldContactsCommonName = value;
                    }
                }

                protected string FieldContactsCommonAlias;
                public string ContactsCommonAlias
                {
                    get { return FieldContactsCommonAlias; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("creative_pseudonym"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldContactsCommonAlias = value;
                    }
                }

                public string chosenStartSectionProfile = null;
                public int quantityofStartSectionProfile = 0;
                protected int SelectStartSectionProfile;
                public int StartSectionProfile
                {
                    get { return SelectStartSectionProfile; }
                    set
                    {
                        SelectStartSectionProfile = value;
                        IWebElement select = driver.FindElement(By.Id("start_section"));
                        SelectElement selectStartSection = new SelectElement(select);
                        selectStartSection.SelectByIndex(SelectStartSectionProfile);
                        chosenStartSectionProfile = selectStartSection.SelectedOption.Text;
                    }
                }
            #endregion

            #region Платёжные реквизиты

                public string chosenKindOfPayment = null;
                protected bool SelectKindOfPayment;
                public bool KindOfPayment
                {
                    get { return SelectKindOfPayment; }
                    set
                    {
                        SelectKindOfPayment = value;
                        IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                        js.ExecuteScript("document.getElementById('currencyData').style.display='block';");
                        IWebElement webelement = driver.FindElement(By.Id("currencyData"));
                        SelectElement listKindOfPayment = new SelectElement(webelement);
                        listKindOfPayment.SelectByIndex(1);
                        chosenKindOfPayment = listKindOfPayment.SelectedOption.Text;
                    }
                }

                protected string FieldRequisites;
                public string Requisites
                {
                    get { return FieldRequisites; }
                    set
                    {
                        FieldRequisites = value;
                        IWebElement webelement = driver.FindElement(By.Id("purse"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                    }
                }

                protected bool ClickButtonAdd;
                public bool ButtonAdd
                {
                    get { return ClickButtonAdd; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("addPurse"));
                        ClickButtonAdd = value;
                        webelement.Click();
                    }
                }
            #endregion

            #region Управление паролем

                protected bool FieldPassword;
                public bool Password
                {
                    get { return FieldPassword; }
                    set
                    {
                        FieldPassword = value;
                        IWebElement webelement = driver.FindElement(By.Id("sendmailicon"));
                        webelement.Click();
                    }
                }

                public string chosenSendPassword = null;
                protected int RadioSendPassword;
                public int SendPassword
                {
                    get { return RadioSendPassword; }
                    set
                    {
                        RadioSendPassword = value;
                        switch (value)
                        {
                            case 0:
                                {
                                    IWebElement commonEmail = driver.FindElement(By.Id("where_send-sendmainmailradio"));
                                    chosenSendPassword = "Отправить на основной e-mail";
                                    commonEmail.Click();
                                    break;
                                }
                            case 1:
                                {
                                    IWebElement additionalEmail = driver.FindElement(By.Id("where_send-sendaddlmailradio"));
                                    chosenSendPassword = "Отправить на дополнительный e-mail";
                                    additionalEmail.Click();
                                    break;
                                }
                            case 2:
                                {
                                    IWebElement allEmails = driver.FindElement(By.Id("where_send-sendallmailradio"));
                                    chosenSendPassword = "Отправить на все e-mail'ы";
                                    allEmails.Click();
                                    break;
                                }
                        }
                    }
                }
            #endregion

            #region Взаимодействие

                public string chosenNewCuratorExternal = null;
                public int quantityofNewCuratorExternal = 0;
                protected int SelectNewCuratorExternal;
                public int NewCuratorExternal
                {
                    get { return SelectNewCuratorExternal; }
                    set
                    {
                        SelectNewCuratorExternal = value;
                        IWebElement select = driver.FindElement(By.Id("curators-outer_curator"));
                        SelectElement selectExtCurator = new SelectElement(select);
                        selectExtCurator.SelectByIndex(SelectNewCuratorExternal);
                        chosenNewCuratorExternal = selectExtCurator.SelectedOption.Text;
                    }
                }

                public string chosenNewCuratorInternal = null;
                public int quantityofNewCuratorInternal = 0;
                protected int SelectNewCuratorInternal;
                public int NewCuratorInternal
                {
                    get { return SelectNewCuratorInternal; }
                    set
                    {
                        SelectNewCuratorInternal = value;
                        IWebElement select = driver.FindElement(By.Id("curators-inner_curator"));
                        SelectElement selectIntCurator = new SelectElement(select);
                        selectIntCurator.SelectByIndex(SelectNewCuratorInternal);
                        chosenNewCuratorInternal = selectIntCurator.SelectedOption.Text;
                    }
                }

                public string chosenAttractorClient = null;
                public int quantityofAttractorClient = 0;
                protected int SelectAttractorClient;
                public int AttractorClient
                {
                    get { return SelectAttractorClient; }
                    set
                    {
                        SelectAttractorClient = value;
                        IWebElement select = driver.FindElement(By.Id("curators-inviter"));
                        SelectElement selectAttractor = new SelectElement(select);
                        selectAttractor.SelectByIndex(SelectAttractorClient);
                        chosenAttractorClient = selectAttractor.SelectedOption.Text;
                    }
                }
            #endregion

            #region Настройки

                protected bool CheckboxClientIsActive;
                public bool ClientIsActive
                {
                    get { return CheckboxClientIsActive; }
                    set
                    {
                        CheckboxClientIsActive = value;
                        IWebElement webelement = driver.FindElement(By.Id("status"));
                        webelement.Click();
                    }
                }

                protected bool CheckboxExchangeInProfile;
                public bool ExchangeInProfile
                {
                    get { return CheckboxExchangeInProfile; }
                    set
                    {
                        CheckboxExchangeInProfile = value;
                        IWebElement webelement = driver.FindElement(By.Id("hide_exchange_flag"));
                        webelement.Click();
                    }
                }

                protected bool CheckboxNewsInProfile;
                public bool NewsInProfile
                {
                    get { return CheckboxNewsInProfile; }
                    set
                    {
                        CheckboxNewsInProfile = value;
                        IWebElement webelement = driver.FindElement(By.Id("hide_news_flag"));
                        webelement.Click();
                    }
                }
                protected bool CheckboxTestClient;
                public bool TestClient
                {
                    get { return CheckboxTestClient; }
                    set
                    {
                        CheckboxTestClient = value;
                        IWebElement webelement = driver.FindElement(By.Id("is_test"));
                        webelement.Click();
                    }
                }

                protected string FieldLimitPkQuantity;
                public string LimitPkQuantity
                {
                    get { return FieldLimitPkQuantity; }
                    set
                    {
                        FieldLimitPkQuantity = value;
                        IWebElement webelement = driver.FindElement(By.Id("goodhits_campaigns_limit"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                    }
                }

                protected bool CheckboxAllowViewFilterByPlatforms;
                public bool AllowViewFilterByPlatforms
                {
                    get { return CheckboxAllowViewFilterByPlatforms; }
                    set
                    {
                        CheckboxAllowViewFilterByPlatforms = value;
                        IWebElement webelement = driver.FindElement(By.Id("goodhits_platform_flag"));
                        webelement.Click();
                    }
                }

                protected string FieldLimitTeasersQuantityPerDay;
                public string LimitTeasersQuantityPerDay
                {
                    get { return FieldLimitTeasersQuantityPerDay; }
                    set
                    {
                        FieldLimitTeasersQuantityPerDay = value;
                        IWebElement webelement = driver.FindElement(By.Id("goods_per_partner_limit"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                    }
                }

                protected bool CheckboxSwitchOffTabAdvBlocks;
                public bool SwitchOffTabAdvBlocks
                {
                    get { return CheckboxSwitchOffTabAdvBlocks; }
                    set
                    {
                        CheckboxSwitchOffTabAdvBlocks = value;
                        IWebElement webelement = driver.FindElement(By.Id("can_see_ghits"));
                        webelement.Click();
                    }
                }

                protected bool CheckboxReflectionStatisticsSpending;
                public bool ReflectionStatisticsSpending
                {
                    get { return CheckboxReflectionStatisticsSpending; }
                    set
                    {
                        CheckboxReflectionStatisticsSpending = value;
                        IWebElement webelement = driver.FindElement(By.Id("show_summ_expenses"));
                        webelement.Click();
                    }
                }

                protected string FieldComments;
                public string Comments
                {
                    get { return FieldComments; }
                    set
                    {
                        FieldComments = value;
                        IWebElement webelement = driver.FindElement(By.Id("comments"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                    }
                }
            #endregion
        #endregion

        #region Узнать значения полей

            #region Узнать Контакты по направлению
                protected string GetFieldContactsByDirectionEmail;
                public string GetContactsByDirectionEmail
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("email"));
                        GetFieldContactsByDirectionEmail = webelement.GetAttribute("value");
                        return GetFieldContactsByDirectionEmail;
                    }
                    set
                    {
                        GetFieldContactsByDirectionEmail = value;
                    }
                }

                protected string GetFieldContactsByDirectionEmailAdditional;
                public string GetContactsByDirectionEmailAdditional
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("additional_email"));
                        GetFieldContactsByDirectionEmailAdditional = webelement.GetAttribute("value");
                        return GetFieldContactsByDirectionEmailAdditional;
                    }
                    set
                    {
                        GetFieldContactsByDirectionEmailAdditional = value;
                    }
                }

                protected string GetFieldContactsByDirectionPhone;
                public string GetContactsByDirectionPhone
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("phoneTextArea"));
                        GetFieldContactsByDirectionPhone = webelement.GetAttribute("value");
                        return GetFieldContactsByDirectionPhone;
                    }
                    set
                    {
                        GetFieldContactsByDirectionPhone = value;
                    }
                }
                protected string GetFieldContactsByDirectionSkype;
                public string GetContactsByDirectionSkype
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("skype"));
                        GetFieldContactsByDirectionSkype = webelement.GetAttribute("value");
                        return GetFieldContactsByDirectionSkype;
                    }
                    set
                    {
                        GetFieldContactsByDirectionSkype = value;
                    }
                }
                protected string GetFieldContactsByDirectionIcq;
                public string GetContactsByDirectionIcq
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("icq"));
                        GetFieldContactsByDirectionIcq = webelement.GetAttribute("value");
                        return GetFieldContactsByDirectionIcq;
                    }
                    set
                    {
                        GetFieldContactsByDirectionIcq = value;
                    }
                }
                protected string GetFieldContactsByDirectionName;
                public string GetContactsByDirectionName
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("name"));
                        GetFieldContactsByDirectionName = webelement.GetAttribute("value");
                        return GetFieldContactsByDirectionName;
                    }
                    set
                    {
                        GetFieldContactsByDirectionName = value;
                    }
                }
            #endregion

            #region Узнать Основные контакты
                protected string GetFieldContactsCommonEmail;
                public string GetContactsCommonEmail
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("email_2"));
                        GetFieldContactsCommonEmail = webelement.GetAttribute("value");
                        return GetFieldContactsCommonEmail;
                    }
                    set
                    {
                        GetFieldContactsCommonEmail = value;
                    }
                }

                protected string GetFieldContactsCommonEmailAdditional;
                public string GetContactsCommonEmailAdditional
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("additional_email_2"));
                        GetFieldContactsCommonEmailAdditional = webelement.GetAttribute("value");
                        return GetFieldContactsCommonEmailAdditional;
                    }
                    set
                    {
                        GetFieldContactsCommonEmailAdditional = value;
                    }
                }

                protected string GetFieldContactsCommonPhone;
                public string GetContactsCommonPhone
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("phone_2"));
                        GetFieldContactsCommonPhone = webelement.GetAttribute("value");
                        return GetFieldContactsCommonPhone;
                    }
                    set
                    {
                        GetFieldContactsCommonPhone = value;
                    }
                }

                protected string GetFieldContactsCommonSkype;
                public string GetContactsCommonSkype
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("skype_2"));
                        GetFieldContactsCommonSkype = webelement.GetAttribute("value");
                        return GetFieldContactsCommonSkype;
                    }
                    set
                    {
                        GetFieldContactsCommonSkype = value;
                    }
                }

                protected string GetFieldContactsCommonIcq;
                public string GetContactsCommonIcq
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("icq_2"));
                        GetFieldContactsCommonIcq = webelement.GetAttribute("value");
                        return GetFieldContactsCommonIcq;
                    }
                    set
                    {
                        GetFieldContactsCommonIcq = value;
                    }
                }

                protected string GetFieldContactsCommonName;
                public string GetContactsCommonName
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("name_2"));
                        GetFieldContactsCommonName = webelement.GetAttribute("value");
                        return GetFieldContactsCommonName;
                    }
                    set
                    {
                        GetFieldContactsCommonName = value;
                    }
                }

                protected string GetFieldContactsCommonAlias;
                public string GetContactsCommonAlias
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("creative_pseudonym"));
                        GetFieldContactsCommonAlias = webelement.GetAttribute("value");
                        return GetFieldContactsCommonAlias;
                    }
                    set
                    {
                        GetFieldContactsCommonAlias = value;
                    }
                }

                protected string GetSelectStartSectionProfile;
                public string GetStartSectionProfile
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("start_section"));
                        SelectElement listStartSections = new SelectElement(webelement);
                        GetSelectStartSectionProfile = listStartSections.SelectedOption.Text;
                        return GetSelectStartSectionProfile;
                    }
                    set
                    {
                        GetSelectStartSectionProfile = value;
                    }
                }
            #endregion

            #region Узнать Платёжные реквизиты
                protected string GetTableRequisites;
                public string GetRequisites
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.CssSelector(".list_purses_block tbody"));
                        return webelement.Text;
                    }
                    set { GetTableRequisites = value; }
                }
            #endregion

            #region Узнать Взаимодействие
                
                protected bool GetLabelCurrentCurator;
                public bool GetCurrentCurator
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.CssSelector("#curators-curatorlabel + .hint"));
                        if (webelement.Text == "") return false; //текущий куратор пуст
                        else return true; //текущий куратор непуст
                    }
                    set { GetLabelCurrentCurator = value; }
                }

                protected string GetSelectAttractorClient;
                public string GetAttractorClient
                {
                    get 
                    {
                        IWebElement select = driver.FindElement(By.Id("curators-inviter"));
                        SelectElement listAttractors = new SelectElement(select);
                        GetSelectAttractorClient = listAttractors.SelectedOption.Text;
                        return GetSelectAttractorClient;
                    }
                    set
                    {
                        GetSelectAttractorClient = value;
                    }
                }
            #endregion

            #region Узнать Настройки

                protected bool GetCheckboxClientIsActive;
                public bool GetClientIsActive
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("status"));
                        try
                        {
                            string _checked = webelement.GetAttribute("checked");
                            return true; //есть атрибут checked--значит чекбокс выбран
                        }
                        catch (Exception) { return false; } //нет атрибута checked--значит чекбокс не выбран
                    }
                    set { GetCheckboxClientIsActive = value; }
                }

                protected bool GetCheckboxExchangeInProfile;
                public bool GetExchangeInProfile
                {
                    get 
                    {
                        IWebElement webelement = driver.FindElement(By.Id("hide_exchange_flag"));
                        try
                        {
                            string _checked = webelement.GetAttribute("checked");
                            return true; //есть атрибут checked--значит чекбокс выбран
                        }
                        catch (Exception) { return false; } //нет атрибута checked--значит чекбокс не выбран
                    }
                    set { GetCheckboxExchangeInProfile = value; }
                }

                protected bool GetCheckboxNewsInProfile;
                public bool GetNewsInProfile
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("hide_news_flag"));
                        try
                        {
                            string _checked = webelement.GetAttribute("checked");
                            return true; //есть атрибут checked--значит чекбокс выбран
                        }
                        catch (Exception) { return false; } //нет атрибута checked--значит чекбокс не выбран
                    }
                    set { GetCheckboxNewsInProfile = value; }
                }
                protected bool GetCheckboxTestClient;
                public bool GetTestClient
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("is_test"));
                        try
                        {
                            string _checked = webelement.GetAttribute("checked");
                            return true; //есть атрибут checked--значит чекбокс выбран
                        }
                        catch (Exception) { return false; } //нет атрибута checked--значит чекбокс не выбран
                    }
                    set { GetCheckboxTestClient = value; }
                }

                protected string GetFieldLimitPkQuantity;
                public string GetLimitPkQuantity
                {
                    get 
                    { 
                        IWebElement webelement = driver.FindElement(By.Id("goodhits_campaigns_limit"));
                        return webelement.GetAttribute("value");
                    }
                    set { GetFieldLimitPkQuantity = value; }
                }

                protected bool GetCheckboxAllowViewFilterByPlatforms;
                public bool GetAllowViewFilterByPlatforms
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("goodhits_platform_flag"));
                        try
                        {
                            string _checked = webelement.GetAttribute("checked");
                            return true; //есть атрибут checked--значит чекбокс выбран
                        }
                        catch (Exception) { return false; } //нет атрибута checked--значит чекбокс не выбран
                    }
                    set { GetCheckboxAllowViewFilterByPlatforms = value; }
                }

                protected string GetFieldLimitTeasersQuantityPerDay;
                public string GetLimitTeasersQuantityPerDay
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("goods_per_partner_limit"));
                        return webelement.GetAttribute("value");
                    }
                    set { GetFieldLimitTeasersQuantityPerDay = value; }
                }

                protected bool GetCheckboxSwitchOffTabAdvBlocks;
                public bool GetSwitchOffTabAdvBlocks
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("can_see_ghits"));
                        try
                        {
                            string _checked = webelement.GetAttribute("checked");
                            return true; //есть атрибут checked--значит чекбокс выбран
                        }
                        catch (Exception) { return false; } //нет атрибута checked--значит чекбокс не выбран
                    }
                    set { GetCheckboxSwitchOffTabAdvBlocks = value; }
                }

                protected bool GetCheckboxReflectionStatisticsSpending;
                public bool GetReflectionStatisticsSpending
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("show_summ_expenses"));
                        try
                        {
                            string _checked = webelement.GetAttribute("checked");
                            return true; //есть атрибут checked--значит чекбокс выбран
                        }
                        catch (Exception) { return false; } //нет атрибута checked--значит чекбокс не выбран
                    }
                    set { GetCheckboxReflectionStatisticsSpending = value; }
                }

                protected string GetFieldComments;
                public string GetComments
                {
                    get
                    {
                        IWebElement webelement = driver.FindElement(By.Id("comments"));
                        return webelement.Text;
                    }
                    set { GetFieldComments = value; }
                }
            #endregion

        #endregion

        public void Submit()
        {
            try
            {
                IWebElement webelement = driver.FindElement(By.Id("submit"));
                webelement.Click();
            }
            catch (Exception)
            {}
        }

        public int quantityItemsInList(string findByCss)
        {
            List<IWebElement> listOfTags = driver.FindElements(By.CssSelector(findByCss)).ToList();
            return listOfTags.Count;
        }

        public List<string> GetErrors()
        {
            List<IWebElement> list = driver.FindElements(By.Id("error_image")).ToList(); //проверка, есть ли на странице ошибки заполнения _обязательных_ полей
            List<string> result = new List<string>();
            if (list.Count != 0) result.Add("               Ошибка(-и) заполнения полей в Контактах");
            return result;
        }
    }
}
