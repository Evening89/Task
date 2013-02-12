using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Task.Utils;
using Task.Model;
using Task.View;

namespace Task.Controller
{
    public class GoodsEditPk_Controller
    {
        IWebDriver driver;
        public string baseUrl = "https://admin.dt00.net/cab/goodhits/campaigns-edit/id/" + Registry.hashTable["pkId"] + "/filters/%252Fclient_id%252F" + Registry.hashTable["clientId"];
        public List<string> errors = new List<string>(); //список ошибок (в каждой строке - спарсенное со страницы описание ошибки)
        Randoms randoms = new Randoms(); //класс генерации случайных строк

        public string namePk;
        public string dateStartPk;
        public string dateEndPk;

        public string dayLimitByBudget;
        public string generalLimitByBudget;

        public string dayLimitByClicks;
        public string generalLimitByClicks;

        public string utmMedium;
        public string utmSource;
        public string utmCampaign;

        public string utmUserStr;

        public string idOfPlatformInLink;

        public string commentsForPk;

        public int chosenVariantLimitsPk;
        public int chosenVariantDemoTargeting;
        public int chosenVariantInterestsTargeting;
        public int chosenVariantBrowserTargeting;
        public int chosenVariantOsTargeting;
        public int chosenVariantProviderTargeting;
        public int chosenVariantGeoTargeting;
        public int variant;

        GoodsEditPK_Model pkEditModel = new GoodsEditPK_Model();

        public void EditPk()
        {
            driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный ранее драйвер
            driver.Navigate().GoToUrl(baseUrl); //заходим по ссылке
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            pkEditModel.driver = driver;

            LogTrace.WriteInLog(Goods_View.tab2 + driver.Url);

            #region Редактирование полей

                #region Разное
                    if (!pkEditModel.GetViewSensors) //если checkbox не выбран...
                    {
                        pkEditModel.ViewSensors = true; //...выбираем его
                        LogTrace.WriteInLog(Goods_View.tab2 + "Выбран checkbox Просмотр датчиков");
                    }

                    if(!pkEditModel.GetViewConversion)
                    {
                        pkEditModel.ViewConversion = true;
                        LogTrace.WriteInLog(Goods_View.tab2 + "Выбран checkbox Просмотр конверсии");
                    }

                    namePk = randoms.RandomString(15) + " " + randoms.RandomNumber(5);
                    pkEditModel.Name = namePk;
                    LogTrace.WriteInLog(Goods_View.tab2 + "Заполняю поле Название РК. Было введено: " + pkEditModel.Name);

                    //dateStartPk = pkEditModel.GenerateDate();
                    pkEditModel.StartPkDate = pkEditModel.GenerateDate();
                    driver.FindElement(By.Id("when_autostart")).Click();//чтобы обновилось содержимое полей и к месяцам и дням < 10 добавились 0
                    driver.FindElement(By.Id("editsite")).Click();
                    dateStartPk = pkEditModel.GetStartPkDate;
                    LogTrace.WriteInLog(Goods_View.tab2 + "Заполняю поле Дата старта РК. Было введено: " + pkEditModel.StartPkDate);

                    //dateEndPk = pkEditModel.GenerateDate();
                    pkEditModel.EndPkDate = pkEditModel.GenerateDate();
                    driver.FindElement(By.Id("limit_date")).Click();//чтобы обновилось содержимое полей и к месяцам и дням < 10 добавились 0
                    driver.FindElement(By.Id("editsite")).Click();
                    dateEndPk = pkEditModel.GetEndPkDate;
                    LogTrace.WriteInLog(Goods_View.tab2 + "Заполняю поле Дата окончания РК. Было введено: " + pkEditModel.EndPkDate);
                    List<string> instantErrorsDate = pkEditModel.ErrorsInFillFields();
                    if (instantErrorsDate.Count != 0) //если список с ошибками заполнения полей даты непуст
                        errors = instantErrorsDate; //копируем в нас общий список ошибок errors

                    //if(!pkEditModel.GetBlockTeasersAfterCreation)
                    //{
                    //    pkEditModel.BlockTeasersAfterCreation = true;
                    //    LogTrace.WriteInLog(Goods_View.tab2 + "Выбран checkbox Блокировать тизеры после их создания");
                    //}

                    if(!pkEditModel.GetStoppedByManager)
                    {
                        pkEditModel.StoppedByManager = true;
                        LogTrace.WriteInLog(Goods_View.tab2 + "Выбран checkbox Остановлена менеджером");
                    }
                #endregion

                #region Ограничения рекламной кампании
                    variant = chosenVariantLimitsPk = needSetRadioButton(3);
                    pkEditModel.LimitsOfPk = variant;
                    switch (variant)
                    {
                        case 0:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Ограничения рекламной кампании. Выбрано: не использовать");
                                break;
                            }
                        case 1:
                            {
                                int num2;
                                do
                                {
                                    num2 = int.Parse(randoms.RandomNumber(2));
                                } while (num2 < 5); //суточный лимит должен быть не менее 5
                                LogTrace.WriteInLog("          Выбираю radiobutton Ограничения рекламной кампании. Выбрано: по бюджету");
                                
                                dayLimitByBudget = num2.ToString() + ".00";//суточный лимит должен быть не менее 5
                                pkEditModel.DayLimitByBudget = dayLimitByBudget;
                                LogTrace.WriteInLog("             Заполняю поле Суточный лимит РК. Было введено: " + pkEditModel.DayLimitByBudget);

                                generalLimitByBudget = randoms.RandomNumber(3) + ".00";
                                pkEditModel.GeneralLimitByBudget = generalLimitByBudget;
                                LogTrace.WriteInLog("             Заполняю поле Общий лимит РК. Было введено: " + pkEditModel.GeneralLimitByBudget);
                                break;
                            }
                        case 2:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Ограничения рекламной кампании. Выбрано: по количеству кликов");
                                
                                dayLimitByClicks = randoms.RandomNumber(3);
                                pkEditModel.DayLimitByClicks = dayLimitByClicks;
                                LogTrace.WriteInLog("             Заполняю поле Суточный лимит кликов РК. Было введено: " + pkEditModel.DayLimitByClicks);
                                
                                generalLimitByClicks = randoms.RandomNumber(3);
                                pkEditModel.GeneralLimitByClicks = generalLimitByClicks;
                                LogTrace.WriteInLog("             Заполняю поле Общий лимит кликов РК. Было введено: " + pkEditModel.GeneralLimitByClicks);
                                break;
                            }
                    }
                #endregion

                #region UTM-разметка рекламной кампании для Google Analytics
                    if (!pkEditModel.GetUtmPkForGoogleAnalytics)
                    {
                        pkEditModel.UtmPkForGoogleAnalytics = true;
                        LogTrace.WriteInLog("          Выбран checkbox UTM-разметка рекламной кампании для Google Analytics");
                    }   
                    utmMedium = randoms.RandomString(5);
                    pkEditModel.UtmMedium = utmMedium;
                    LogTrace.WriteInLog("             Заполняю поле utm_medium (средство кампании). Было введено: " + pkEditModel.UtmMedium);
                        
                    utmSource = randoms.RandomString(5);
                    pkEditModel.UtmSource = utmSource;
                    LogTrace.WriteInLog("             Заполняю поле utm_source (источник кампании). Было введено: " + pkEditModel.UtmSource);
                        
                    utmCampaign = randoms.RandomString(5);
                    pkEditModel.UtmCampaign = utmCampaign;
                    LogTrace.WriteInLog("             Заполняю поле utm_campaign (название кампании). Было введено: " + pkEditModel.UtmCampaign);
                #endregion

                #region UTM-разметка пользователя
                    if (!pkEditModel.GetUtmUser)
                    {
                        pkEditModel.UtmUser = true;
                        LogTrace.WriteInLog("          Выбран checkbox UTM-разметка пользователя");
                    }
                    utmUserStr = randoms.RandomString(5);
                    pkEditModel.UtmUserStr = utmUserStr;
                    LogTrace.WriteInLog("             Заполняю поле UTM-разметка пользователя. Было введено: " + pkEditModel.UtmUserStr);
                #endregion

                #region Крутить в сети Товарро
                    if (!pkEditModel.GetScrewInTovarro)
                    {
                        LogTrace.WriteInLog("          Выбран checkbox Крутить в сети Товарро");
                        pkEditModel.ScrewInTovarro = true;
                    }
                #endregion

                #region Блокировка по расписанию
                    if (!pkEditModel.GetBlockBySchedule)
                    {
                        pkEditModel.BlockBySchedule = true;
                        LogTrace.WriteInLog("          Выбран checkbox Блокировка по расписанию");
                    }
                    if (!pkEditModel.GetWeekends)
                    {
                        pkEditModel.Weekends = true;
                        LogTrace.WriteInLog("             Выбран checkbox Выходные");
                    }
                    if (!pkEditModel.GetWeekdays)
                    {
                        pkEditModel.Weekdays = true;
                        LogTrace.WriteInLog("             Выбран checkbox Будни");
                    }
                    if (!pkEditModel.GetWorkingTime)
                    {
                        pkEditModel.WorkingTime = true;
                        LogTrace.WriteInLog("             Выбран checkbox Рабочее время (9-18 по будням)");
                    }
                #endregion

                #region Передавать id площадки в ссылке
                    if (!pkEditModel.GetIdOfPlatformInLink)
                    {
                        pkEditModel.IdOfPlatformInLink = true;
                        LogTrace.WriteInLog("          Выбран checkbox Передавать id площадки в ссылке");
                    }
                    idOfPlatformInLink = randoms.RandomString(5);
                    pkEditModel.IdOfPlatformInLinkStr = idOfPlatformInLink;
                    LogTrace.WriteInLog("             Заполняю поле Передавать id площадки в ссылке. Было введено: " + pkEditModel.IdOfPlatformInLinkStr);
                #endregion

                #region Передавать id площадки в ссылке
                    if (!pkEditModel.GetAddIdOfTeaserInLink)
                    {
                        pkEditModel.AddIdOfTeaserInLink = true;
                        LogTrace.WriteInLog("          Выбран checkbox Добавлять id тизера в конец ссылки");
                    }
                #endregion

                #region Комментарий к кампании
                    commentsForPk = randoms.RandomString(20) + " " + randoms.RandomString(10);
                    pkEditModel.CommentsForPk = commentsForPk;
                    LogTrace.WriteInLog("          Заполняю textarea Комментарий к кампании. Было введено: " + pkEditModel.CommentsForPk);
                #endregion

                #region Площадки
                    if (!pkEditModel.GetPlatforms)
                    {
                        pkEditModel.Platforms = true;
                        LogTrace.WriteInLog("          Выбран checkbox Площадки");
                    }
                        if (!pkEditModel.GetPlatformsNotSpecified)
                        {
                            pkEditModel.PlatformsNotSpecified = true;
                            LogTrace.WriteInLog("             Выбран checkbox Не определено");
                        }
                        if (!pkEditModel.GetPlatformsPolitics)
                        {
                            pkEditModel.PlatformsPolitics = true;
                            LogTrace.WriteInLog("             Выбран checkbox Политика, общество, происшествия, религия");
                        }

                        if (!pkEditModel.GetPlatformsEconomics)
                        {
                            pkEditModel.PlatformsEconomics = true;
                            LogTrace.WriteInLog("             Выбран checkbox Экономика, финансы, недвижимость, работа и карьера");
                        }
                        if (!pkEditModel.GetPlatformsCelebrities)
                        {
                            pkEditModel.PlatformsCelebrities = true;
                            LogTrace.WriteInLog("             Выбран checkbox Знаменитости, шоу-бизнес, кино, музыка");
                        }
                        if (!pkEditModel.GetPlatformsScience)
                        {
                            pkEditModel.PlatformsScience = true;
                            LogTrace.WriteInLog("             Выбран checkbox Наука и технологии");
                        }
                        if (!pkEditModel.GetPlatformsConnection)
                        {
                            pkEditModel.PlatformsConnection = true;
                            LogTrace.WriteInLog("             Выбран checkbox Связь, компьютеры, программы");
                        }
                        if (!pkEditModel.GetPlatformsSports)
                        {
                            pkEditModel.PlatformsSports = true;
                            LogTrace.WriteInLog("             Выбран checkbox Спорт");
                        }
                        if (!pkEditModel.GetPlatformsAuto)
                        {
                            pkEditModel.PlatformsAuto = true;
                            LogTrace.WriteInLog("             Выбран checkbox Авто-вело-мото");
                        }
                        if (!pkEditModel.GetPlatformsFashion)
                        {
                            pkEditModel.PlatformsFashion = true;
                            LogTrace.WriteInLog("             Выбран checkbox Мода и стиль, здоровье и красота, фитнес и диета, кулинария");
                        }
                        if (!pkEditModel.GetPlatformsMedicine)
                        {
                            pkEditModel.PlatformsMedicine = true;
                            LogTrace.WriteInLog("             Выбран checkbox Медицина");
                        }
                        if (!pkEditModel.GetPlatformsTourism)
                        {
                            pkEditModel.PlatformsTourism = true;
                            LogTrace.WriteInLog("             Выбран checkbox Туризм и отдых (путевки, отели, рестораны)");
                        }
                        if (!pkEditModel.GetPlatformsGlobalPortals)
                        {
                            pkEditModel.PlatformsGlobalPortals = true;
                            LogTrace.WriteInLog("             Выбран checkbox Глобальные порталы с множеством подпроектов");
                        }
                        if (!pkEditModel.GetPlatformsHumor)
                        {
                            pkEditModel.PlatformsHumor = true;
                            LogTrace.WriteInLog("             Выбран checkbox Юмор (приколы, картинки, обои), каталог фотографий, блоги");
                        }
                        if (!pkEditModel.GetPlatformsFileshares)
                        {
                            pkEditModel.PlatformsFileshares = true;
                            LogTrace.WriteInLog("             Выбран checkbox Файлообменники, файлокачалки (кино, музыка, игры, программы)");
                        }
                        if (!pkEditModel.PlatformsSocialNetworks)
                        {
                            pkEditModel.PlatformsSocialNetworks = true;
                            LogTrace.WriteInLog("             Выбран checkbox Социальные сети, сайты знакомства, личные дневники");
                        }
                        if (!pkEditModel.GetPlatformsAnimals)
                        {
                            pkEditModel.PlatformsAnimals = true;
                            LogTrace.WriteInLog("             Выбран checkbox Животный и растительный мир");
                        }
                        if (!pkEditModel.GetPlatformsReligion)
                        {
                            pkEditModel.PlatformsReligion = true;
                            LogTrace.WriteInLog("             Выбран checkbox Религия");
                        }
                        if (!pkEditModel.GetPlatformsChildren)
                        {
                            pkEditModel.PlatformsChildren = true;
                            LogTrace.WriteInLog("             Выбран checkbox Дети и родители");
                        }
                        if (!pkEditModel.GetPlatformsBuilding)
                        {
                            pkEditModel.PlatformsBuilding = true;
                            LogTrace.WriteInLog("             Выбран checkbox Строительство, ремонт, дача, огород");
                        }
                        if (!pkEditModel.GetPlatformsCookery)
                        {
                            pkEditModel.PlatformsCookery = true;
                            LogTrace.WriteInLog("             Выбран checkbox Кулинария");
                        }
                        if (!pkEditModel.GetPlatformsJob)
                        {
                            pkEditModel.PlatformsJob = true;
                            LogTrace.WriteInLog("             Выбран checkbox Работа и карьера. Поиск работы, поиск персонала");
                        }
                        if (!pkEditModel.GetPlatformsNotSites)
                        {
                            pkEditModel.PlatformsNotSites = true;
                            LogTrace.WriteInLog("             Выбран checkbox Не сайты (программы, тулбары, таскбары)");
                        }
                        if (!pkEditModel.GetPlatformsSitesStartPagesBrowsers)
                        {
                            pkEditModel.PlatformsSitesStartPagesBrowsers = true;
                            LogTrace.WriteInLog("             Выбран checkbox Сайты, размещенные на стартовых страницах браузеров");
                        }
                        if (!pkEditModel.GetPlatformsSearchSystems)
                        {
                            pkEditModel.PlatformsSearchSystems = true;
                            LogTrace.WriteInLog("             Выбран checkbox Поисковые системы");
                        }
                        if (!pkEditModel.GetPlatformsEmail)
                        {
                            pkEditModel.PlatformsEmail = true;
                            LogTrace.WriteInLog("             Выбран checkbox Почта");
                        }
                        if (!pkEditModel.GetPlatformsPhotoCatalogues)
                        {
                            pkEditModel.PlatformsPhotoCatalogues = true;
                            LogTrace.WriteInLog("             Выбран checkbox Каталоги фотографий");
                        }
                        if (!pkEditModel.GetPlatformsVarez)
                        {
                            pkEditModel.PlatformsVarez = true;
                            LogTrace.WriteInLog("             Выбран checkbox Варезники");
                        }
                        if (!pkEditModel.GetPlatformsOnlineVideo)
                        {
                            pkEditModel.PlatformsOnlineVideo = true;
                            LogTrace.WriteInLog("             Выбран checkbox Онлайн видео, телевидение, радио");
                        }
                        if (!pkEditModel.GetPlatformsOnlineLibraries)
                        {
                            pkEditModel.PlatformsOnlineLibraries = true;
                            LogTrace.WriteInLog("             Выбран checkbox Онлайн-библиотеки");
                        }
                        if (!pkEditModel.GetPlatformsInternet)
                        {
                            pkEditModel.PlatformsInternet = true;
                            LogTrace.WriteInLog("             Выбран checkbox Интернет, поисковые сайты, электронная почта, интернет-магазины, аукционы, каталоги ресурсов, фирм и предприятий");
                        }
                        if (!pkEditModel.GetPlatformsOnlineGames)
                        {
                            pkEditModel.PlatformsOnlineGames = true;
                            LogTrace.WriteInLog("             Выбран checkbox Онлайн игры");
                        }
                        if (!pkEditModel.GetPlatformsInternetRepresentatives)
                        {
                            pkEditModel.PlatformsInternetRepresentatives = true;
                            LogTrace.WriteInLog("             Выбран checkbox Интернет-представительства бизнеса.");
                        }
                        if (!pkEditModel.GetPlatformsProgramms)
                        {
                            pkEditModel.PlatformsProgramms = true;
                            LogTrace.WriteInLog("             Выбран checkbox Программы, прошивки, игры для КПК и мобильных устройств");
                        }
                        if (!pkEditModel.GetPlatformsCataloguesInternetResources)
                        {
                            pkEditModel.PlatformsCataloguesInternetResources = true;
                            LogTrace.WriteInLog("             Выбран checkbox Каталоги Интернет - ресурсов, фирм и предприятий");
                        }
                        if (!pkEditModel.GetPlatformsForWagesInInternet)
                        {
                            pkEditModel.PlatformsForWagesInInternet = true;
                            LogTrace.WriteInLog("             Выбран checkbox Для заработка в Интернете. Партнерские программы");
                        }
                        if (!pkEditModel.GetPlatformsHobbies)
                        {
                            pkEditModel.PlatformsHobbies = true;
                            LogTrace.WriteInLog("             Выбран checkbox Хобби и увлечения");
                        }
                        if (!pkEditModel.GetPlatformsMarketgid)
                        {
                            pkEditModel.PlatformsMarketgid = true;
                            LogTrace.WriteInLog("             Выбран checkbox Маркетгид");
                        }
                        if (!pkEditModel.GetPlatformsShock)
                        {
                            pkEditModel.PlatformsShock = true;
                            LogTrace.WriteInLog("             Выбран checkbox Шокодром");
                        }
                        if (!pkEditModel.GetPlatformsEsoteric)
                        {
                            pkEditModel.PlatformsEsoteric = true;
                            LogTrace.WriteInLog("             Выбран checkbox Эзотерика. Непознанное, астрология, гороскопы, гадания");
                        }
                        if (!pkEditModel.GetPlatformsPsychology)
                        {
                            pkEditModel.PlatformsPsychology = true;
                            LogTrace.WriteInLog("             Выбран checkbox Психология, мужчина и женщина");
                        }
                        if (!pkEditModel.GetPlatformsHistory)
                        {
                            pkEditModel.PlatformsHistory = true;
                            LogTrace.WriteInLog("             Выбран checkbox История, образование, культура");
                        }
                        if (!pkEditModel.GetPlatformsMarketgidWomenNet)
                        {
                            pkEditModel.PlatformsMarketgidWomenNet = true;
                            LogTrace.WriteInLog("             Выбран checkbox Маркетгид ЖС");
                        }
                #endregion

                #region Демографический таргетинг
                    variant = chosenVariantDemoTargeting = needSetRadioButton(2);
                    pkEditModel.DemoTargeting = variant;
                    switch (variant)
                    {
                        case 0:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Демографический таргетинг. Выбрано: не использовать");
                                break;
                            }
                        case 1:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Демографический таргетинг. Выбрано: использовать");
                                //развернуть все пункты (Мужчины, Женщины, Пол не определен)
                                pkEditModel.DemoTargetingMenExpand = true;
                                pkEditModel.DemoTargetingWomenExpand = true;
                                pkEditModel.DemoTargetingHermaphroditeExpand = true;

                                #region Мужчины
                                    //pkEditModel.DemoTargetingMenNotSpecified = true;
                                    if (!pkEditModel.GetDemoTargetingMenChoseAll)
                                    {
                                        pkEditModel.DemoTargetingMenChoseAll = true;
                                        LogTrace.WriteInLog("             Мужчины. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetDemoTargetingMenNotSpecified)
                                    {
                                        pkEditModel.DemoTargetingMenNotSpecified = true;
                                        LogTrace.WriteInLog("             Мужчины. Выбран checkbox Не определен");
                                    }
                                    if (!pkEditModel.GetDemoTargetingMen618)
                                    {
                                        pkEditModel.DemoTargetingMen618 = true;
                                        LogTrace.WriteInLog("             Мужчины. Выбран checkbox 6-18");
                                    }     
                                    if (!pkEditModel.GetDemoTargetingMen1924)
                                    {
                                        pkEditModel.DemoTargetingMen1924 = true;
                                        LogTrace.WriteInLog("             Мужчины. Выбран checkbox 19-24");
                                    }
                                    if (!pkEditModel.GetDemoTargetingMen2534)
                                    {
                                        pkEditModel.DemoTargetingMen2534 = true;
                                        LogTrace.WriteInLog("             Мужчины. Выбран checkbox 25-34");
                                    }
                                    if (!pkEditModel.GetDemoTargetingMen3544)
                                    {
                                        pkEditModel.DemoTargetingMen3544 = true;
                                        LogTrace.WriteInLog("             Мужчины. Выбран checkbox 35-44");
                                    }
                                    if (!pkEditModel.GetDemoTargetingMen4590)
                                    {
                                        pkEditModel.DemoTargetingMen4590 = true;
                                        LogTrace.WriteInLog("             Мужчины. Выбран checkbox 45-90");
                                    }
                                #endregion

                                #region Женщины
                                    if (!pkEditModel.GetDemoTargetingWomenChoseAll)
                                    {
                                        pkEditModel.DemoTargetingWomenChoseAll = true;
                                        LogTrace.WriteInLog("             Женщины. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetDemoTargetingWomenNotSpecified)
                                    {
                                        pkEditModel.DemoTargetingWomenNotSpecified = true;
                                        LogTrace.WriteInLog("             Женщины. Выбран checkbox Не определен");
                                    }
                                    if (!pkEditModel.GetDemoTargetingWomen618)
                                    {
                                        pkEditModel.DemoTargetingWomen618 = true;
                                        LogTrace.WriteInLog("             Женщины. Выбран checkbox 6-18");
                                    }
                                    if (!pkEditModel.GetDemoTargetingWomen1924)
                                    {
                                        pkEditModel.DemoTargetingWomen1924 = true;
                                        LogTrace.WriteInLog("             Женщины. Выбран checkbox 19-24");
                                    }
                                    if (!pkEditModel.GetDemoTargetingWomen2534)
                                    {
                                        pkEditModel.DemoTargetingWomen2534 = true;
                                        LogTrace.WriteInLog("             Женщины. Выбран checkbox 25-34");
                                    }
                                    if (!pkEditModel.GetDemoTargetingWomen3544)
                                    {
                                        pkEditModel.DemoTargetingWomen3544 = true;
                                        LogTrace.WriteInLog("             Женщины. Выбран checkbox 35-44");
                                    }
                                    if (!pkEditModel.GetDemoTargetingWomen4590)
                                    {
                                        pkEditModel.DemoTargetingWomen4590 = true;
                                        LogTrace.WriteInLog("             Женщины. Выбран checkbox 45-90");
                                    }
                                #endregion

                                #region Пол не определен
                                    if (!pkEditModel.GetDemoTargetingHermaphroditeChoseAll)
                                    {
                                        pkEditModel.DemoTargetingHermaphroditeChoseAll = true;
                                        LogTrace.WriteInLog("             Пол не определен. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetDemoTargetingHermaphrodite618)
                                    {
                                        pkEditModel.DemoTargetingHermaphrodite618 = true;
                                        LogTrace.WriteInLog("             Пол не определен. Выбран checkbox 6-18");
                                    }
                                    if (!pkEditModel.GetDemoTargetingHermaphrodite1924)
                                    {
                                        pkEditModel.DemoTargetingHermaphrodite1924 = true;
                                        LogTrace.WriteInLog("             Пол не определен. Выбран checkbox 19-24");
                                    }
                                    if (!pkEditModel.GetDemoTargetingHermaphrodite2534)
                                    {
                                        pkEditModel.DemoTargetingHermaphrodite2534 = true;
                                        LogTrace.WriteInLog("             Пол не определен. Выбран checkbox 25-34");
                                    }
                                    if (!pkEditModel.GetDemoTargetingHermaphrodite3544)
                                    {
                                        pkEditModel.DemoTargetingHermaphrodite3544 = true;
                                        LogTrace.WriteInLog("             Пол не определен. Выбран checkbox 35-44");
                                    }
                                    if (!pkEditModel.GetDemoTargetingHermaphrodite4590)
                                    {
                                        pkEditModel.DemoTargetingHermaphrodite4590 = true;
                                        LogTrace.WriteInLog("             Пол не определен. Выбран checkbox 45-90");
                                    }
                                #endregion

                                break;
                            }
                    }
                #endregion

                #region Таргетинг по интересам
                    variant = chosenVariantInterestsTargeting = needSetRadioButton(2);
                    pkEditModel.InterestsTargeting = variant;
                    switch (variant)
                    {
                        case 0:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Таргетинг по интересам. Выбрано: не использовать");
                                break;
                            }
                        case 1:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Таргетинг по интересам. Выбрано: использовать");
                                pkEditModel.InterestsTargetingBusinessExpand = true;
                                pkEditModel.InterestsTargetingRealtyExpand = true;
                                pkEditModel.InterestsTargetingEducationExpand = true;
                                pkEditModel.InterestsTargetingRestExpand = true;
                                pkEditModel.InterestsTargetingTelephonesExpand = true;
                                pkEditModel.InterestsTargetingMedicineExpand = true;
                                pkEditModel.InterestsTargetingHouseExpand = true;
                                pkEditModel.InterestsTargetingFinanceExpand = true;
                                pkEditModel.InterestsTargetingComputersExpand = true;
                                pkEditModel.InterestsTargetingAutoExpand = true;
                                pkEditModel.InterestsTargetingAudioExpand = true;

                                //pkEditModel.InterestsTargetingOther = true;
                                if (!pkEditModel.GetInterestsTargetingOther)
                                {
                                    pkEditModel.InterestsTargetingOther = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Прочее");
                                }

                                #region Бизнес
                                    if (!pkEditModel.GetInterestsTargetingBusinessChoseAll)
                                    {
                                        pkEditModel.InterestsTargetingBusinessChoseAll = true;
                                        LogTrace.WriteInLog("             Бизнес. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingBusinessAcoountancy)
                                    {
                                        pkEditModel.InterestsTargetingBusinessAcoountancy = true;
                                        LogTrace.WriteInLog("             Бизнес. Выбран checkbox Бухгалтерия");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingBusinessPlacement)
                                    {
                                        pkEditModel.InterestsTargetingBusinessPlacement = true;
                                        LogTrace.WriteInLog("             Бизнес. Выбран checkbox Трудоустройство, персонал");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingBusinessAudit)
                                    {
                                        pkEditModel.InterestsTargetingBusinessAudit = true;
                                        LogTrace.WriteInLog("             Бизнес. Выбран checkbox Аудит, консалтинг");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingBusinessAdverts)
                                    {
                                        pkEditModel.InterestsTargetingBusinessAdverts = true;
                                        LogTrace.WriteInLog("             Бизнес. Выбран checkbox Реклама");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingBusinessMiscellanea)
                                    {
                                        pkEditModel.InterestsTargetingBusinessMiscellanea = true;
                                        LogTrace.WriteInLog("             Бизнес. Выбран checkbox Разное");
                                    }
                                #endregion

                                #region Недвижимость
                                    if (!pkEditModel.GetInterestsTargetingRealtyChoseAll)
                                    {
                                        pkEditModel.InterestsTargetingRealtyChoseAll = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingRealtyMiscelanea)
                                    {
                                        pkEditModel.InterestsTargetingRealtyMiscelanea = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Разное");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingRealtyGarages)
                                    {
                                        pkEditModel.InterestsTargetingRealtyGarages = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Гаражи");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingRealtyFlats)
                                    {
                                        pkEditModel.InterestsTargetingRealtyFlats = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Квартиры");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingRealtyAbroad)
                                    {
                                        pkEditModel.InterestsTargetingRealtyAbroad = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Зарубежная недвижимость");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingRealtyLand)
                                    {
                                        pkEditModel.InterestsTargetingRealtyLand = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Земля");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingRealtySuburban)
                                    {
                                        pkEditModel.InterestsTargetingRealtySuburban = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Загородная недвижимость");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingRealtyHypothec)
                                    {
                                        pkEditModel.InterestsTargetingRealtyHypothec = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Ипотека");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingRealtyCommerce)
                                    {
                                        pkEditModel.InterestsTargetingRealtyCommerce = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Коммерческая недвижимость");
                                    }
                                #endregion

                                if (!pkEditModel.GetInterestsTargetingExhibitions)
                                {
                                    pkEditModel.InterestsTargetingExhibitions = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Выставки, концерты, театры, кино");
                                }

                                #region Образование
                                    if (!pkEditModel.GetInterestsTargetingEducationChoseAll)
                                    {
                                        pkEditModel.InterestsTargetingEducationChoseAll = true;
                                        LogTrace.WriteInLog("             Образование. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingEducationForeignLanguages)
                                    {
                                        pkEditModel.InterestsTargetingEducationForeignLanguages = true;
                                        LogTrace.WriteInLog("             Образование. Выбран checkbox Иностранные языки");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingEducationAbroad)
                                    {
                                        pkEditModel.InterestsTargetingEducationAbroad = true;
                                        LogTrace.WriteInLog("             Образование. Выбран checkbox Образование за рубежом");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingEducationHigh)
                                    {
                                        pkEditModel.InterestsTargetingEducationHigh = true;
                                        LogTrace.WriteInLog("             Образование. Выбран checkbox Образование высшее");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingEducationMiscelanea)
                                    {
                                        pkEditModel.InterestsTargetingEducationMiscelanea = true;
                                        LogTrace.WriteInLog("             Образование. Выбран checkbox Разное");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingEducationChildren)
                                    {
                                        pkEditModel.InterestsTargetingEducationChildren = true;
                                        LogTrace.WriteInLog("             Образование. Выбран checkbox Образование для детей");
                                    }
                                #endregion

                                #region Отдых, туризм, путешествия
                                    if (!pkEditModel.GetInterestsTargetingRestChoseAll)
                                    {
                                        pkEditModel.InterestsTargetingRestChoseAll = true;
                                        LogTrace.WriteInLog("             Отдых, туризм, путешествия. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingRestMiscellanea)
                                    {
                                        pkEditModel.InterestsTargetingRestMiscellanea = true;
                                        LogTrace.WriteInLog("             Отдых, туризм, путешествия. Выбран checkbox Разное");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingRestRuUa)
                                    {
                                        pkEditModel.InterestsTargetingRestRuUa = true;
                                        LogTrace.WriteInLog("             Отдых, туризм, путешествия. Выбран checkbox Отдых в России и Украине");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingRestAbroad)
                                    {
                                        pkEditModel.InterestsTargetingRestAbroad = true;
                                        LogTrace.WriteInLog("             Отдых, туризм, путешествия. Выбран checkbox Отдых за рубежом");
                                    }
                                #endregion

                                #region Телефоны, связь
                                    if (!pkEditModel.GetInterestsTargetingTelephonesChoseAll)
                                    {
                                        pkEditModel.InterestsTargetingTelephonesChoseAll = true;
                                        LogTrace.WriteInLog("             Телефоны, связь. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingTelephonesMiscellanea)
                                    {
                                        pkEditModel.InterestsTargetingTelephonesMiscellanea = true;
                                        LogTrace.WriteInLog("             Телефоны, связь. Выбран checkbox Разное");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingTelephonesNavigation)
                                    {
                                        pkEditModel.InterestsTargetingTelephonesNavigation = true;
                                        LogTrace.WriteInLog("             Телефоны, связь. Выбран checkbox Навигация");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingTelephonesMobileApps)
                                    {
                                        pkEditModel.InterestsTargetingTelephonesMobileApps = true;
                                        LogTrace.WriteInLog("             Телефоны, связь. Выбран checkbox Мобильные приложения и услуги");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingTelephonesMobile)
                                    {
                                        pkEditModel.InterestsTargetingTelephonesMobile = true;
                                        LogTrace.WriteInLog("             Телефоны, связь. Выбран checkbox Мобильные телефоны");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingTelephonesStationary)
                                    {
                                        pkEditModel.InterestsTargetingTelephonesStationary = true;
                                        LogTrace.WriteInLog("             Телефоны, связь. Выбран checkbox Стационарная связь");
                                    }
                                #endregion

                                if (!pkEditModel.GetInterestsTargetingHouseAplliances)
                                {
                                    pkEditModel.InterestsTargetingHouseAplliances = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Бытовая техника");
                                }
                                

                                #region Медицина, здоровье
                                    if (!pkEditModel.GetInterestsTargetingMedicineChoseAll)
                                    {
                                        pkEditModel.InterestsTargetingMedicineChoseAll = true;
                                        LogTrace.WriteInLog("             Медицина, здоровье. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingMedicineSport)
                                    {
                                        pkEditModel.InterestsTargetingMedicineSport = true;
                                        LogTrace.WriteInLog("             Медицина, здоровье. Выбран checkbox Спорт, фитнес, йога");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingMedicineEyesight)
                                    {
                                        pkEditModel.InterestsTargetingMedicineEyesight = true;
                                        LogTrace.WriteInLog("             Медицина, здоровье. Выбран checkbox Зрение");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingMedicineMiscellanea)
                                    {
                                        pkEditModel.InterestsTargetingMedicineMiscellanea = true;
                                        LogTrace.WriteInLog("             Медицина, здоровье. Выбран checkbox Разное");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingMedicineDiets)
                                    {
                                        pkEditModel.InterestsTargetingMedicineDiets = true;
                                        LogTrace.WriteInLog("             Медицина, здоровье. Выбран checkbox Диеты");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingMedicineExtraWeight)
                                    {
                                        pkEditModel.InterestsTargetingMedicineExtraWeight = true;
                                        LogTrace.WriteInLog("             Медицина, здоровье. Выбран checkbox Лишний вес");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingMedicinePregnancy)
                                    {
                                        pkEditModel.InterestsTargetingMedicinePregnancy = true;
                                        LogTrace.WriteInLog("             Медицина, здоровье. Выбран checkbox Беременность и роды");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingMedicineStomatology)
                                    {
                                        pkEditModel.InterestsTargetingMedicineStomatology = true;
                                        LogTrace.WriteInLog("             Медицина, здоровье. Выбран checkbox Стоматология");
                                    }
                                #endregion

                                #region Дом и семья
                                    if (!pkEditModel.GetInterestsTargetingHouseChoseAll)
                                    {
                                        pkEditModel.InterestsTargetingHouseChoseAll = true;
                                        LogTrace.WriteInLog("             Дом и семья. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingHouseChildren)
                                    {
                                        pkEditModel.InterestsTargetingHouseChildren = true;
                                        LogTrace.WriteInLog("             Дом и семья. Выбран checkbox Маленькие дети");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingHouseDogs)
                                    {
                                        pkEditModel.InterestsTargetingHouseDogs = true;
                                        LogTrace.WriteInLog("             Дом и семья. Выбран checkbox Собаки");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingHouseMiscellanea)
                                    {
                                        pkEditModel.InterestsTargetingHouseMiscellanea = true;
                                        LogTrace.WriteInLog("             Дом и семья. Выбран checkbox Разное");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingHouseCats)
                                    {
                                        pkEditModel.InterestsTargetingHouseCats = true;
                                        LogTrace.WriteInLog("             Дом и семья. Выбран checkbox Кошки");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingHouseCookery)
                                    {
                                        pkEditModel.InterestsTargetingHouseCookery = true;
                                        LogTrace.WriteInLog("             Дом и семья. Выбран checkbox Кулинария, рецепты");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingHouseKindergartens)
                                    {
                                        pkEditModel.InterestsTargetingHouseKindergartens = true;
                                        LogTrace.WriteInLog("             Дом и семья. Выбран checkbox Детские сады");
                                    }
                                #endregion

                                #region Финансы
                                    if (!pkEditModel.GetInterestsTargetingFinanceChoseAll)
                                    {
                                        pkEditModel.InterestsTargetingFinanceChoseAll = true;
                                        LogTrace.WriteInLog("             Финансы. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingFinanceStockMarket)
                                    {
                                        pkEditModel.InterestsTargetingFinanceStockMarket = true;
                                        LogTrace.WriteInLog("             Финансы. Выбран checkbox Фондовый рынок");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingFinanceCurrency)
                                    {
                                        pkEditModel.InterestsTargetingFinanceCurrency = true;
                                        LogTrace.WriteInLog("             Финансы. Выбран checkbox Валюта");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingFinanceInsurence)
                                    {
                                        pkEditModel.InterestsTargetingFinanceInsurence = true;
                                        LogTrace.WriteInLog("             Финансы. Выбран checkbox Страхование");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingFinanceMoneyTransfers)
                                    {
                                        pkEditModel.InterestsTargetingFinanceMoneyTransfers = true;
                                        LogTrace.WriteInLog("             Финансы. Выбран checkbox Денежные переводы");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingFinanceCredits)
                                    {
                                        pkEditModel.InterestsTargetingFinanceCredits = true;
                                        LogTrace.WriteInLog("             Финансы. Выбран checkbox Кредиты");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingFinanceMiscellanea)
                                    {
                                        pkEditModel.InterestsTargetingFinanceMiscellanea = true;
                                        LogTrace.WriteInLog("             Финансы. Выбран checkbox Разное");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingFinanceDeposits)
                                    {
                                        pkEditModel.InterestsTargetingFinanceDeposits = true;
                                        LogTrace.WriteInLog("             Финансы. Выбран checkbox Вклады, депозиты");
                                    }
                                #endregion

                                #region Компьютеры, оргтехника
                                    if (!pkEditModel.GetInterestsTargetingComputersChoseAll)
                                    {
                                        pkEditModel.InterestsTargetingComputersChoseAll = true;
                                        LogTrace.WriteInLog("             Компьютеры, оргтехника. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingComputersLaptops)
                                    {
                                        pkEditModel.InterestsTargetingComputersLaptops = true;
                                        LogTrace.WriteInLog("             Компьютеры, оргтехника. Выбран checkbox Ноутбуки");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingComputersParts)
                                    {
                                        pkEditModel.InterestsTargetingComputersParts = true;
                                        LogTrace.WriteInLog("             Компьютеры, оргтехника. Выбран checkbox Компьютерные комплектующие");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingComputersPrinters)
                                    {
                                        pkEditModel.InterestsTargetingComputersPrinters = true;
                                        LogTrace.WriteInLog("             Компьютеры, оргтехника. Выбран checkbox Принтеры");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingComputersTablets)
                                    {
                                        pkEditModel.InterestsTargetingComputersTablets = true;
                                        LogTrace.WriteInLog("             Компьютеры, оргтехника. Выбран checkbox Планшетные ПК");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingComputersMonitors)
                                    {
                                        pkEditModel.InterestsTargetingComputersMonitors = true;
                                        LogTrace.WriteInLog("             Компьютеры, оргтехника. Выбран checkbox Мониторы");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingComputersMiscellanea)
                                    {
                                        pkEditModel.InterestsTargetingComputersMiscellanea = true;
                                        LogTrace.WriteInLog("             Компьютеры, оргтехника. Выбран checkbox Разное");
                                    }
                                #endregion

                                #region Авто
                                    if (!pkEditModel.GetInterestsTargetingAutoChoseAll)
                                    {
                                        pkEditModel.InterestsTargetingAutoChoseAll = true;
                                        LogTrace.WriteInLog("             Авто. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingAutoInsurence)
                                    {
                                        pkEditModel.InterestsTargetingAutoInsurence = true;
                                        LogTrace.WriteInLog("             Авто. Выбран checkbox Автострахование");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingAutoMiscellanea)
                                    {
                                        pkEditModel.InterestsTargetingAutoMiscellanea = true;
                                        LogTrace.WriteInLog("             Авто. Выбран checkbox Разное");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingAutoNational)
                                    {
                                        pkEditModel.InterestsTargetingAutoNational = true;
                                        LogTrace.WriteInLog("             Авто. Выбран checkbox Отечественные");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingAutoWheels)
                                    {
                                        pkEditModel.InterestsTargetingAutoWheels = true;
                                        LogTrace.WriteInLog("             Авто. Выбран checkbox Колёса, Шины");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingAutoImported)
                                    {
                                        pkEditModel.InterestsTargetingAutoImported = true;
                                        LogTrace.WriteInLog("             Авто. Выбран checkbox Иномарки");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingAutoMoto)
                                    {
                                        pkEditModel.InterestsTargetingAutoMoto = true;
                                        LogTrace.WriteInLog("             Авто. Выбран checkbox Мото-, Квадроциклы, Снегоходы");
                                    }
                                #endregion

                                #region Аудио, Видео, Фото
                                    if (!pkEditModel.GetInterestsTargetingAudioChoseAll)
                                    {
                                        pkEditModel.InterestsTargetingAudioChoseAll = true;
                                        LogTrace.WriteInLog("             Аудио, Видео, Фото. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingAudioVideoEquips)
                                    {
                                        pkEditModel.InterestsTargetingAudioVideoEquips = true;
                                        LogTrace.WriteInLog("             Аудио, Видео, Фото. Выбран checkbox Видеоаппаратура");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingAudioMiscellanea)
                                    {
                                        pkEditModel.InterestsTargetingAudioMiscellanea = true;
                                        LogTrace.WriteInLog("             Аудио, Видео, Фото. Выбран checkbox Разное");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingAudioTech)
                                    {
                                        pkEditModel.InterestsTargetingAudioTech = true;
                                        LogTrace.WriteInLog("             Аудио, Видео, Фото. Выбран checkbox Аудио-техника");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingAudioCameras)
                                    {
                                        pkEditModel.InterestsTargetingAudioCameras = true;
                                        LogTrace.WriteInLog("             Аудио, Видео, Фото. Выбран checkbox Фотоаппараты");
                                    }
                                    if (!pkEditModel.GetInterestsTargetingAudioTvs)
                                    {
                                        pkEditModel.InterestsTargetingAudioTvs = true;
                                        LogTrace.WriteInLog("             Аудио, Видео, Фото. Выбран checkbox Телевизоры, DVD-проигрыватели");
                                    }
                                #endregion

                                break;
                            }
                    }
                #endregion

                #region Браузеры
                    variant = chosenVariantBrowserTargeting = needSetRadioButton(2);
                    pkEditModel.BrowserTargeting = variant;
                    switch (variant)
                    {
                        case 0:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Браузеры. Выбрано: не использовать");
                                break;
                            }
                        case 1:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Браузеры. Выбрано: использовать");
                                //развернуть все пункты
                                pkEditModel.BrowserTargetingOtherExpand = true;
                                pkEditModel.BrowserTargetingOperaExpand = true;
                                pkEditModel.BrowserTargetingChromeExpand = true;
                                pkEditModel.BrowserTargetingFirefoxExpand = true;
                                pkEditModel.BrowserTargetingSafariExpand = true;
                                pkEditModel.BrowserTargetingIeExpand = true;
                                //pkEditModel.BrowserTargetingGoogleChromeMobileExpand = true;
                                //pkEditModel.BrowserTargetingOtherAll = true;

                                if (!pkEditModel.GetBrowserTargetingOtherChoseAll)
                                {
                                    pkEditModel.BrowserTargetingOtherChoseAll = true;
                                    LogTrace.WriteInLog("        Другие. Выбран checkbox Другие Все");
                                }
                                if (!pkEditModel.GetBrowserTargetingOtherAll)
                                {
                                    pkEditModel.BrowserTargetingOtherAll = true;
                                    LogTrace.WriteInLog("             Другие. Выбран checkbox Все");
                                }

                                #region Опера
                                    //pkEditModel.BrowserTargetingOperaOther = true;
                                    //LogTrace.WriteInLog("             Опера. Выбран checkbox Другие");
                                    if (!pkEditModel.GetBrowserTargetingOperaChoseAll)
                                    {
                                        pkEditModel.BrowserTargetingOperaChoseAll = true;
                                        LogTrace.WriteInLog("             Опера. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingOperaOther)
                                    {
                                        pkEditModel.BrowserTargetingOperaOther = true;
                                        LogTrace.WriteInLog("             Опера. Выбран checkbox Другие");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingOpera10)
                                    {
                                        pkEditModel.BrowserTargetingOpera10 = true;
                                        LogTrace.WriteInLog("        Опера. Выбран checkbox 10");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingOpera11)
                                    {
                                        pkEditModel.BrowserTargetingOpera11 = true;
                                        LogTrace.WriteInLog("        Опера. Выбран checkbox 11");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingOperaMini)
                                    {
                                        pkEditModel.BrowserTargetingOperaMini = true;
                                        LogTrace.WriteInLog("        Опера. Выбран checkbox Mini");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingOperaMobile)
                                    {
                                        pkEditModel.BrowserTargetingOperaMobile = true;
                                        LogTrace.WriteInLog("        Опера. Выбран checkbox Mobile");
                                    }
                                #endregion

                                #region Chrome
                                    if (!pkEditModel.GetBrowserTargetingChromeChoseAll)
                                    {
                                        pkEditModel.BrowserTargetingChromeChoseAll = true;
                                        LogTrace.WriteInLog("             Chrome. Выбран checkbox Chrome Все");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingChromeAll)
                                    {
                                        pkEditModel.BrowserTargetingChromeAll = true;
                                        LogTrace.WriteInLog("        Chrome. Выбран checkbox Все");
                                    }
                                #endregion

                                #region Firefox
                                    if (!pkEditModel.GetBrowserTargetingFirefoxChoseAll)
                                    {
                                        pkEditModel.BrowserTargetingFirefoxChoseAll = true;
                                        LogTrace.WriteInLog("             Firefox. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingFirefox3)
                                    {
                                        pkEditModel.BrowserTargetingFirefox3 = true;
                                        LogTrace.WriteInLog("        Firefox. Выбран checkbox 3");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingFirefox4)
                                    {
                                        pkEditModel.BrowserTargetingFirefox4 = true;
                                        LogTrace.WriteInLog("        Firefox. Выбран checkbox 4");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingFirefox5)
                                    {
                                        pkEditModel.BrowserTargetingFirefox5 = true;
                                        LogTrace.WriteInLog("        Firefox. Выбран checkbox 5");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingFirefox6)
                                    {
                                        pkEditModel.BrowserTargetingFirefox6 = true;
                                        LogTrace.WriteInLog("        Firefox. Выбран checkbox 6");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingFirefoxOther)
                                    {
                                        pkEditModel.BrowserTargetingFirefoxOther = true;
                                        LogTrace.WriteInLog("        Firefox. Выбран checkbox Другие");
                                    }
                                #endregion

                                #region Safari
                                    if (!pkEditModel.GetBrowserTargetingSafariChoseAll)
                                    {
                                        pkEditModel.BrowserTargetingSafariChoseAll = true;
                                        LogTrace.WriteInLog("             Safari. Выбран checkbox Safari Все");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingSafariAll)
                                    {
                                        pkEditModel.BrowserTargetingSafariAll = true;
                                        LogTrace.WriteInLog("        Safari. Выбран checkbox Все");
                                    }
                                #endregion

                                #region MSIE
                                    if (!pkEditModel.GetBrowserTargetingIeChoseAll)
                                    {
                                        pkEditModel.BrowserTargetingIeChoseAll = true;
                                        LogTrace.WriteInLog("             MSIE. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingIe6)
                                    {
                                        pkEditModel.BrowserTargetingIe6 = true;
                                        LogTrace.WriteInLog("        MSIE. Выбран checkbox 6");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingIe7)
                                    {
                                        pkEditModel.BrowserTargetingIe7 = true;
                                        LogTrace.WriteInLog("        MSIE. Выбран checkbox 7");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingIe8)
                                    {
                                        pkEditModel.BrowserTargetingIe8 = true;
                                        LogTrace.WriteInLog("        MSIE. Выбран checkbox 8");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingIe9)
                                    {
                                        pkEditModel.BrowserTargetingIe9 = true;
                                        LogTrace.WriteInLog("        MSIE. Выбран checkbox 9");
                                    }
                                    if (!pkEditModel.GetBrowserTargetingIeOther)
                                    {
                                        pkEditModel.BrowserTargetingIeOther = true;
                                        LogTrace.WriteInLog("        MSIE. Выбран checkbox Другие");
                                    }
                                #endregion

                                #region Google Chrome Mobile
                                    //if (!pkEditModel.GetBrowserTargetingGoogleChromeMobileChoseAll)
                                    //{
                                    //    pkEditModel.BrowserTargetingGoogleChromeMobileChoseAll = true;
                                    //    LogTrace.WriteInLog("             Google Chrome Mobile. Выбран checkbox Google Chrome Mobile");
                                    //}
                                #endregion

                                break;
                            }
                    }
                #endregion

                #region OC таргетинг
                    variant = chosenVariantOsTargeting = needSetRadioButton(2);
                    pkEditModel.OsTargeting = variant;
                    switch (variant)
                    {
                        case 0:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton OC таргетинг. Выбрано: не использовать");
                                break;
                            }
                        case 1:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton OC таргетинг. Выбрано: использовать");
                                //pkEditModel.OsTargetingOther = true;
                                //LogTrace.WriteInLog("        Выбран checkbox Другие");
                                if (!pkEditModel.GetOsTargetingOther)
                                {
                                    pkEditModel.OsTargetingOther = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Другие");
                                }
                                if (!pkEditModel.GetOsTargetingMacOs)
                                {
                                    pkEditModel.OsTargetingMacOs = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Mac OS");
                                }
                                if (!pkEditModel.GetOsTargetingOtherMobileOs)
                                {
                                    pkEditModel.OsTargetingOtherMobileOs = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Прочие мобильные ОС");
                                }
                                if (!pkEditModel.GetOsTargetingWindows)
                                {
                                    pkEditModel.OsTargetingWindows = true;
                                    LogTrace.WriteInLog("             Выбран checkbox WIndows");
                                }
                                if (!pkEditModel.GetOsTargetingOtherIoS)
                                {
                                    pkEditModel.OsTargetingOtherIoS = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Прочие iOS системы");
                                }
                                if (!pkEditModel.GetOsTargetingIpad)
                                {
                                    pkEditModel.OsTargetingIpad = true;
                                    LogTrace.WriteInLog("             Выбран checkbox iPAD");
                                }
                                if (!pkEditModel.GetOsTargetingIphone)
                                {
                                    pkEditModel.OsTargetingIphone = true;
                                    LogTrace.WriteInLog("             Выбран checkbox IPHONE");
                                }
                                if (!pkEditModel.GetOsTargetingAndroid)
                                {
                                    pkEditModel.OsTargetingAndroid = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Android");
                                }
                                break;
                            }
                    }
                #endregion

                #region Провайдеры
                    variant = chosenVariantProviderTargeting = needSetRadioButton(2);
                    pkEditModel.ProviderTargeting = variant;
                    switch (variant)
                    {
                        case 0:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Провайдеры. Выбрано: не использовать");
                                break;
                            }
                        case 1:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Провайдеры. Выбрано: использовать");
                                //pkEditModel.ProviderTargetingOther = true;
                                //LogTrace.WriteInLog("        Выбран checkbox Другие");
                                if (!pkEditModel.GetProviderTargetingOther)
                                {
                                    pkEditModel.ProviderTargetingOther = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Другие");
                                }
                                if (!pkEditModel.GetProviderTargetingMegafon)
                                {
                                    pkEditModel.ProviderTargetingMegafon = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Мегафон");
                                }
                                if (!pkEditModel.GetProviderTargetingMtc)
                                {
                                    pkEditModel.ProviderTargetingMtc = true;
                                    LogTrace.WriteInLog("             Выбран checkbox МТС Россия");
                                }
                                break;
                            }
                    }
                #endregion

                #region Геотаргетинг
                    variant = chosenVariantGeoTargeting = needSetRadioButton(2);
                    pkEditModel.GeoTargeting = variant;
                    switch (variant)
                    {
                        case 0:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Геотаргетинг. Выбрано: не использовать");
                                break;
                            }
                        case 1:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Геотаргетинг. Выбрано: использовать");
                                pkEditModel.GeoTargetingRussiaExpand = true;
                                pkEditModel.GeoTargetingUkraineExpand = true;

                                //pkEditModel.GeoTargetingOther = true;
                                if (!pkEditModel.GetGeoTargetingOther)
                                {
                                    pkEditModel.GeoTargetingOther = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Прочие страны");
                                }
                                if (!pkEditModel.GetGeoTargetingAustria)
                                {
                                    pkEditModel.GeoTargetingAustria = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Австрия");
                                }
                                if (!pkEditModel.GetGeoTargetingBelorussia)
                                {
                                    pkEditModel.GeoTargetingBelorussia = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Белоруссия");
                                }
                                if (!pkEditModel.GetGeoTargetingUk)
                                {
                                    pkEditModel.GeoTargetingUk = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Великобритания");
                                }
                                if (!pkEditModel.GetGeoTargetingGermany)
                                {
                                    pkEditModel.GeoTargetingGermany = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Германия");
                                }
                                if (!pkEditModel.GetGeoTargetingIsrael)
                                {
                                    pkEditModel.GeoTargetingIsrael = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Израиль");
                                }
                                if (!pkEditModel.GetGeoTargetingKazakhstan)
                                {
                                    pkEditModel.GeoTargetingKazakhstan = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Казахстан");
                                }
                                if (!pkEditModel.GetGeoTargetingLatvia)
                                {
                                    pkEditModel.GeoTargetingLatvia = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Латвия");
                                }
                                if (!pkEditModel.GetGeoTargetingLithuania)
                                {
                                    pkEditModel.GeoTargetingLithuania = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Литва");
                                }

                                #region Россия
                                    if (!pkEditModel.GetGeoTargetingRussiaChoseAll)
                                    {
                                        pkEditModel.GeoTargetingRussiaChoseAll = true;
                                        LogTrace.WriteInLog("             Россия. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetGeoTargetingRussiaEburg)
                                    {
                                        pkEditModel.GeoTargetingRussiaEburg = true;
                                        LogTrace.WriteInLog("        Россия. Выбран checkbox Екатеринбург");
                                    }
                                    if (!pkEditModel.GetGeoTargetingRussiaMoscow)
                                    {
                                        pkEditModel.GeoTargetingRussiaMoscow = true;
                                        LogTrace.WriteInLog("        Россия. Выбран checkbox Москва");
                                    }
                                    if (!pkEditModel.GetGeoTargetingRussiaNovosibirsk)
                                    {
                                        pkEditModel.GeoTargetingRussiaNovosibirsk = true;
                                        LogTrace.WriteInLog("        Россия. Выбран checkbox Новосибирск");
                                    }
                                    if (!pkEditModel.GetGeoTargetingRussiaOther)
                                    {
                                        pkEditModel.GeoTargetingRussiaOther = true;
                                        LogTrace.WriteInLog("        Россия. Выбран checkbox Прочие регионы");
                                    }
                                    if (!pkEditModel.GetGeoTargetingRussiaSpb)
                                    {
                                        pkEditModel.GeoTargetingRussiaSpb = true;
                                        LogTrace.WriteInLog("        Россия. Выбран checkbox Санкт-Петербург");
                                    }
                                    if (!pkEditModel.GetGeoTargetingRussiaHabarovsk)
                                    {
                                        pkEditModel.GeoTargetingRussiaHabarovsk = true;
                                        LogTrace.WriteInLog("        Россия. Выбран checkbox Хабаровск");
                                    }
                                #endregion

                                if (!pkEditModel.GetGeoTargetingUsa)
                                {
                                    pkEditModel.GeoTargetingUsa = true;
                                    LogTrace.WriteInLog("             Выбран checkbox США");
                                }

                                #region Украина
                                    if (!pkEditModel.GetGeoTargetingUkraineChoseAll)
                                    {
                                        pkEditModel.GeoTargetingUkraineChoseAll = true;
                                        LogTrace.WriteInLog("             Украина. Выбран checkbox Все");
                                    }
                                    if (!pkEditModel.GetGeoTargetingUkraineDnepr)
                                    {
                                        pkEditModel.GeoTargetingUkraineDnepr = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Днепропетровск");
                                    }
                                    if (!pkEditModel.GetGeoTargetingUkraineDonetzk)
                                    {
                                        pkEditModel.GeoTargetingUkraineDonetzk = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Донецк");
                                    }
                                    if (!pkEditModel.GetGeoTargetingUkraineZakarpattya)
                                    {
                                        pkEditModel.GeoTargetingUkraineZakarpattya = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Закарпатье");
                                    }
                                    if (!pkEditModel.GetGeoTargetingUkraineKiev)
                                    {
                                        pkEditModel.GeoTargetingUkraineKiev = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Киев");
                                    }
                                    if (!pkEditModel.GetGeoTargetingUkraineCrimea)
                                    {
                                        pkEditModel.GeoTargetingUkraineCrimea = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Крым");
                                    }
                                    if (!pkEditModel.GetGeoTargetingUkraineLvov)
                                    {
                                        pkEditModel.GeoTargetingUkraineLvov = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Львов");
                                    }
                                    if (!pkEditModel.GetGeoTargetingUkraineNikolaev)
                                    {
                                        pkEditModel.GeoTargetingUkraineNikolaev = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Николаев");
                                    }
                                    if (!pkEditModel.GetGeoTargetingUkraineOdessa)
                                    {
                                        pkEditModel.GeoTargetingUkraineOdessa = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Одесса");
                                    }
                                    if (!pkEditModel.GetGeoTargetingUkraineOther)
                                    {
                                        pkEditModel.GeoTargetingUkraineOther = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Прочие области");
                                    }
                                    if (!pkEditModel.GetGeoTargetingUkraineHarkov)
                                    {
                                        pkEditModel.GeoTargetingUkraineHarkov = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Харьков");
                                    }
                                    if (!pkEditModel.GetGeoTargetingUkraineHerson)
                                    {
                                        pkEditModel.GeoTargetingUkraineHerson = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Херсон");
                                    }
                                    if (!pkEditModel.GetGeoTargetingUkraineCherkassy)
                                    {
                                        pkEditModel.GeoTargetingUkraineCherkassy = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Черкассы");
                                    }
                                    if (!pkEditModel.GetGeoTargetingUkraineChernovzi)
                                    {
                                        pkEditModel.GeoTargetingUkraineChernovzi = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Черновцы");
                                    }
                                #endregion

                                if (!pkEditModel.GetGeoTargetingEstonia)
                                {
                                    pkEditModel.GeoTargetingEstonia = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Эстония");
                                }
                                break;
                            }
                    }
                #endregion
            #endregion

            string editPktUrl = driver.Url; //запоминаем url страницы

            pkEditModel.Submit(); //пытаемся сохранить форму
            LogTrace.WriteInLog(Goods_View.tab2 + "Нажал кнопку Сохранить");
            LogTrace.WriteInLog(Goods_View.tab2 + driver.Url);
            LogTrace.WriteInLog("");

            string isEditedPkUrl = driver.Url; //запоминаем url страницы, открывшейся после нажатия "Сохранить"
            //если editPktUrl и isEditedPkUrl совпали - мы никуда не перешли и значит есть ошибки заполнения полей
            //если editPktUrl и isEditedPkUrl не совпали - клиент отредактировался и ошибки искать не надо
            if (editPktUrl == isEditedPkUrl)
            {
                errors = pkEditModel.GetErrors(); //проверяем, появились ли на форме ошибки заполнения полей
            }
            else
            {
                LogTrace.WriteInLog(Goods_View.tab2 + "РК успешно отредактирована");
                LogForClickers.WriteInLog(Goods_View.tab2 + "РК успешно отредактирована");
            }
            Registry.hashTable["driver"] = driver;
        }

        public bool wasMismatch = false;

        public void CheckEditingPk()
        {
            driver = (IWebDriver) Registry.hashTable["driver"];
                //забираем из хештаблицы сохраненный при создании клиента драйвер
            driver.Navigate().GoToUrl(baseUrl); //заходим по ссылке
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            LogTrace.WriteInLog("          " + driver.Url);

            #region Проверка заполнения
                #region Разное
                    if (pkEditModel.GetViewSensors) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Просмотр датчиков' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Просмотр датчиков' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (pkEditModel.GetViewConversion) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Просмотр конверсии' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Просмотр конверсии' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (namePk == pkEditModel.GetName) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Название ({0}) и введенное при редактировании ({1})", pkEditModel.GetName, namePk)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Название ({0}) и введенное при редактировании ({1})", pkEditModel.GetName, namePk));
                        wasMismatch = true;
                    }

                    if (dateStartPk == pkEditModel.GetStartPkDate) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Дата старта РК ({0}) и введенное при редактировании ({1})", pkEditModel.GetStartPkDate, dateStartPk)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Дата старта РК ({0}) и введенное при редактировании ({1})", pkEditModel.GetStartPkDate, dateStartPk));
                        wasMismatch = true;
                    }

                    if (dateEndPk == pkEditModel.GetEndPkDate) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Дата окончания РК ({0}) и введенное при редактировании ({1})", pkEditModel.GetEndPkDate, dateEndPk)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Дата окончания РК ({0}) и введенное при редактировании ({1})", pkEditModel.GetEndPkDate, dateEndPk));
                        wasMismatch = true;
                    }

                    //if (pkEditModel.GetBlockTeasersAfterCreation) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Блокировать тизеры после их создания' и выбранное при редактировании"); }
                    //else
                    //{
                    //    LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Блокировать тизеры после их создания' и выбранное при редактировании");
                    //    wasMismatch = true;
                    //}

                    if (pkEditModel.GetStoppedByManager) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Остановлена менеджером' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Остановлена менеджером' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                #endregion

                #region Ограничения рекламной кампании
                    if (chosenVariantLimitsPk != 0)
                    {
                        if (chosenVariantLimitsPk == 1)
                        {
                            if (dayLimitByBudget == pkEditModel.GetDayLimitByBudget)
                            {
                                LogTrace.WriteInLog(
                                    string.Format("          Совпадают: содержимое поля 'Суточный лимит РК' ({0}) и введенное при редактировании ({1})", pkEditModel.GetDayLimitByBudget, dayLimitByBudget));
                            }
                            else
                            {
                                LogTrace.WriteInLog(
                                    string.Format("НЕ СОВПАДАЮТ: содержимое поля 'Суточный лимит РК' ({0}) и введенное при редактировании ({1})", pkEditModel.GetDayLimitByBudget, dayLimitByBudget));
                                wasMismatch = true;
                            }

                            if (generalLimitByBudget == pkEditModel.GetGeneralLimitByBudget)
                            {
                                LogTrace.WriteInLog(
                                    string.Format("          Совпадают: содержимое поля 'Общий лимит РК' ({0}) и введенное при редактировании ({1})", pkEditModel.GetGeneralLimitByBudget, generalLimitByBudget));
                            }
                            else
                            {
                                LogTrace.WriteInLog(
                                    string.Format("НЕ СОВПАДАЮТ: содержимое поля 'Общий лимит РК' ({0}) и введенное при редактировании ({1})", pkEditModel.GetGeneralLimitByBudget, generalLimitByBudget));
                                wasMismatch = true;
                            }
                        }

                        if (chosenVariantLimitsPk == 2)
                        {
                            if (dayLimitByClicks == pkEditModel.GetDayLimitByClicks)
                            {
                                LogTrace.WriteInLog(
                                    string.Format("          Совпадают: содержимое поля 'Суточный лимит кликов' ({0}) и введенное при редактировании ({1})", pkEditModel.GetDayLimitByClicks, dayLimitByClicks));
                            }
                            else
                            {
                                LogTrace.WriteInLog(
                                    string.Format("НЕ СОВПАДАЮТ: содержимое поля 'Суточный лимит кликов' ({0}) и введенное при редактировании ({1})", pkEditModel.GetDayLimitByClicks, dayLimitByClicks));
                                wasMismatch = true;
                            }

                            if (generalLimitByClicks == pkEditModel.GetGeneralLimitByClicks)
                            {
                                LogTrace.WriteInLog(
                                    string.Format("          Совпадают: содержимое поля 'Лимит на кампанию' ({0}) и введенное при редактировании ({1})", pkEditModel.GetGeneralLimitByClicks, generalLimitByClicks));
                            }
                            else
                            {
                                LogTrace.WriteInLog(
                                    string.Format("НЕ СОВПАДАЮТ: содержимое поля 'Лимит на кампанию' ({0}) и введенное при редактировании ({1})", pkEditModel.GetGeneralLimitByClicks, generalLimitByClicks));
                                wasMismatch = true;
                            }
                        }
                    }

            #endregion

                #region UTM-разметка рекламной кампании для Google Analytics
                    if (pkEditModel.GetUtmPkForGoogleAnalytics) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'UTM-разметка рекламной кампании для Google Analytics' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'UTM-разметка рекламной кампании для Google Analytics' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (utmMedium == pkEditModel.GetUtmMedium) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'utm_medium (средство кампании)' ({0}) и введенное при редактировании ({1})", pkEditModel.GetUtmMedium, utmMedium)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: Совпадают: содержимое поля 'utm_medium (средство кампании)' ({0}) и введенное при редактировании ({1})", pkEditModel.GetUtmMedium, utmMedium));
                        wasMismatch = true;
                    }

                    if (utmSource == pkEditModel.GetUtmSource) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'utm_source (источник кампании)' ({0}) и введенное при редактировании ({1})", pkEditModel.GetUtmSource, utmSource)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: Совпадают: содержимое поля 'utm_source (источник кампании)' ({0}) и введенное при редактировании ({1})", pkEditModel.GetUtmSource, utmSource));
                        wasMismatch = true;
                    }

                    if (utmCampaign == pkEditModel.GetUtmCampaign) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'utm_campaign (название кампании)' ({0}) и введенное при редактировании ({1})", pkEditModel.GetUtmCampaign, utmCampaign)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: Совпадают: содержимое поля 'utm_campaign (название кампании)' ({0}) и введенное при редактировании ({1})", pkEditModel.GetUtmCampaign, utmCampaign));
                        wasMismatch = true;
                    }
                #endregion

                #region UTM-разметка пользователя
                    if (pkEditModel.GetUtmUser) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'UTM-разметка пользователя' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'UTM-разметка пользователя' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (utmUserStr == pkEditModel.GetUtmUserStr) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'UTM-разметка пользователя' ({0}) и введенное при редактировании ({1})", pkEditModel.GetUtmUserStr, utmUserStr)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: Совпадают: содержимое поля 'UTM-разметка пользователя' ({0}) и введенное при редактировании ({1})", pkEditModel.GetUtmUserStr, utmUserStr));
                        wasMismatch = true;
                    }
                #endregion

                #region Крутить в сети Товарро
                    if (pkEditModel.GetScrewInTovarro) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Крутить в сети Товарро' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Крутить в сети Товарро' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                #endregion

                #region Блокировка по расписанию
                    if (pkEditModel.GetBlockBySchedule) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Блокировка по расписанию' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Блокировка по расписанию' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (pkEditModel.GetWeekends) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'выходные' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'выходные' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (pkEditModel.GetWeekdays) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'будни' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'будни' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (pkEditModel.GetWorkingTime) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'рабочее время (9-18 по будням)' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'рабочее время (9-18 по будням)' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                #endregion

                #region Передавать id площадки в ссылке
                    if (pkEditModel.GetIdOfPlatformInLink) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Передавать id площадки в ссылке' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Передавать id площадки в ссылке' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (idOfPlatformInLink == pkEditModel.GetIdOfPlatformInLinkStr) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'Передавать id площадки в ссылке' ({0}) и введенное при редактировании ({1})", pkEditModel.GetIdOfPlatformInLinkStr, idOfPlatformInLink)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: Совпадают: содержимое поля 'Передавать id площадки в ссылке' ({0}) и введенное при редактировании ({1})", pkEditModel.GetIdOfPlatformInLinkStr, idOfPlatformInLink));
                        wasMismatch = true;
                    }
                #endregion

                #region Добавлять id тизера в конец ссылки
                    if (pkEditModel.GetAddIdOfTeaserInLink) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Добавлять id тизера в конец ссылки' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Добавлять id тизера в конец ссылки' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                #endregion

                #region Комментарий к кампании
                    if (commentsForPk == pkEditModel.GetCommentsForPk) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'Комментарий к кампании' ({0}) и введенное при редактировании ({1})", pkEditModel.GetCommentsForPk, commentsForPk)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля 'Комментарий к кампании' ({0}) и введенное при редактировании ({1})", pkEditModel.GetCommentsForPk, commentsForPk));
                        wasMismatch = true;
                    }
                #endregion

                #region Площадки
                    if (pkEditModel.GetPlatformsNotSpecified) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Не определено' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Не определено' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsPolitics) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Политика, общество, происшествия, религия' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Политика, общество, происшествия, религия' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsEconomics) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Экономика, финансы, недвижимость, работа и карьера' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Экономика, финансы, недвижимость, работа и карьера' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsCelebrities) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Знаменитости, шоу-бизнес, кино, музыка' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Знаменитости, шоу-бизнес, кино, музыка' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsScience) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Наука и технологии' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Наука и технологии' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsConnection) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Связь, компьютеры, программы' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Связь, компьютеры, программы' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsSports) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Спорт' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Спорт' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsAuto) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Авто-вело-мото' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Авто-вело-мото' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsFashion) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Мода и стиль, здоровье и красота, фитнес и диета, кулинария' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Мода и стиль, здоровье и красота, фитнес и диета, кулинария' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsMedicine) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Медицина' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Медицина' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsTourism) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Туризм и отдых (путевки, отели, рестораны)' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Туризм и отдых (путевки, отели, рестораны)' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsGlobalPortals) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Глобальные порталы с множеством подпроектов' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Глобальные порталы с множеством подпроектов' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsHumor) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Юмор (приколы, картинки, обои), каталог фотографий, блоги' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Юмор (приколы, картинки, обои), каталог фотографий, блоги' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsFileshares) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Файлообменники, файлокачалки (кино, музыка, игры, программы)' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Файлообменники, файлокачалки (кино, музыка, игры, программы)' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsSocialNetworks) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Социальные сети, сайты знакомства, личные дневники' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Социальные сети, сайты знакомства, личные дневники' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsAnimals) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Животный и растительный мир' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Животный и растительный мир' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsReligion) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Религия' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Религия' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsChildren) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Дети и родители' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Дети и родители' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsBuilding) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Строительство, ремонт, дача, огород' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Строительство, ремонт, дача, огород' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsCookery) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Кулинария' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Кулинария' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsJob) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Работа и карьера. Поиск работы, поиск персонала' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Работа и карьера. Поиск работы, поиск персонала' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsNotSites) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Не сайты (программы, тулбары, таскбары)' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Не сайты (программы, тулбары, таскбары)' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsSitesStartPagesBrowsers) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Сайты, размещенные на стартовых страницах браузеров' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Сайты, размещенные на стартовых страницах браузеров' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsSearchSystems) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Поисковые системы' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Поисковые системы' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsEmail) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Почта' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Почта' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsPhotoCatalogues) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Каталоги фотографий' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Каталоги фотографий' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsVarez) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Варезники' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Варезники' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsOnlineVideo) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Онлайн видео, телевидение, радио' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Онлайн видео, телевидение, радио' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsOnlineLibraries) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Онлайн-библиотеки' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Онлайн-библиотеки' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsInternet) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Интернет, поисковые сайты, электронная почта' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Интернет, поисковые сайты, электронная почта' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsOnlineGames) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Онлайн игры' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Онлайн игры' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsInternetRepresentatives) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Интернет-представительства бизнеса' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Интернет-представительства бизнеса' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsProgramms) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Программы, прошивки, игры для КПК и мобильных устройств' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Программы, прошивки, игры для КПК и мобильных устройств' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsCataloguesInternetResources) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Каталоги Интернет - ресурсов, фирм и предприятий' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Каталоги Интернет - ресурсов, фирм и предприятий' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsForWagesInInternet) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Для заработка в Интернете. Партнерские программы' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Для заработка в Интернете. Партнерские программы' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsHobbies) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Хобби и увлечения' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Хобби и увлечения' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsMarketgid) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Маркетгид' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Маркетгид' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsShock) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Шокодром' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Шокодром' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsEsoteric) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Эзотерика. Непознанное, астрология, гороскопы, гадания' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Эзотерика. Непознанное, астрология, гороскопы, гадания' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsPsychology) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Психология, мужчина и женщина' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Психология, мужчина и женщина' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsHistory) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'История, образование, культура' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'История, образование, культура' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                    if (pkEditModel.GetPlatformsMarketgidWomenNet) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Маркетгид ЖС' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Маркетгид ЖС' и выбранное при редактировании");
                        wasMismatch = true;
                    }
                #endregion

                #region Демографический таргетинг
                    if (chosenVariantDemoTargeting != 0)
                    {
                        if (pkEditModel.GetDemoTargetingMenChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Мужчины' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Мужчины' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetDemoTargetingWomenChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Женщины' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Женщины' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetDemoTargetingHermaphroditeChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Пол не определен' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Пол не определен' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                    }
            #endregion

                #region Таргетинг по интересам
                    if (chosenVariantInterestsTargeting != 0)
                    {
                        if (pkEditModel.GetInterestsTargetingOther)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Прочее' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Прочее' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetInterestsTargetingBusinessChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Бизнес' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Бизнес' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetInterestsTargetingRealtyChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Недвижимость' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Недвижимость' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetInterestsTargetingExhibitions)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Выставки, концерты, театры, кино' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Выставки, концерты, театры, кино' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetInterestsTargetingMedicineChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Медицина, здоровье' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Медицина, здоровье' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetInterestsTargetingHouseChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Дом и семья' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Дом и семья' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetInterestsTargetingFinanceChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Финансы' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Финансы' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetInterestsTargetingComputersChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Компьютеры, оргтехника' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Компьютеры, оргтехника' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetInterestsTargetingAutoChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Авто' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Авто' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetInterestsTargetingAudioChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Аудио, Видео, Фото' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Аудио, Видео, Фото' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                    }

            #endregion

                #region Таргетинг по браузерам
                    if (chosenVariantBrowserTargeting != 0)
                    {
                        if (pkEditModel.GetBrowserTargetingIeOther)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Другие' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Другие' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetBrowserTargetingOperaChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Opera' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Opera' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetBrowserTargetingChromeChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Google Chrome' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Google Chrome' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetBrowserTargetingFirefoxChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Firefox' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Firefox' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetBrowserTargetingSafariChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Safari' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Safari' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetBrowserTargetingIeChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'MSIE' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'MSIE' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        //if (pkEditModel.GetBrowserTargetingGoogleChromeMobileChoseAll)
                        //{
                        //    LogTrace.WriteInLog(
                        //        "          Совпадают: состояние checkbox 'Google Chrome Mobile' и выбранное при редактировании");
                        //}
                        //else
                        //{
                        //    LogTrace.WriteInLog(
                        //        "НЕ СОВПАДАЮТ: состояние checkbox 'Google Chrome Mobile' и выбранное при редактировании");
                        //    wasMismatch = true;
                        //}
                    }

            #endregion

                #region OC таргетинг
                    if (chosenVariantOsTargeting != 0)
                    {
                        if (pkEditModel.OsTargetingOther)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Другие' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Другие' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetOsTargetingMacOs)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Mac OS' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Mac OS' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetOsTargetingOtherMobileOs)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Прочие мобильные ОС' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Прочие мобильные ОС' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetOsTargetingWindows)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'WIndows' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'WIndows' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetOsTargetingOtherIoS)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Прочие iOS системы' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Прочие iOS системы' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetOsTargetingIpad)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'iPAD' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'iPAD' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetOsTargetingIphone)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'IPHONE' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'IPHONE' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetOsTargetingAndroid)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Android' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Android' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                    }
            #endregion

                #region Таргетинг по провайдерам
                    if (chosenVariantProviderTargeting != 0)
                    {
                        if (pkEditModel.GetProviderTargetingOther)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Другие' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Другие' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetProviderTargetingMegafon)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Мегафон' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Мегафон' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetProviderTargetingMtc)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'МТС Россия' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'МТС Россия' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                    }
            #endregion

                #region Геотаргетинг
                    if (chosenVariantGeoTargeting != 0)
                    {
                        if (pkEditModel.GetGeoTargetingOther)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Прочие страны' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Прочие страны' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetGeoTargetingAustria)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Австрия' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Австрия' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetGeoTargetingBelorussia)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Белоруссия' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Белоруссия' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetGeoTargetingUk)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Великобритания' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Великобритания' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetGeoTargetingGermany)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Германия' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Германия' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetGeoTargetingIsrael)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Израиль' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Израиль' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetGeoTargetingKazakhstan)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Казахстан' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Казахстан' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetGeoTargetingLatvia)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Латвия' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Латвия' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetGeoTargetingLithuania)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Литва' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Литва' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetGeoTargetingRussiaChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Россия' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Россия' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetGeoTargetingUsa)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'США' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'США' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetGeoTargetingUkraineChoseAll)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Украина' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Украина' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                        if (pkEditModel.GetGeoTargetingEstonia)
                        {
                            LogTrace.WriteInLog(
                                "          Совпадают: состояние checkbox 'Эстония' и выбранное при редактировании");
                        }
                        else
                        {
                            LogTrace.WriteInLog(
                                "НЕ СОВПАДАЮТ: состояние checkbox 'Эстония' и выбранное при редактировании");
                            wasMismatch = true;
                        }
                    }
            #endregion
            #endregion

            LogTrace.WriteInLog(Goods_View.tab2 + driver.Url);
            LogTrace.WriteInLog("");
            if (!wasMismatch)
            {
                LogTrace.WriteInLog(Goods_View.tab2 + "ОК, всё ранее введенное совпадает с текущими значениями");
                LogForClickers.WriteInLog(Goods_View.tab2 + "ОК, всё ранее введенное совпадает с текущими значениями");
            }
        }

        protected int needSetRadioButton(int variants) //генерируем номер варианта выбора для needSetRadioButton. variants - кол-во вариантов выбора
        {
            Random rnd = new Random();
            return rnd.Next(0, variants);
        }
    }
}
