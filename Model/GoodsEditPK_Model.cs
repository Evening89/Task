using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Task.Model
{
    public class GoodsEditPK_Model
    {
        public IWebDriver driver;

        public string GenerateDate() //генерирует дату для полей "Дата старта РК" и "Дата окончания РК"
        {
            string date;
            date = DateTime.Now.Day.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Year.ToString();
            return date;
        }

        #region Required fields
            protected string FieldName;
            public string Name
            {
                get { return FieldName; }
                set
                {
                    FieldName = value;
                    IWebElement webelement = driver.FindElement(By.Id("name"));
                    webelement.Clear();
                    webelement.SendKeys(value);
                }
            }
        #endregion

        #region Unrequired fields
            protected bool CheckboxViewSensors;
            public bool ViewSensors
            {
                get { return CheckboxViewSensors; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("display_goods_buying"));
                    webelement.Click();
                    CheckboxViewSensors = value;
                }
            }

            protected bool CheckboxViewConversion;
            public bool ViewConversion
            {
                get { return CheckboxViewConversion; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("display_goods_conversion"));
                    webelement.Click();
                    CheckboxViewConversion = value;
                }
            }

            protected string FieldStartPkDate;
            public string StartPkDate
            {
                get { return FieldStartPkDate; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("when_autostart"));
                    webelement.Clear();
                    webelement.SendKeys(value);
                    FieldStartPkDate = value;
                }
            }

            protected string FieldEndPkDate;
            public string EndPkDate
            {
                get { return FieldEndPkDate; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("limit_date"));
                    webelement.Clear();
                    webelement.SendKeys(value);
                    FieldEndPkDate = value;
                }
            }

            protected bool CheckboxBlockTeasersAfterCreation;
            public bool BlockTeasersAfterCreation
            {
                get { return CheckboxBlockTeasersAfterCreation; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("block_before_show"));
                    webelement.Click();
                    CheckboxBlockTeasersAfterCreation = value;
                }
            }

            protected bool CheckboxStoppedByManager;
            public bool StoppedByManager
            {
                get { return CheckboxStoppedByManager; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("manager_delete"));
                    webelement.Click();
                    CheckboxStoppedByManager = value;
                }
            }

            #region Ограничения PK
                //Блок радиобаттонов "ограничения рекламной кампании"
                protected string RadioLimitsOfPk;
                public string LimitsOfPk
                {
                    get { return RadioLimitsOfPk; }
                    set
                    {
                        RadioLimitsOfPk = value;
                        switch (value)
                        {
                            case "0":
                                {
                                    IWebElement unlimited = driver.FindElement(By.Id("limit_type-unlimited"));
                                    unlimited.Click();
                                    break;
                                }
                            case "1":
                                {
                                    IWebElement byBudget = driver.FindElement(By.Id("limit_type-budgetlimits"));
                                    byBudget.Click();
                                    break;
                                }
                            case "2":
                                {
                                    IWebElement byClickNum = driver.FindElement(By.Id("limit_type-clickslimits"));
                                    byClickNum.Click();
                                    break;
                                }
                        }
                    }
                }
                protected string FieldDayLimitByBudget;
                public string DayLimitByBudget
                {
                    get { return FieldDayLimitByBudget; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("limit_per_day"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldDayLimitByBudget = value;
                    }
                }
                protected string FieldGeneralLimitByBudget;
                public string GeneralLimitByBudget
                {
                    get { return FieldGeneralLimitByBudget; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("limit_per_campaign"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldGeneralLimitByBudget = value;
                    }
                }
                protected string FieldDayLimitByClicks;
                public string DayLimitByClicks
                {
                    get { return FieldDayLimitByClicks; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("limit_clicks_per_day"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldDayLimitByClicks = value;
                    }
                }
                protected string FieldGeneralLimitByClicks;
                public string GeneralLimitByClicks
                {
                    get { return FieldGeneralLimitByClicks; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("limit_clicks"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldGeneralLimitByClicks = value;
                    }
                }
                //Конец Блока радиобаттонов "ограничения рекламной кампании"
            #endregion

            #region UTM-разметка РК для Google Analytics
                protected bool CheckboxUtmPkForGoogleAnalytics;
                public bool UtmPkForGoogleAnalytics
                {
                    get { return CheckboxUtmPkForGoogleAnalytics; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("utm_flag"));
                        webelement.Click();
                        CheckboxUtmPkForGoogleAnalytics = value;
                    }
                }
                protected string FieldUtmMedium;
                public string UtmMedium
                {
                    get { return FieldUtmMedium; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("utm-utm_medium"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldUtmMedium = value;
                    }
                }
                protected string FieldUtmSource;
                public string UtmSource
                {
                    get { return FieldUtmSource; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("utm-utm_source"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldUtmSource = value;
                    }
                }
                protected string FieldUtmCampaign;
                public string UtmCampaign
                {
                    get { return FieldUtmCampaign; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("utm-utm_campaign"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldUtmCampaign = value;
                    }
                }
            #endregion

            #region UTM-разметка пользователя
                protected bool CheckboxUtmUser;
                public bool UtmUser
                {
                    get { return CheckboxUtmUser; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("user_utm_flag"));
                        webelement.Click();
                        CheckboxUtmUser = value;
                    }
                }
                protected string FieldUtmUserStr;
                public string UtmUserStr
                {
                    get { return FieldUtmUserStr; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("user_utm-utm_custom"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldUtmUserStr = value;
                    }
                }
            #endregion

            protected bool CheckboxScrewInTovarro;
            public bool ScrewInTovarro
            {
                get { return CheckboxScrewInTovarro; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("tovarro_on"));
                    webelement.Click();
                    CheckboxScrewInTovarro = value;
                }
            }

            #region Блокировка по расписанию
                protected bool CheckboxBlockBySchedule;
                public bool BlockBySchedule
                {
                    get { return CheckboxBlockBySchedule; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("proc_tblock"));
                        webelement.Click();
                        CheckboxBlockBySchedule = value;
                    }
                }
                protected bool CheckboxWeekends;
                public bool Weekends
                {
                    get { return CheckboxWeekends; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("blocking-buttonsbarbottom-weekends_checkbox"));
                        webelement.Click();
                        CheckboxWeekends = value;
                    }
                }
                protected bool CheckboxWeekdays;
                public bool Weekdays
                {
                    get { return CheckboxWeekdays; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("blocking-buttonsbarbottom-days_checkbox"));
                        webelement.Click();
                        CheckboxWeekdays = value;
                    }
                }
                protected bool CheckboxWorkingTime;
                public bool WorkingTime
                {
                    get { return CheckboxWorkingTime; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("blocking-buttonsbarbottom-business_time_checkbox"));
                        webelement.Click();
                        CheckboxWorkingTime = value;
                    }
                }
            #endregion

            #region Передавать id площадки в ссылке
                protected bool CheckboxIdOfPlatformInLink;
                public bool IdOfPlatformInLink
                {
                    get { return CheckboxIdOfPlatformInLink; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("add_ticker_id"));
                        webelement.Click();
                        CheckboxIdOfPlatformInLink = value;
                    }
                }
                protected string FieldIdOfPlatformInLink;
                public string IdOfPlatformInLinkStr
                {
                    get { return FieldIdOfPlatformInLink; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("add_ticker_id_var_name"));
                        webelement.Clear();
                        webelement.SendKeys(value);
                        FieldIdOfPlatformInLink = value;
                    }
                }
            #endregion

            protected bool CheckboxAddIdOfTeaserInLink;
            public bool AddIdOfTeaserInLink
            {
                get { return CheckboxAddIdOfTeaserInLink; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("add_hit_id"));
                    webelement.Click();
                    CheckboxAddIdOfTeaserInLink = value;
                }
            }

            protected string FieldCommentsForPk;
            public string CommentsForPk
            {
                get { return FieldCommentsForPk; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("comments"));
                    webelement.Clear();
                    webelement.SendKeys(value);
                    FieldCommentsForPk = value;
                }
            }

            #region Площадки
                protected bool CheckboxPlatforms;
                public bool Platforms
                {
                    get { return CheckboxPlatforms; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("proc_category_platform"));
                        webelement.Click();
                        CheckboxPlatforms = value;
                    }
                }
                protected bool CheckboxPlatformsNotSpecified;
                public bool PlatformsNotSpecified
                {
                    get { return CheckboxPlatformsNotSpecified; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-0"));
                        webelement.Click();
                        CheckboxPlatformsNotSpecified = value;
                    }
                }
                protected bool CheckboxPlatformsPolitics;
                public bool PlatformsPolitics
                {
                    get { return CheckboxPlatformsPolitics; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-1"));
                        webelement.Click();
                        CheckboxPlatformsPolitics = value;
                    }
                }
                protected bool CheckboxPlatformsEconomics;
                public bool PlatformsEconomics
                {
                    get { return CheckboxPlatformsEconomics; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-2"));
                        webelement.Click();
                        CheckboxPlatformsEconomics = value;
                    }
                }
                protected bool CheckboxPlatformsCelebrities;
                public bool PlatformsCelebrities
                {
                    get { return CheckboxPlatformsCelebrities; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-3"));
                        webelement.Click();
                        CheckboxPlatformsCelebrities = value;
                    }
                }
                protected bool CheckboxPlatformsScience;
                public bool PlatformsScience
                {
                    get { return CheckboxPlatformsScience; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-4"));
                        webelement.Click();
                        CheckboxPlatformsScience = value;
                    }
                }
                protected bool CheckboxPlatformsConnection;
                public bool PlatformsConnection
                {
                    get { return CheckboxPlatformsConnection; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-5"));
                        webelement.Click();
                        CheckboxPlatformsConnection = value;
                    }
                }
                protected bool CheckboxPlatformsSports;
                public bool PlatformsSports
                {
                    get { return CheckboxPlatformsSports; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-6"));
                        webelement.Click();
                        CheckboxPlatformsSports = value;
                    }
                }
                protected bool CheckboxPlatformsAuto;
                public bool PlatformsAuto
                {
                    get { return CheckboxPlatformsAuto; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-7"));
                        webelement.Click();
                        CheckboxPlatformsAuto = value;
                    }
                }
                protected bool CheckboxPlatformsFashion;
                public bool PlatformsFashion
                {
                    get { return CheckboxPlatformsFashion; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-8"));
                        webelement.Click();
                        CheckboxPlatformsFashion = value;
                    }
                }
                protected bool CheckboxPlatformsMedicine;
                public bool PlatformsMedicine
                {
                    get { return CheckboxPlatformsMedicine; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-9"));
                        webelement.Click();
                        CheckboxPlatformsMedicine = value;
                    }
                }
                protected bool CheckboxPlatformsTourism;
                public bool PlatformsTourism
                {
                    get { return CheckboxPlatformsTourism; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-10"));
                        webelement.Click();
                        CheckboxPlatformsTourism = value;
                    }
                }
                protected bool CheckboxPlatformsGlobalPortals;
                public bool PlatformsGlobalPortals
                {
                    get { return CheckboxPlatformsGlobalPortals; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-11"));
                        webelement.Click();
                        CheckboxPlatformsGlobalPortals = value;
                    }
                }
                protected bool CheckboxPlatformsHumor;
                public bool PlatformsHumor
                {
                    get { return CheckboxPlatformsHumor; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-12"));
                        webelement.Click();
                        CheckboxPlatformsHumor = value;
                    }
                }
                protected bool CheckboxPlatformsFileshares;
                public bool PlatformsFileshares
                {
                    get { return CheckboxPlatformsFileshares; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-13"));
                        webelement.Click();
                        CheckboxPlatformsFileshares = value;
                    }
                }
                protected bool CheckboxPlatformsSocialNetworks;
                public bool PlatformsSocialNetworks
                {
                    get { return CheckboxPlatformsSocialNetworks; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-14"));
                        webelement.Click();
                        CheckboxPlatformsSocialNetworks = value;
                    }
                }
                protected bool CheckboxPlatformsAnimals;
                public bool PlatformsAnimals
                {
                    get { return CheckboxPlatformsAnimals; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-15"));
                        webelement.Click();
                        CheckboxPlatformsAnimals = value;
                    }
                }
                protected bool CheckboxPlatformsReligion;
                public bool PlatformsReligion
                {
                    get { return CheckboxPlatformsReligion; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-16"));
                        webelement.Click();
                        CheckboxPlatformsReligion = value;
                    }
                }
                protected bool CheckboxPlatformsChildren;
                public bool PlatformsChildren
                {
                    get { return CheckboxPlatformsChildren; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-17"));
                        webelement.Click();
                        CheckboxPlatformsChildren = value;
                    }
                }
                protected bool CheckboxPlatformsBuilding;
                public bool PlatformsBuilding
                {
                    get { return CheckboxPlatformsBuilding; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-18"));
                        webelement.Click();
                        CheckboxPlatformsBuilding = value;
                    }
                }
                protected bool CheckboxPlatformsCookery;
                public bool PlatformsCookery
                {
                    get { return CheckboxPlatformsCookery; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-19"));
                        webelement.Click();
                        CheckboxPlatformsCookery = value;
                    }
                }
                protected bool CheckboxPlatformsJob;
                public bool PlatformsJob
                {
                    get { return CheckboxPlatformsJob; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-20"));
                        webelement.Click();
                        CheckboxPlatformsJob = value;
                    }
                }
                protected bool CheckboxPlatformsNotSites;
                public bool PlatformsNotSites
                {
                    get { return CheckboxPlatformsNotSites; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-21"));
                        webelement.Click();
                        CheckboxPlatformsNotSites = value;
                    }
                }
                protected bool CheckboxPlatformsSitesStartPagesBrowsers;
                public bool PlatformsSitesStartPagesBrowsers
                {
                    get { return CheckboxPlatformsSitesStartPagesBrowsers; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-22"));
                        webelement.Click();
                        CheckboxPlatformsSitesStartPagesBrowsers = value;
                    }
                }
                protected bool CheckboxPlatformsSearchSystems;
                public bool PlatformsSearchSystems
                {
                    get { return CheckboxPlatformsSearchSystems; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-23"));
                        webelement.Click();
                        CheckboxPlatformsSearchSystems = value;
                    }
                }
                protected bool CheckboxPlatformsEmail;
                public bool PlatformsEmail
                {
                    get { return CheckboxPlatformsEmail; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-24"));
                        webelement.Click();
                        CheckboxPlatformsEmail = value;
                    }
                }
                protected bool CheckboxPlatformsPhotoCatalogues;
                public bool PlatformsPhotoCatalogues
                {
                    get { return CheckboxPlatformsPhotoCatalogues; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-25"));
                        webelement.Click();
                        CheckboxPlatformsPhotoCatalogues = value;
                    }
                }
                protected bool CheckboxPlatformsVarez;
                public bool PlatformsVarez
                {
                    get { return CheckboxPlatformsVarez; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-26"));
                        webelement.Click();
                        CheckboxPlatformsVarez = value;
                    }
                }
                protected bool CheckboxPlatformsOnlineVideo;
                public bool PlatformsOnlineVideo
                {
                    get { return CheckboxPlatformsOnlineVideo; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-27"));
                        webelement.Click();
                        CheckboxPlatformsOnlineVideo = value;
                    }
                }
                protected bool CheckboxPlatformsOnlineLibraries;
                public bool PlatformsOnlineLibraries
                {
                    get { return CheckboxPlatformsOnlineLibraries; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-28"));
                        webelement.Click();
                        CheckboxPlatformsOnlineLibraries = value;
                    }
                }
                protected bool CheckboxPlatformsInternet;
                public bool PlatformsInternet
                {
                    get { return CheckboxPlatformsInternet; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-29"));
                        webelement.Click();
                        CheckboxPlatformsInternet = value;
                    }
                }
                protected bool CheckboxPlatformsOnlineGames;
                public bool PlatformsOnlineGames
                {
                    get { return CheckboxPlatformsOnlineGames; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-30"));
                        webelement.Click();
                        CheckboxPlatformsOnlineGames = value;
                    }
                }
                protected bool CheckboxPlatformsInternetRepresentatives;
                public bool PlatformsInternetRepresentatives
                {
                    get { return CheckboxPlatformsInternetRepresentatives; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-31"));
                        webelement.Click();
                        CheckboxPlatformsInternetRepresentatives = value;
                    }
                }
                protected bool CheckboxPlatformsProgramms;
                public bool PlatformsProgramms
                {
                    get { return CheckboxPlatformsProgramms; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-32"));
                        webelement.Click();
                        CheckboxPlatformsProgramms = value;
                    }
                }
                protected bool CheckboxPlatformsCataloguesInternetResources;
                public bool PlatformsCataloguesInternetResources
                {
                    get { return CheckboxPlatformsCataloguesInternetResources; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-33"));
                        webelement.Click();
                        CheckboxPlatformsCataloguesInternetResources = value;
                    }
                }
                protected bool CheckboxPlatformsForWagesInInternet;
                public bool PlatformsForWagesInInternet
                {
                    get { return CheckboxPlatformsForWagesInInternet; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-34"));
                        webelement.Click();
                        CheckboxPlatformsForWagesInInternet = value;
                    }
                }
                protected bool CheckboxPlatformsHobbies;
                public bool PlatformsHobbies
                {
                    get { return CheckboxPlatformsHobbies; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-35"));
                        webelement.Click();
                        CheckboxPlatformsHobbies = value;
                    }
                }
                protected bool CheckboxPlatformsMarketgid;
                public bool PlatformsMarketgid
                {
                    get { return CheckboxPlatformsMarketgid; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-36"));
                        webelement.Click();
                        CheckboxPlatformsMarketgid = value;
                    }
                }
                protected bool CheckboxPlatformsShock;
                public bool PlatformsShock
                {
                    get { return CheckboxPlatformsShock; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-37"));
                        webelement.Click();
                        CheckboxPlatformsShock = value;
                    }
                }
                protected bool CheckboxPlatformsEsoteric;
                public bool PlatformsEsoteric
                {
                    get { return CheckboxPlatformsEsoteric; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-38"));
                        webelement.Click();
                        CheckboxPlatformsEsoteric = value;
                    }
                }
                protected bool CheckboxPlatformsPsychology;
                public bool PlatformsPsychology
                {
                    get { return CheckboxPlatformsPsychology; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-39"));
                        webelement.Click();
                        CheckboxPlatformsPsychology = value;
                    }
                }
                protected bool CheckboxPlatformsHistory;
                public bool PlatformsHistory
                {
                    get { return CheckboxPlatformsHistory; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-40"));
                        webelement.Click();
                        CheckboxPlatformsHistory = value;
                    }
                }
                protected bool CheckboxPlatformsMarketgidWomenNet;
                public bool PlatformsMarketgidWomenNet
                {
                    get { return CheckboxPlatformsMarketgidWomenNet; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("category_platform-ids-41"));
                        webelement.Click();
                        CheckboxPlatformsMarketgidWomenNet = value;
                    }
                }
            #endregion

            #region Демографический Таргетинг
                protected string RadioDemoTargeting;
                public string DemoTargeting
                {
                    get { return RadioDemoTargeting; }
                    set
                    {
                        RadioDemoTargeting = value;
                        switch (value)
                        {
                            case "0":
                                {
                                    IWebElement notUseDemoTargeting = driver.FindElement(By.Id("use_socdem-0")); //не использовать демотаргетинг
                                    notUseDemoTargeting.Click();
                                    break;
                                }
                            case "1":
                                {
                                    IWebElement useDemoTargeting = driver.FindElement(By.Id("use_socdem-1")); //использовать демотаргетинг
                                    useDemoTargeting.Click();
                                    break;
                                }
                        }
                    }
                }
                #region Мужчины
                    protected bool ClickDemoTargetingMen;
                    public bool DemoTargetingMenExpand
                    {
                        get { return ClickDemoTargetingMen; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#socdem-country_M + .hint"));
                            webelement.Click();
                            ClickDemoTargetingMen = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingMen;
                    public bool DemoTargetingMenChoseAll
                    {
                        get { return CheckboxDemoTargetingMen; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-country_M"));
                            webelement.Click();
                            CheckboxDemoTargetingMen = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingMenNotSpecified;
                    public bool DemoTargetingMenNotSpecified
                    {
                        get { return CheckboxDemoTargetingMenNotSpecified; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-M-2"));
                            webelement.Click();
                            CheckboxDemoTargetingMenNotSpecified = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingMen618;
                    public bool DemoTargetingMen618
                    {
                        get { return CheckboxDemoTargetingMen618; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-M-4"));
                            webelement.Click();
                            CheckboxDemoTargetingMen618 = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingMen1924;
                    public bool DemoTargetingMen1924
                    {
                        get { return CheckboxDemoTargetingMen1924; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-M-6"));
                            webelement.Click();
                            CheckboxDemoTargetingMen1924 = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingMen2534;
                    public bool DemoTargetingMen2534
                    {
                        get { return CheckboxDemoTargetingMen2534; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-M-8"));
                            webelement.Click();
                            CheckboxDemoTargetingMen2534 = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingMen3544;
                    public bool DemoTargetingMen3544
                    {
                        get { return CheckboxDemoTargetingMen3544; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-M-10"));
                            webelement.Click();
                            CheckboxDemoTargetingMen3544 = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingMen4590;
                    public bool DemoTargetingMen4590
                    {
                        get { return CheckboxDemoTargetingMen4590; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-M-12"));
                            webelement.Click();
                            CheckboxDemoTargetingMen4590 = value;
                        }
                    }
                #endregion

                #region Женщины
                    protected bool ClickDemoTargetingWomen;
                    public bool DemoTargetingWomenExpand
                    {
                        get { return ClickDemoTargetingWomen; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#socdem-country_W + .hint"));
                            webelement.Click();
                            ClickDemoTargetingWomen = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingWomen;
                    public bool DemoTargetingWomenChoseAll
                    {
                        get { return CheckboxDemoTargetingWomen; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-country_W"));
                            webelement.Click();
                            CheckboxDemoTargetingWomen = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingWomenNotSpecified;
                    public bool DemoTargetingWomenNotSpecified
                    {
                        get { return CheckboxDemoTargetingWomenNotSpecified; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-W-1"));
                            webelement.Click();
                            CheckboxDemoTargetingWomenNotSpecified = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingWomen618;
                    public bool DemoTargetingWomen618
                    {
                        get { return CheckboxDemoTargetingWomen618; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-W-3"));
                            webelement.Click();
                            CheckboxDemoTargetingWomen618 = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingWomen1924;
                    public bool DemoTargetingWomen1924
                    {
                        get { return CheckboxDemoTargetingWomen1924; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-W-5"));
                            webelement.Click();
                            CheckboxDemoTargetingWomen1924 = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingWomen2534;
                    public bool DemoTargetingWomen2534
                    {
                        get { return CheckboxDemoTargetingWomen2534; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-W-7"));
                            webelement.Click();
                            CheckboxDemoTargetingWomen2534 = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingWomen3544;
                    public bool DemoTargetingWomen3544
                    {
                        get { return CheckboxDemoTargetingWomen3544; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-W-9"));
                            webelement.Click();
                            CheckboxDemoTargetingWomen3544 = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingWomen4590;
                    public bool DemoTargetingWomen4590
                    {
                        get { return CheckboxDemoTargetingWomen4590; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-W-11"));
                            webelement.Click();
                            CheckboxDemoTargetingWomen4590 = value;
                        }
                    }
                #endregion

                #region Пол не определен
                    protected bool ClickDemoTargetingHermaphrodite;
                    public bool DemoTargetingHermaphroditeExpand
                    {
                        get { return ClickDemoTargetingHermaphrodite; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#socdem-country_N + .hint"));
                            webelement.Click();
                            ClickDemoTargetingHermaphrodite = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingHermaphrodite;
                    public bool DemoTargetingHermaphroditeChoseAll
                    {
                        get { return CheckboxDemoTargetingHermaphrodite; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-country_N"));
                            webelement.Click();
                            CheckboxDemoTargetingHermaphrodite = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingHermaphrodite618;
                    public bool DemoTargetingHermaphrodite618
                    {
                        get { return CheckboxDemoTargetingHermaphrodite618; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-N-13"));
                            webelement.Click();
                            CheckboxDemoTargetingHermaphrodite618 = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingHermaphrodite1924;
                    public bool DemoTargetingHermaphrodite1924
                    {
                        get { return CheckboxDemoTargetingHermaphrodite1924; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-N-14"));
                            webelement.Click();
                            CheckboxDemoTargetingHermaphrodite1924 = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingHermaphrodite2534;
                    public bool DemoTargetingHermaphrodite2534
                    {
                        get { return CheckboxDemoTargetingHermaphrodite2534; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-N-15"));
                            webelement.Click();
                            CheckboxDemoTargetingHermaphrodite2534 = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingHermaphrodite3544;
                    public bool DemoTargetingHermaphrodite3544
                    {
                        get { return CheckboxDemoTargetingHermaphrodite3544; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-N-16"));
                            webelement.Click();
                            CheckboxDemoTargetingHermaphrodite3544 = value;
                        }
                    }
                    protected bool CheckboxDemoTargetingHermaphrodite4590;
                    public bool DemoTargetingHermaphrodite4590
                    {
                        get { return CheckboxDemoTargetingHermaphrodite4590; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("socdem-N-17"));
                            webelement.Click();
                            CheckboxDemoTargetingHermaphrodite4590 = value;
                        }
                    }
                #endregion
            #endregion

            #region  Таргетинг по интересам
                protected string RadioInterestsTargeting;
                public string InterestsTargeting
                {
                    get { return RadioInterestsTargeting; }
                    set
                    {
                        RadioInterestsTargeting = value;
                        switch (value)
                        {
                            case "0":
                                {
                                    IWebElement notUseDemoTargeting = driver.FindElement(By.Id("use_socdem_interests-0")); //не использовать таргетинг
                                    notUseDemoTargeting.Click();
                                    break;
                                }
                            case "1":
                                {
                                    IWebElement useDemoTargeting = driver.FindElement(By.Id("use_socdem_interests-1")); //использовать таргетинг
                                    useDemoTargeting.Click();
                                    break;
                                }
                        }
                    }
                }
                protected bool CheckboxInterestsTargetingOther;
                public bool InterestsTargetingOther
                {
                    get { return CheckboxInterestsTargetingOther; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("interests-country_0"));
                        webelement.Click();
                        CheckboxInterestsTargetingOther = value;
                    }
                }
                #region Бизнес
                    protected bool ClickInterestsTargetingBusiness;
                    public bool InterestsTargetingBusinessExpand
                    {
                        get { return ClickInterestsTargetingBusiness; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#interests-country_14 + .hint"));
                            webelement.Click();
                            ClickInterestsTargetingBusiness = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingBusiness;
                    public bool InterestsTargetingBusinessChoseAll
                    {
                        get { return CheckboxInterestsTargetingBusiness; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-country_14"));
                            webelement.Click();
                            CheckboxInterestsTargetingBusiness = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingBusinessAcoountancy;
                    public bool InterestsTargetingBusinessAcoountancy
                    {
                        get { return CheckboxInterestsTargetingBusinessAcoountancy; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-14-16"));
                            webelement.Click();
                            CheckboxInterestsTargetingBusinessAcoountancy = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingBusinessPlacement;
                    public bool InterestsTargetingBusinessPlacement
                    {
                        get { return CheckboxInterestsTargetingBusinessPlacement; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-14-19"));
                            webelement.Click();
                            CheckboxInterestsTargetingBusinessPlacement = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingBusinessAudit;
                    public bool InterestsTargetingBusinessAudit
                    {
                        get { return CheckboxInterestsTargetingBusinessAudit; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-14-18"));
                            webelement.Click();
                            CheckboxInterestsTargetingBusinessAudit = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingBusinessAdverts;
                    public bool InterestsTargetingBusinessAdverts
                    {
                        get { return CheckboxInterestsTargetingBusinessAdverts; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-14-17"));
                            webelement.Click();
                            CheckboxInterestsTargetingBusinessAdverts = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingBusinessMiscellanea;
                    public bool InterestsTargetingBusinessMiscellanea
                    {
                        get { return CheckboxInterestsTargetingBusinessMiscellanea; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-14-15"));
                            webelement.Click();
                            CheckboxInterestsTargetingBusinessMiscellanea = value;
                        }
                    }
                #endregion

                #region Недвижимость
                    protected bool ClickInterestsTargetingRealty;
                    public bool InterestsTargetingRealtyExpand
                    {
                        get { return ClickInterestsTargetingRealty; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#interests-country_44 + .hint"));
                            webelement.Click();
                            ClickInterestsTargetingRealty = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingRealty;
                    public bool InterestsTargetingRealtyChoseAll
                    {
                        get { return CheckboxInterestsTargetingRealty; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-country_44"));
                            webelement.Click();
                            CheckboxInterestsTargetingRealty = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingRealtyMiscelanea;
                    public bool InterestsTargetingRealtyMiscelanea
                    {
                        get { return CheckboxInterestsTargetingRealtyMiscelanea; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-44-45"));
                            webelement.Click();
                            CheckboxInterestsTargetingRealtyMiscelanea = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingRealtyGarages;
                    public bool InterestsTargetingRealtyGarages
                    {
                        get { return CheckboxInterestsTargetingRealtyGarages; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-44-47"));
                            webelement.Click();
                            CheckboxInterestsTargetingRealtyGarages = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingRealtyFlats;
                    public bool InterestsTargetingRealtyFlats
                    {
                        get { return CheckboxInterestsTargetingRealtyFlats; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-44-46"));
                            webelement.Click();
                            CheckboxInterestsTargetingRealtyFlats = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingRealtyAbroad;
                    public bool InterestsTargetingRealtyAbroad
                    {
                        get { return CheckboxInterestsTargetingRealtyAbroad; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-44-52"));
                            webelement.Click();
                            CheckboxInterestsTargetingRealtyAbroad = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingRealtyLand;
                    public bool InterestsTargetingRealtyLand
                    {
                        get { return CheckboxInterestsTargetingRealtyLand; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-44-51"));
                            webelement.Click();
                            CheckboxInterestsTargetingRealtyLand = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingRealtySuburban;
                    public bool InterestsTargetingRealtySuburban
                    {
                        get { return CheckboxInterestsTargetingRealtySuburban; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-44-48"));
                            webelement.Click();
                            CheckboxInterestsTargetingRealtySuburban = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingRealtyHypothec;
                    public bool InterestsTargetingRealtyHypothec
                    {
                        get { return CheckboxInterestsTargetingRealtyHypothec; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-44-50"));
                            webelement.Click();
                            CheckboxInterestsTargetingRealtyHypothec = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingRealtyCommerce;
                    public bool InterestsTargetingRealtyCommerce
                    {
                        get { return CheckboxInterestsTargetingRealtyCommerce; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-44-49"));
                            webelement.Click();
                            CheckboxInterestsTargetingRealtyCommerce = value;
                        }
                    }
                #endregion

                protected bool CheckboxInterestsTargetingExhibitions;
                public bool InterestsTargetingExhibitions
                {
                    get { return CheckboxInterestsTargetingExhibitions; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("interests-country_21"));
                        webelement.Click();
                        CheckboxInterestsTargetingExhibitions = value;
                    }
                }

                #region Образование
                    protected bool ClickInterestsTargetingEducation;
                    public bool InterestsTargetingEducationExpand
                    {
                        get { return ClickInterestsTargetingEducation; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#interests-country_53 + .hint"));
                            webelement.Click();
                            ClickInterestsTargetingEducation = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingEducation;
                    public bool InterestsTargetingEducationChoseAll
                    {
                        get { return CheckboxInterestsTargetingEducation; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-country_53"));
                            webelement.Click();
                            CheckboxInterestsTargetingEducation = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingEducationForeignLanguages;
                    public bool InterestsTargetingEducationForeignLanguages
                    {
                        get { return CheckboxInterestsTargetingEducationForeignLanguages; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-53-58"));
                            webelement.Click();
                            CheckboxInterestsTargetingEducationForeignLanguages = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingEducationAbroad;
                    public bool InterestsTargetingEducationAbroad
                    {
                        get { return CheckboxInterestsTargetingEducationAbroad; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-53-57"));
                            webelement.Click();
                            CheckboxInterestsTargetingEducationAbroad = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingEducationHigh;
                    public bool InterestsTargetingEducationHigh
                    {
                        get { return CheckboxInterestsTargetingEducationHigh; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-53-56"));
                            webelement.Click();
                            CheckboxInterestsTargetingEducationHigh = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingEducationMiscelanea;
                    public bool InterestsTargetingEducationMiscelanea
                    {
                        get { return CheckboxInterestsTargetingEducationMiscelanea; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-53-54"));
                            webelement.Click();
                            CheckboxInterestsTargetingEducationMiscelanea = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingEducationChildren;
                    public bool InterestsTargetingEducationChildren
                    {
                        get { return CheckboxInterestsTargetingEducationChildren; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-53-55"));
                            webelement.Click();
                            CheckboxInterestsTargetingEducationChildren = value;
                        }
                    }
                #endregion

                #region Отдых, туризм, путешествия
                    protected bool ClickInterestsTargetingRest;
                    public bool InterestsTargetingRestExpand
                    {
                        get { return ClickInterestsTargetingRest; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#interests-country_59 + .hint"));
                            webelement.Click();
                            ClickInterestsTargetingRest = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingRest;
                    public bool InterestsTargetingRestChoseAll
                    {
                        get { return CheckboxInterestsTargetingRest; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-country_59"));
                            webelement.Click();
                            CheckboxInterestsTargetingRest = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingRestMiscellanea;
                    public bool InterestsTargetingRestMiscellanea
                    {
                        get { return CheckboxInterestsTargetingRestMiscellanea; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-59-77"));
                            webelement.Click();
                            CheckboxInterestsTargetingRestMiscellanea = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingRestRuUa;
                    public bool InterestsTargetingRestRuUa
                    {
                        get { return CheckboxInterestsTargetingRestRuUa; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-59-61"));
                            webelement.Click();
                            CheckboxInterestsTargetingRestRuUa = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingRestAbroad;
                    public bool InterestsTargetingRestAbroad
                    {
                        get { return CheckboxInterestsTargetingRestAbroad; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-59-62"));
                            webelement.Click();
                            CheckboxInterestsTargetingRestAbroad = value;
                        }
                    }
                #endregion

                #region Телефоны, связь
                    protected bool ClickInterestsTargetingTelephones;
                    public bool InterestsTargetingTelephonesExpand
                    {
                        get { return ClickInterestsTargetingTelephones; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#interests-country_63 + .hint"));
                            webelement.Click();
                            ClickInterestsTargetingTelephones = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingTelephones;
                    public bool InterestsTargetingTelephonesChoseAll
                    {
                        get { return CheckboxInterestsTargetingTelephones; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-country_63"));
                            webelement.Click();
                            CheckboxInterestsTargetingTelephones = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingTelephonesMiscellanea;
                    public bool InterestsTargetingTelephonesMiscellanea
                    {
                        get { return CheckboxInterestsTargetingTelephonesMiscellanea; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-63-64"));
                            webelement.Click();
                            CheckboxInterestsTargetingTelephonesMiscellanea = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingTelephonesNavigation;
                    public bool InterestsTargetingTelephonesNavigation
                    {
                        get { return CheckboxInterestsTargetingTelephonesNavigation; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-63-68"));
                            webelement.Click();
                            CheckboxInterestsTargetingTelephonesNavigation = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingTelephonesMobileApps;
                    public bool InterestsTargetingTelephonesMobileApps
                    {
                        get { return CheckboxInterestsTargetingTelephonesMobileApps; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-63-67"));
                            webelement.Click();
                            CheckboxInterestsTargetingTelephonesMobileApps = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingTelephonesMobile;
                    public bool InterestsTargetingTelephonesMobile
                    {
                        get { return CheckboxInterestsTargetingTelephonesMobile; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-63-66"));
                            webelement.Click();
                            CheckboxInterestsTargetingTelephonesMobile = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingTelephonesStationary;
                    public bool InterestsTargetingTelephonesStationary
                    {
                        get { return CheckboxInterestsTargetingTelephonesStationary; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-63-65"));
                            webelement.Click();
                            CheckboxInterestsTargetingTelephonesStationary = value;
                        }
                    }
                #endregion

                protected bool CheckboxInterestsTargetingHouseAplliances;
                public bool InterestsTargetingHouseAplliances
                {
                    get { return CheckboxInterestsTargetingHouseAplliances; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("interests-country_20"));
                        webelement.Click();
                        CheckboxInterestsTargetingHouseAplliances = value;
                    }
                }

                #region Медицина, здоровье
                    protected bool ClickInterestsTargetingMedicine;
                    public bool InterestsTargetingMedicineExpand
                    {
                        get { return ClickInterestsTargetingMedicine; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#interests-country_36 + .hint"));
                            webelement.Click();
                            ClickInterestsTargetingMedicine = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingMedicine;
                    public bool InterestsTargetingMedicineChoseAll
                    {
                        get { return CheckboxInterestsTargetingMedicine; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-country_36"));
                            webelement.Click();
                            CheckboxInterestsTargetingMedicine = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingMedicineSport;
                    public bool InterestsTargetingMedicineSport
                    {
                        get { return CheckboxInterestsTargetingMedicineSport; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-36-43"));
                            webelement.Click();
                            CheckboxInterestsTargetingMedicineSport = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingMedicineEyesight;
                    public bool InterestsTargetingMedicineEyesight
                    {
                        get { return CheckboxInterestsTargetingMedicineEyesight; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-36-42"));
                            webelement.Click();
                            CheckboxInterestsTargetingMedicineEyesight = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingMedicineMiscellanea;
                    public bool InterestsTargetingMedicineMiscellanea
                    {
                        get { return CheckboxInterestsTargetingMedicineMiscellanea; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-36-37"));
                            webelement.Click();
                            CheckboxInterestsTargetingMedicineMiscellanea = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingMedicineDiets;
                    public bool InterestsTargetingMedicineDiets
                    {
                        get { return CheckboxInterestsTargetingMedicineDiets; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-36-39"));
                            webelement.Click();
                            CheckboxInterestsTargetingMedicineDiets = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingMedicineExtraWeight;
                    public bool InterestsTargetingMedicineExtraWeight
                    {
                        get { return CheckboxInterestsTargetingMedicineExtraWeight; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-36-40"));
                            webelement.Click();
                            CheckboxInterestsTargetingMedicineExtraWeight = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingMedicinePregnancy;
                    public bool InterestsTargetingMedicinePregnancy
                    {
                        get { return CheckboxInterestsTargetingMedicinePregnancy; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-36-38"));
                            webelement.Click();
                            CheckboxInterestsTargetingMedicinePregnancy = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingMedicineStomatology;
                    public bool InterestsTargetingMedicineStomatology
                    {
                        get { return CheckboxInterestsTargetingMedicineStomatology; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-36-41"));
                            webelement.Click();
                            CheckboxInterestsTargetingMedicineStomatology = value;
                        }
                    }
                #endregion

                #region Дом и семья
                    protected bool ClickInterestsTargetingHouse;
                    public bool InterestsTargetingHouseExpand
                    {
                        get { return ClickInterestsTargetingHouse; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#interests-country_22 + .hint"));
                            webelement.Click();
                            ClickInterestsTargetingHouse = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingHouse;
                    public bool InterestsTargetingHouseChoseAll
                    {
                        get { return CheckboxInterestsTargetingHouse; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-country_22"));
                            webelement.Click();
                            CheckboxInterestsTargetingHouse = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingHouseChildren;
                    public bool InterestsTargetingHouseChildren
                    {
                        get { return CheckboxInterestsTargetingHouseChildren; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-22-24"));
                            webelement.Click();
                            CheckboxInterestsTargetingHouseChildren = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingHouseDogs;
                    public bool InterestsTargetingHouseDogs
                    {
                        get { return CheckboxInterestsTargetingHouseDogs; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-22-28"));
                            webelement.Click();
                            CheckboxInterestsTargetingHouseDogs = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingHouseMiscellanea;
                    public bool InterestsTargetingHouseMiscellanea
                    {
                        get { return CheckboxInterestsTargetingHouseMiscellanea; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-22-23"));
                            webelement.Click();
                            CheckboxInterestsTargetingHouseMiscellanea = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingHouseCats;
                    public bool InterestsTargetingHouseCats
                    {
                        get { return CheckboxInterestsTargetingHouseCats; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-22-27"));
                            webelement.Click();
                            CheckboxInterestsTargetingHouseCats = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingHouseCookery;
                    public bool InterestsTargetingHouseCookery
                    {
                        get { return CheckboxInterestsTargetingHouseCookery; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-22-26"));
                            webelement.Click();
                            CheckboxInterestsTargetingHouseCookery = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingHouseKindergartens;
                    public bool InterestsTargetingHouseKindergartens
                    {
                        get { return CheckboxInterestsTargetingHouseKindergartens; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-22-25"));
                            webelement.Click();
                            CheckboxInterestsTargetingHouseKindergartens = value;
                        }
                    }
                #endregion

                #region Финансы
                    protected bool ClickInterestsTargetingFinance;
                    public bool InterestsTargetingFinanceExpand
                    {
                        get { return ClickInterestsTargetingFinance; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#interests-country_69 + .hint"));
                            webelement.Click();
                            ClickInterestsTargetingFinance = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingFinance;
                    public bool InterestsTargetingFinanceChoseAll
                    {
                        get { return CheckboxInterestsTargetingFinance; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-country_69"));
                            webelement.Click();
                            CheckboxInterestsTargetingFinance = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingFinanceStockMarket;
                    public bool InterestsTargetingFinanceStockMarket
                    {
                        get { return CheckboxInterestsTargetingFinanceStockMarket; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-69-76"));
                            webelement.Click();
                            CheckboxInterestsTargetingFinanceStockMarket = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingFinanceCurrency;
                    public bool InterestsTargetingFinanceCurrency
                    {
                        get { return CheckboxInterestsTargetingFinanceCurrency; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-69-75"));
                            webelement.Click();
                            CheckboxInterestsTargetingFinanceCurrency = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingFinanceInsurence;
                    public bool InterestsTargetingFinanceInsurence
                    {
                        get { return CheckboxInterestsTargetingFinanceInsurence; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-69-74"));
                            webelement.Click();
                            CheckboxInterestsTargetingFinanceInsurence = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingFinanceMoneyTransfers;
                    public bool InterestsTargetingFinanceMoneyTransfers
                    {
                        get { return CheckboxInterestsTargetingFinanceMoneyTransfers; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-69-73"));
                            webelement.Click();
                            CheckboxInterestsTargetingFinanceMoneyTransfers = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingFinanceCredits;
                    public bool InterestsTargetingFinanceCredits
                    {
                        get { return CheckboxInterestsTargetingFinanceCredits; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-69-72"));
                            webelement.Click();
                            CheckboxInterestsTargetingFinanceCredits = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingFinanceMiscellanea;
                    public bool InterestsTargetingFinanceMiscellanea
                    {
                        get { return CheckboxInterestsTargetingFinanceMiscellanea; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-69-70"));
                            webelement.Click();
                            CheckboxInterestsTargetingFinanceMiscellanea = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingFinanceDeposits;
                    public bool InterestsTargetingFinanceDeposits
                    {
                        get { return CheckboxInterestsTargetingFinanceDeposits; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-69-71"));
                            webelement.Click();
                            CheckboxInterestsTargetingFinanceDeposits = value;
                        }
                    }
                #endregion

                #region Компьютеры, оргтехника
                    protected bool ClickInterestsTargetingComputers;
                    public bool InterestsTargetingComputersExpand
                    {
                        get { return ClickInterestsTargetingComputers; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#interests-country_29 + .hint"));
                            webelement.Click();
                            ClickInterestsTargetingComputers = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingComputers;
                    public bool InterestsTargetingComputersChoseAll
                    {
                        get { return CheckboxInterestsTargetingComputers; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-country_29"));
                            webelement.Click();
                            CheckboxInterestsTargetingComputers = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingComputersLaptops;
                    public bool InterestsTargetingComputersLaptops
                    {
                        get { return CheckboxInterestsTargetingComputersLaptops; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-29-32"));
                            webelement.Click();
                            CheckboxInterestsTargetingComputersLaptops = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingComputersParts;
                    public bool InterestsTargetingComputersParts
                    {
                        get { return CheckboxInterestsTargetingComputersParts; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-29-35"));
                            webelement.Click();
                            CheckboxInterestsTargetingComputersParts = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingComputersPrinters;
                    public bool InterestsTargetingComputersPrinters
                    {
                        get { return CheckboxInterestsTargetingComputersPrinters; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-29-34"));
                            webelement.Click();
                            CheckboxInterestsTargetingComputersPrinters = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingComputersTablets;
                    public bool InterestsTargetingComputersTablets
                    {
                        get { return CheckboxInterestsTargetingComputersTablets; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-29-33"));
                            webelement.Click();
                            CheckboxInterestsTargetingComputersTablets = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingComputersMonitors;
                    public bool InterestsTargetingComputersMonitors
                    {
                        get { return CheckboxInterestsTargetingComputersMonitors; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-29-31"));
                            webelement.Click();
                            CheckboxInterestsTargetingComputersMonitors = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingComputersMiscellanea;
                    public bool InterestsTargetingComputersMiscellanea
                    {
                        get { return CheckboxInterestsTargetingComputersMiscellanea; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-29-30"));
                            webelement.Click();
                            CheckboxInterestsTargetingComputersMiscellanea = value;
                        }
                    }
                #endregion

                #region Авто
                    protected bool ClickInterestsTargetingAuto;
                    public bool InterestsTargetingAutoExpand
                    {
                        get { return ClickInterestsTargetingAuto; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#interests-country_1 + .hint"));
                            webelement.Click();
                            ClickInterestsTargetingAuto = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingAuto;
                    public bool InterestsTargetingAutoChoseAll
                    {
                        get { return CheckboxInterestsTargetingAuto; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-country_1"));
                            webelement.Click();
                            CheckboxInterestsTargetingAuto = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingAutoInsurence;
                    public bool InterestsTargetingAutoInsurence
                    {
                        get { return CheckboxInterestsTargetingAutoInsurence; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-1-6"));
                            webelement.Click();
                            CheckboxInterestsTargetingAutoInsurence = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingAutoMiscellanea;
                    public bool InterestsTargetingAutoMiscellanea
                    {
                        get { return CheckboxInterestsTargetingAutoMiscellanea; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-1-2"));
                            webelement.Click();
                            CheckboxInterestsTargetingAutoMiscellanea = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingAutoNational;
                    public bool InterestsTargetingAutoNational
                    {
                        get { return CheckboxInterestsTargetingAutoNational; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-1-3"));
                            webelement.Click();
                            CheckboxInterestsTargetingAutoNational = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingAutoWheels;
                    public bool InterestsTargetingAutoWheels
                    {
                        get { return CheckboxInterestsTargetingAutoWheels; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-1-7"));
                            webelement.Click();
                            CheckboxInterestsTargetingAutoWheels = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingAutoImported;
                    public bool InterestsTargetingAutoImported
                    {
                        get { return CheckboxInterestsTargetingAutoImported; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-1-4"));
                            webelement.Click();
                            CheckboxInterestsTargetingAutoImported = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingAutoMoto;
                    public bool InterestsTargetingAutoMoto
                    {
                        get { return CheckboxInterestsTargetingAutoMoto; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-1-5"));
                            webelement.Click();
                            CheckboxInterestsTargetingAutoMoto = value;
                        }
                    }
                #endregion

                #region Аудио, Видео, Фото
                    protected bool ClickInterestsTargetingAudio;
                    public bool InterestsTargetingAudioExpand
                    {
                        get { return ClickInterestsTargetingAudio; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#interests-country_8 + .hint"));
                            webelement.Click();
                            ClickInterestsTargetingAudio = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingAudio;
                    public bool InterestsTargetingAudioChoseAll
                    {
                        get { return CheckboxInterestsTargetingAudio; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-country_8"));
                            webelement.Click();
                            CheckboxInterestsTargetingAudio = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingAudioVideoEquips;
                    public bool InterestsTargetingAudioVideoEquips
                    {
                        get { return CheckboxInterestsTargetingAudioVideoEquips; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-8-11"));
                            webelement.Click();
                            CheckboxInterestsTargetingAudioVideoEquips = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingAudioMiscellanea;
                    public bool InterestsTargetingAudioMiscellanea
                    {
                        get { return CheckboxInterestsTargetingAudioMiscellanea; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-8-9"));
                            webelement.Click();
                            CheckboxInterestsTargetingAudioMiscellanea = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingAudioTech;
                    public bool InterestsTargetingAudioTech
                    {
                        get { return CheckboxInterestsTargetingAudioTech; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-8-10"));
                            webelement.Click();
                            CheckboxInterestsTargetingAudioTech = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingAudioCameras;
                    public bool InterestsTargetingAudioCameras
                    {
                        get { return CheckboxInterestsTargetingAudioCameras; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-8-13"));
                            webelement.Click();
                            CheckboxInterestsTargetingAudioCameras = value;
                        }
                    }
                    protected bool CheckboxInterestsTargetingAudioTvs;
                    public bool InterestsTargetingAudioTvs
                    {
                        get { return CheckboxInterestsTargetingAudioTvs; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("interests-8-12"));
                            webelement.Click();
                            CheckboxInterestsTargetingAudioTvs = value;
                        }
                    }
                #endregion
            #endregion

            #region Браузеры
                protected string RadioBrowserTargeting;
                public string BrowserTargeting
                {
                    get { return RadioBrowserTargeting; }
                    set
                    {
                        RadioBrowserTargeting = value;
                        switch (value)
                        {
                            case "0":
                                {
                                    IWebElement notUseBrowserTargeting = driver.FindElement(By.Id("use_browsers_targeting-0")); //не использовать таргетинг
                                    notUseBrowserTargeting.Click();
                                    break;
                                }
                            case "1":
                                {
                                    IWebElement useBrowserTargeting = driver.FindElement(By.Id("use_browsers_targeting-1")); //использовать таргетинг
                                    useBrowserTargeting.Click();
                                    break;
                                }
                        }
                    }
                }
                #region Другие
                    protected bool ClickBrowserTargetingOther;
                    public bool BrowserTargetingOtherExpand
                    {
                        get { return ClickBrowserTargetingOther; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#browser-country_Другие + .hint"));
                            webelement.Click();
                            ClickBrowserTargetingOther = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingOther;
                    public bool BrowserTargetingOtherChoseAll
                    {
                        get { return CheckboxBrowserTargetingOther; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-country_Другие"));
                            webelement.Click();
                            CheckboxBrowserTargetingOther = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingOtherAll;
                    public bool BrowserTargetingOtherAll
                    {
                        get { return CheckboxBrowserTargetingOtherAll; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-Другие-0"));
                            webelement.Click();
                            CheckboxBrowserTargetingOtherAll = value;
                        }
                    }
                #endregion

                #region Opera
                    protected bool ClickBrowserTargetingOpera;
                    public bool BrowserTargetingOperaExpand
                    {
                        get { return ClickBrowserTargetingOpera; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#browser-country_Opera + .hint"));
                            webelement.Click();
                            ClickBrowserTargetingOpera = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingOpera;
                    public bool BrowserTargetingOperaChoseAll
                    {
                        get { return CheckboxBrowserTargetingOpera; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-country_Opera"));
                            webelement.Click();
                            CheckboxBrowserTargetingOpera = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingOperaOther;
                    public bool BrowserTargetingOperaOther
                    {
                        get { return CheckboxBrowserTargetingOperaOther; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-Opera-2"));
                            webelement.Click();
                            CheckboxBrowserTargetingOperaOther = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingOpera10;
                    public bool BrowserTargetingOpera10
                    {
                        get { return CheckboxBrowserTargetingOpera10; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-Opera-3"));
                            webelement.Click();
                            CheckboxBrowserTargetingOpera10 = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingOpera11;
                    public bool BrowserTargetingOpera11
                    {
                        get { return CheckboxBrowserTargetingOpera11; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-Opera-4"));
                            webelement.Click();
                            CheckboxBrowserTargetingOpera11 = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingOperaMini;
                    public bool BrowserTargetingOperaMini
                    {
                        get { return CheckboxBrowserTargetingOperaMini; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-Opera-18"));
                            webelement.Click();
                            CheckboxBrowserTargetingOperaMini = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingOperaMobile;
                    public bool BrowserTargetingOperaMobile
                    {
                        get { return CheckboxBrowserTargetingOperaMobile; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-Opera-19"));
                            webelement.Click();
                            CheckboxBrowserTargetingOperaMobile = value;
                        }
                    }
                #endregion

                #region Chrome
                    protected bool ClickBrowserTargetingChrome;
                    public bool BrowserTargetingChromeExpand
                    {
                        get { return ClickBrowserTargetingChrome; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#browser-country_GoogleChrome + .hint"));
                            webelement.Click();
                            ClickBrowserTargetingChrome = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingChrome;
                    public bool BrowserTargetingChromeChoseAll
                    {
                        get { return CheckboxBrowserTargetingChrome; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-country_GoogleChrome"));
                            webelement.Click();
                            CheckboxBrowserTargetingChrome = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingChromeAll;
                    public bool BrowserTargetingChromeAll
                    {
                        get { return CheckboxBrowserTargetingChromeAll; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-GoogleChrome-5"));
                            webelement.Click();
                            CheckboxBrowserTargetingChromeAll = value;
                        }
                    }
                #endregion

                #region Firefox
                    protected bool ClickBrowserTargetingFirefox;
                    public bool BrowserTargetingFirefoxExpand
                    {
                        get { return ClickBrowserTargetingFirefox; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#browser-country_Firefox + .hint"));
                            webelement.Click();
                            ClickBrowserTargetingFirefox = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingFirefox;
                    public bool BrowserTargetingFirefoxChoseAll
                    {
                        get { return CheckboxBrowserTargetingFirefox; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-country_Firefox"));
                            webelement.Click();
                            CheckboxBrowserTargetingFirefox = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingFirefox3;
                    public bool BrowserTargetingFirefox3
                    {
                        get { return CheckboxBrowserTargetingFirefox3; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-Firefox-6"));
                            webelement.Click();
                            CheckboxBrowserTargetingFirefox3 = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingFirefox4;
                    public bool BrowserTargetingFirefox4
                    {
                        get { return CheckboxBrowserTargetingFirefox4; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-Firefox-7"));
                            webelement.Click();
                            CheckboxBrowserTargetingFirefox4 = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingFirefox5;
                    public bool BrowserTargetingFirefox5
                    {
                        get { return CheckboxBrowserTargetingFirefox5; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-Firefox-8"));
                            webelement.Click();
                            CheckboxBrowserTargetingFirefox5 = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingFirefox6;
                    public bool BrowserTargetingFirefox6
                    {
                        get { return CheckboxBrowserTargetingFirefox6; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-Firefox-9"));
                            webelement.Click();
                            CheckboxBrowserTargetingFirefox6 = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingFirefoxOther;
                    public bool BrowserTargetingFirefoxOther
                    {
                        get { return CheckboxBrowserTargetingFirefoxOther; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-Firefox-15"));
                            webelement.Click();
                            CheckboxBrowserTargetingFirefoxOther = value;
                        }
                    }
                #endregion

                #region Safari
                    protected bool ClickBrowserTargetingSafari;
                    public bool BrowserTargetingSafariExpand
                    {
                        get { return ClickBrowserTargetingSafari; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#browser-country_Safari + .hint"));
                            webelement.Click();
                            ClickBrowserTargetingSafari = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingSafari;
                    public bool BrowserTargetingSafariChoseAll
                    {
                        get { return CheckboxBrowserTargetingSafari; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-country_Safari"));
                            webelement.Click();
                            CheckboxBrowserTargetingSafari = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingSafariAll;
                    public bool BrowserTargetingSafariAll
                    {
                        get { return CheckboxBrowserTargetingSafariAll; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-Safari-10"));
                            webelement.Click();
                            CheckboxBrowserTargetingSafariAll = value;
                        }
                    }
                #endregion

                #region MSIE
                    protected bool ClickBrowserTargetingIe;
                    public bool BrowserTargetingIeExpand
                    {
                        get { return ClickBrowserTargetingIe; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#browser-country_MSIE + .hint"));
                            webelement.Click();
                            ClickBrowserTargetingIe = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingIe;
                    public bool BrowserTargetingIeChoseAll
                    {
                        get { return CheckboxBrowserTargetingIe; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-country_MSIE"));
                            webelement.Click();
                            CheckboxBrowserTargetingIe = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingIe6;
                    public bool BrowserTargetingIe6
                    {
                        get { return CheckboxBrowserTargetingIe6; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-MSIE-11"));
                            webelement.Click();
                            CheckboxBrowserTargetingIe6 = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingIe7;
                    public bool BrowserTargetingIe7
                    {
                        get { return CheckboxBrowserTargetingIe7; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-MSIE-12"));
                            webelement.Click();
                            CheckboxBrowserTargetingIe7 = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingIe8;
                    public bool BrowserTargetingIe8
                    {
                        get { return CheckboxBrowserTargetingIe8; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-MSIE-13"));
                            webelement.Click();
                            CheckboxBrowserTargetingIe8 = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingIe9;
                    public bool BrowserTargetingIe9
                    {
                        get { return CheckboxBrowserTargetingIe9; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-MSIE-14"));
                            webelement.Click();
                            CheckboxBrowserTargetingIe9 = value;
                        }
                    }
                    protected bool CheckboxBrowserTargetingIeOther;
                    public bool BrowserTargetingIeOther
                    {
                        get { return CheckboxBrowserTargetingIeOther; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("browser-MSIE-16"));
                            webelement.Click();
                            CheckboxBrowserTargetingIeOther = value;
                        }
                    }
                #endregion

            #endregion

            #region ОС таргетинг
                protected string RadioOsTargeting;
                public string OsTargeting
                {
                    get { return RadioOsTargeting; }
                    set
                    {
                        RadioOsTargeting = value;
                        switch (value)
                        {
                            case "0":
                                {
                                    IWebElement notUseOsTargeting = driver.FindElement(By.Id("use_os_targeting-0")); //не использовать таргетинг
                                    notUseOsTargeting.Click();
                                    break;
                                }
                            case "1":
                                {
                                    IWebElement useOsTargeting = driver.FindElement(By.Id("use_os_targeting-1")); //использовать таргетинг
                                    useOsTargeting.Click();
                                    break;
                                }
                        }
                    }
                }
                protected bool CheckboxOsTargetingOther;
                public bool OsTargetingOther
                {
                    get { return CheckboxOsTargetingOther; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("osList-use_os_targeting_0"));
                        webelement.Click();
                        CheckboxOsTargetingOther = value;
                    }
                }
                protected bool CheckboxOsTargetingMacOs;
                public bool OsTargetingMacOs
                {
                    get { return CheckboxOsTargetingMacOs; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("osList-use_os_targeting_1"));
                        webelement.Click();
                        CheckboxOsTargetingMacOs = value;
                    }
                }
                protected bool CheckboxOsTargetingOtherMobileOs;
                public bool OsTargetingOtherMobileOs
                {
                    get { return CheckboxOsTargetingOtherMobileOs; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("osList-use_os_targeting_2"));
                        webelement.Click();
                        CheckboxOsTargetingOtherMobileOs = value;
                    }
                }
                protected bool CheckboxOsTargetingWindows;
                public bool OsTargetingWindows
                {
                    get { return CheckboxOsTargetingWindows; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("osList-use_os_targeting_3"));
                        webelement.Click();
                        CheckboxOsTargetingWindows = value;
                    }
                }
                protected bool CheckboxOsTargetingOtherIoS;
                public bool OsTargetingOtherIoS
                {
                    get { return CheckboxOsTargetingOtherIoS; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("osList-use_os_targeting_4"));
                        webelement.Click();
                        CheckboxOsTargetingOtherIoS = value;
                    }
                }
                protected bool CheckboxOsTargetingIpad;
                public bool OsTargetingIpad
                {
                    get { return CheckboxOsTargetingIpad; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("osList-use_os_targeting_5"));
                        webelement.Click();
                        CheckboxOsTargetingIpad = value;
                    }
                }
                protected bool CheckboxOsTargetingIphone;
                public bool OsTargetingIphone
                {
                    get { return CheckboxOsTargetingIphone; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("osList-use_os_targeting_6"));
                        webelement.Click();
                        CheckboxOsTargetingIphone = value;
                    }
                }
                protected bool CheckboxOsTargetingAndroid;
                public bool OsTargetingAndroid
                {
                    get { return CheckboxOsTargetingAndroid; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("osList-use_os_targeting_7"));
                        webelement.Click();
                        CheckboxOsTargetingAndroid = value;
                    }
                }
            #endregion

            #region Таргетинг по провайдерам
                protected string RadioProviderTargeting;
                public string ProviderTargeting
                {
                    get { return RadioProviderTargeting; }
                    set
                    {
                        RadioProviderTargeting = value;
                        switch (value)
                        {
                            case "0":
                                {
                                    IWebElement notUseProviderTargeting = driver.FindElement(By.Id("use_providers_targeting-0")); //не использовать таргетинг
                                    notUseProviderTargeting.Click();
                                    break;
                                }
                            case "1":
                                {
                                    IWebElement useProviderTargeting = driver.FindElement(By.Id("use_providers_targeting-1")); //использовать таргетинг
                                    useProviderTargeting.Click();
                                    break;
                                }
                        }
                    }
                }
                protected bool CheckboxProviderTargetingOther;
                public bool ProviderTargetingOther
                {
                    get { return CheckboxProviderTargetingOther; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("provider-country_0"));
                        webelement.Click();
                        CheckboxProviderTargetingOther = value;
                    }
                }
                protected bool CheckboxProviderTargetingMegafon;
                public bool ProviderTargetingMegafon
                {
                    get { return CheckboxProviderTargetingMegafon; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("provider-country_1"));
                        webelement.Click();
                        CheckboxProviderTargetingMegafon = value;
                    }
                }
                protected bool CheckboxProviderTargetingMtc;
                public bool ProviderTargetingMtc
                {
                    get { return CheckboxProviderTargetingMtc; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("provider-country_2"));
                        webelement.Click();
                        CheckboxProviderTargetingMtc = value;
                    }
                }
            #endregion

            #region Геотаргетинг
                protected string RadioGeoTargeting;
                public string GeoTargeting
                {
                    get { return RadioGeoTargeting; }
                    set
                    {
                        RadioGeoTargeting = value;
                        switch (value)
                        {
                            case "0":
                                {
                                    IWebElement notUseGeoTargeting = driver.FindElement(By.Id("geo-0")); //не использовать таргетинг
                                    notUseGeoTargeting.Click();
                                    break;
                                }
                            case "1":
                                {
                                    IWebElement useGeoTargeting = driver.FindElement(By.Id("geo-1")); //использовать таргетинг
                                    useGeoTargeting.Click();
                                    break;
                                }
                        }
                    }
                }
                protected bool CheckboxGeoTargetingOther;
                public bool GeoTargetingOther
                {
                    get { return CheckboxGeoTargetingOther; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("countries-country_0"));
                        webelement.Click();
                        CheckboxGeoTargetingOther = value;
                    }
                }
                protected bool CheckboxGeoTargetingAustria;
                public bool GeoTargetingAustria
                {
                    get { return CheckboxGeoTargetingAustria; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("countries-country_14"));
                        webelement.Click();
                        CheckboxGeoTargetingAustria = value;
                    }
                }
                protected bool CheckboxGeoTargetingBelorussia;
                public bool GeoTargetingBelorussia
                {
                    get { return CheckboxGeoTargetingBelorussia; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("countries-country_6"));
                        webelement.Click();
                        CheckboxGeoTargetingBelorussia = value;
                    }
                }
                protected bool CheckboxGeoTargetingUk;
                public bool GeoTargetingUk
                {
                    get { return CheckboxGeoTargetingUk; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("countries-country_25"));
                        webelement.Click();
                        CheckboxGeoTargetingUk = value;
                    }
                }
                protected bool CheckboxGeoTargetingGermany;
                public bool GeoTargetingGermany
                {
                    get { return CheckboxGeoTargetingGermany; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("countries-country_7"));
                        webelement.Click();
                        CheckboxGeoTargetingGermany = value;
                    }
                }
                protected bool CheckboxGeoTargetingIsrael;
                public bool GeoTargetingIsrael
                {
                    get { return CheckboxGeoTargetingIsrael; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("countries-country_10"));
                        webelement.Click();
                        CheckboxGeoTargetingIsrael = value;
                    }
                }
                protected bool CheckboxGeoTargetingKazakhstan;
                public bool GeoTargetingKazakhstan
                {
                    get { return CheckboxGeoTargetingKazakhstan; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("countries-country_9"));
                        webelement.Click();
                        CheckboxGeoTargetingKazakhstan = value;
                    }
                }
                protected bool CheckboxGeoTargetingLatvia;
                public bool GeoTargetingLatvia
                {
                    get { return CheckboxGeoTargetingLatvia; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("countries-country_11"));
                        webelement.Click();
                        CheckboxGeoTargetingLatvia = value;
                    }
                }
                protected bool CheckboxGeoTargetingLithuania;
                public bool GeoTargetingLithuania
                {
                    get { return CheckboxGeoTargetingLithuania; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("countries-country_12"));
                        webelement.Click();
                        CheckboxGeoTargetingLithuania = value;
                    }
                }

                #region Россия
                    protected bool ClickGeoTargetingRussia;
                    public bool GeoTargetingRussiaExpand
                    {
                        get { return ClickGeoTargetingRussia; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#countries-country_24 + .hint"));
                            webelement.Click();
                            ClickGeoTargetingRussia = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingRussia;
                    public bool GeoTargetingRussiaChoseAll
                    {
                        get { return CheckboxGeoTargetingRussia; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-country_24"));
                            webelement.Click();
                            CheckboxGeoTargetingRussia = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingRussiaEburg;
                    public bool GeoTargetingRussiaEburg
                    {
                        get { return CheckboxGeoTargetingRussiaEburg; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-24-24"));
                            webelement.Click();
                            CheckboxGeoTargetingRussiaEburg = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingRussiaMoscow;
                    public bool GeoTargetingRussiaMoscow
                    {
                        get { return CheckboxGeoTargetingRussiaMoscow; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-24-4"));
                            webelement.Click();
                            CheckboxGeoTargetingRussiaMoscow = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingRussiaNovosibirsk;
                    public bool GeoTargetingRussiaNovosibirsk
                    {
                        get { return CheckboxGeoTargetingRussiaNovosibirsk; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-24-16"));
                            webelement.Click();
                            CheckboxGeoTargetingRussiaNovosibirsk = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingRussiaOther;
                    public bool GeoTargetingRussiaOther
                    {
                        get { return CheckboxGeoTargetingRussiaOther; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-24-2"));
                            webelement.Click();
                            CheckboxGeoTargetingRussiaOther = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingRussiaSpb;
                    public bool GeoTargetingRussiaSpb
                    {
                        get { return CheckboxGeoTargetingRussiaSpb; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-24-5"));
                            webelement.Click();
                            CheckboxGeoTargetingRussiaSpb = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingRussiaHabarovsk;
                    public bool GeoTargetingRussiaHabarovsk
                    {
                        get { return CheckboxGeoTargetingRussiaHabarovsk; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-24-29"));
                            webelement.Click();
                            CheckboxGeoTargetingRussiaHabarovsk = value;
                        }
                    }
                #endregion

                protected bool CheckboxGeoTargetingUsa;
                public bool GeoTargetingUsa
                {
                    get { return CheckboxGeoTargetingUsa; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("countries-country_8"));
                        webelement.Click();
                        CheckboxGeoTargetingUsa = value;
                    }
                }

                #region Украина
                    protected bool ClickGeoTargetingUkraine;
                    public bool GeoTargetingUkraineExpand
                    {
                        get { return ClickGeoTargetingUkraine; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.CssSelector("#countries-country_28 + .hint"));
                            webelement.Click();
                            ClickGeoTargetingUkraine = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingUkraine;
                    public bool GeoTargetingUkraineChoseAll
                    {
                        get { return CheckboxGeoTargetingUkraine; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-country_28"));
                            webelement.Click();
                            CheckboxGeoTargetingUkraine = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingUkraineDnepr;
                    public bool GeoTargetingUkraineDnepr
                    {
                        get { return CheckboxGeoTargetingUkraineDnepr; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-28-28"));
                            webelement.Click();
                            CheckboxGeoTargetingUkraineDnepr = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingUkraineDonetzk;
                    public bool GeoTargetingUkraineDonetzk
                    {
                        get { return CheckboxGeoTargetingUkraineDonetzk; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-28-23"));
                            webelement.Click();
                            CheckboxGeoTargetingUkraineDonetzk = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingUkraineZakarpattya;
                    public bool GeoTargetingUkraineZakarpattya
                    {
                        get { return CheckboxGeoTargetingUkraineZakarpattya; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-28-17"));
                            webelement.Click();
                            CheckboxGeoTargetingUkraineZakarpattya = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingUkraineKiev;
                    public bool GeoTargetingUkraineKiev
                    {
                        get { return CheckboxGeoTargetingUkraineKiev; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-28-3"));
                            webelement.Click();
                            CheckboxGeoTargetingUkraineKiev = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingUkraineCrimea;
                    public bool GeoTargetingUkraineCrimea
                    {
                        get { return CheckboxGeoTargetingUkraineCrimea; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-28-27"));
                            webelement.Click();
                            CheckboxGeoTargetingUkraineCrimea = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingUkraineLvov;
                    public bool GeoTargetingUkraineLvov
                    {
                        get { return CheckboxGeoTargetingUkraineLvov; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-28-18"));
                            webelement.Click();
                            CheckboxGeoTargetingUkraineLvov = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingUkraineNikolaev;
                    public bool GeoTargetingUkraineNikolaev
                    {
                        get { return CheckboxGeoTargetingUkraineNikolaev; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-28-26"));
                            webelement.Click();
                            CheckboxGeoTargetingUkraineNikolaev = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingUkraineOdessa;
                    public bool GeoTargetingUkraineOdessa
                    {
                        get { return CheckboxGeoTargetingUkraineOdessa; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-28-15"));
                            webelement.Click();
                            CheckboxGeoTargetingUkraineOdessa = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingUkraineOther;
                    public bool GeoTargetingUkraineOther
                    {
                        get { return CheckboxGeoTargetingUkraineOther; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-28-1"));
                            webelement.Click();
                            CheckboxGeoTargetingUkraineOther = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingUkraineHarkov;
                    public bool GeoTargetingUkraineHarkov
                    {
                        get { return CheckboxGeoTargetingUkraineHarkov; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-28-19"));
                            webelement.Click();
                            CheckboxGeoTargetingUkraineHarkov = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingUkraineHerson;
                    public bool GeoTargetingUkraineHerson
                    {
                        get { return CheckboxGeoTargetingUkraineHerson; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-28-20"));
                            webelement.Click();
                            CheckboxGeoTargetingUkraineHerson = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingUkraineCherkassy;
                    public bool GeoTargetingUkraineCherkassy
                    {
                        get { return CheckboxGeoTargetingUkraineCherkassy; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-28-22"));
                            webelement.Click();
                            CheckboxGeoTargetingUkraineCherkassy = value;
                        }
                    }
                    protected bool CheckboxGeoTargetingUkraineChernovzi;
                    public bool GeoTargetingUkraineChernovzi
                    {
                        get { return CheckboxGeoTargetingUkraineChernovzi; }
                        set
                        {
                            IWebElement webelement = driver.FindElement(By.Id("countries-28-21"));
                            webelement.Click();
                            CheckboxGeoTargetingUkraineChernovzi = value;
                        }
                    }
                #endregion

                protected bool CheckboxGeoTargetingEstonia;
                public bool GeoTargetingEstonia
                {
                    get { return CheckboxGeoTargetingEstonia; }
                    set
                    {
                        IWebElement webelement = driver.FindElement(By.Id("countries-country_13"));
                        webelement.Click();
                        CheckboxGeoTargetingEstonia = value;
                    }
                }

                protected string RadioAdditionalGeoTargeting;
                public string AdditionalGeoTargeting
                {
                    get { return RadioAdditionalGeoTargeting; }
                    set
                    {
                        RadioAdditionalGeoTargeting = value;
                        switch (value)
                        {
                            case "0":
                                {
                                    IWebElement notUseAdditionalGeoTargeting = driver.FindElement(By.Id("use_detailed_geo-0")); //не использовать таргетинг
                                    notUseAdditionalGeoTargeting.Click();
                                    break;
                                }
                            case "1":
                                {
                                    IWebElement useAdditionalGeoTargeting = driver.FindElement(By.Id("use_detailed_geo-1")); //использовать таргетинг
                                    useAdditionalGeoTargeting.Click();
                                    break;
                                }
                        }
                    }
                }

                //public List<string> ListAdditionalGeoTargeting()
                //{
                //    List<IWebElement> list = driver.FindElements(By.CssSelector("#detailed-geo-countries li")).ToList();
                //    List<string> result = new List<string>();
                //    for (int i = 0; i < list.Count; i++)
                //    {
                //        result.Add(list[i].Text);
                //    }
                //    return result;
                //}
            #endregion

        #endregion

        public void Submit()
        {
            IWebElement webelement = driver.FindElement(By.Id("sbtbutton"));
            webelement.Click();
        }

        #region Обработка ошибок
            //функцию вызывать сразу после заполнения (SendKeys) полей Даты 
            public List<string> ErrorsInFillFields() //функция обрабатывает выскакивающие мгновенные ошибки заполнения полей Даты. 
            {
                List<string> instantErrorsDate = new List<string>();

                IWebElement dateStartPk = driver.FindElement(By.CssSelector("div#widget_when_autostart")); //Дата старта РК
                string errorDateStartPk = dateStartPk.GetAttribute("class");
                if (errorDateStartPk.Contains("dijitError"))
                    instantErrorsDate.Add("Ошибка заполнения поля \"Дата старта РК\"");

                IWebElement dateEndPk = driver.FindElement(By.CssSelector("div#widget_limit_date")); //Дата окончания РК
                string errorDateEndPk = dateEndPk.GetAttribute("class");
                if (errorDateEndPk.Contains("dijitError"))
                    instantErrorsDate.Add("Ошибка заполнения поля \"Дата окончания РК\"");

                return instantErrorsDate;
            }

            public string DayLimitClicksPkError() //функция поиска ошибки заполнения поля "Ограничения рекламной кампании"
            {
                string result;
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5)); //явное ожидание
                try
                {
                    IWebElement webelement = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("#message1 .message")));
                    result = webelement.Text;
                    return result; //удалось отследить всплывающую ошибку
                }
                catch
                {
                    result = "";
                    return result;
                }
            }

            public string CrossError() //функция поиска ошибки заполнения поля "UTM-разметка пользователя"
            {
                try
                {
                    IWebElement webelement = driver.FindElement(By.Id("error_image"));//рядом с полем появился крестик
                    return webelement.GetAttribute("title");
                }
                catch (Exception)
                {
                    return "";
                }
            }

            public List<string> GetErrors() //функцию вызывать после нажатия Сохранить/Завершить/Submit
            {
                List<IWebElement> list = driver.FindElements(By.CssSelector(".errors > li")).ToList(); //проверка, есть ли на странице ошибки заполнения _обязательных_ полей
                List<string> result = new List<string>();
                for (int i = 0; i < list.Count; i++)
                    result.Add(list[i].Text);

                string crossError = CrossError();//функция поиска ошибки заполнения поля "UTM-разметка пользователя"
                if (crossError != "") result.Add(crossError);

                string dayLimitClicksPkError = DayLimitClicksPkError();//функция поиска ошибки заполнения поля "Ограничения рекламной кампании"
                if (dayLimitClicksPkError != "") result.Add(dayLimitClicksPkError);

                return result;
            }
        #endregion
    }
}
