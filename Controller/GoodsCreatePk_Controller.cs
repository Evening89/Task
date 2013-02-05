using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.Model;
using Task.Utils;
using OpenQA.Selenium;

namespace Task.Controller
{
    public class GoodsCreatePk_Controller
    {
        public IWebDriver driver;
        //берем сохраненный ранее 
        //(при создании клиента Task.Controller.GoodsCreateClient_Controller) ID клиента
        //и дописываем в URL
        public string baseUrl = "https://" + Registry.hashTable["Login"] + ":" + Registry.hashTable["Password"] + "@" + "admin.dt00.net/cab/goodhits/clients-new-campaign/client_id/" + Registry.hashTable["clientId"];
        public List<string> errors = new List<string>(); //список ошибок (в каждой строке - спарсенное со страницы описание ошибки)
        public string pkId; //переменная для хранения ID только что созданной РК
        public string clientId;
        public string pkName; //переменная для хранения названия только что созданной РК
        Randoms randoms = new Randoms(); //класс генерации случайных строк

        public void CreatePk(bool setNecessaryFields, bool setUnnecessaryFields)
        {
            driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный ранее драйвер
            driver.Navigate().GoToUrl(baseUrl); //заходим по ссылке
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            //ставим ожидание в 10 сек на случай, если страница медленно грузится и нужные эл-ты появятся не сразу

            GoodsCreatePK_Model pkModel = new GoodsCreatePK_Model();
            pkModel.driver = driver;

            LogTrace.WriteInLog("     " + driver.Url);

            #region Required fields
                if (setNecessaryFields) //выбрано заполнение обязательных полей
                {
                    LogTrace.WriteInLog("     ...Заполняю обязательные поля...");
                    //Название РК
                    pkName = randoms.RandomString(15) + " " + randoms.RandomNumber(5);
                    pkModel.Name = pkName;
                    LogTrace.WriteInLog("     Заполняю поле Название РК. Было введено: " + pkModel.Name);

                    //Геотаргетинг выбран по умолчанию при открытии страницы - делаем выбор "не использовать"
                    pkModel.GeoTargeting = "0";
                    LogTrace.WriteInLog("     Выбираю radiobutton Геотаргетинг. Выбрано: не использовать");
                }
            #endregion

            #region Unrequired fields
                if (setUnnecessaryFields) //выбрано заполнение также необязательных полей
                {
                    LogTrace.WriteInLog("     ...Заполняю необязательные поля...");
                    if (needSetCheckBox())
                    {
                        pkModel.StartPkDate = pkModel.GenerateDate();
                        LogTrace.WriteInLog("     Заполняю поле Дата старта РК. Было введено: " + pkModel.StartPkDate);
                    }

                    if (needSetCheckBox())
                    {
                        pkModel.EndPkDate = pkModel.GenerateDate();
                        LogTrace.WriteInLog("     Заполняю поле Дата окончания РК. Было введено: " + pkModel.EndPkDate);
                        List<string> instantErrorsDate = pkModel.ErrorsInFillFields();
                        if (instantErrorsDate.Count != 0) //если список с ошибками заполнения полей даты непуст
                            errors = instantErrorsDate; //копируем в нас общий список ошибок errors
                    }
                    //if (needSetCheckBox())
                    //{
                    //    pkModel.BlockTeasersAfterCreation = true;
                    //    LogTrace.WriteInLog("     Выбран checkbox Блокировать тизеры после их создания");
                    //}
                    if (needSetCheckBox())
                    {
                        pkModel.StoppedByManager = true;
                        LogTrace.WriteInLog("     Выбран checkbox Остановлена менеджером");
                    }

                    #region Radiobutton Ограничения рекламной кампании
                        string variant = needSetRadioButton(3).ToString();
                        pkModel.LimitsOfPk = variant;
                        switch (variant)
                        {
                            case "0":
                                {
                                    LogTrace.WriteInLog("     Выбираю radiobutton Ограничения рекламной кампании. Выбрано: не использовать");
                                    break;
                                }
                            case "1":
                                {
                                    int num2;
                                    do
                                    {
                                        num2 = int.Parse(randoms.RandomNumber(2));
                                    } while (num2 < 5); //суточный лимит должен быть не менее 5
                                    LogTrace.WriteInLog("     Выбираю radiobutton Ограничения рекламной кампании. Выбрано: по бюджету");
                                    pkModel.DayLimitByBudget = num2.ToString();//суточный лимит должен быть не менее 5
                                    LogTrace.WriteInLog("        Заполняю поле Суточный лимит РК. Было введено: " + pkModel.DayLimitByBudget);
                                    pkModel.GeneralLimitByBudget = randoms.RandomNumber(3);
                                    LogTrace.WriteInLog("        Заполняю поле Общий лимит РК. Было введено: " + pkModel.GeneralLimitByBudget);
                                    break;
                                }
                            case "2":
                                {
                                    LogTrace.WriteInLog("     Выбираю radiobutton Ограничения рекламной кампании. Выбрано: по количеству кликов");
                                    pkModel.DayLimitByClicks = randoms.RandomNumber(3);
                                    LogTrace.WriteInLog("        Заполняю поле Суточный лимит кликов РК. Было введено: " + pkModel.DayLimitByClicks);
                                    pkModel.GeneralLimitByClicks = randoms.RandomNumber(3);
                                    LogTrace.WriteInLog("        Заполняю поле Общий лимит кликов РК. Было введено: " + pkModel.GeneralLimitByClicks);
                                    break;
                                }
                        }
                    #endregion

                    #region Checkbox UTM-разметка рекламной кампании для Google Analytics
                        if (needSetCheckBox())
                        {
                            pkModel.UtmPkForGoogleAnalytics = true;
                            LogTrace.WriteInLog("     Выбран checkbox UTM-разметка рекламной кампании для Google Analytics");
                            pkModel.UtmMedium = randoms.RandomString(5);
                            LogTrace.WriteInLog("        Заполняю поле utm_medium (средство кампании). Было введено: " + pkModel.UtmMedium);
                            pkModel.UtmSource = randoms.RandomString(5);
                            LogTrace.WriteInLog("        Заполняю поле utm_source (источник кампании). Было введено: " + pkModel.UtmSource);
                            pkModel.UtmCampaign = randoms.RandomString(5);
                            LogTrace.WriteInLog("        Заполняю поле utm_campaign (название кампании). Было введено: " + pkModel.UtmCampaign);
                        }
                    #endregion

                    #region Checkbox UTM-разметка пользователя
                        if (needSetCheckBox())
                        {
                            pkModel.UtmUser = true;
                            LogTrace.WriteInLog("     Выбран checkbox UTM-разметка пользователя");
                            pkModel.UtmUserStr = randoms.RandomString(5);
                            LogTrace.WriteInLog("        Заполняю поле UTM-разметка пользователя. Было введено: " + pkModel.UtmUserStr);
                        }
                    #endregion

                    if (needSetCheckBox())
                    {
                        LogTrace.WriteInLog("     Выбран checkbox Крутить в сети Товарро");
                        pkModel.ScrewInTovarro = true;
                    }

                    #region Checkbox Блокировка по расписанию
                        if (needSetCheckBox())
                        {
                            pkModel.BlockBySchedule = true;
                            LogTrace.WriteInLog("     Выбран checkbox Блокировка по расписанию");
                            if (needSetCheckBox()) pkModel.Weekends = true;
                            LogTrace.WriteInLog("        Выбран checkbox Выходные");
                            if (needSetCheckBox()) pkModel.Weekdays = true;
                            LogTrace.WriteInLog("        Выбран checkbox Будни");
                            if (needSetCheckBox()) pkModel.WorkingTime = true;
                            LogTrace.WriteInLog("        Выбран checkbox Рабочее время (9-18 по будням)");
                        }
                    #endregion

                    #region Checkbox Передавать id площадки в ссылке
                        if (needSetCheckBox())
                        {
                            pkModel.IdOfPlatformInLink = true;
                            LogTrace.WriteInLog("     Выбран checkbox Передавать id площадки в ссылке");
                            pkModel.IdOfPlatformInLinkStr = randoms.RandomString(5);
                            LogTrace.WriteInLog("        Заполняю поле Передавать id площадки в ссылке. Было введено: " + pkModel.IdOfPlatformInLinkStr);
                        }
                    #endregion

                    if (needSetCheckBox())
                    {
                        pkModel.AddIdOfTeaserInLink = true;
                        LogTrace.WriteInLog("     Выбран checkbox Добавлять id тизера в конец ссылки");
                    }

                    if (needSetCheckBox())
                    {
                        pkModel.CommentsForPk = randoms.RandomString(20) + " " + randoms.RandomString(10);
                        LogTrace.WriteInLog("     Заполняю textarea Комментарий к кампании. Было введено: " + pkModel.CommentsForPk);
                    }

                    #region Checkbox Площадки
                        if (needSetCheckBox())
                        {
                            pkModel.Platforms = true;
                            LogTrace.WriteInLog("     Выбран checkbox Площадки");
                            pkModel.PlatformsNotSpecified = true;
                            LogTrace.WriteInLog("        Выбран checkbox Не определено");
                            //if (needSetCheckBox()) pkModel.PlatformsNotSpecified = true;
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsPolitics = true;
                                LogTrace.WriteInLog("        Выбран checkbox Политика, общество, происшествия, религия");
                            }
                        
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsEconomics = true;
                                LogTrace.WriteInLog("        Выбран checkbox Экономика, финансы, недвижимость, работа и карьера");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsCelebrities = true;
                                LogTrace.WriteInLog("        Выбран checkbox Знаменитости, шоу-бизнес, кино, музыка");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsScience = true;
                                LogTrace.WriteInLog("        Выбран checkbox Наука и технологии");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsConnection = true;
                                LogTrace.WriteInLog("        Выбран checkbox Связь, компьютеры, программы");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsSports = true;
                                LogTrace.WriteInLog("        Выбран checkbox Спорт");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsAuto = true;
                                LogTrace.WriteInLog("        Выбран checkbox Авто-вело-мото");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsFashion = true;
                                LogTrace.WriteInLog("        Выбран checkbox Мода и стиль, здоровье и красота, фитнес и диета, кулинария");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsMedicine = true;
                                LogTrace.WriteInLog("        Выбран checkbox Медицина");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsTourism = true;
                                LogTrace.WriteInLog("        Выбран checkbox Туризм и отдых (путевки, отели, рестораны)");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsGlobalPortals = true;
                                LogTrace.WriteInLog("        Выбран checkbox Глобальные порталы с множеством подпроектов");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsHumor = true;
                                LogTrace.WriteInLog("        Выбран checkbox Юмор (приколы, картинки, обои), каталог фотографий, блоги");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsFileshares = true;
                                LogTrace.WriteInLog("        Выбран checkbox Файлообменники, файлокачалки (кино, музыка, игры, программы)");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsSocialNetworks = true;
                                LogTrace.WriteInLog("        Выбран checkbox Социальные сети, сайты знакомства, личные дневники");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsAnimals = true;
                                LogTrace.WriteInLog("        Выбран checkbox Животный и растительный мир");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsReligion = true;
                                LogTrace.WriteInLog("        Выбран checkbox Религия");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsChildren = true;
                                LogTrace.WriteInLog("        Выбран checkbox Дети и родители");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsBuilding = true;
                                LogTrace.WriteInLog("        Выбран checkbox Строительство, ремонт, дача, огород");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsCookery = true;
                                LogTrace.WriteInLog("        Выбран checkbox Кулинария");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsJob = true;
                                LogTrace.WriteInLog("        Выбран checkbox Работа и карьера. Поиск работы, поиск персонала");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsNotSites = true;
                                LogTrace.WriteInLog("        Выбран checkbox Не сайты (программы, тулбары, таскбары)");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsSitesStartPagesBrowsers = true;
                                LogTrace.WriteInLog("        Выбран checkbox Сайты, размещенные на стартовых страницах браузеров");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsSearchSystems = true;
                                LogTrace.WriteInLog("        Выбран checkbox Поисковые системы");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsEmail = true;
                                LogTrace.WriteInLog("        Выбран checkbox Почта");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsPhotoCatalogues = true;
                                LogTrace.WriteInLog("        Выбран checkbox Каталоги фотографий");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsVarez = true;
                                LogTrace.WriteInLog("        Выбран checkbox Варезники");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsOnlineVideo = true;
                                LogTrace.WriteInLog("        Выбран checkbox Онлайн видео, телевидение, радио");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsOnlineLibraries = true;
                                LogTrace.WriteInLog("        Выбран checkbox Онлайн-библиотеки");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsInternet = true;
                                LogTrace.WriteInLog("        Выбран checkbox Интернет, поисковые сайты, электронная почта, интернет-магазины, аукционы, каталоги ресурсов, фирм и предприятий");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsOnlineGames = true;
                                LogTrace.WriteInLog("        Выбран checkbox Онлайн игры");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsInternetRepresentatives = true;
                                LogTrace.WriteInLog("        Выбран checkbox Интернет-представительства бизнеса.");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsProgramms = true;
                                LogTrace.WriteInLog("        Выбран checkbox Программы, прошивки, игры для КПК и мобильных устройств");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsCataloguesInternetResources = true;
                                LogTrace.WriteInLog("        Выбран checkbox Каталоги Интернет - ресурсов, фирм и предприятий");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsForWagesInInternet = true;
                                LogTrace.WriteInLog("        Выбран checkbox Для заработка в Интернете. Партнерские программы");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsHobbies = true;
                                LogTrace.WriteInLog("        Выбран checkbox Хобби и увлечения");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsMarketgid = true;
                                LogTrace.WriteInLog("        Выбран checkbox Маркетгид");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsShock = true;
                                LogTrace.WriteInLog("        Выбран checkbox Шокодром");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsEsoteric = true;
                                LogTrace.WriteInLog("        Выбран checkbox Эзотерика. Непознанное, астрология, гороскопы, гадания");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsPsychology = true;
                                LogTrace.WriteInLog("        Выбран checkbox Психология, мужчина и женщина");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsHistory = true;
                                LogTrace.WriteInLog("        Выбран checkbox История, образование, культура");
                            }
                            if (needSetCheckBox())
                            {
                                pkModel.PlatformsMarketgidWomenNet = true;
                                LogTrace.WriteInLog("        Выбран checkbox Маркетгид ЖС");
                            }
                        }
                    #endregion

                    #region Radiobutton Демографический таргетинг
                        variant = needSetRadioButton(2).ToString();
                        pkModel.DemoTargeting = variant;
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
                                    pkModel.DemoTargetingMenExpand = true;
                                    pkModel.DemoTargetingWomenExpand = true;
                                    pkModel.DemoTargetingHermaphroditeExpand = true;

                                    #region Мужчины
                                        pkModel.DemoTargetingMenNotSpecified = true;
                                        LogTrace.WriteInLog("        Мужчины. Выбран checkbox Не определен");
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingMenChoseAll = true;
                                            LogTrace.WriteInLog("        Мужчины. Выбран checkbox Все");
                                        }
                                        //if (needSetCheckBox()) pkModel.DemoTargetingMenNotSpecified = true;
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingMen618 = true;
                                            LogTrace.WriteInLog("        Мужчины. Выбран checkbox 6-18");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingMen1924 = true;
                                            LogTrace.WriteInLog("        Мужчины. Выбран checkbox 19-24");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingMen2534 = true;
                                            LogTrace.WriteInLog("        Мужчины. Выбран checkbox 25-34");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingMen3544 = true;
                                            LogTrace.WriteInLog("        Мужчины. Выбран checkbox 35-44");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingMen4590 = true;
                                            LogTrace.WriteInLog("        Мужчины. Выбран checkbox 45-90");
                                        }
                                    #endregion

                                    #region Женщины
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingWomenChoseAll = true;
                                            LogTrace.WriteInLog("        Женщины. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingWomenNotSpecified = true;
                                            LogTrace.WriteInLog("        Женщины. Выбран checkbox Не определен");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingWomen618 = true;
                                            LogTrace.WriteInLog("        Женщины. Выбран checkbox 6-18");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingWomen1924 = true;
                                            LogTrace.WriteInLog("        Женщины. Выбран checkbox 19-24");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingWomen2534 = true;
                                            LogTrace.WriteInLog("        Женщины. Выбран checkbox 25-34");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingWomen3544 = true;
                                            LogTrace.WriteInLog("        Женщины. Выбран checkbox 35-44");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingWomen4590 = true;
                                            LogTrace.WriteInLog("        Женщины. Выбран checkbox 45-90");
                                        }
                                    #endregion

                                    #region Пол не определен
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingHermaphroditeChoseAll = true;
                                            LogTrace.WriteInLog("        Пол не определен. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingHermaphrodite618 = true;
                                            LogTrace.WriteInLog("        Пол не определен. Выбран checkbox 6-18");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingHermaphrodite1924 = true;
                                            LogTrace.WriteInLog("        Пол не определен. Выбран checkbox 19-24");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingHermaphrodite2534 = true;
                                            LogTrace.WriteInLog("        Пол не определен. Выбран checkbox 25-34");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingHermaphrodite3544 = true;
                                            LogTrace.WriteInLog("        Пол не определен. Выбран checkbox 35-44");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.DemoTargetingHermaphrodite4590 = true;
                                            LogTrace.WriteInLog("        Пол не определен. Выбран checkbox 45-90");
                                        }
                                    #endregion

                                    break;
                                }
                        }
                    #endregion

                    #region Radiobutton Таргетинг по интересам
                        variant = needSetRadioButton(2).ToString();
                        pkModel.InterestsTargeting = variant;
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
                                    pkModel.InterestsTargetingBusinessExpand = true;
                                    pkModel.InterestsTargetingRealtyExpand = true;
                                    pkModel.InterestsTargetingEducationExpand = true;
                                    pkModel.InterestsTargetingRestExpand = true;
                                    pkModel.InterestsTargetingTelephonesExpand = true;
                                    pkModel.InterestsTargetingMedicineExpand = true;
                                    pkModel.InterestsTargetingHouseExpand = true;
                                    pkModel.InterestsTargetingFinanceExpand = true;
                                    pkModel.InterestsTargetingComputersExpand = true;
                                    pkModel.InterestsTargetingAutoExpand = true;
                                    pkModel.InterestsTargetingAudioExpand = true;

                                    pkModel.InterestsTargetingOther = true;
                                    LogTrace.WriteInLog("        Выбран checkbox Прочее");
                                    //if (needSetCheckBox()) pkModel.InterestsTargetingOther = true;

                                    #region Бизнес
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingBusinessChoseAll = true;
                                            LogTrace.WriteInLog("        Бизнес. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingBusinessAcoountancy = true;
                                            LogTrace.WriteInLog("        Бизнес. Выбран checkbox Бухгалтерия");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingBusinessPlacement = true;
                                            LogTrace.WriteInLog("        Бизнес. Выбран checkbox Трудоустройство, персонал");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingBusinessAudit = true;
                                            LogTrace.WriteInLog("        Бизнес. Выбран checkbox Аудит, консалтинг");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingBusinessAdverts = true;
                                            LogTrace.WriteInLog("        Бизнес. Выбран checkbox Реклама");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingBusinessMiscellanea = true;
                                            LogTrace.WriteInLog("        Бизнес. Выбран checkbox Разное");
                                        }
                                    #endregion

                                    #region Недвижимость
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingRealtyChoseAll = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingRealtyMiscelanea = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingRealtyGarages = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Гаражи");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingRealtyFlats = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Квартиры");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingRealtyAbroad = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Зарубежная недвижимость");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingRealtyLand = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Земля");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingRealtySuburban = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Загородная недвижимость");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingRealtyHypothec = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Ипотека");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingRealtyCommerce = true;
                                            LogTrace.WriteInLog("        Недвижимость. Выбран checkbox Коммерческая недвижимость");
                                        }
                                    #endregion

                                    if (needSetCheckBox())
                                    {
                                        pkModel.InterestsTargetingExhibitions = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Выставки, концерты, театры, кино");
                                    }

                                    #region Образование
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingEducationChoseAll = true;
                                            LogTrace.WriteInLog("        Образование. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingEducationForeignLanguages = true;
                                            LogTrace.WriteInLog("        Образование. Выбран checkbox Иностранные языки");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingEducationAbroad = true;
                                            LogTrace.WriteInLog("        Образование. Выбран checkbox Образование за рубежом");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingEducationHigh = true;
                                            LogTrace.WriteInLog("        Образование. Выбран checkbox Образование высшее");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingEducationMiscelanea = true;
                                            LogTrace.WriteInLog("        Образование. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingEducationChildren = true;
                                            LogTrace.WriteInLog("        Образование. Выбран checkbox Образование для детей");
                                        }
                                    #endregion

                                    #region Отдых, туризм, путешествия
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingRestChoseAll = true;
                                            LogTrace.WriteInLog("        Отдых, туризм, путешествия. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingRestMiscellanea = true;
                                            LogTrace.WriteInLog("        Отдых, туризм, путешествия. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingRestRuUa = true;
                                            LogTrace.WriteInLog("        Отдых, туризм, путешествия. Выбран checkbox Отдых в России и Украине");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingRestAbroad = true;
                                            LogTrace.WriteInLog("        Отдых, туризм, путешествия. Выбран checkbox Отдых за рубежом");
                                        }
                                    #endregion

                                    #region Телефоны, связь
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingTelephonesChoseAll = true;
                                            LogTrace.WriteInLog("        Телефоны, связь. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingTelephonesMiscellanea = true;
                                            LogTrace.WriteInLog("        Телефоны, связь. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingTelephonesNavigation = true;
                                            LogTrace.WriteInLog("        Телефоны, связь. Выбран checkbox Навигация");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingTelephonesMobileApps = true;
                                            LogTrace.WriteInLog("        Телефоны, связь. Выбран checkbox Мобильные приложения и услуги");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingTelephonesMobile = true;
                                            LogTrace.WriteInLog("        Телефоны, связь. Выбран checkbox Мобильные телефоны");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingTelephonesStationary = true;
                                            LogTrace.WriteInLog("        Телефоны, связь. Выбран checkbox Стационарная связь");
                                        }
                                    #endregion

                                    if (needSetCheckBox()) pkModel.InterestsTargetingHouseAplliances = true;
                                    LogTrace.WriteInLog("        Выбран checkbox Бытовая техника");

                                    #region Медицина, здоровье
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingMedicineChoseAll = true;
                                            LogTrace.WriteInLog("        Медицина, здоровье. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingMedicineSport = true;
                                            LogTrace.WriteInLog("        Медицина, здоровье. Выбран checkbox Спорт, фитнес, йога");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingMedicineEyesight = true;
                                            LogTrace.WriteInLog("        Медицина, здоровье. Выбран checkbox Зрение");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingMedicineMiscellanea = true;
                                            LogTrace.WriteInLog("        Медицина, здоровье. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingMedicineDiets = true;
                                            LogTrace.WriteInLog("        Медицина, здоровье. Выбран checkbox Диеты");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingMedicineExtraWeight = true;
                                            LogTrace.WriteInLog("        Медицина, здоровье. Выбран checkbox Лишний вес");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingMedicinePregnancy = true;
                                            LogTrace.WriteInLog("        Медицина, здоровье. Выбран checkbox Беременность и роды");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingMedicineStomatology = true;
                                            LogTrace.WriteInLog("        Медицина, здоровье. Выбран checkbox Стоматология");
                                        }
                                    #endregion

                                    #region Дом и семья
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingHouseChoseAll = true;
                                            LogTrace.WriteInLog("        Дом и семья. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingHouseChildren = true;
                                            LogTrace.WriteInLog("        Дом и семья. Выбран checkbox Маленькие дети");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingHouseDogs = true;
                                            LogTrace.WriteInLog("        Дом и семья. Выбран checkbox Собаки");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingHouseMiscellanea = true;
                                            LogTrace.WriteInLog("        Дом и семья. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingHouseCats = true;
                                            LogTrace.WriteInLog("        Дом и семья. Выбран checkbox Кошки");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingHouseCookery = true;
                                            LogTrace.WriteInLog("        Дом и семья. Выбран checkbox Кулинария, рецепты");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingHouseKindergartens = true;
                                            LogTrace.WriteInLog("        Дом и семья. Выбран checkbox Детские сады");
                                        }
                                    #endregion
                                    
                                    #region Финансы
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingFinanceChoseAll = true;
                                            LogTrace.WriteInLog("        Финансы. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingFinanceStockMarket = true;
                                            LogTrace.WriteInLog("        Финансы. Выбран checkbox Фондовый рынок");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingFinanceCurrency = true;
                                            LogTrace.WriteInLog("        Финансы. Выбран checkbox Валюта");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingFinanceInsurence = true;
                                            LogTrace.WriteInLog("        Финансы. Выбран checkbox Страхование");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingFinanceMoneyTransfers = true;
                                            LogTrace.WriteInLog("        Финансы. Выбран checkbox Денежные переводы");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingFinanceCredits = true;
                                            LogTrace.WriteInLog("        Финансы. Выбран checkbox Кредиты");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingFinanceMiscellanea = true;
                                            LogTrace.WriteInLog("        Финансы. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingFinanceDeposits = true;
                                            LogTrace.WriteInLog("        Финансы. Выбран checkbox Вклады, депозиты");
                                        }
                                    #endregion

                                    #region Компьютеры, оргтехника
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingComputersChoseAll = true;
                                            LogTrace.WriteInLog("        Компьютеры, оргтехника. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingComputersLaptops = true;
                                            LogTrace.WriteInLog("        Компьютеры, оргтехника. Выбран checkbox Ноутбуки");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingComputersParts = true;
                                            LogTrace.WriteInLog("        Компьютеры, оргтехника. Выбран checkbox Компьютерные комплектующие");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingComputersPrinters = true;
                                            LogTrace.WriteInLog("        Компьютеры, оргтехника. Выбран checkbox Принтеры");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingComputersTablets = true;
                                            LogTrace.WriteInLog("        Компьютеры, оргтехника. Выбран checkbox Планшетные ПК");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingComputersMonitors = true;
                                            LogTrace.WriteInLog("        Компьютеры, оргтехника. Выбран checkbox Мониторы");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingComputersMiscellanea = true;
                                            LogTrace.WriteInLog("        Компьютеры, оргтехника. Выбран checkbox Разное");
                                        }
                                    #endregion

                                    #region Авто
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingAutoChoseAll = true;
                                            LogTrace.WriteInLog("        Авто. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingAutoInsurence = true;
                                            LogTrace.WriteInLog("        Авто. Выбран checkbox Автострахование");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingAutoMiscellanea = true;
                                            LogTrace.WriteInLog("        Авто. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingAutoNational = true;
                                            LogTrace.WriteInLog("        Авто. Выбран checkbox Отечественные");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingAutoWheels = true;
                                            LogTrace.WriteInLog("        Авто. Выбран checkbox Колёса, Шины");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingAutoImported = true;
                                            LogTrace.WriteInLog("        Авто. Выбран checkbox Иномарки");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingAutoMoto = true;
                                            LogTrace.WriteInLog("        Авто. Выбран checkbox Мото-, Квадроциклы, Снегоходы");
                                        }
                                    #endregion

                                    #region Аудио, Видео, Фото
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingAudioChoseAll = true;
                                            LogTrace.WriteInLog("        Аудио, Видео, Фото. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingAudioVideoEquips = true;
                                            LogTrace.WriteInLog("        Аудио, Видео, Фото. Выбран checkbox Видеоаппаратура");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingAudioMiscellanea = true;
                                            LogTrace.WriteInLog("        Аудио, Видео, Фото. Выбран checkbox Разное");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingAudioTech = true;
                                            LogTrace.WriteInLog("        Аудио, Видео, Фото. Выбран checkbox Аудио-техника");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingAudioCameras = true;
                                            LogTrace.WriteInLog("        Аудио, Видео, Фото. Выбран checkbox Фотоаппараты");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.InterestsTargetingAudioTvs = true;
                                            LogTrace.WriteInLog("        Аудио, Видео, Фото. Выбран checkbox Телевизоры, DVD-проигрыватели");
                                        }
                                    #endregion
                                    break;
                                }
                        }
                    #endregion

                    #region Radiobutton Браузеры
                        variant = needSetRadioButton(2).ToString();
                        pkModel.BrowserTargeting = variant;
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
                                    pkModel.BrowserTargetingOtherExpand = true;
                                    pkModel.BrowserTargetingOperaExpand = true;
                                    pkModel.BrowserTargetingChromeExpand = true;
                                    pkModel.BrowserTargetingFirefoxExpand = true;
                                    pkModel.BrowserTargetingSafariExpand = true;
                                    pkModel.BrowserTargetingIeExpand = true;
                                    //pkModel.BrowserTargetingGoogleChromeMobileExpand = true;

                                    pkModel.BrowserTargetingOtherAll = true;

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
                                        pkModel.BrowserTargetingOperaOther = true;
                                        LogTrace.WriteInLog("        Опера. Выбран checkbox Другие");
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingOperaChoseAll = true;
                                            LogTrace.WriteInLog("        Опера. Выбран checkbox Все");
                                        }
                                        //if (needSetCheckBox()) pkModel.BrowserTargetingOperaOther = true;
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingOpera10 = true;
                                            LogTrace.WriteInLog("        Опера. Выбран checkbox 10");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingOpera11 = true;
                                            LogTrace.WriteInLog("        Опера. Выбран checkbox 11");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingOperaMini = true;
                                            LogTrace.WriteInLog("        Опера. Выбран checkbox Mini");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingOperaMobile = true;
                                            LogTrace.WriteInLog("        Опера. Выбран checkbox Mobile");
                                        }
                                    #endregion

                                    #region Chrome
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingChromeChoseAll = true;
                                            LogTrace.WriteInLog("        Chrome. Выбран checkbox Chrome Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingChromeAll = true;
                                            LogTrace.WriteInLog("        Chrome. Выбран checkbox Все");
                                        }
                                    #endregion

                                    #region Firefox
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingFirefoxChoseAll = true;
                                            LogTrace.WriteInLog("        Firefox. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingFirefox3 = true;
                                            LogTrace.WriteInLog("        Firefox. Выбран checkbox 3");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingFirefox4 = true;
                                            LogTrace.WriteInLog("        Firefox. Выбран checkbox 4");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingFirefox5 = true;
                                            LogTrace.WriteInLog("        Firefox. Выбран checkbox 5");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingFirefox6 = true;
                                            LogTrace.WriteInLog("        Firefox. Выбран checkbox 6");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingFirefoxOther = true;
                                            LogTrace.WriteInLog("        Firefox. Выбран checkbox Другие");
                                        }
                                    #endregion

                                    #region Safari
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingSafariChoseAll = true;
                                            LogTrace.WriteInLog("        Safari. Выбран checkbox Safari Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingSafariAll = true;
                                            LogTrace.WriteInLog("        Safari. Выбран checkbox Все");
                                        }
                                    #endregion

                                    #region MSIE
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingIeChoseAll = true;
                                            LogTrace.WriteInLog("        MSIE. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingIe6 = true;
                                            LogTrace.WriteInLog("        MSIE. Выбран checkbox 6");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingIe7 = true;
                                            LogTrace.WriteInLog("        MSIE. Выбран checkbox 7");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingIe8 = true;
                                            LogTrace.WriteInLog("        MSIE. Выбран checkbox 8");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingIe9 = true;
                                            LogTrace.WriteInLog("        MSIE. Выбран checkbox 9");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.BrowserTargetingIeOther = true;
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
                        pkModel.OsTargeting = variant;
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
                                    pkModel.OsTargetingOther = true;
                                    LogTrace.WriteInLog("        Выбран checkbox Другие");
                                    //if (needSetCheckBox()) pkModel.OsTargetingOther = true;
                                    if (needSetCheckBox())
                                    {
                                        pkModel.OsTargetingMacOs = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Mac OS");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        pkModel.OsTargetingOtherMobileOs = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Прочие мобильные ОС");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        pkModel.OsTargetingWindows = true;
                                        LogTrace.WriteInLog("        Выбран checkbox WIndows");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        pkModel.OsTargetingOtherIoS = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Прочие iOS системы");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        pkModel.OsTargetingIpad = true;
                                        LogTrace.WriteInLog("        Выбран checkbox iPAD");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        pkModel.OsTargetingIphone = true;
                                        LogTrace.WriteInLog("        Выбран checkbox IPHONE");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        pkModel.OsTargetingAndroid = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Android");
                                    }
                                    break;
                                }
                        }
                    #endregion

                    #region Radiobutton Провайдеры
                        variant = needSetRadioButton(2).ToString();
                        pkModel.ProviderTargeting = variant;
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
                                    pkModel.ProviderTargetingOther = true;
                                    LogTrace.WriteInLog("        Выбран checkbox Другие");
                                    //if (needSetCheckBox()) pkModel.ProviderTargetingOther = true;
                                    if (needSetCheckBox())
                                    {
                                        pkModel.ProviderTargetingMegafon = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Мегафон");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        pkModel.ProviderTargetingMtc = true;
                                        LogTrace.WriteInLog("        Выбран checkbox МТС Россия");
                                    }
                                    break;
                                }
                        }
                    #endregion

                    #region Radiobutton Геотаргетинг
                        variant = needSetRadioButton(2).ToString();
                        pkModel.GeoTargeting = variant;
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
                                    pkModel.GeoTargetingRussiaExpand = true;
                                    pkModel.GeoTargetingUkraineExpand = true;

                                    pkModel.GeoTargetingOther = true;
                                    LogTrace.WriteInLog("        Выбран checkbox Прочие страны");
                                    //if (needSetCheckBox()) pkModel.GeoTargetingOther = true;
                                    if (needSetCheckBox())
                                    {
                                        pkModel.GeoTargetingAustria = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Австрия");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        pkModel.GeoTargetingBelorussia = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Белоруссия");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        pkModel.GeoTargetingUk = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Великобритания");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        pkModel.GeoTargetingGermany = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Германия");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        pkModel.GeoTargetingIsrael = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Израиль");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        pkModel.GeoTargetingKazakhstan = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Казахстан");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        pkModel.GeoTargetingLatvia = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Латвия");
                                    }
                                    if (needSetCheckBox())
                                    {
                                        pkModel.GeoTargetingLithuania = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Литва");
                                    }

                                    #region Россия
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingRussiaChoseAll = true;
                                            LogTrace.WriteInLog("        Россия. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingRussiaEburg = true;
                                            LogTrace.WriteInLog("        Россия. Выбран checkbox Екатеринбург");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingRussiaMoscow = true;
                                            LogTrace.WriteInLog("        Россия. Выбран checkbox Москва");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingRussiaNovosibirsk = true;
                                            LogTrace.WriteInLog("        Россия. Выбран checkbox Новосибирск");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingRussiaOther = true;
                                            LogTrace.WriteInLog("        Россия. Выбран checkbox Прочие регионы");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingRussiaSpb = true;
                                            LogTrace.WriteInLog("        Россия. Выбран checkbox Санкт-Петербург");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingRussiaHabarovsk = true;
                                            LogTrace.WriteInLog("        Россия. Выбран checkbox Хабаровск");
                                        }
                                    #endregion

                                    if (needSetCheckBox())
                                    {
                                        pkModel.GeoTargetingUsa = true;
                                        LogTrace.WriteInLog("        Выбран checkbox США");
                                    }

                                    #region Украина
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingUkraineChoseAll = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Все");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingUkraineDnepr = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Днепропетровск");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingUkraineDonetzk = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Донецк");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingUkraineZakarpattya = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Закарпатье");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingUkraineKiev = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Киев");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingUkraineCrimea = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Крым");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingUkraineLvov = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Львов");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingUkraineNikolaev = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Николаев");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingUkraineOdessa = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Одесса");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingUkraineOther = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Прочие области");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingUkraineHarkov = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Харьков");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingUkraineHerson = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Херсон");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingUkraineCherkassy = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Черкассы");
                                        }
                                        if (needSetCheckBox())
                                        {
                                            pkModel.GeoTargetingUkraineChernovzi = true;
                                            LogTrace.WriteInLog("        Украина. Выбран checkbox Черновцы");
                                        }
                                    #endregion

                                    if (needSetCheckBox())
                                    {
                                        pkModel.GeoTargetingEstonia = true;
                                        LogTrace.WriteInLog("        Выбран checkbox Эстония");
                                    }
                                    break;
                                }
                        }
                    #endregion
                }

            #endregion


            string parentWindow = driver.CurrentWindowHandle;//запоминаем текущее (родительское окно)

            string createPkUrl = driver.Url; //запоминаем url страницы "Добавление РК"
            pkModel.Submit(); //пытаемся сохранить форму
            LogTrace.WriteInLog("     Нажал кнопку Завершить");
            string isCreatedPkUrl = driver.Url; //запоминаем url страницы, открывшейся после нажатия "Завершить"
            //если createPkUrl и isCreatedPkUrl совпали - мы никуда не перешли и значит есть ошибки заполнения полей
            //если createPkUrl и isCreatedPkUrl не совпали - РК создалась и ошибки искать не надо
            if (createPkUrl == isCreatedPkUrl)
            {
                errors.Add(pkModel.GetErrors().ToString()); //проверяем, появились ли на форме ошибки заполнения полей
            }
            else
            {
                //т.к. после успешного создания РК нас перебрасывает на страницу Клиента (а не РК)
                //поэтому, чтобы получить ID РК, находим на странице название только что созданной РК
                // и переходим по этому названию-ссылке на страницу РК
                // открывается новое окно с РК - мы должны на него переключиться
                // и там получаем ID РК из URL
                if(driver.PageSource.Contains(pkName))
                {
                    IWebElement webelement = driver.FindElement(By.LinkText(pkName));
                    string href = webelement.GetAttribute("href");
                    
                    char[] slash = new char[] { '/' };
                    string[] url = href.Split(slash); //разбиваем URL по /
                    pkId = url[url.Length - 1]; //берем последний элемент массива - это id новой РК
                    clientId = Registry.hashTable["clientId"].ToString(); //берется для вывода в listBox и логи
                    LogTrace.WriteInLog("     " + driver.Url);
                }
            }
            Registry.hashTable["pkId"] = pkId; //запомнили глобально ID созданной РК
            Registry.hashTable["driver"] = driver;
        }

        protected bool needSetCheckBox() //генерируем 0 или 1.  1 - заполняем необязательное поле, 0 - не заполняем
        {
            Random rnd = new Random();
            return rnd.Next(0, 2) == 1 ? true : false;
        }

        protected int needSetRadioButton(int variants) //генерируем номер варианта выбора для needSetRadioButton. variants - кол-во вариантов выбора
        {
            Random rnd = new Random();
            return rnd.Next(0, variants);
        }
    }
}
