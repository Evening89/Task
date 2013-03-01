using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using Task.Model;
using Task.Utils;
using OpenQA.Selenium;

namespace Task.Controller
{
    public class GoodsCreatePkController
    {
        private IWebDriver _driver;
        private GoodsCreatePkModel _pkModel;//берем сохраненный ранее 
                                            //(при создании клиента Task.Controller.GoodsCreateClient_Controller) ID клиента
                                            //и дописываем в URL
        private readonly string _baseUrl = "https://" + Registry.hashTable["Login"] + ":" + Registry.hashTable["Password"] + "@" + "admin.dt00.net/cab/goodhits/clients-new-campaign/client_id/" + Registry.hashTable["clientId"];
        private readonly Randoms _randoms = new Randoms(); //класс генерации случайных строк

        public List<string> Errors = new List<string>(); //список ошибок (в каждой строке - спарсенное со страницы описание ошибки)
        public string PkId; //переменная для хранения ID только что созданной РК
        public string ClientId;
        public string PkName; //переменная для хранения названия только что созданной РК

        public void CreatePk(bool setNecessaryFields, bool setUnnecessaryFields)
        {
            GetDriver();
            SetUpFields(setNecessaryFields, setUnnecessaryFields);
            CreationIsSuccessful();
        }

        private void GetDriver()
        {
            _driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный ранее драйвер
            _driver.Navigate().GoToUrl(_baseUrl); //заходим по ссылке
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            //ставим ожидание в 10 сек на случай, если страница медленно грузится и нужные эл-ты появятся не сразу
        }

        private void SetUpFields(bool setNecessaryFields, bool setUnnecessaryFields)
        {
            _pkModel = new GoodsCreatePkModel();
            _pkModel.driver = _driver;

            LogTrace.WriteInLog("     " + _driver.Url);

            #region Required fields
                if (setNecessaryFields) //выбрано заполнение обязательных полей
                {
                    LogTrace.WriteInLog("     ...Заполняю обязательные поля...");
                    //Название РК
                    PkName = _randoms.RandomString(15) + " " + _randoms.RandomNumber(5);
                    _pkModel.Name = PkName;
                    LogTrace.WriteInLog("     Заполняю поле Название РК. Было введено: " + _pkModel.Name);

                    //Геотаргетинг выбран по умолчанию при открытии страницы - делаем выбор "не использовать"
                    _pkModel.GeoTargeting = "0";
                    LogTrace.WriteInLog("     Выбираю radiobutton Геотаргетинг. Выбрано: не использовать");
                }
            #endregion

            #region Unrequired fields
                if (setUnnecessaryFields) //выбрано заполнение также необязательных полей
                {
                    LogTrace.WriteInLog("     ...Заполняю необязательные поля...");
                    if (needSetCheckBox())
                    {
                        _pkModel.StartPkDate = _pkModel.GenerateDate();
                        LogTrace.WriteInLog("     Заполняю поле Дата старта РК. Было введено: " + _pkModel.StartPkDate);
                    }

                    if (needSetCheckBox())
                    {
                        _pkModel.EndPkDate = _pkModel.GenerateDate();
                        LogTrace.WriteInLog("     Заполняю поле Дата окончания РК. Было введено: " + _pkModel.EndPkDate);
                        List<string> instantErrorsDate = _pkModel.ErrorsInFillFields();
                        if (instantErrorsDate.Count != 0) //если список с ошибками заполнения полей даты непуст
                            Errors = instantErrorsDate; //копируем в нас общий список ошибок errors
                    }
                    //if (needSetCheckBox())
                    //{
                    //    pkModel.BlockTeasersAfterCreation = true;
                    //    LogTrace.WriteInLog("     Выбран checkbox Блокировать тизеры после их создания");
                    //}
                    if (needSetCheckBox())
                    {
                        _pkModel.StoppedByManager = true;
                        LogTrace.WriteInLog("     Выбран checkbox Остановлена менеджером");
                    }

                    #region Radiobutton Ограничения рекламной кампании
                        string variant = needSetRadioButton(3).ToString();
                        _pkModel.LimitsOfPk = variant;
                        switch (variant)
                        {
                            case "0":
                                {
                                    LogTrace.WriteInLog("     Выбираю radiobutton Ограничения рекламной кампании. Выбрано: не использовать");
                                    break;
                                }
                            case "1":
                                {
                                    //int num2;
                                    //do
                                    //{
                                    //    num2 = int.Parse(_randoms.RandomNumber(2));
                                    //} while (num2 < 5); //суточный лимит должен быть не менее 5
                                    Random rndd = new Random();
                                    int dayLimitByBudget = rndd.Next(5, 20);
                                    int generalLimitByBudget = rndd.Next(20, 50);
                                    LogTrace.WriteInLog("     Выбираю radiobutton Ограничения рекламной кампании. Выбрано: по бюджету");
                                    _pkModel.DayLimitByBudget = dayLimitByBudget.ToString();//суточный лимит должен быть не менее 5
                                    LogTrace.WriteInLog("        Заполняю поле Суточный лимит РК. Было введено: " + _pkModel.DayLimitByBudget);
                                    _pkModel.GeneralLimitByBudget = generalLimitByBudget.ToString();
                                    LogTrace.WriteInLog("        Заполняю поле Общий лимит РК. Было введено: " + _pkModel.GeneralLimitByBudget);
                                    break;
                                }
                            case "2":
                                {
                                    Random rnd = new Random();
                                    int dayLimitByClicks = rnd.Next(50, 70);
                                    int generalLimitByClicks = rnd.Next(70, 100);
                                    LogTrace.WriteInLog("     Выбираю radiobutton Ограничения рекламной кампании. Выбрано: по количеству кликов");
                                    _pkModel.DayLimitByClicks = _randoms.RandomNumber(3);
                                    LogTrace.WriteInLog("        Заполняю поле Суточный лимит кликов РК. Было введено: " + _pkModel.DayLimitByClicks);
                                    _pkModel.GeneralLimitByClicks = _randoms.RandomNumber(3);
                                    LogTrace.WriteInLog("        Заполняю поле Общий лимит кликов РК. Было введено: " + _pkModel.GeneralLimitByClicks);
                                    break;
                                }
                        }
                    #endregion

                    #region Checkbox UTM-разметка рекламной кампании для Google Analytics
                        if (needSetCheckBox())
                        {
                            _pkModel.UtmPkForGoogleAnalytics = true;
                            LogTrace.WriteInLog("     Выбран checkbox UTM-разметка рекламной кампании для Google Analytics");
                            _pkModel.UtmMedium = _randoms.RandomString(5);
                            LogTrace.WriteInLog("        Заполняю поле utm_medium (средство кампании). Было введено: " + _pkModel.UtmMedium);
                            _pkModel.UtmSource = _randoms.RandomString(5);
                            LogTrace.WriteInLog("        Заполняю поле utm_source (источник кампании). Было введено: " + _pkModel.UtmSource);
                            _pkModel.UtmCampaign = _randoms.RandomString(5);
                            LogTrace.WriteInLog("        Заполняю поле utm_campaign (название кампании). Было введено: " + _pkModel.UtmCampaign);
                        }
                    #endregion

                    #region Checkbox UTM-разметка пользователя
                        if (needSetCheckBox())
                        {
                            _pkModel.UtmUser = true;
                            LogTrace.WriteInLog("     Выбран checkbox UTM-разметка пользователя");
                            _pkModel.UtmUserStr = _randoms.RandomString(5);
                            LogTrace.WriteInLog("        Заполняю поле UTM-разметка пользователя. Было введено: " + _pkModel.UtmUserStr);
                        }
                    #endregion

                    if (needSetCheckBox())
                    {
                        LogTrace.WriteInLog("     Выбран checkbox Крутить в сети Товарро");
                        _pkModel.ScrewInTovarro = true;
                    }

                    #region Checkbox Блокировка по расписанию
                        if (needSetCheckBox())
                        {
                            _pkModel.BlockBySchedule = true;
                            LogTrace.WriteInLog("     Выбран checkbox Блокировка по расписанию");
                            if (needSetCheckBox()) _pkModel.Weekends = true;
                            LogTrace.WriteInLog("        Выбран checkbox Выходные");
                            if (needSetCheckBox()) _pkModel.Weekdays = true;
                            LogTrace.WriteInLog("        Выбран checkbox Будни");
                            if (needSetCheckBox()) _pkModel.WorkingTime = true;
                            LogTrace.WriteInLog("        Выбран checkbox Рабочее время (9-18 по будням)");
                        }
                    #endregion

                    #region Checkbox Передавать id площадки в ссылке
                        if (needSetCheckBox())
                        {
                            _pkModel.IdOfPlatformInLink = true;
                            LogTrace.WriteInLog("     Выбран checkbox Передавать id площадки в ссылке");
                            _pkModel.IdOfPlatformInLinkStr = _randoms.RandomString(5);
                            LogTrace.WriteInLog("        Заполняю поле Передавать id площадки в ссылке. Было введено: " + _pkModel.IdOfPlatformInLinkStr);
                        }
                    #endregion

                    if (needSetCheckBox())
                    {
                        _pkModel.AddIdOfTeaserInLink = true;
                        LogTrace.WriteInLog("     Выбран checkbox Добавлять id тизера в конец ссылки");
                    }

                    if (needSetCheckBox())
                    {
                        _pkModel.CommentsForPk = _randoms.RandomString(20) + " " + _randoms.RandomString(10);
                        LogTrace.WriteInLog("     Заполняю textarea Комментарий к кампании. Было введено: " + _pkModel.CommentsForPk);
                    }

                    #region Checkbox Площадки
                        if (needSetCheckBox())
                        {
                            _pkModel.Platforms = true;
                            LogTrace.WriteInLog("     Выбран checkbox Площадки");
                            _pkModel.PlatformsNotSpecified = true;
                            LogTrace.WriteInLog("        Выбран checkbox Не определено");
                            //if (needSetCheckBox()) pkModel.PlatformsNotSpecified = true;
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsPolitics = true;
                                LogTrace.WriteInLog("        Выбран checkbox Политика, общество, происшествия, религия");
                            }

                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsEconomics = true;
                                LogTrace.WriteInLog("        Выбран checkbox Экономика, финансы, недвижимость, работа и карьера");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsCelebrities = true;
                                LogTrace.WriteInLog("        Выбран checkbox Знаменитости, шоу-бизнес, кино, музыка");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsScience = true;
                                LogTrace.WriteInLog("        Выбран checkbox Наука и технологии");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsConnection = true;
                                LogTrace.WriteInLog("        Выбран checkbox Связь, компьютеры, программы");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsSports = true;
                                LogTrace.WriteInLog("        Выбран checkbox Спорт");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsAuto = true;
                                LogTrace.WriteInLog("        Выбран checkbox Авто-вело-мото");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsFashion = true;
                                LogTrace.WriteInLog("        Выбран checkbox Мода и стиль, здоровье и красота, фитнес и диета, кулинария");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsMedicine = true;
                                LogTrace.WriteInLog("        Выбран checkbox Медицина");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsTourism = true;
                                LogTrace.WriteInLog("        Выбран checkbox Туризм и отдых (путевки, отели, рестораны)");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsGlobalPortals = true;
                                LogTrace.WriteInLog("        Выбран checkbox Глобальные порталы с множеством подпроектов");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsHumor = true;
                                LogTrace.WriteInLog("        Выбран checkbox Юмор (приколы, картинки, обои), каталог фотографий, блоги");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsFileshares = true;
                                LogTrace.WriteInLog("        Выбран checkbox Файлообменники, файлокачалки (кино, музыка, игры, программы)");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsSocialNetworks = true;
                                LogTrace.WriteInLog("        Выбран checkbox Социальные сети, сайты знакомства, личные дневники");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsAnimals = true;
                                LogTrace.WriteInLog("        Выбран checkbox Животный и растительный мир");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsReligion = true;
                                LogTrace.WriteInLog("        Выбран checkbox Религия");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsChildren = true;
                                LogTrace.WriteInLog("        Выбран checkbox Дети и родители");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsBuilding = true;
                                LogTrace.WriteInLog("        Выбран checkbox Строительство, ремонт, дача, огород");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsCookery = true;
                                LogTrace.WriteInLog("        Выбран checkbox Кулинария");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsJob = true;
                                LogTrace.WriteInLog("        Выбран checkbox Работа и карьера. Поиск работы, поиск персонала");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsNotSites = true;
                                LogTrace.WriteInLog("        Выбран checkbox Не сайты (программы, тулбары, таскбары)");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsSitesStartPagesBrowsers = true;
                                LogTrace.WriteInLog("        Выбран checkbox Сайты, размещенные на стартовых страницах браузеров");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsSearchSystems = true;
                                LogTrace.WriteInLog("        Выбран checkbox Поисковые системы");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsEmail = true;
                                LogTrace.WriteInLog("        Выбран checkbox Почта");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsPhotoCatalogues = true;
                                LogTrace.WriteInLog("        Выбран checkbox Каталоги фотографий");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsVarez = true;
                                LogTrace.WriteInLog("        Выбран checkbox Варезники");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsOnlineVideo = true;
                                LogTrace.WriteInLog("        Выбран checkbox Онлайн видео, телевидение, радио");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsOnlineLibraries = true;
                                LogTrace.WriteInLog("        Выбран checkbox Онлайн-библиотеки");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsInternet = true;
                                LogTrace.WriteInLog("        Выбран checkbox Интернет, поисковые сайты, электронная почта, интернет-магазины, аукционы, каталоги ресурсов, фирм и предприятий");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsOnlineGames = true;
                                LogTrace.WriteInLog("        Выбран checkbox Онлайн игры");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsInternetRepresentatives = true;
                                LogTrace.WriteInLog("        Выбран checkbox Интернет-представительства бизнеса.");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsProgramms = true;
                                LogTrace.WriteInLog("        Выбран checkbox Программы, прошивки, игры для КПК и мобильных устройств");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsCataloguesInternetResources = true;
                                LogTrace.WriteInLog("        Выбран checkbox Каталоги Интернет - ресурсов, фирм и предприятий");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsForWagesInInternet = true;
                                LogTrace.WriteInLog("        Выбран checkbox Для заработка в Интернете. Партнерские программы");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsHobbies = true;
                                LogTrace.WriteInLog("        Выбран checkbox Хобби и увлечения");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsMarketgid = true;
                                LogTrace.WriteInLog("        Выбран checkbox Маркетгид");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsShock = true;
                                LogTrace.WriteInLog("        Выбран checkbox Шокодром");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsEsoteric = true;
                                LogTrace.WriteInLog("        Выбран checkbox Эзотерика. Непознанное, астрология, гороскопы, гадания");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsPsychology = true;
                                LogTrace.WriteInLog("        Выбран checkbox Психология, мужчина и женщина");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsHistory = true;
                                LogTrace.WriteInLog("        Выбран checkbox История, образование, культура");
                            }
                            if (needSetCheckBox())
                            {
                                _pkModel.PlatformsMarketgidWomenNet = true;
                                LogTrace.WriteInLog("        Выбран checkbox Маркетгид ЖС");
                            }
                        }
                    #endregion

                    #region Radiobutton Демографический таргетинг
                        variant = needSetRadioButton(2).ToString();
                        _pkModel.DemoTargeting = variant;
                        switch (variant)
                        {
                            case "0":
                                {
                                    LogTrace.WriteInLog("     Выбираю radiobutton Демографический таргетинг. Выбрано: не использовать");
                                    break;
                                }
                            case "1":
                                {
                                    LogTrace.WriteInLog("     Выбираю radiobutton Демографический таргетинг. Выбрано: использовать");
                                    //развернуть все пункты (Мужчины, Женщины, Пол не определен)
                                    _pkModel.DemoTargetingMenExpand = true;
                                    _pkModel.DemoTargetingWomenExpand = true;
                                    _pkModel.DemoTargetingHermaphroditeExpand = true;

                                    #region Мужчины
                                        _pkModel.DemoTargetingMenNotSpecified = true;
                                        LogTrace.WriteInLog("        Мужчины. Выбран checkbox Не определен");
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingMenChoseAll = true;
                                            LogTrace.WriteInLog("        Мужчины. Выбран checkbox Все");
                                        }
                                        //if (needSetCheckBox()) pkModel.DemoTargetingMenNotSpecified = true;
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingMen618 = true;
                                            LogTrace.WriteInLog("        Мужчины. Выбран checkbox 6-18");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingMen1924 = true;
                                            LogTrace.WriteInLog("        Мужчины. Выбран checkbox 19-24");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingMen2534 = true;
                                            LogTrace.WriteInLog("        Мужчины. Выбран checkbox 25-34");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingMen3544 = true;
                                            LogTrace.WriteInLog("        Мужчины. Выбран checkbox 35-44");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingMen4590 = true;
                                            LogTrace.WriteInLog("        Мужчины. Выбран checkbox 45-90");
                                        }
                                    #endregion

                                    #region Женщины
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingWomenChoseAll = true;
                                            LogTrace.WriteInLog("        Женщины. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingWomenNotSpecified = true;
                                            LogTrace.WriteInLog("        Женщины. Выбран checkbox Не определен");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingWomen618 = true;
                                            LogTrace.WriteInLog("        Женщины. Выбран checkbox 6-18");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingWomen1924 = true;
                                            LogTrace.WriteInLog("        Женщины. Выбран checkbox 19-24");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingWomen2534 = true;
                                            LogTrace.WriteInLog("        Женщины. Выбран checkbox 25-34");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingWomen3544 = true;
                                            LogTrace.WriteInLog("        Женщины. Выбран checkbox 35-44");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingWomen4590 = true;
                                            LogTrace.WriteInLog("        Женщины. Выбран checkbox 45-90");
                                        }
                                    #endregion

                                    #region Пол не определен
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingHermaphroditeChoseAll = true;
                                            LogTrace.WriteInLog("        Пол не определен. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingHermaphrodite618 = true;
                                            LogTrace.WriteInLog("        Пол не определен. Выбран checkbox 6-18");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingHermaphrodite1924 = true;
                                            LogTrace.WriteInLog("        Пол не определен. Выбран checkbox 19-24");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingHermaphrodite2534 = true;
                                            LogTrace.WriteInLog("        Пол не определен. Выбран checkbox 25-34");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingHermaphrodite3544 = true;
                                            LogTrace.WriteInLog("        Пол не определен. Выбран checkbox 35-44");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.DemoTargetingHermaphrodite4590 = true;
                                            LogTrace.WriteInLog("        Пол не определен. Выбран checkbox 45-90");
                                        }
                                    #endregion

                                    break;
                                }
                        }
                    #endregion

                    #region Radiobutton Таргетинг по интересам
                        variant = needSetRadioButton(2).ToString();
                        _pkModel.InterestsTargeting = variant;
                        switch (variant)
                        {
                            case "0":
                                {
                                    LogTrace.WriteInLog("     Выбираю radiobutton Таргетинг по интересам. Выбрано: не использовать");
                                    break;
                                }
                            case "1":
                                {
                                    LogTrace.WriteInLog("     Выбираю radiobutton Таргетинг по интересам. Выбрано: использовать");
                                    _pkModel.InterestsTargetingBusinessExpand = true;
                                    _pkModel.InterestsTargetingRealtyExpand = true;
                                    _pkModel.InterestsTargetingEducationExpand = true;
                                    _pkModel.InterestsTargetingRestExpand = true;
                                    _pkModel.InterestsTargetingTelephonesExpand = true;
                                    _pkModel.InterestsTargetingMedicineExpand = true;
                                    _pkModel.InterestsTargetingHouseExpand = true;
                                    _pkModel.InterestsTargetingFinanceExpand = true;
                                    _pkModel.InterestsTargetingComputersExpand = true;
                                    _pkModel.InterestsTargetingAutoExpand = true;
                                    _pkModel.InterestsTargetingAudioExpand = true;

                                    _pkModel.InterestsTargetingOther = true;
                                    LogTrace.WriteInLog("        Выбран checkbox Прочее");
                                    //if (needSetCheckBox()) pkModel.InterestsTargetingOther = true;

                                    #region Бизнес
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingBusinessChoseAll = true;
                                            LogTrace.WriteInLog("        Бизнес. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingBusinessAcoountancy = true;
                                            LogTrace.WriteInLog("        Бизнес. Выбран checkbox Бухгалтерия");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingBusinessPlacement = true;
                                            LogTrace.WriteInLog("        Бизнес. Выбран checkbox Трудоустройство, персонал");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingBusinessAudit = true;
                                            LogTrace.WriteInLog("        Бизнес. Выбран checkbox Аудит, консалтинг");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingBusinessAdverts = true;
                                            LogTrace.WriteInLog("        Бизнес. Выбран checkbox Реклама");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingBusinessMiscellanea = true;
                                            LogTrace.WriteInLog("        Бизнес. Выбран checkbox Разное");
                                        }
                                    #endregion

                                    #region Недвижимость
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingRealtyChoseAll = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingRealtyMiscelanea = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingRealtyGarages = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Гаражи");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingRealtyFlats = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Квартиры");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingRealtyAbroad = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Зарубежная недвижимость");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingRealtyLand = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Земля");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingRealtySuburban = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Загородная недвижимость");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingRealtyHypothec = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Ипотека");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingRealtyCommerce = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Коммерческая недвижимость");
                                        }
                                    #endregion

                                    if (needSetCheckBox())
                                    {
                                        _pkModel.InterestsTargetingExhibitions = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Выставки, концерты, театры, кино");
                                    }

                                    #region Образование
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingEducationChoseAll = true;
                                            LogTrace.WriteInLog("        Образование. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingEducationForeignLanguages = true;
                                            LogTrace.WriteInLog("        Образование. Выбран checkbox Иностранные языки");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingEducationAbroad = true;
                                            LogTrace.WriteInLog("        Образование. Выбран checkbox Образование за рубежом");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingEducationHigh = true;
                                            LogTrace.WriteInLog("        Образование. Выбран checkbox Образование высшее");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingEducationMiscelanea = true;
                                            LogTrace.WriteInLog("        Образование. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingEducationChildren = true;
                                            LogTrace.WriteInLog("        Образование. Выбран checkbox Образование для детей");
                                        }
                                    #endregion

                                    #region Отдых, туризм, путешествия
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingRestChoseAll = true;
                                            LogTrace.WriteInLog("        Отдых, туризм, путешествия. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingRestMiscellanea = true;
                                            LogTrace.WriteInLog("        Отдых, туризм, путешествия. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingRestRuUa = true;
                                            LogTrace.WriteInLog("        Отдых, туризм, путешествия. Выбран checkbox Отдых в России и Украине");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingRestAbroad = true;
                                            LogTrace.WriteInLog("        Отдых, туризм, путешествия. Выбран checkbox Отдых за рубежом");
                                        }
                                    #endregion

                                    #region Телефоны, связь
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingTelephonesChoseAll = true;
                                            LogTrace.WriteInLog("        Телефоны, связь. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingTelephonesMiscellanea = true;
                                            LogTrace.WriteInLog("        Телефоны, связь. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingTelephonesNavigation = true;
                                            LogTrace.WriteInLog("        Телефоны, связь. Выбран checkbox Навигация");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingTelephonesMobileApps = true;
                                            LogTrace.WriteInLog("        Телефоны, связь. Выбран checkbox Мобильные приложения и услуги");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingTelephonesMobile = true;
                                            LogTrace.WriteInLog("        Телефоны, связь. Выбран checkbox Мобильные телефоны");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingTelephonesStationary = true;
                                            LogTrace.WriteInLog("        Телефоны, связь. Выбран checkbox Стационарная связь");
                                        }
                                    #endregion

                                    if (needSetCheckBox()) _pkModel.InterestsTargetingHouseAplliances = true;
                                    LogTrace.WriteInLog("        Выбран checkbox Бытовая техника");

                                    #region Медицина, здоровье
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingMedicineChoseAll = true;
                                            LogTrace.WriteInLog("        Медицина, здоровье. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingMedicineSport = true;
                                            LogTrace.WriteInLog("        Медицина, здоровье. Выбран checkbox Спорт, фитнес, йога");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingMedicineEyesight = true;
                                            LogTrace.WriteInLog("        Медицина, здоровье. Выбран checkbox Зрение");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingMedicineMiscellanea = true;
                                            LogTrace.WriteInLog("        Медицина, здоровье. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingMedicineDiets = true;
                                            LogTrace.WriteInLog("        Медицина, здоровье. Выбран checkbox Диеты");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingMedicineExtraWeight = true;
                                            LogTrace.WriteInLog("        Медицина, здоровье. Выбран checkbox Лишний вес");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingMedicinePregnancy = true;
                                            LogTrace.WriteInLog("        Медицина, здоровье. Выбран checkbox Беременность и роды");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingMedicineStomatology = true;
                                            LogTrace.WriteInLog("        Медицина, здоровье. Выбран checkbox Стоматология");
                                        }
                                    #endregion

                                    #region Дом и семья
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingHouseChoseAll = true;
                                            LogTrace.WriteInLog("        Дом и семья. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingHouseChildren = true;
                                            LogTrace.WriteInLog("        Дом и семья. Выбран checkbox Маленькие дети");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingHouseDogs = true;
                                            LogTrace.WriteInLog("        Дом и семья. Выбран checkbox Собаки");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingHouseMiscellanea = true;
                                            LogTrace.WriteInLog("        Дом и семья. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingHouseCats = true;
                                            LogTrace.WriteInLog("        Дом и семья. Выбран checkbox Кошки");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingHouseCookery = true;
                                            LogTrace.WriteInLog("        Дом и семья. Выбран checkbox Кулинария, рецепты");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingHouseKindergartens = true;
                                            LogTrace.WriteInLog("        Дом и семья. Выбран checkbox Детские сады");
                                        }
                                    #endregion

                                    #region Финансы
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingFinanceChoseAll = true;
                                            LogTrace.WriteInLog("        Финансы. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingFinanceStockMarket = true;
                                            LogTrace.WriteInLog("        Финансы. Выбран checkbox Фондовый рынок");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingFinanceCurrency = true;
                                            LogTrace.WriteInLog("        Финансы. Выбран checkbox Валюта");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingFinanceInsurence = true;
                                            LogTrace.WriteInLog("        Финансы. Выбран checkbox Страхование");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingFinanceMoneyTransfers = true;
                                            LogTrace.WriteInLog("        Финансы. Выбран checkbox Денежные переводы");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingFinanceCredits = true;
                                            LogTrace.WriteInLog("        Финансы. Выбран checkbox Кредиты");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingFinanceMiscellanea = true;
                                            LogTrace.WriteInLog("        Финансы. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingFinanceDeposits = true;
                                            LogTrace.WriteInLog("        Финансы. Выбран checkbox Вклады, депозиты");
                                        }
                                    #endregion

                                    #region Компьютеры, оргтехника
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingComputersChoseAll = true;
                                            LogTrace.WriteInLog("        Компьютеры, оргтехника. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingComputersLaptops = true;
                                            LogTrace.WriteInLog("        Компьютеры, оргтехника. Выбран checkbox Ноутбуки");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingComputersParts = true;
                                            LogTrace.WriteInLog("        Компьютеры, оргтехника. Выбран checkbox Компьютерные комплектующие");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingComputersPrinters = true;
                                            LogTrace.WriteInLog("        Компьютеры, оргтехника. Выбран checkbox Принтеры");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingComputersTablets = true;
                                            LogTrace.WriteInLog("        Компьютеры, оргтехника. Выбран checkbox Планшетные ПК");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingComputersMonitors = true;
                                            LogTrace.WriteInLog("        Компьютеры, оргтехника. Выбран checkbox Мониторы");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingComputersMiscellanea = true;
                                            LogTrace.WriteInLog("        Компьютеры, оргтехника. Выбран checkbox Разное");
                                        }
                                    #endregion

                                    #region Авто
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingAutoChoseAll = true;
                                            LogTrace.WriteInLog("        Авто. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingAutoInsurence = true;
                                            LogTrace.WriteInLog("        Авто. Выбран checkbox Автострахование");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingAutoMiscellanea = true;
                                            LogTrace.WriteInLog("        Авто. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingAutoNational = true;
                                            LogTrace.WriteInLog("        Авто. Выбран checkbox Отечественные");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingAutoWheels = true;
                                            LogTrace.WriteInLog("        Авто. Выбран checkbox Колёса, Шины");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingAutoImported = true;
                                            LogTrace.WriteInLog("        Авто. Выбран checkbox Иномарки");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingAutoMoto = true;
                                            LogTrace.WriteInLog("        Авто. Выбран checkbox Мото-, Квадроциклы, Снегоходы");
                                        }
                                    #endregion

                                    #region Аудио, Видео, Фото
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingAudioChoseAll = true;
                                            LogTrace.WriteInLog("        Аудио, Видео, Фото. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingAudioVideoEquips = true;
                                            LogTrace.WriteInLog("        Аудио, Видео, Фото. Выбран checkbox Видеоаппаратура");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingAudioMiscellanea = true;
                                            LogTrace.WriteInLog("        Аудио, Видео, Фото. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingAudioTech = true;
                                            LogTrace.WriteInLog("        Аудио, Видео, Фото. Выбран checkbox Аудио-техника");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingAudioCameras = true;
                                            LogTrace.WriteInLog("        Аудио, Видео, Фото. Выбран checkbox Фотоаппараты");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.InterestsTargetingAudioTvs = true;
                                            LogTrace.WriteInLog("        Аудио, Видео, Фото. Выбран checkbox Телевизоры, DVD-проигрыватели");
                                        }
                                    #endregion
                                    break;
                                }
                        }
                    #endregion

                    #region Radiobutton Браузеры
                        variant = needSetRadioButton(2).ToString();
                        _pkModel.BrowserTargeting = variant;
                        switch (variant)
                        {
                            case "0":
                                {
                                    LogTrace.WriteInLog("     Выбираю radiobutton Браузеры. Выбрано: не использовать");
                                    break;
                                }
                            case "1":
                                {
                                    LogTrace.WriteInLog("     Выбираю radiobutton Браузеры. Выбрано: использовать");
                                    //развернуть все пункты
                                    _pkModel.BrowserTargetingOtherExpand = true;
                                    _pkModel.BrowserTargetingOperaExpand = true;
                                    _pkModel.BrowserTargetingChromeExpand = true;
                                    _pkModel.BrowserTargetingFirefoxExpand = true;
                                    _pkModel.BrowserTargetingSafariExpand = true;
                                    _pkModel.BrowserTargetingIeExpand = true;
                                    //pkModel.BrowserTargetingGoogleChromeMobileExpand = true;

                                    _pkModel.BrowserTargetingOtherAll = true;

                                    //if (needSetCheckBox())
                                    //{
                                    //    pkModel.BrowserTargetingOtherChoseAll = true;
                                    //    LogTrace.WriteInLog("        Другие. Выбран checkbox Другие Все");
                                    //}
                                    //if (needSetCheckBox())
                                    //{
                                    //pkModel.BrowserTargetingOtherAll = true;
                                    //LogTrace.WriteInLog("        Другие. Выбран checkbox Все");
                                    //}

                                    #region Опера
                                        _pkModel.BrowserTargetingOperaOther = true;
                                        LogTrace.WriteInLog("        Опера. Выбран checkbox Другие");
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingOperaChoseAll = true;
                                            LogTrace.WriteInLog("        Опера. Выбран checkbox Все");
                                        }
                                        //if (needSetCheckBox()) pkModel.BrowserTargetingOperaOther = true;
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingOpera10 = true;
                                            LogTrace.WriteInLog("        Опера. Выбран checkbox 10");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingOpera11 = true;
                                            LogTrace.WriteInLog("        Опера. Выбран checkbox 11");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingOperaMini = true;
                                            LogTrace.WriteInLog("        Опера. Выбран checkbox Mini");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingOperaMobile = true;
                                            LogTrace.WriteInLog("        Опера. Выбран checkbox Mobile");
                                        }
                                    #endregion

                                    #region Chrome
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingChromeChoseAll = true;
                                            LogTrace.WriteInLog("        Chrome. Выбран checkbox Chrome Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingChromeAll = true;
                                            LogTrace.WriteInLog("        Chrome. Выбран checkbox Все");
                                        }
                                    #endregion

                                    #region Firefox
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingFirefoxChoseAll = true;
                                            LogTrace.WriteInLog("        Firefox. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingFirefox3 = true;
                                            LogTrace.WriteInLog("        Firefox. Выбран checkbox 3");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingFirefox4 = true;
                                            LogTrace.WriteInLog("        Firefox. Выбран checkbox 4");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingFirefox5 = true;
                                            LogTrace.WriteInLog("        Firefox. Выбран checkbox 5");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingFirefox6 = true;
                                            LogTrace.WriteInLog("        Firefox. Выбран checkbox 6");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingFirefoxOther = true;
                                            LogTrace.WriteInLog("        Firefox. Выбран checkbox Другие");
                                        }
                                    #endregion

                                    #region Safari
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingSafariChoseAll = true;
                                            LogTrace.WriteInLog("        Safari. Выбран checkbox Safari Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingSafariAll = true;
                                            LogTrace.WriteInLog("        Safari. Выбран checkbox Все");
                                        }
                                    #endregion

                                    #region MSIE
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingIeChoseAll = true;
                                            LogTrace.WriteInLog("        MSIE. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingIe6 = true;
                                            LogTrace.WriteInLog("        MSIE. Выбран checkbox 6");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingIe7 = true;
                                            LogTrace.WriteInLog("        MSIE. Выбран checkbox 7");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingIe8 = true;
                                            LogTrace.WriteInLog("        MSIE. Выбран checkbox 8");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingIe9 = true;
                                            LogTrace.WriteInLog("        MSIE. Выбран checkbox 9");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.BrowserTargetingIeOther = true;
                                            LogTrace.WriteInLog("        MSIE. Выбран checkbox Другие");
                                        }
                                    #endregion

                                    #region Google Chrome Mobile
                                    //if (needSetCheckBox())
                                    //{
                                    //    pkModel.BrowserTargetingGoogleChromeMobileChoseAll = true;
                                    //    LogTrace.WriteInLog("        Google Chrome Mobile. Выбран checkbox Google Chrome Mobile");
                                    //}
                                    //if (needSetCheckBox())
                                    //{
                                    //    pkModel.BrowserTargetingGoogleChromeMobile = true;
                                    //    LogTrace.WriteInLog("        Google Chrome Mobile. Выбран checkbox Mobile");
                                    //}
                                    #endregion

                                    break;
                                }
                        }
                    #endregion

                    #region Radiobutton OC таргетинг
                        variant = needSetRadioButton(2).ToString();
                        _pkModel.OsTargeting = variant;
                        switch (variant)
                        {
                            case "0":
                                {
                                    LogTrace.WriteInLog("     Выбираю radiobutton OC таргетинг. Выбрано: не использовать");
                                    break;
                                }
                            case "1":
                                {
                                    LogTrace.WriteInLog("     Выбираю radiobutton OC таргетинг. Выбрано: использовать");
                                    _pkModel.OsTargetingOther = true;
                                    LogTrace.WriteInLog("        Выбран checkbox Другие");
                                    //if (needSetCheckBox()) pkModel.OsTargetingOther = true;
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.OsTargetingMacOs = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Mac OS");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.OsTargetingOtherMobileOs = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Прочие мобильные ОС");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.OsTargetingWindows = true;
                                        LogTrace.WriteInLog("        Выбран checkbox WIndows");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.OsTargetingOtherIoS = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Прочие iOS системы");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.OsTargetingIpad = true;
                                        LogTrace.WriteInLog("        Выбран checkbox iPAD");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.OsTargetingIphone = true;
                                        LogTrace.WriteInLog("        Выбран checkbox IPHONE");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.OsTargetingAndroid = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Android");
                                    }
                                    break;
                                }
                        }
                    #endregion

                    #region Radiobutton Провайдеры
                        variant = needSetRadioButton(2).ToString();
                        _pkModel.ProviderTargeting = variant;
                        switch (variant)
                        {
                            case "0":
                                {
                                    LogTrace.WriteInLog("     Выбираю radiobutton Провайдеры. Выбрано: не использовать");
                                    break;
                                }
                            case "1":
                                {
                                    LogTrace.WriteInLog("     Выбираю radiobutton Провайдеры. Выбрано: использовать");
                                    _pkModel.ProviderTargetingOther = true;
                                    LogTrace.WriteInLog("        Выбран checkbox Другие");
                                    //if (needSetCheckBox()) pkModel.ProviderTargetingOther = true;
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.ProviderTargetingMegafon = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Мегафон");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.ProviderTargetingMtc = true;
                                        LogTrace.WriteInLog("        Выбран checkbox МТС Россия");
                                    }
                                    break;
                                }
                        }
                    #endregion

                    #region Radiobutton Геотаргетинг
                        variant = needSetRadioButton(2).ToString();
                        _pkModel.GeoTargeting = variant;
                        switch (variant)
                        {
                            case "0":
                                {
                                    LogTrace.WriteInLog("     Выбираю radiobutton Геотаргетинг. Выбрано: не использовать");
                                    break;
                                }
                            case "1":
                                {
                                    LogTrace.WriteInLog("     Выбираю radiobutton Геотаргетинг. Выбрано: использовать");
                                    _pkModel.GeoTargetingRussiaExpand = true;
                                    _pkModel.GeoTargetingUkraineExpand = true;

                                    _pkModel.GeoTargetingOther = true;
                                    LogTrace.WriteInLog("        Выбран checkbox Прочие страны");
                                    //if (needSetCheckBox()) pkModel.GeoTargetingOther = true;
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.GeoTargetingAustria = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Австрия");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.GeoTargetingBelorussia = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Белоруссия");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.GeoTargetingUk = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Великобритания");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.GeoTargetingGermany = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Германия");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.GeoTargetingIsrael = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Израиль");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.GeoTargetingKazakhstan = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Казахстан");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.GeoTargetingLatvia = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Латвия");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        _pkModel.GeoTargetingLithuania = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Литва");
                                    }

                                    #region Россия
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingRussiaChoseAll = true;
                                            LogTrace.WriteInLog("        Россия. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingRussiaEburg = true;
                                            LogTrace.WriteInLog("        Россия. Выбран checkbox Екатеринбург");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingRussiaMoscow = true;
                                            LogTrace.WriteInLog("        Россия. Выбран checkbox Москва");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingRussiaNovosibirsk = true;
                                            LogTrace.WriteInLog("        Россия. Выбран checkbox Новосибирск");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingRussiaOther = true;
                                            LogTrace.WriteInLog("        Россия. Выбран checkbox Прочие регионы");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingRussiaSpb = true;
                                            LogTrace.WriteInLog("        Россия. Выбран checkbox Санкт-Петербург");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingRussiaHabarovsk = true;
                                            LogTrace.WriteInLog("        Россия. Выбран checkbox Хабаровск");
                                        }
                                    #endregion

                                    if (needSetCheckBox())
                                    {
                                        _pkModel.GeoTargetingUsa = true;
                                        LogTrace.WriteInLog("        Выбран checkbox США");
                                    }

                                    #region Украина
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingUkraineChoseAll = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingUkraineDnepr = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Днепропетровск");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingUkraineDonetzk = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Донецк");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingUkraineZakarpattya = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Закарпатье");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingUkraineKiev = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Киев");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingUkraineCrimea = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Крым");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingUkraineLvov = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Львов");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingUkraineNikolaev = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Николаев");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingUkraineOdessa = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Одесса");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingUkraineOther = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Прочие области");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingUkraineHarkov = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Харьков");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingUkraineHerson = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Херсон");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingUkraineCherkassy = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Черкассы");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            _pkModel.GeoTargetingUkraineChernovzi = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Черновцы");
                                        }
                                    #endregion

                                    if (needSetCheckBox())
                                    {
                                        _pkModel.GeoTargetingEstonia = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Эстония");
                                    }
                                    break;
                                }
                        }
                    #endregion
                }
            #endregion
        }

        private void CreationIsSuccessful()
        {
            string parentWindow = _driver.CurrentWindowHandle;//запоминаем текущее (родительское окно)

            string createPkUrl = _driver.Url; //запоминаем url страницы "Добавление РК"
            _pkModel.Submit(); //пытаемся сохранить форму
            LogTrace.WriteInLog("     Нажал кнопку Завершить");

            if (_driver.Url == createPkUrl)
                Errors.Add(_pkModel.GetErrors().ToString()); //проверяем, появились ли на форме ошибки заполнения полей
            else
                GetPkIdFromLink();

            Registry.hashTable["pkId"] = PkId; //запомнили глобально ID созданной РК
            Registry.hashTable["driver"] = _driver;
        }

        private void GetPkIdFromLink()
        {
            //т.к. после успешного создания РК нас перебрасывает на страницу Клиента (а не РК)
            //поэтому, чтобы получить ID РК, находим на странице название только что созданной РК
            // и переходим по этому названию-ссылке на страницу РК
            // открывается новое окно с РК - мы должны на него переключиться
            // и там получаем ID РК из URL
            if (_driver.PageSource.Contains(PkName))
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                IWebElement webelement = wait.Until(ExpectedConditions.ElementExists(By.LinkText(PkName)));//_driver.FindElement(By.LinkText(PkName));
                string href = webelement.GetAttribute("href");
                
                char[] slash = new char[] { '/' };
                string[] url = href.Split(slash); //разбиваем URL по /
                PkId = url[url.Length - 1]; //берем последний элемент массива - это id новой РК
                ClientId = Registry.hashTable["clientId"].ToString(); //берется для вывода в listBox и логи
                LogTrace.WriteInLog("     " + _driver.Url);
            }
        }

        private bool needSetCheckBox() //генерируем 0 или 1.  1 - заполняем необязательное поле, 0 - не заполняем
        {
            Random rnd = new Random();
            return rnd.Next(0, 2) == 1 ? true : false;
        }

        private int needSetRadioButton(int variants) //генерируем номер варианта выбора для needSetRadioButton. variants - кол-во вариантов выбора
        {
            Random rnd = new Random();
            return rnd.Next(0, variants);
        }
    }
}
