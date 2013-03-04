using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Task.Utils;
using Task.Model;
using Task.View;

namespace Task.Controller
{
    public class GoodsEditPkController
    {
        private IWebDriver _driver;
        private GoodsEditPkModel _pkEditModel;
        //private readonly string _baseUrl = "https://admin.dt00.net/cab/goodhits/campaigns-edit/id/" + Registry.hashTable["pkId"] + "/filters/%252Fclient_id%252F" + Registry.hashTable["clientId"];
        readonly Randoms _randoms = new Randoms(); //класс генерации случайных строк
        private string _namePk;
        private string _dateStartPk;
        private string _dateEndPk;
        private string _dayLimitByBudget;
        private string _generalLimitByBudget;
        private string _dayLimitByClicks;
        private string _generalLimitByClicks;
        private string _utmMedium;
        private string _utmSource;
        private string _utmCampaign;
        private string _utmUserStr;
        private string _idOfPlatformInLink;
        private string _commentsForPk;
        private int _chosenVariantLimitsPk;
        private int _chosenVariantDemoTargeting;
        private int _chosenVariantInterestsTargeting;
        private int _chosenVariantBrowserTargeting;
        private int _chosenVariantOsTargeting;
        private int _chosenVariantProviderTargeting;
        private int _chosenVariantGeoTargeting;
        private int _variant;

        public List<string> Errors = new List<string>(); //список ошибок (в каждой строке - спарсенное со страницы описание ошибки)
        public bool WasMismatch = false;

        public void EditPk()
        {
            GetDriver();
            SetUpFields();
            EditingIsSuccessful();
        }

        private void GetDriver()
        {
            _driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный ранее драйвер
            _driver.Navigate().GoToUrl(Paths.UrlEditPk); //заходим по ссылке
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
        }

        private void SetUpFields()
        {
            _pkEditModel = new GoodsEditPkModel();
            _pkEditModel.driver = _driver;
            LogTrace.WriteInLog(Goods_View.tab2 + _driver.Url);

            #region Редактирование полей
                #region Разное
                    if (!_pkEditModel.GetViewSensors) //если checkbox не выбран...
                    {
                        _pkEditModel.ViewSensors = true; //...выбираем его
                        LogTrace.WriteInLog(Goods_View.tab2 + "Выбран checkbox Просмотр датчиков");
                    }

                    if (!_pkEditModel.GetViewConversion)
                    {
                        _pkEditModel.ViewConversion = true;
                        LogTrace.WriteInLog(Goods_View.tab2 + "Выбран checkbox Просмотр конверсии");
                    }

                    _namePk = _randoms.RandomString(15) + " " + _randoms.RandomNumber(5);
                    _pkEditModel.Name = _namePk;
                    LogTrace.WriteInLog(Goods_View.tab2 + "Заполняю поле Название РК. Было введено: " + _pkEditModel.Name);

                    //dateStartPk = pkEditModel.GenerateDate();
                    _pkEditModel.StartPkDate = _pkEditModel.GenerateDate();
                    _driver.FindElement(By.Id("when_autostart")).Click();//чтобы обновилось содержимое полей и к месяцам и дням < 10 добавились 0
                    _driver.FindElement(By.Id("editsite")).Click();
                    _dateStartPk = _pkEditModel.GetStartPkDate;
                    LogTrace.WriteInLog(Goods_View.tab2 + "Заполняю поле Дата старта РК. Было введено: " + _pkEditModel.StartPkDate);

                    //dateEndPk = pkEditModel.GenerateDate();
                    _pkEditModel.EndPkDate = _pkEditModel.GenerateDate();
                    _driver.FindElement(By.Id("limit_date")).Click();//чтобы обновилось содержимое полей и к месяцам и дням < 10 добавились 0
                    _driver.FindElement(By.Id("editsite")).Click();
                    _dateEndPk = _pkEditModel.GetEndPkDate;
                    LogTrace.WriteInLog(Goods_View.tab2 + "Заполняю поле Дата окончания РК. Было введено: " + _pkEditModel.EndPkDate);
                    List<string> instantErrorsDate = _pkEditModel.ErrorsInFillFields();
                    if (instantErrorsDate.Count != 0) //если список с ошибками заполнения полей даты непуст
                        Errors = instantErrorsDate; //копируем в нас общий список ошибок errors

                    //if(!pkEditModel.GetBlockTeasersAfterCreation)
                    //{
                    //    pkEditModel.BlockTeasersAfterCreation = true;
                    //    LogTrace.WriteInLog(Goods_View.tab2 + "Выбран checkbox Блокировать тизеры после их создания");
                    //}

                    if (!_pkEditModel.GetStoppedByManager)
                    {
                        _pkEditModel.StoppedByManager = true;
                        LogTrace.WriteInLog(Goods_View.tab2 + "Выбран checkbox Остановлена менеджером");
                    }
                #endregion

                #region Ограничения рекламной кампании
                    _variant = _chosenVariantLimitsPk = needSetRadioButton(3);
                    _pkEditModel.LimitsOfPk = _variant;
                    switch (_variant)
                    {
                        case 0:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Ограничения рекламной кампании. Выбрано: не использовать");
                                break;
                            }
                        case 1:
                            {
                                Random rndd = new Random();
                                _dayLimitByBudget = rndd.Next(5, 20).ToString();
                                _generalLimitByBudget = rndd.Next(20, 50).ToString();
                                
                                LogTrace.WriteInLog("     Выбираю radiobutton Ограничения рекламной кампании. Выбрано: по бюджету");
                                _pkEditModel.DayLimitByBudget = _dayLimitByBudget.ToString();//суточный лимит должен быть не менее 5
                                LogTrace.WriteInLog("        Заполняю поле Суточный лимит РК. Было введено: " + _pkEditModel.DayLimitByBudget);
                                //_dayLimitByBudget = _dayLimitByBudget + ".00";

                                _pkEditModel.GeneralLimitByBudget = _generalLimitByBudget.ToString();
                                LogTrace.WriteInLog("        Заполняю поле Общий лимит РК. Было введено: " + _pkEditModel.GeneralLimitByBudget);
                                //_generalLimitByBudget = _generalLimitByBudget + ".00";
                                break;
                            }
                        case 2:
                            {
                                Random rnd = new Random();
                                _dayLimitByClicks = rnd.Next(50, 70).ToString();
                                _generalLimitByClicks = rnd.Next(70, 100).ToString();
                                
                                LogTrace.WriteInLog("     Выбираю radiobutton Ограничения рекламной кампании. Выбрано: по количеству кликов");
                                _pkEditModel.DayLimitByClicks = _dayLimitByClicks.ToString();
                                LogTrace.WriteInLog("        Заполняю поле Суточный лимит кликов РК. Было введено: " + _pkEditModel.DayLimitByClicks);
                                //_dayLimitByClicks = _dayLimitByClicks + ".00";
                                
                                _pkEditModel.GeneralLimitByClicks = _generalLimitByClicks.ToString();
                                LogTrace.WriteInLog("        Заполняю поле Общий лимит кликов РК. Было введено: " + _pkEditModel.GeneralLimitByClicks);
                                //_generalLimitByClicks = _generalLimitByClicks + ".00";
                                break;
                            }
                    }
                #endregion

                #region UTM-разметка рекламной кампании для Google Analytics
                    if (!_pkEditModel.GetUtmPkForGoogleAnalytics)
                    {
                        _pkEditModel.UtmPkForGoogleAnalytics = true;
                        LogTrace.WriteInLog("          Выбран checkbox UTM-разметка рекламной кампании для Google Analytics");
                    }
                    _utmMedium = _randoms.RandomString(5);
                    _pkEditModel.UtmMedium = _utmMedium;
                    LogTrace.WriteInLog("             Заполняю поле utm_medium (средство кампании). Было введено: " + _pkEditModel.UtmMedium);

                    _utmSource = _randoms.RandomString(5);
                    _pkEditModel.UtmSource = _utmSource;
                    LogTrace.WriteInLog("             Заполняю поле utm_source (источник кампании). Было введено: " + _pkEditModel.UtmSource);

                    _utmCampaign = _randoms.RandomString(5);
                    _pkEditModel.UtmCampaign = _utmCampaign;
                    LogTrace.WriteInLog("             Заполняю поле utm_campaign (название кампании). Было введено: " + _pkEditModel.UtmCampaign);
                #endregion

                #region UTM-разметка пользователя
                    if (!_pkEditModel.GetUtmUser)
                    {
                        _pkEditModel.UtmUser = true;
                        LogTrace.WriteInLog("          Выбран checkbox UTM-разметка пользователя");
                    }
                    _utmUserStr = _randoms.RandomString(5);
                    _pkEditModel.UtmUserStr = _utmUserStr;
                    LogTrace.WriteInLog("             Заполняю поле UTM-разметка пользователя. Было введено: " + _pkEditModel.UtmUserStr);
                #endregion

                #region Крутить в сети Товарро
                    if (!_pkEditModel.GetScrewInTovarro)
                    {
                        LogTrace.WriteInLog("          Выбран checkbox Крутить в сети Товарро");
                        _pkEditModel.ScrewInTovarro = true;
                    }
                #endregion

                #region Блокировка по расписанию
                    if (!_pkEditModel.GetBlockBySchedule)
                    {
                        _pkEditModel.BlockBySchedule = true;
                        LogTrace.WriteInLog("          Выбран checkbox Блокировка по расписанию");
                    }
                    if (!_pkEditModel.GetWeekends)
                    {
                        _pkEditModel.Weekends = true;
                        LogTrace.WriteInLog("             Выбран checkbox Выходные");
                    }
                    if (!_pkEditModel.GetWeekdays)
                    {
                        _pkEditModel.Weekdays = true;
                        LogTrace.WriteInLog("             Выбран checkbox Будни");
                    }
                    if (!_pkEditModel.GetWorkingTime)
                    {
                        _pkEditModel.WorkingTime = true;
                        LogTrace.WriteInLog("             Выбран checkbox Рабочее время (9-18 по будням)");
                    }
                #endregion

                #region Передавать id площадки в ссылке
                    if (!_pkEditModel.GetIdOfPlatformInLink)
                    {
                        _pkEditModel.IdOfPlatformInLink = true;
                        LogTrace.WriteInLog("          Выбран checkbox Передавать id площадки в ссылке");
                    }
                    _idOfPlatformInLink = _randoms.RandomString(5);
                    _pkEditModel.IdOfPlatformInLinkStr = _idOfPlatformInLink;
                    LogTrace.WriteInLog("             Заполняю поле Передавать id площадки в ссылке. Было введено: " + _pkEditModel.IdOfPlatformInLinkStr);
                #endregion

                #region Передавать id площадки в ссылке
                    if (!_pkEditModel.GetAddIdOfTeaserInLink)
                    {
                        _pkEditModel.AddIdOfTeaserInLink = true;
                        LogTrace.WriteInLog("          Выбран checkbox Добавлять id тизера в конец ссылки");
                    }
                #endregion

                #region Комментарий к кампании
                    _commentsForPk = _randoms.RandomString(20) + " " + _randoms.RandomString(10);
                    _pkEditModel.CommentsForPk = _commentsForPk;
                    LogTrace.WriteInLog("          Заполняю textarea Комментарий к кампании. Было введено: " + _pkEditModel.CommentsForPk);
                #endregion

                #region Площадки
                    if (!_pkEditModel.GetPlatforms)
                    {
                        _pkEditModel.Platforms = true;
                        LogTrace.WriteInLog("          Выбран checkbox Площадки");
                    }
                    if (!_pkEditModel.GetPlatformsNotSpecified)
                    {
                        _pkEditModel.PlatformsNotSpecified = true;
                        LogTrace.WriteInLog("             Выбран checkbox Не определено");
                    }
                    if (!_pkEditModel.GetPlatformsPolitics)
                    {
                        _pkEditModel.PlatformsPolitics = true;
                        LogTrace.WriteInLog("             Выбран checkbox Политика, общество, происшествия, религия");
                    }

                    if (!_pkEditModel.GetPlatformsEconomics)
                    {
                        _pkEditModel.PlatformsEconomics = true;
                        LogTrace.WriteInLog("             Выбран checkbox Экономика, финансы, недвижимость, работа и карьера");
                    }
                    if (!_pkEditModel.GetPlatformsCelebrities)
                    {
                        _pkEditModel.PlatformsCelebrities = true;
                        LogTrace.WriteInLog("             Выбран checkbox Знаменитости, шоу-бизнес, кино, музыка");
                    }
                    if (!_pkEditModel.GetPlatformsScience)
                    {
                        _pkEditModel.PlatformsScience = true;
                        LogTrace.WriteInLog("             Выбран checkbox Наука и технологии");
                    }
                    if (!_pkEditModel.GetPlatformsConnection)
                    {
                        _pkEditModel.PlatformsConnection = true;
                        LogTrace.WriteInLog("             Выбран checkbox Связь, компьютеры, программы");
                    }
                    if (!_pkEditModel.GetPlatformsSports)
                    {
                        _pkEditModel.PlatformsSports = true;
                        LogTrace.WriteInLog("             Выбран checkbox Спорт");
                    }
                    if (!_pkEditModel.GetPlatformsAuto)
                    {
                        _pkEditModel.PlatformsAuto = true;
                        LogTrace.WriteInLog("             Выбран checkbox Авто-вело-мото");
                    }
                    if (!_pkEditModel.GetPlatformsFashion)
                    {
                        _pkEditModel.PlatformsFashion = true;
                        LogTrace.WriteInLog("             Выбран checkbox Мода и стиль, здоровье и красота, фитнес и диета, кулинария");
                    }
                    if (!_pkEditModel.GetPlatformsMedicine)
                    {
                        _pkEditModel.PlatformsMedicine = true;
                        LogTrace.WriteInLog("             Выбран checkbox Медицина");
                    }
                    if (!_pkEditModel.GetPlatformsTourism)
                    {
                        _pkEditModel.PlatformsTourism = true;
                        LogTrace.WriteInLog("             Выбран checkbox Туризм и отдых (путевки, отели, рестораны)");
                    }
                    if (!_pkEditModel.GetPlatformsGlobalPortals)
                    {
                        _pkEditModel.PlatformsGlobalPortals = true;
                        LogTrace.WriteInLog("             Выбран checkbox Глобальные порталы с множеством подпроектов");
                    }
                    if (!_pkEditModel.GetPlatformsHumor)
                    {
                        _pkEditModel.PlatformsHumor = true;
                        LogTrace.WriteInLog("             Выбран checkbox Юмор (приколы, картинки, обои), каталог фотографий, блоги");
                    }
                    if (!_pkEditModel.GetPlatformsFileshares)
                    {
                        _pkEditModel.PlatformsFileshares = true;
                        LogTrace.WriteInLog("             Выбран checkbox Файлообменники, файлокачалки (кино, музыка, игры, программы)");
                    }
                    if (!_pkEditModel.PlatformsSocialNetworks)
                    {
                        _pkEditModel.PlatformsSocialNetworks = true;
                        LogTrace.WriteInLog("             Выбран checkbox Социальные сети, сайты знакомства, личные дневники");
                    }
                    if (!_pkEditModel.GetPlatformsAnimals)
                    {
                        _pkEditModel.PlatformsAnimals = true;
                        LogTrace.WriteInLog("             Выбран checkbox Животный и растительный мир");
                    }
                    if (!_pkEditModel.GetPlatformsReligion)
                    {
                        _pkEditModel.PlatformsReligion = true;
                        LogTrace.WriteInLog("             Выбран checkbox Религия");
                    }
                    if (!_pkEditModel.GetPlatformsChildren)
                    {
                        _pkEditModel.PlatformsChildren = true;
                        LogTrace.WriteInLog("             Выбран checkbox Дети и родители");
                    }
                    if (!_pkEditModel.GetPlatformsBuilding)
                    {
                        _pkEditModel.PlatformsBuilding = true;
                        LogTrace.WriteInLog("             Выбран checkbox Строительство, ремонт, дача, огород");
                    }
                    if (!_pkEditModel.GetPlatformsCookery)
                    {
                        _pkEditModel.PlatformsCookery = true;
                        LogTrace.WriteInLog("             Выбран checkbox Кулинария");
                    }
                    if (!_pkEditModel.GetPlatformsJob)
                    {
                        _pkEditModel.PlatformsJob = true;
                        LogTrace.WriteInLog("             Выбран checkbox Работа и карьера. Поиск работы, поиск персонала");
                    }
                    if (!_pkEditModel.GetPlatformsNotSites)
                    {
                        _pkEditModel.PlatformsNotSites = true;
                        LogTrace.WriteInLog("             Выбран checkbox Не сайты (программы, тулбары, таскбары)");
                    }
                    if (!_pkEditModel.GetPlatformsSitesStartPagesBrowsers)
                    {
                        _pkEditModel.PlatformsSitesStartPagesBrowsers = true;
                        LogTrace.WriteInLog("             Выбран checkbox Сайты, размещенные на стартовых страницах браузеров");
                    }
                    if (!_pkEditModel.GetPlatformsSearchSystems)
                    {
                        _pkEditModel.PlatformsSearchSystems = true;
                        LogTrace.WriteInLog("             Выбран checkbox Поисковые системы");
                    }
                    if (!_pkEditModel.GetPlatformsEmail)
                    {
                        _pkEditModel.PlatformsEmail = true;
                        LogTrace.WriteInLog("             Выбран checkbox Почта");
                    }
                    if (!_pkEditModel.GetPlatformsPhotoCatalogues)
                    {
                        _pkEditModel.PlatformsPhotoCatalogues = true;
                        LogTrace.WriteInLog("             Выбран checkbox Каталоги фотографий");
                    }
                    if (!_pkEditModel.GetPlatformsVarez)
                    {
                        _pkEditModel.PlatformsVarez = true;
                        LogTrace.WriteInLog("             Выбран checkbox Варезники");
                    }
                    if (!_pkEditModel.GetPlatformsOnlineVideo)
                    {
                        _pkEditModel.PlatformsOnlineVideo = true;
                        LogTrace.WriteInLog("             Выбран checkbox Онлайн видео, телевидение, радио");
                    }
                    if (!_pkEditModel.GetPlatformsOnlineLibraries)
                    {
                        _pkEditModel.PlatformsOnlineLibraries = true;
                        LogTrace.WriteInLog("             Выбран checkbox Онлайн-библиотеки");
                    }
                    if (!_pkEditModel.GetPlatformsInternet)
                    {
                        _pkEditModel.PlatformsInternet = true;
                        LogTrace.WriteInLog("             Выбран checkbox Интернет, поисковые сайты, электронная почта, интернет-магазины, аукционы, каталоги ресурсов, фирм и предприятий");
                    }
                    if (!_pkEditModel.GetPlatformsOnlineGames)
                    {
                        _pkEditModel.PlatformsOnlineGames = true;
                        LogTrace.WriteInLog("             Выбран checkbox Онлайн игры");
                    }
                    if (!_pkEditModel.GetPlatformsInternetRepresentatives)
                    {
                        _pkEditModel.PlatformsInternetRepresentatives = true;
                        LogTrace.WriteInLog("             Выбран checkbox Интернет-представительства бизнеса.");
                    }
                    if (!_pkEditModel.GetPlatformsProgramms)
                    {
                        _pkEditModel.PlatformsProgramms = true;
                        LogTrace.WriteInLog("             Выбран checkbox Программы, прошивки, игры для КПК и мобильных устройств");
                    }
                    if (!_pkEditModel.GetPlatformsCataloguesInternetResources)
                    {
                        _pkEditModel.PlatformsCataloguesInternetResources = true;
                        LogTrace.WriteInLog("             Выбран checkbox Каталоги Интернет - ресурсов, фирм и предприятий");
                    }
                    if (!_pkEditModel.GetPlatformsForWagesInInternet)
                    {
                        _pkEditModel.PlatformsForWagesInInternet = true;
                        LogTrace.WriteInLog("             Выбран checkbox Для заработка в Интернете. Партнерские программы");
                    }
                    if (!_pkEditModel.GetPlatformsHobbies)
                    {
                        _pkEditModel.PlatformsHobbies = true;
                        LogTrace.WriteInLog("             Выбран checkbox Хобби и увлечения");
                    }
                    if (!_pkEditModel.GetPlatformsMarketgid)
                    {
                        _pkEditModel.PlatformsMarketgid = true;
                        LogTrace.WriteInLog("             Выбран checkbox Маркетгид");
                    }
                    if (!_pkEditModel.GetPlatformsShock)
                    {
                        _pkEditModel.PlatformsShock = true;
                        LogTrace.WriteInLog("             Выбран checkbox Шокодром");
                    }
                    if (!_pkEditModel.GetPlatformsEsoteric)
                    {
                        _pkEditModel.PlatformsEsoteric = true;
                        LogTrace.WriteInLog("             Выбран checkbox Эзотерика. Непознанное, астрология, гороскопы, гадания");
                    }
                    if (!_pkEditModel.GetPlatformsPsychology)
                    {
                        _pkEditModel.PlatformsPsychology = true;
                        LogTrace.WriteInLog("             Выбран checkbox Психология, мужчина и женщина");
                    }
                    if (!_pkEditModel.GetPlatformsHistory)
                    {
                        _pkEditModel.PlatformsHistory = true;
                        LogTrace.WriteInLog("             Выбран checkbox История, образование, культура");
                    }
                    if (!_pkEditModel.GetPlatformsMarketgidWomenNet)
                    {
                        _pkEditModel.PlatformsMarketgidWomenNet = true;
                        LogTrace.WriteInLog("             Выбран checkbox Маркетгид ЖС");
                    }
                #endregion

                #region Демографический таргетинг
                    _variant = _chosenVariantDemoTargeting = needSetRadioButton(2);
                    _pkEditModel.DemoTargeting = _variant;
                    switch (_variant)
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
                                _pkEditModel.DemoTargetingMenExpand = true;
                                _pkEditModel.DemoTargetingWomenExpand = true;
                                _pkEditModel.DemoTargetingHermaphroditeExpand = true;

                                #region Мужчины
                                    //pkEditModel.DemoTargetingMenNotSpecified = true;
                                    if (!_pkEditModel.GetDemoTargetingMenChoseAll)
                                    {
                                        _pkEditModel.DemoTargetingMenChoseAll = true;
                                        LogTrace.WriteInLog("             Мужчины. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingMenNotSpecified)
                                    {
                                        _pkEditModel.DemoTargetingMenNotSpecified = true;
                                        LogTrace.WriteInLog("             Мужчины. Выбран checkbox Не определен");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingMen618)
                                    {
                                        _pkEditModel.DemoTargetingMen618 = true;
                                        LogTrace.WriteInLog("             Мужчины. Выбран checkbox 6-18");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingMen1924)
                                    {
                                        _pkEditModel.DemoTargetingMen1924 = true;
                                        LogTrace.WriteInLog("             Мужчины. Выбран checkbox 19-24");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingMen2534)
                                    {
                                        _pkEditModel.DemoTargetingMen2534 = true;
                                        LogTrace.WriteInLog("             Мужчины. Выбран checkbox 25-34");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingMen3544)
                                    {
                                        _pkEditModel.DemoTargetingMen3544 = true;
                                        LogTrace.WriteInLog("             Мужчины. Выбран checkbox 35-44");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingMen4590)
                                    {
                                        _pkEditModel.DemoTargetingMen4590 = true;
                                        LogTrace.WriteInLog("             Мужчины. Выбран checkbox 45-90");
                                    }
                                #endregion

                                #region Женщины
                                    if (!_pkEditModel.GetDemoTargetingWomenChoseAll)
                                    {
                                        _pkEditModel.DemoTargetingWomenChoseAll = true;
                                        LogTrace.WriteInLog("             Женщины. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingWomenNotSpecified)
                                    {
                                        _pkEditModel.DemoTargetingWomenNotSpecified = true;
                                        LogTrace.WriteInLog("             Женщины. Выбран checkbox Не определен");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingWomen618)
                                    {
                                        _pkEditModel.DemoTargetingWomen618 = true;
                                        LogTrace.WriteInLog("             Женщины. Выбран checkbox 6-18");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingWomen1924)
                                    {
                                        _pkEditModel.DemoTargetingWomen1924 = true;
                                        LogTrace.WriteInLog("             Женщины. Выбран checkbox 19-24");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingWomen2534)
                                    {
                                        _pkEditModel.DemoTargetingWomen2534 = true;
                                        LogTrace.WriteInLog("             Женщины. Выбран checkbox 25-34");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingWomen3544)
                                    {
                                        _pkEditModel.DemoTargetingWomen3544 = true;
                                        LogTrace.WriteInLog("             Женщины. Выбран checkbox 35-44");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingWomen4590)
                                    {
                                        _pkEditModel.DemoTargetingWomen4590 = true;
                                        LogTrace.WriteInLog("             Женщины. Выбран checkbox 45-90");
                                    }
                                #endregion

                                #region Пол не определен
                                    if (!_pkEditModel.GetDemoTargetingHermaphroditeChoseAll)
                                    {
                                        _pkEditModel.DemoTargetingHermaphroditeChoseAll = true;
                                        LogTrace.WriteInLog("             Пол не определен. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingHermaphrodite618)
                                    {
                                        _pkEditModel.DemoTargetingHermaphrodite618 = true;
                                        LogTrace.WriteInLog("             Пол не определен. Выбран checkbox 6-18");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingHermaphrodite1924)
                                    {
                                        _pkEditModel.DemoTargetingHermaphrodite1924 = true;
                                        LogTrace.WriteInLog("             Пол не определен. Выбран checkbox 19-24");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingHermaphrodite2534)
                                    {
                                        _pkEditModel.DemoTargetingHermaphrodite2534 = true;
                                        LogTrace.WriteInLog("             Пол не определен. Выбран checkbox 25-34");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingHermaphrodite3544)
                                    {
                                        _pkEditModel.DemoTargetingHermaphrodite3544 = true;
                                        LogTrace.WriteInLog("             Пол не определен. Выбран checkbox 35-44");
                                    }
                                    if (!_pkEditModel.GetDemoTargetingHermaphrodite4590)
                                    {
                                        _pkEditModel.DemoTargetingHermaphrodite4590 = true;
                                        LogTrace.WriteInLog("             Пол не определен. Выбран checkbox 45-90");
                                    }
                                #endregion

                                break;
                            }
                    }
                #endregion

                #region Таргетинг по интересам
                    _variant = _chosenVariantInterestsTargeting = needSetRadioButton(2);
                    _pkEditModel.InterestsTargeting = _variant;
                    switch (_variant)
                    {
                        case 0:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Таргетинг по интересам. Выбрано: не использовать");
                                break;
                            }
                        case 1:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Таргетинг по интересам. Выбрано: использовать");
                                _pkEditModel.InterestsTargetingBusinessExpand = true;
                                _pkEditModel.InterestsTargetingRealtyExpand = true;
                                _pkEditModel.InterestsTargetingEducationExpand = true;
                                _pkEditModel.InterestsTargetingRestExpand = true;
                                _pkEditModel.InterestsTargetingTelephonesExpand = true;
                                _pkEditModel.InterestsTargetingMedicineExpand = true;
                                _pkEditModel.InterestsTargetingHouseExpand = true;
                                _pkEditModel.InterestsTargetingFinanceExpand = true;
                                _pkEditModel.InterestsTargetingComputersExpand = true;
                                _pkEditModel.InterestsTargetingAutoExpand = true;
                                _pkEditModel.InterestsTargetingAudioExpand = true;

                                //pkEditModel.InterestsTargetingOther = true;
                                if (!_pkEditModel.GetInterestsTargetingOther)
                                {
                                    _pkEditModel.InterestsTargetingOther = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Прочее");
                                }

                                #region Бизнес
                                    if (!_pkEditModel.GetInterestsTargetingBusinessChoseAll)
                                    {
                                        _pkEditModel.InterestsTargetingBusinessChoseAll = true;
                                        LogTrace.WriteInLog("             Бизнес. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingBusinessAcoountancy)
                                    {
                                        _pkEditModel.InterestsTargetingBusinessAcoountancy = true;
                                        LogTrace.WriteInLog("             Бизнес. Выбран checkbox Бухгалтерия");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingBusinessPlacement)
                                    {
                                        _pkEditModel.InterestsTargetingBusinessPlacement = true;
                                        LogTrace.WriteInLog("             Бизнес. Выбран checkbox Трудоустройство, персонал");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingBusinessAudit)
                                    {
                                        _pkEditModel.InterestsTargetingBusinessAudit = true;
                                        LogTrace.WriteInLog("             Бизнес. Выбран checkbox Аудит, консалтинг");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingBusinessAdverts)
                                    {
                                        _pkEditModel.InterestsTargetingBusinessAdverts = true;
                                        LogTrace.WriteInLog("             Бизнес. Выбран checkbox Реклама");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingBusinessMiscellanea)
                                    {
                                        _pkEditModel.InterestsTargetingBusinessMiscellanea = true;
                                        LogTrace.WriteInLog("             Бизнес. Выбран checkbox Разное");
                                    }
                                #endregion

                                #region Недвижимость
                                    if (!_pkEditModel.GetInterestsTargetingRealtyChoseAll)
                                    {
                                        _pkEditModel.InterestsTargetingRealtyChoseAll = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingRealtyMiscelanea)
                                    {
                                        _pkEditModel.InterestsTargetingRealtyMiscelanea = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Разное");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingRealtyGarages)
                                    {
                                        _pkEditModel.InterestsTargetingRealtyGarages = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Гаражи");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingRealtyFlats)
                                    {
                                        _pkEditModel.InterestsTargetingRealtyFlats = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Квартиры");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingRealtyAbroad)
                                    {
                                        _pkEditModel.InterestsTargetingRealtyAbroad = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Зарубежная недвижимость");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingRealtyLand)
                                    {
                                        _pkEditModel.InterestsTargetingRealtyLand = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Земля");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingRealtySuburban)
                                    {
                                        _pkEditModel.InterestsTargetingRealtySuburban = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Загородная недвижимость");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingRealtyHypothec)
                                    {
                                        _pkEditModel.InterestsTargetingRealtyHypothec = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Ипотека");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingRealtyCommerce)
                                    {
                                        _pkEditModel.InterestsTargetingRealtyCommerce = true;
                                        LogTrace.WriteInLog("             Недвижимость. Выбран checkbox Коммерческая недвижимость");
                                    }
                                #endregion

                                if (!_pkEditModel.GetInterestsTargetingExhibitions)
                                {
                                    _pkEditModel.InterestsTargetingExhibitions = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Выставки, концерты, театры, кино");
                                }

                                #region Образование
                                    if (!_pkEditModel.GetInterestsTargetingEducationChoseAll)
                                    {
                                        _pkEditModel.InterestsTargetingEducationChoseAll = true;
                                        LogTrace.WriteInLog("             Образование. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingEducationForeignLanguages)
                                    {
                                        _pkEditModel.InterestsTargetingEducationForeignLanguages = true;
                                        LogTrace.WriteInLog("             Образование. Выбран checkbox Иностранные языки");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingEducationAbroad)
                                    {
                                        _pkEditModel.InterestsTargetingEducationAbroad = true;
                                        LogTrace.WriteInLog("             Образование. Выбран checkbox Образование за рубежом");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingEducationHigh)
                                    {
                                        _pkEditModel.InterestsTargetingEducationHigh = true;
                                        LogTrace.WriteInLog("             Образование. Выбран checkbox Образование высшее");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingEducationMiscelanea)
                                    {
                                        _pkEditModel.InterestsTargetingEducationMiscelanea = true;
                                        LogTrace.WriteInLog("             Образование. Выбран checkbox Разное");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingEducationChildren)
                                    {
                                        _pkEditModel.InterestsTargetingEducationChildren = true;
                                        LogTrace.WriteInLog("             Образование. Выбран checkbox Образование для детей");
                                    }
                                #endregion

                                #region Отдых, туризм, путешествия
                                    if (!_pkEditModel.GetInterestsTargetingRestChoseAll)
                                    {
                                        _pkEditModel.InterestsTargetingRestChoseAll = true;
                                        LogTrace.WriteInLog("             Отдых, туризм, путешествия. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingRestMiscellanea)
                                    {
                                        _pkEditModel.InterestsTargetingRestMiscellanea = true;
                                        LogTrace.WriteInLog("             Отдых, туризм, путешествия. Выбран checkbox Разное");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingRestRuUa)
                                    {
                                        _pkEditModel.InterestsTargetingRestRuUa = true;
                                        LogTrace.WriteInLog("             Отдых, туризм, путешествия. Выбран checkbox Отдых в России и Украине");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingRestAbroad)
                                    {
                                        _pkEditModel.InterestsTargetingRestAbroad = true;
                                        LogTrace.WriteInLog("             Отдых, туризм, путешествия. Выбран checkbox Отдых за рубежом");
                                    }
                                #endregion

                                #region Телефоны, связь
                                    if (!_pkEditModel.GetInterestsTargetingTelephonesChoseAll)
                                    {
                                        _pkEditModel.InterestsTargetingTelephonesChoseAll = true;
                                        LogTrace.WriteInLog("             Телефоны, связь. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingTelephonesMiscellanea)
                                    {
                                        _pkEditModel.InterestsTargetingTelephonesMiscellanea = true;
                                        LogTrace.WriteInLog("             Телефоны, связь. Выбран checkbox Разное");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingTelephonesNavigation)
                                    {
                                        _pkEditModel.InterestsTargetingTelephonesNavigation = true;
                                        LogTrace.WriteInLog("             Телефоны, связь. Выбран checkbox Навигация");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingTelephonesMobileApps)
                                    {
                                        _pkEditModel.InterestsTargetingTelephonesMobileApps = true;
                                        LogTrace.WriteInLog("             Телефоны, связь. Выбран checkbox Мобильные приложения и услуги");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingTelephonesMobile)
                                    {
                                        _pkEditModel.InterestsTargetingTelephonesMobile = true;
                                        LogTrace.WriteInLog("             Телефоны, связь. Выбран checkbox Мобильные телефоны");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingTelephonesStationary)
                                    {
                                        _pkEditModel.InterestsTargetingTelephonesStationary = true;
                                        LogTrace.WriteInLog("             Телефоны, связь. Выбран checkbox Стационарная связь");
                                    }
                                #endregion

                                if (!_pkEditModel.GetInterestsTargetingHouseAplliances)
                                {
                                    _pkEditModel.InterestsTargetingHouseAplliances = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Бытовая техника");
                                }


                                #region Медицина, здоровье
                                    if (!_pkEditModel.GetInterestsTargetingMedicineChoseAll)
                                    {
                                        _pkEditModel.InterestsTargetingMedicineChoseAll = true;
                                        LogTrace.WriteInLog("             Медицина, здоровье. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingMedicineSport)
                                    {
                                        _pkEditModel.InterestsTargetingMedicineSport = true;
                                        LogTrace.WriteInLog("             Медицина, здоровье. Выбран checkbox Спорт, фитнес, йога");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingMedicineEyesight)
                                    {
                                        _pkEditModel.InterestsTargetingMedicineEyesight = true;
                                        LogTrace.WriteInLog("             Медицина, здоровье. Выбран checkbox Зрение");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingMedicineMiscellanea)
                                    {
                                        _pkEditModel.InterestsTargetingMedicineMiscellanea = true;
                                        LogTrace.WriteInLog("             Медицина, здоровье. Выбран checkbox Разное");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingMedicineDiets)
                                    {
                                        _pkEditModel.InterestsTargetingMedicineDiets = true;
                                        LogTrace.WriteInLog("             Медицина, здоровье. Выбран checkbox Диеты");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingMedicineExtraWeight)
                                    {
                                        _pkEditModel.InterestsTargetingMedicineExtraWeight = true;
                                        LogTrace.WriteInLog("             Медицина, здоровье. Выбран checkbox Лишний вес");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingMedicinePregnancy)
                                    {
                                        _pkEditModel.InterestsTargetingMedicinePregnancy = true;
                                        LogTrace.WriteInLog("             Медицина, здоровье. Выбран checkbox Беременность и роды");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingMedicineStomatology)
                                    {
                                        _pkEditModel.InterestsTargetingMedicineStomatology = true;
                                        LogTrace.WriteInLog("             Медицина, здоровье. Выбран checkbox Стоматология");
                                    }
                                #endregion

                                #region Дом и семья
                                    if (!_pkEditModel.GetInterestsTargetingHouseChoseAll)
                                    {
                                        _pkEditModel.InterestsTargetingHouseChoseAll = true;
                                        LogTrace.WriteInLog("             Дом и семья. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingHouseChildren)
                                    {
                                        _pkEditModel.InterestsTargetingHouseChildren = true;
                                        LogTrace.WriteInLog("             Дом и семья. Выбран checkbox Маленькие дети");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingHouseDogs)
                                    {
                                        _pkEditModel.InterestsTargetingHouseDogs = true;
                                        LogTrace.WriteInLog("             Дом и семья. Выбран checkbox Собаки");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingHouseMiscellanea)
                                    {
                                        _pkEditModel.InterestsTargetingHouseMiscellanea = true;
                                        LogTrace.WriteInLog("             Дом и семья. Выбран checkbox Разное");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingHouseCats)
                                    {
                                        _pkEditModel.InterestsTargetingHouseCats = true;
                                        LogTrace.WriteInLog("             Дом и семья. Выбран checkbox Кошки");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingHouseCookery)
                                    {
                                        _pkEditModel.InterestsTargetingHouseCookery = true;
                                        LogTrace.WriteInLog("             Дом и семья. Выбран checkbox Кулинария, рецепты");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingHouseKindergartens)
                                    {
                                        _pkEditModel.InterestsTargetingHouseKindergartens = true;
                                        LogTrace.WriteInLog("             Дом и семья. Выбран checkbox Детские сады");
                                    }
                                #endregion

                                #region Финансы
                                    if (!_pkEditModel.GetInterestsTargetingFinanceChoseAll)
                                    {
                                        _pkEditModel.InterestsTargetingFinanceChoseAll = true;
                                        LogTrace.WriteInLog("             Финансы. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingFinanceStockMarket)
                                    {
                                        _pkEditModel.InterestsTargetingFinanceStockMarket = true;
                                        LogTrace.WriteInLog("             Финансы. Выбран checkbox Фондовый рынок");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingFinanceCurrency)
                                    {
                                        _pkEditModel.InterestsTargetingFinanceCurrency = true;
                                        LogTrace.WriteInLog("             Финансы. Выбран checkbox Валюта");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingFinanceInsurence)
                                    {
                                        _pkEditModel.InterestsTargetingFinanceInsurence = true;
                                        LogTrace.WriteInLog("             Финансы. Выбран checkbox Страхование");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingFinanceMoneyTransfers)
                                    {
                                        _pkEditModel.InterestsTargetingFinanceMoneyTransfers = true;
                                        LogTrace.WriteInLog("             Финансы. Выбран checkbox Денежные переводы");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingFinanceCredits)
                                    {
                                        _pkEditModel.InterestsTargetingFinanceCredits = true;
                                        LogTrace.WriteInLog("             Финансы. Выбран checkbox Кредиты");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingFinanceMiscellanea)
                                    {
                                        _pkEditModel.InterestsTargetingFinanceMiscellanea = true;
                                        LogTrace.WriteInLog("             Финансы. Выбран checkbox Разное");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingFinanceDeposits)
                                    {
                                        _pkEditModel.InterestsTargetingFinanceDeposits = true;
                                        LogTrace.WriteInLog("             Финансы. Выбран checkbox Вклады, депозиты");
                                    }
                                #endregion

                                #region Компьютеры, оргтехника
                                    if (!_pkEditModel.GetInterestsTargetingComputersChoseAll)
                                    {
                                        _pkEditModel.InterestsTargetingComputersChoseAll = true;
                                        LogTrace.WriteInLog("             Компьютеры, оргтехника. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingComputersLaptops)
                                    {
                                        _pkEditModel.InterestsTargetingComputersLaptops = true;
                                        LogTrace.WriteInLog("             Компьютеры, оргтехника. Выбран checkbox Ноутбуки");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingComputersParts)
                                    {
                                        _pkEditModel.InterestsTargetingComputersParts = true;
                                        LogTrace.WriteInLog("             Компьютеры, оргтехника. Выбран checkbox Компьютерные комплектующие");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingComputersPrinters)
                                    {
                                        _pkEditModel.InterestsTargetingComputersPrinters = true;
                                        LogTrace.WriteInLog("             Компьютеры, оргтехника. Выбран checkbox Принтеры");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingComputersTablets)
                                    {
                                        _pkEditModel.InterestsTargetingComputersTablets = true;
                                        LogTrace.WriteInLog("             Компьютеры, оргтехника. Выбран checkbox Планшетные ПК");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingComputersMonitors)
                                    {
                                        _pkEditModel.InterestsTargetingComputersMonitors = true;
                                        LogTrace.WriteInLog("             Компьютеры, оргтехника. Выбран checkbox Мониторы");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingComputersMiscellanea)
                                    {
                                        _pkEditModel.InterestsTargetingComputersMiscellanea = true;
                                        LogTrace.WriteInLog("             Компьютеры, оргтехника. Выбран checkbox Разное");
                                    }
                                #endregion

                                #region Авто
                                    if (!_pkEditModel.GetInterestsTargetingAutoChoseAll)
                                    {
                                        _pkEditModel.InterestsTargetingAutoChoseAll = true;
                                        LogTrace.WriteInLog("             Авто. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingAutoInsurence)
                                    {
                                        _pkEditModel.InterestsTargetingAutoInsurence = true;
                                        LogTrace.WriteInLog("             Авто. Выбран checkbox Автострахование");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingAutoMiscellanea)
                                    {
                                        _pkEditModel.InterestsTargetingAutoMiscellanea = true;
                                        LogTrace.WriteInLog("             Авто. Выбран checkbox Разное");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingAutoNational)
                                    {
                                        _pkEditModel.InterestsTargetingAutoNational = true;
                                        LogTrace.WriteInLog("             Авто. Выбран checkbox Отечественные");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingAutoWheels)
                                    {
                                        _pkEditModel.InterestsTargetingAutoWheels = true;
                                        LogTrace.WriteInLog("             Авто. Выбран checkbox Колёса, Шины");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingAutoImported)
                                    {
                                        _pkEditModel.InterestsTargetingAutoImported = true;
                                        LogTrace.WriteInLog("             Авто. Выбран checkbox Иномарки");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingAutoMoto)
                                    {
                                        _pkEditModel.InterestsTargetingAutoMoto = true;
                                        LogTrace.WriteInLog("             Авто. Выбран checkbox Мото-, Квадроциклы, Снегоходы");
                                    }
                                #endregion

                                #region Аудио, Видео, Фото
                                    if (!_pkEditModel.GetInterestsTargetingAudioChoseAll)
                                    {
                                        _pkEditModel.InterestsTargetingAudioChoseAll = true;
                                        LogTrace.WriteInLog("             Аудио, Видео, Фото. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingAudioVideoEquips)
                                    {
                                        _pkEditModel.InterestsTargetingAudioVideoEquips = true;
                                        LogTrace.WriteInLog("             Аудио, Видео, Фото. Выбран checkbox Видеоаппаратура");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingAudioMiscellanea)
                                    {
                                        _pkEditModel.InterestsTargetingAudioMiscellanea = true;
                                        LogTrace.WriteInLog("             Аудио, Видео, Фото. Выбран checkbox Разное");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingAudioTech)
                                    {
                                        _pkEditModel.InterestsTargetingAudioTech = true;
                                        LogTrace.WriteInLog("             Аудио, Видео, Фото. Выбран checkbox Аудио-техника");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingAudioCameras)
                                    {
                                        _pkEditModel.InterestsTargetingAudioCameras = true;
                                        LogTrace.WriteInLog("             Аудио, Видео, Фото. Выбран checkbox Фотоаппараты");
                                    }
                                    if (!_pkEditModel.GetInterestsTargetingAudioTvs)
                                    {
                                        _pkEditModel.InterestsTargetingAudioTvs = true;
                                        LogTrace.WriteInLog("             Аудио, Видео, Фото. Выбран checkbox Телевизоры, DVD-проигрыватели");
                                    }
                                #endregion

                                break;
                            }
                    }
                #endregion

                #region Браузеры
                    _variant = _chosenVariantBrowserTargeting = needSetRadioButton(2);
                    _pkEditModel.BrowserTargeting = _variant;
                    switch (_variant)
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
                                _pkEditModel.BrowserTargetingOtherExpand = true;
                                _pkEditModel.BrowserTargetingOperaExpand = true;
                                _pkEditModel.BrowserTargetingChromeExpand = true;
                                _pkEditModel.BrowserTargetingFirefoxExpand = true;
                                _pkEditModel.BrowserTargetingSafariExpand = true;
                                _pkEditModel.BrowserTargetingIeExpand = true;
                                //pkEditModel.BrowserTargetingGoogleChromeMobileExpand = true;
                                //pkEditModel.BrowserTargetingOtherAll = true;

                                if (!_pkEditModel.GetBrowserTargetingOtherChoseAll)
                                {
                                    _pkEditModel.BrowserTargetingOtherChoseAll = true;
                                    LogTrace.WriteInLog("        Другие. Выбран checkbox Другие Все");
                                }
                                if (!_pkEditModel.GetBrowserTargetingOtherAll)
                                {
                                    _pkEditModel.BrowserTargetingOtherAll = true;
                                    LogTrace.WriteInLog("             Другие. Выбран checkbox Все");
                                }

                                #region Опера
                                    //pkEditModel.BrowserTargetingOperaOther = true;
                                    //LogTrace.WriteInLog("             Опера. Выбран checkbox Другие");
                                    if (!_pkEditModel.GetBrowserTargetingOperaChoseAll)
                                    {
                                        _pkEditModel.BrowserTargetingOperaChoseAll = true;
                                        LogTrace.WriteInLog("             Опера. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingOperaOther)
                                    {
                                        _pkEditModel.BrowserTargetingOperaOther = true;
                                        LogTrace.WriteInLog("             Опера. Выбран checkbox Другие");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingOpera10)
                                    {
                                        _pkEditModel.BrowserTargetingOpera10 = true;
                                        LogTrace.WriteInLog("        Опера. Выбран checkbox 10");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingOpera11)
                                    {
                                        _pkEditModel.BrowserTargetingOpera11 = true;
                                        LogTrace.WriteInLog("        Опера. Выбран checkbox 11");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingOperaMini)
                                    {
                                        _pkEditModel.BrowserTargetingOperaMini = true;
                                        LogTrace.WriteInLog("        Опера. Выбран checkbox Mini");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingOperaMobile)
                                    {
                                        _pkEditModel.BrowserTargetingOperaMobile = true;
                                        LogTrace.WriteInLog("        Опера. Выбран checkbox Mobile");
                                    }
                                #endregion

                                #region Chrome
                                    if (!_pkEditModel.GetBrowserTargetingChromeChoseAll)
                                    {
                                        _pkEditModel.BrowserTargetingChromeChoseAll = true;
                                        LogTrace.WriteInLog("             Chrome. Выбран checkbox Chrome Все");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingChromeAll)
                                    {
                                        _pkEditModel.BrowserTargetingChromeAll = true;
                                        LogTrace.WriteInLog("        Chrome. Выбран checkbox Все");
                                    }
                                #endregion

                                #region Firefox
                                    if (!_pkEditModel.GetBrowserTargetingFirefoxChoseAll)
                                    {
                                        _pkEditModel.BrowserTargetingFirefoxChoseAll = true;
                                        LogTrace.WriteInLog("             Firefox. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingFirefox3)
                                    {
                                        _pkEditModel.BrowserTargetingFirefox3 = true;
                                        LogTrace.WriteInLog("        Firefox. Выбран checkbox 3");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingFirefox4)
                                    {
                                        _pkEditModel.BrowserTargetingFirefox4 = true;
                                        LogTrace.WriteInLog("        Firefox. Выбран checkbox 4");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingFirefox5)
                                    {
                                        _pkEditModel.BrowserTargetingFirefox5 = true;
                                        LogTrace.WriteInLog("        Firefox. Выбран checkbox 5");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingFirefox6)
                                    {
                                        _pkEditModel.BrowserTargetingFirefox6 = true;
                                        LogTrace.WriteInLog("        Firefox. Выбран checkbox 6");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingFirefoxOther)
                                    {
                                        _pkEditModel.BrowserTargetingFirefoxOther = true;
                                        LogTrace.WriteInLog("        Firefox. Выбран checkbox Другие");
                                    }
                                #endregion

                                #region Safari
                                    if (!_pkEditModel.GetBrowserTargetingSafariChoseAll)
                                    {
                                        _pkEditModel.BrowserTargetingSafariChoseAll = true;
                                        LogTrace.WriteInLog("             Safari. Выбран checkbox Safari Все");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingSafariAll)
                                    {
                                        _pkEditModel.BrowserTargetingSafariAll = true;
                                        LogTrace.WriteInLog("        Safari. Выбран checkbox Все");
                                    }
                                #endregion

                                #region MSIE
                                    if (!_pkEditModel.GetBrowserTargetingIeChoseAll)
                                    {
                                        _pkEditModel.BrowserTargetingIeChoseAll = true;
                                        LogTrace.WriteInLog("             MSIE. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingIe6)
                                    {
                                        _pkEditModel.BrowserTargetingIe6 = true;
                                        LogTrace.WriteInLog("        MSIE. Выбран checkbox 6");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingIe7)
                                    {
                                        _pkEditModel.BrowserTargetingIe7 = true;
                                        LogTrace.WriteInLog("        MSIE. Выбран checkbox 7");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingIe8)
                                    {
                                        _pkEditModel.BrowserTargetingIe8 = true;
                                        LogTrace.WriteInLog("        MSIE. Выбран checkbox 8");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingIe9)
                                    {
                                        _pkEditModel.BrowserTargetingIe9 = true;
                                        LogTrace.WriteInLog("        MSIE. Выбран checkbox 9");
                                    }
                                    if (!_pkEditModel.GetBrowserTargetingIeOther)
                                    {
                                        _pkEditModel.BrowserTargetingIeOther = true;
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
                    _variant = _chosenVariantOsTargeting = needSetRadioButton(2);
                    _pkEditModel.OsTargeting = _variant;
                    switch (_variant)
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
                                if (!_pkEditModel.GetOsTargetingOther)
                                {
                                    _pkEditModel.OsTargetingOther = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Другие");
                                }
                                if (!_pkEditModel.GetOsTargetingMacOs)
                                {
                                    _pkEditModel.OsTargetingMacOs = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Mac OS");
                                }
                                if (!_pkEditModel.GetOsTargetingOtherMobileOs)
                                {
                                    _pkEditModel.OsTargetingOtherMobileOs = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Прочие мобильные ОС");
                                }
                                if (!_pkEditModel.GetOsTargetingWindows)
                                {
                                    _pkEditModel.OsTargetingWindows = true;
                                    LogTrace.WriteInLog("             Выбран checkbox WIndows");
                                }
                                if (!_pkEditModel.GetOsTargetingOtherIoS)
                                {
                                    _pkEditModel.OsTargetingOtherIoS = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Прочие iOS системы");
                                }
                                if (!_pkEditModel.GetOsTargetingIpad)
                                {
                                    _pkEditModel.OsTargetingIpad = true;
                                    LogTrace.WriteInLog("             Выбран checkbox iPAD");
                                }
                                if (!_pkEditModel.GetOsTargetingIphone)
                                {
                                    _pkEditModel.OsTargetingIphone = true;
                                    LogTrace.WriteInLog("             Выбран checkbox IPHONE");
                                }
                                if (!_pkEditModel.GetOsTargetingAndroid)
                                {
                                    _pkEditModel.OsTargetingAndroid = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Android");
                                }
                                break;
                            }
                    }
                #endregion

                #region Провайдеры
                    _variant = _chosenVariantProviderTargeting = needSetRadioButton(2);
                    _pkEditModel.ProviderTargeting = _variant;
                    switch (_variant)
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
                                if (!_pkEditModel.GetProviderTargetingOther)
                                {
                                    _pkEditModel.ProviderTargetingOther = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Другие");
                                }
                                if (!_pkEditModel.GetProviderTargetingMegafon)
                                {
                                    _pkEditModel.ProviderTargetingMegafon = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Мегафон");
                                }
                                if (!_pkEditModel.GetProviderTargetingMtc)
                                {
                                    _pkEditModel.ProviderTargetingMtc = true;
                                    LogTrace.WriteInLog("             Выбран checkbox МТС Россия");
                                }
                                break;
                            }
                    }
                #endregion

                #region Геотаргетинг
                    _variant = _chosenVariantGeoTargeting = needSetRadioButton(2);
                    _pkEditModel.GeoTargeting = _variant;
                    switch (_variant)
                    {
                        case 0:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Геотаргетинг. Выбрано: не использовать");
                                break;
                            }
                        case 1:
                            {
                                LogTrace.WriteInLog("          Выбираю radiobutton Геотаргетинг. Выбрано: использовать");
                                _pkEditModel.GeoTargetingRussiaExpand = true;
                                _pkEditModel.GeoTargetingUkraineExpand = true;

                                //pkEditModel.GeoTargetingOther = true;
                                if (!_pkEditModel.GetGeoTargetingOther)
                                {
                                    _pkEditModel.GeoTargetingOther = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Прочие страны");
                                }
                                if (!_pkEditModel.GetGeoTargetingAustria)
                                {
                                    _pkEditModel.GeoTargetingAustria = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Австрия");
                                }
                                if (!_pkEditModel.GetGeoTargetingBelorussia)
                                {
                                    _pkEditModel.GeoTargetingBelorussia = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Белоруссия");
                                }
                                if (!_pkEditModel.GetGeoTargetingUk)
                                {
                                    _pkEditModel.GeoTargetingUk = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Великобритания");
                                }
                                if (!_pkEditModel.GetGeoTargetingGermany)
                                {
                                    _pkEditModel.GeoTargetingGermany = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Германия");
                                }
                                if (!_pkEditModel.GetGeoTargetingIsrael)
                                {
                                    _pkEditModel.GeoTargetingIsrael = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Израиль");
                                }
                                if (!_pkEditModel.GetGeoTargetingKazakhstan)
                                {
                                    _pkEditModel.GeoTargetingKazakhstan = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Казахстан");
                                }
                                if (!_pkEditModel.GetGeoTargetingLatvia)
                                {
                                    _pkEditModel.GeoTargetingLatvia = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Латвия");
                                }
                                if (!_pkEditModel.GetGeoTargetingLithuania)
                                {
                                    _pkEditModel.GeoTargetingLithuania = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Литва");
                                }

                                #region Россия
                                    if (!_pkEditModel.GetGeoTargetingRussiaChoseAll)
                                    {
                                        _pkEditModel.GeoTargetingRussiaChoseAll = true;
                                        LogTrace.WriteInLog("             Россия. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingRussiaEburg)
                                    {
                                        _pkEditModel.GeoTargetingRussiaEburg = true;
                                        LogTrace.WriteInLog("        Россия. Выбран checkbox Екатеринбург");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingRussiaMoscow)
                                    {
                                        _pkEditModel.GeoTargetingRussiaMoscow = true;
                                        LogTrace.WriteInLog("        Россия. Выбран checkbox Москва");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingRussiaNovosibirsk)
                                    {
                                        _pkEditModel.GeoTargetingRussiaNovosibirsk = true;
                                        LogTrace.WriteInLog("        Россия. Выбран checkbox Новосибирск");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingRussiaOther)
                                    {
                                        _pkEditModel.GeoTargetingRussiaOther = true;
                                        LogTrace.WriteInLog("        Россия. Выбран checkbox Прочие регионы");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingRussiaSpb)
                                    {
                                        _pkEditModel.GeoTargetingRussiaSpb = true;
                                        LogTrace.WriteInLog("        Россия. Выбран checkbox Санкт-Петербург");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingRussiaHabarovsk)
                                    {
                                        _pkEditModel.GeoTargetingRussiaHabarovsk = true;
                                        LogTrace.WriteInLog("        Россия. Выбран checkbox Хабаровск");
                                    }
                                #endregion

                                if (!_pkEditModel.GetGeoTargetingUsa)
                                {
                                    _pkEditModel.GeoTargetingUsa = true;
                                    LogTrace.WriteInLog("             Выбран checkbox США");
                                }

                                #region Украина
                                    if (!_pkEditModel.GetGeoTargetingUkraineChoseAll)
                                    {
                                        _pkEditModel.GeoTargetingUkraineChoseAll = true;
                                        LogTrace.WriteInLog("             Украина. Выбран checkbox Все");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingUkraineDnepr)
                                    {
                                        _pkEditModel.GeoTargetingUkraineDnepr = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Днепропетровск");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingUkraineDonetzk)
                                    {
                                        _pkEditModel.GeoTargetingUkraineDonetzk = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Донецк");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingUkraineZakarpattya)
                                    {
                                        _pkEditModel.GeoTargetingUkraineZakarpattya = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Закарпатье");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingUkraineKiev)
                                    {
                                        _pkEditModel.GeoTargetingUkraineKiev = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Киев");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingUkraineCrimea)
                                    {
                                        _pkEditModel.GeoTargetingUkraineCrimea = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Крым");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingUkraineLvov)
                                    {
                                        _pkEditModel.GeoTargetingUkraineLvov = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Львов");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingUkraineNikolaev)
                                    {
                                        _pkEditModel.GeoTargetingUkraineNikolaev = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Николаев");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingUkraineOdessa)
                                    {
                                        _pkEditModel.GeoTargetingUkraineOdessa = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Одесса");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingUkraineOther)
                                    {
                                        _pkEditModel.GeoTargetingUkraineOther = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Прочие области");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingUkraineHarkov)
                                    {
                                        _pkEditModel.GeoTargetingUkraineHarkov = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Харьков");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingUkraineHerson)
                                    {
                                        _pkEditModel.GeoTargetingUkraineHerson = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Херсон");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingUkraineCherkassy)
                                    {
                                        _pkEditModel.GeoTargetingUkraineCherkassy = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Черкассы");
                                    }
                                    if (!_pkEditModel.GetGeoTargetingUkraineChernovzi)
                                    {
                                        _pkEditModel.GeoTargetingUkraineChernovzi = true;
                                        LogTrace.WriteInLog("        Украина. Выбран checkbox Черновцы");
                                    }
                                #endregion

                                if (!_pkEditModel.GetGeoTargetingEstonia)
                                {
                                    _pkEditModel.GeoTargetingEstonia = true;
                                    LogTrace.WriteInLog("             Выбран checkbox Эстония");
                                }
                                break;
                            }
                    }
                #endregion
            #endregion
        }

        private void EditingIsSuccessful()
        {
            string editPktUrl = _driver.Url; //запоминаем url страницы

            _pkEditModel.Submit(); //пытаемся сохранить форму
            LogTrace.WriteInLog(Goods_View.tab2 + "Нажал кнопку Сохранить");
            LogTrace.WriteInLog(Goods_View.tab2 + _driver.Url);
            LogTrace.WriteInLog("");
            
            //если editPktUrl и текущий url совпали - мы никуда не перешли и значит есть ошибки заполнения полей
            if (_driver.Url == editPktUrl)
            {
                Errors = _pkEditModel.GetErrors(); //проверяем, появились ли на форме ошибки заполнения полей
            }
            else
            {
                LogTrace.WriteInLog(Goods_View.tab2 + "РК успешно отредактирована");
                LogForClickers.WriteInLog(Goods_View.tab2 + "РК успешно отредактирована");
            }
            Registry.hashTable["driver"] = _driver;
        }

        public void CheckEditingPk()
        {
            GetDriver();
            if(!CheckFields())
            {
                LogTrace.WriteInLog(Goods_View.tab2 + "ОК, всё ранее введенное совпадает с текущими значениями");
                LogForClickers.WriteInLog(Goods_View.tab2 + "ОК, всё ранее введенное совпадает с текущими значениями");
            }
        }

        private bool CheckFields()
        {
            LogTrace.WriteInLog("          " + _driver.Url);

            #region Проверка заполнения
            #region Разное
            if (_pkEditModel.GetViewSensors) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Просмотр датчиков' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Просмотр датчиков' и выбранное при редактировании");
                WasMismatch = true;
            }

            if (_pkEditModel.GetViewConversion) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Просмотр конверсии' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Просмотр конверсии' и выбранное при редактировании");
                WasMismatch = true;
            }

            if (_namePk == _pkEditModel.GetName) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Название ({0}) и введенное при редактировании ({1})", _pkEditModel.GetName, _namePk)); }
            else
            {
                LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Название ({0}) и введенное при редактировании ({1})", _pkEditModel.GetName, _namePk));
                WasMismatch = true;
            }

            if (_dateStartPk == _pkEditModel.GetStartPkDate) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Дата старта РК ({0}) и введенное при редактировании ({1})", _pkEditModel.GetStartPkDate, _dateStartPk)); }
            else
            {
                LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Дата старта РК ({0}) и введенное при редактировании ({1})", _pkEditModel.GetStartPkDate, _dateStartPk));
                WasMismatch = true;
            }

            if (_dateEndPk == _pkEditModel.GetEndPkDate) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Дата окончания РК ({0}) и введенное при редактировании ({1})", _pkEditModel.GetEndPkDate, _dateEndPk)); }
            else
            {
                LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Дата окончания РК ({0}) и введенное при редактировании ({1})", _pkEditModel.GetEndPkDate, _dateEndPk));
                WasMismatch = true;
            }

            //if (pkEditModel.GetBlockTeasersAfterCreation) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Блокировать тизеры после их создания' и выбранное при редактировании"); }
            //else
            //{
            //    LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Блокировать тизеры после их создания' и выбранное при редактировании");
            //    wasMismatch = true;
            //}

            if (_pkEditModel.GetStoppedByManager) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Остановлена менеджером' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Остановлена менеджером' и выбранное при редактировании");
                WasMismatch = true;
            }
            #endregion

            #region Ограничения рекламной кампании
            if (_chosenVariantLimitsPk != 0)
            {
                if (_chosenVariantLimitsPk == 1)
                {
                    string _pkEditModelGetDayLimitByBudget = _pkEditModel.GetDayLimitByBudget;
                    Regex regex = new Regex(@"\.00");
                    Match match = regex.Match(_pkEditModelGetDayLimitByBudget);
                    if (match.Success) _pkEditModelGetDayLimitByBudget = regex.Replace(_pkEditModelGetDayLimitByBudget, "");

                    if (_dayLimitByBudget == _pkEditModelGetDayLimitByBudget)
                    {
                        LogTrace.WriteInLog(
                            string.Format("          Совпадают: содержимое поля 'Суточный лимит РК' ({0}) и введенное при редактировании ({1})", _pkEditModelGetDayLimitByBudget, _dayLimitByBudget));
                    }
                    else
                    {
                        LogTrace.WriteInLog(
                            string.Format("НЕ СОВПАДАЮТ: содержимое поля 'Суточный лимит РК' ({0}) и введенное при редактировании ({1})", _pkEditModelGetDayLimitByBudget, _dayLimitByBudget));
                        WasMismatch = true;
                    }

                    string _pkEditModelGetGeneralLimitByBudget = _pkEditModel.GetGeneralLimitByBudget;
                    regex = new Regex(@"\.00");
                    match = regex.Match(_pkEditModelGetGeneralLimitByBudget);
                    if (match.Success) _pkEditModelGetGeneralLimitByBudget = regex.Replace(_pkEditModelGetGeneralLimitByBudget, "");

                    if (_generalLimitByBudget == _pkEditModelGetGeneralLimitByBudget)
                    {
                        LogTrace.WriteInLog(
                            string.Format("          Совпадают: содержимое поля 'Общий лимит РК' ({0}) и введенное при редактировании ({1})", _pkEditModelGetGeneralLimitByBudget, _generalLimitByBudget));
                    }
                    else
                    {
                        LogTrace.WriteInLog(
                            string.Format("НЕ СОВПАДАЮТ: содержимое поля 'Общий лимит РК' ({0}) и введенное при редактировании ({1})", _pkEditModelGetGeneralLimitByBudget, _generalLimitByBudget));
                        WasMismatch = true;
                    }
                }

                if (_chosenVariantLimitsPk == 2)
                {
                    string _pkEditModelGetDayLimitByClicks = _pkEditModel.GetDayLimitByClicks;
                    Regex regex = new Regex(@"\.00");
                    Match match = regex.Match(_pkEditModelGetDayLimitByClicks);
                    if (match.Success) _pkEditModelGetDayLimitByClicks = regex.Replace(_pkEditModelGetDayLimitByClicks, "");

                    if (_dayLimitByClicks == _pkEditModelGetDayLimitByClicks)
                    {
                        LogTrace.WriteInLog(
                            string.Format("          Совпадают: содержимое поля 'Суточный лимит кликов' ({0}) и введенное при редактировании ({1})", _pkEditModelGetDayLimitByClicks, _dayLimitByClicks));
                    }
                    else
                    {
                        LogTrace.WriteInLog(
                            string.Format("НЕ СОВПАДАЮТ: содержимое поля 'Суточный лимит кликов' ({0}) и введенное при редактировании ({1})", _pkEditModelGetDayLimitByClicks, _dayLimitByClicks));
                        WasMismatch = true;
                    }

                    string _pkEditModelGetGeneralLimitByClicks = _pkEditModel.GetGeneralLimitByClicks;
                    regex = new Regex(@"\.00");
                    match = regex.Match(_pkEditModelGetGeneralLimitByClicks);
                    if (match.Success) _pkEditModelGetGeneralLimitByClicks = regex.Replace(_pkEditModelGetGeneralLimitByClicks, "");

                    if (_generalLimitByClicks == _pkEditModelGetGeneralLimitByClicks)
                    {
                        LogTrace.WriteInLog(
                            string.Format("          Совпадают: содержимое поля 'Лимит на кампанию' ({0}) и введенное при редактировании ({1})", _pkEditModelGetGeneralLimitByClicks, _generalLimitByClicks));
                    }
                    else
                    {
                        LogTrace.WriteInLog(
                            string.Format("НЕ СОВПАДАЮТ: содержимое поля 'Лимит на кампанию' ({0}) и введенное при редактировании ({1})", _pkEditModelGetGeneralLimitByClicks, _generalLimitByClicks));
                        WasMismatch = true;
                    }
                }
            }

            #endregion

            #region UTM-разметка рекламной кампании для Google Analytics
            if (_pkEditModel.GetUtmPkForGoogleAnalytics) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'UTM-разметка рекламной кампании для Google Analytics' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'UTM-разметка рекламной кампании для Google Analytics' и выбранное при редактировании");
                WasMismatch = true;
            }

            if (_utmMedium == _pkEditModel.GetUtmMedium) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'utm_medium (средство кампании)' ({0}) и введенное при редактировании ({1})", _pkEditModel.GetUtmMedium, _utmMedium)); }
            else
            {
                LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: Совпадают: содержимое поля 'utm_medium (средство кампании)' ({0}) и введенное при редактировании ({1})", _pkEditModel.GetUtmMedium, _utmMedium));
                WasMismatch = true;
            }

            if (_utmSource == _pkEditModel.GetUtmSource) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'utm_source (источник кампании)' ({0}) и введенное при редактировании ({1})", _pkEditModel.GetUtmSource, _utmSource)); }
            else
            {
                LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: Совпадают: содержимое поля 'utm_source (источник кампании)' ({0}) и введенное при редактировании ({1})", _pkEditModel.GetUtmSource, _utmSource));
                WasMismatch = true;
            }

            if (_utmCampaign == _pkEditModel.GetUtmCampaign) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'utm_campaign (название кампании)' ({0}) и введенное при редактировании ({1})", _pkEditModel.GetUtmCampaign, _utmCampaign)); }
            else
            {
                LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: Совпадают: содержимое поля 'utm_campaign (название кампании)' ({0}) и введенное при редактировании ({1})", _pkEditModel.GetUtmCampaign, _utmCampaign));
                WasMismatch = true;
            }
            #endregion

            #region UTM-разметка пользователя
            if (_pkEditModel.GetUtmUser) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'UTM-разметка пользователя' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'UTM-разметка пользователя' и выбранное при редактировании");
                WasMismatch = true;
            }

            if (_utmUserStr == _pkEditModel.GetUtmUserStr) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'UTM-разметка пользователя' ({0}) и введенное при редактировании ({1})", _pkEditModel.GetUtmUserStr, _utmUserStr)); }
            else
            {
                LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: Совпадают: содержимое поля 'UTM-разметка пользователя' ({0}) и введенное при редактировании ({1})", _pkEditModel.GetUtmUserStr, _utmUserStr));
                WasMismatch = true;
            }
            #endregion

            #region Крутить в сети Товарро
            if (_pkEditModel.GetScrewInTovarro) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Крутить в сети Товарро' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Крутить в сети Товарро' и выбранное при редактировании");
                WasMismatch = true;
            }
            #endregion

            #region Блокировка по расписанию
            if (_pkEditModel.GetBlockBySchedule) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Блокировка по расписанию' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Блокировка по расписанию' и выбранное при редактировании");
                WasMismatch = true;
            }

            if (_pkEditModel.GetWeekends) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'выходные' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'выходные' и выбранное при редактировании");
                WasMismatch = true;
            }

            if (_pkEditModel.GetWeekdays) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'будни' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'будни' и выбранное при редактировании");
                WasMismatch = true;
            }

            if (_pkEditModel.GetWorkingTime) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'рабочее время (9-18 по будням)' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'рабочее время (9-18 по будням)' и выбранное при редактировании");
                WasMismatch = true;
            }
            #endregion

            #region Передавать id площадки в ссылке
            if (_pkEditModel.GetIdOfPlatformInLink) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Передавать id площадки в ссылке' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Передавать id площадки в ссылке' и выбранное при редактировании");
                WasMismatch = true;
            }

            if (_idOfPlatformInLink == _pkEditModel.GetIdOfPlatformInLinkStr) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'Передавать id площадки в ссылке' ({0}) и введенное при редактировании ({1})", _pkEditModel.GetIdOfPlatformInLinkStr, _idOfPlatformInLink)); }
            else
            {
                LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: Совпадают: содержимое поля 'Передавать id площадки в ссылке' ({0}) и введенное при редактировании ({1})", _pkEditModel.GetIdOfPlatformInLinkStr, _idOfPlatformInLink));
                WasMismatch = true;
            }
            #endregion

            #region Добавлять id тизера в конец ссылки
            if (_pkEditModel.GetAddIdOfTeaserInLink) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Добавлять id тизера в конец ссылки' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Добавлять id тизера в конец ссылки' и выбранное при редактировании");
                WasMismatch = true;
            }
            #endregion

            #region Комментарий к кампании
            if (_commentsForPk == _pkEditModel.GetCommentsForPk) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'Комментарий к кампании' ({0}) и введенное при редактировании ({1})", _pkEditModel.GetCommentsForPk, _commentsForPk)); }
            else
            {
                LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля 'Комментарий к кампании' ({0}) и введенное при редактировании ({1})", _pkEditModel.GetCommentsForPk, _commentsForPk));
                WasMismatch = true;
            }
            #endregion

            #region Площадки
            if (_pkEditModel.GetPlatformsNotSpecified) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Не определено' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Не определено' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsPolitics) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Политика, общество, происшествия, религия' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Политика, общество, происшествия, религия' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsEconomics) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Экономика, финансы, недвижимость, работа и карьера' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Экономика, финансы, недвижимость, работа и карьера' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsCelebrities) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Знаменитости, шоу-бизнес, кино, музыка' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Знаменитости, шоу-бизнес, кино, музыка' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsScience) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Наука и технологии' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Наука и технологии' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsConnection) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Связь, компьютеры, программы' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Связь, компьютеры, программы' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsSports) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Спорт' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Спорт' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsAuto) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Авто-вело-мото' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Авто-вело-мото' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsFashion) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Мода и стиль, здоровье и красота, фитнес и диета, кулинария' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Мода и стиль, здоровье и красота, фитнес и диета, кулинария' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsMedicine) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Медицина' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Медицина' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsTourism) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Туризм и отдых (путевки, отели, рестораны)' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Туризм и отдых (путевки, отели, рестораны)' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsGlobalPortals) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Глобальные порталы с множеством подпроектов' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Глобальные порталы с множеством подпроектов' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsHumor) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Юмор (приколы, картинки, обои), каталог фотографий, блоги' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Юмор (приколы, картинки, обои), каталог фотографий, блоги' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsFileshares) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Файлообменники, файлокачалки (кино, музыка, игры, программы)' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Файлообменники, файлокачалки (кино, музыка, игры, программы)' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsSocialNetworks) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Социальные сети, сайты знакомства, личные дневники' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Социальные сети, сайты знакомства, личные дневники' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsAnimals) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Животный и растительный мир' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Животный и растительный мир' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsReligion) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Религия' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Религия' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsChildren) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Дети и родители' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Дети и родители' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsBuilding) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Строительство, ремонт, дача, огород' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Строительство, ремонт, дача, огород' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsCookery) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Кулинария' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Кулинария' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsJob) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Работа и карьера. Поиск работы, поиск персонала' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Работа и карьера. Поиск работы, поиск персонала' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsNotSites) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Не сайты (программы, тулбары, таскбары)' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Не сайты (программы, тулбары, таскбары)' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsSitesStartPagesBrowsers) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Сайты, размещенные на стартовых страницах браузеров' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Сайты, размещенные на стартовых страницах браузеров' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsSearchSystems) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Поисковые системы' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Поисковые системы' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsEmail) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Почта' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Почта' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsPhotoCatalogues) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Каталоги фотографий' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Каталоги фотографий' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsVarez) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Варезники' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Варезники' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsOnlineVideo) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Онлайн видео, телевидение, радио' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Онлайн видео, телевидение, радио' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsOnlineLibraries) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Онлайн-библиотеки' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Онлайн-библиотеки' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsInternet) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Интернет, поисковые сайты, электронная почта' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Интернет, поисковые сайты, электронная почта' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsOnlineGames) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Онлайн игры' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Онлайн игры' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsInternetRepresentatives) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Интернет-представительства бизнеса' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Интернет-представительства бизнеса' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsProgramms) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Программы, прошивки, игры для КПК и мобильных устройств' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Программы, прошивки, игры для КПК и мобильных устройств' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsCataloguesInternetResources) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Каталоги Интернет - ресурсов, фирм и предприятий' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Каталоги Интернет - ресурсов, фирм и предприятий' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsForWagesInInternet) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Для заработка в Интернете. Партнерские программы' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Для заработка в Интернете. Партнерские программы' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsHobbies) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Хобби и увлечения' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Хобби и увлечения' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsMarketgid) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Маркетгид' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Маркетгид' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsShock) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Шокодром' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Шокодром' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsEsoteric) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Эзотерика. Непознанное, астрология, гороскопы, гадания' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Эзотерика. Непознанное, астрология, гороскопы, гадания' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsPsychology) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Психология, мужчина и женщина' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Психология, мужчина и женщина' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsHistory) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'История, образование, культура' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'История, образование, культура' и выбранное при редактировании");
                WasMismatch = true;
            }
            if (_pkEditModel.GetPlatformsMarketgidWomenNet) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Маркетгид ЖС' и выбранное при редактировании"); }
            else
            {
                LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Маркетгид ЖС' и выбранное при редактировании");
                WasMismatch = true;
            }
            #endregion

            #region Демографический таргетинг
            if (_chosenVariantDemoTargeting != 0)
            {
                if (_pkEditModel.GetDemoTargetingMenChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Мужчины' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Мужчины' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetDemoTargetingWomenChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Женщины' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Женщины' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetDemoTargetingHermaphroditeChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Пол не определен' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Пол не определен' и выбранное при редактировании");
                    WasMismatch = true;
                }
            }
            #endregion

            #region Таргетинг по интересам
            if (_chosenVariantInterestsTargeting != 0)
            {
                if (_pkEditModel.GetInterestsTargetingOther)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Прочее' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Прочее' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetInterestsTargetingBusinessChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Бизнес' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Бизнес' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetInterestsTargetingRealtyChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Недвижимость' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Недвижимость' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetInterestsTargetingExhibitions)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Выставки, концерты, театры, кино' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Выставки, концерты, театры, кино' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetInterestsTargetingMedicineChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Медицина, здоровье' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Медицина, здоровье' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetInterestsTargetingHouseChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Дом и семья' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Дом и семья' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetInterestsTargetingFinanceChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Финансы' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Финансы' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetInterestsTargetingComputersChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Компьютеры, оргтехника' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Компьютеры, оргтехника' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetInterestsTargetingAutoChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Авто' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Авто' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetInterestsTargetingAudioChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Аудио, Видео, Фото' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Аудио, Видео, Фото' и выбранное при редактировании");
                    WasMismatch = true;
                }
            }

            #endregion

            #region Таргетинг по браузерам
            if (_chosenVariantBrowserTargeting != 0)
            {
                if (_pkEditModel.GetBrowserTargetingIeOther)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Другие' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Другие' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetBrowserTargetingOperaChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Opera' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Opera' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetBrowserTargetingChromeChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Google Chrome' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Google Chrome' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetBrowserTargetingFirefoxChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Firefox' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Firefox' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetBrowserTargetingSafariChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Safari' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Safari' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetBrowserTargetingIeChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'MSIE' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'MSIE' и выбранное при редактировании");
                    WasMismatch = true;
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
            if (_chosenVariantOsTargeting != 0)
            {
                if (_pkEditModel.GetOsTargetingOther)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Другие' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Другие' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetOsTargetingMacOs)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Mac OS' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Mac OS' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetOsTargetingOtherMobileOs)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Прочие мобильные ОС' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Прочие мобильные ОС' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetOsTargetingWindows)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'WIndows' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'WIndows' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetOsTargetingOtherIoS)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Прочие iOS системы' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Прочие iOS системы' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetOsTargetingIpad)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'iPAD' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'iPAD' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetOsTargetingIphone)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'IPHONE' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'IPHONE' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetOsTargetingAndroid)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Android' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Android' и выбранное при редактировании");
                    WasMismatch = true;
                }
            }
            #endregion

            #region Таргетинг по провайдерам
            if (_chosenVariantProviderTargeting != 0)
            {
                if (_pkEditModel.GetProviderTargetingOther)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Другие' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Другие' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetProviderTargetingMegafon)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Мегафон' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Мегафон' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetProviderTargetingMtc)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'МТС Россия' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'МТС Россия' и выбранное при редактировании");
                    WasMismatch = true;
                }
            }
            #endregion

            #region Геотаргетинг
            if (_chosenVariantGeoTargeting != 0)
            {
                if (_pkEditModel.GetGeoTargetingOther)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Прочие страны' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Прочие страны' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetGeoTargetingAustria)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Австрия' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Австрия' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetGeoTargetingBelorussia)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Белоруссия' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Белоруссия' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetGeoTargetingUk)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Великобритания' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Великобритания' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetGeoTargetingGermany)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Германия' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Германия' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetGeoTargetingIsrael)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Израиль' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Израиль' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetGeoTargetingKazakhstan)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Казахстан' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Казахстан' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetGeoTargetingLatvia)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Латвия' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Латвия' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetGeoTargetingLithuania)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Литва' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Литва' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetGeoTargetingRussiaChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Россия' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Россия' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetGeoTargetingUsa)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'США' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'США' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetGeoTargetingUkraineChoseAll)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Украина' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Украина' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_pkEditModel.GetGeoTargetingEstonia)
                {
                    LogTrace.WriteInLog(
                        "          Совпадают: состояние checkbox 'Эстония' и выбранное при редактировании");
                }
                else
                {
                    LogTrace.WriteInLog(
                        "НЕ СОВПАДАЮТ: состояние checkbox 'Эстония' и выбранное при редактировании");
                    WasMismatch = true;
                }
            }
            #endregion
            #endregion

            LogTrace.WriteInLog(Goods_View.tab2 + _driver.Url);
            LogTrace.WriteInLog("");
            return WasMismatch;
        }

        private int needSetRadioButton(int variants) //генерируем номер варианта выбора для needSetRadioButton. variants - кол-во вариантов выбора
        {
            Random rnd = new Random();
            return rnd.Next(0, variants);
        }
    }
}
