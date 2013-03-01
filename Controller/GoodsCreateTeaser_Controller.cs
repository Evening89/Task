using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Task.Model;
using Task.Utils;
using System.IO;
using System.Net;
using Task.View;

namespace Task.Controller
{
    public class GoodsCreateTeaserController
    {
        private IWebDriver _driver;
        private readonly string _baseUrl = "https://" + Registry.hashTable["Login"] + ":" + Registry.hashTable["Password"] + "@" + "admin.dt00.net/cab/goodhits/ghits-add/campaign_id/" + Registry.hashTable["pkId"] + "/filters/client_id/" + Registry.hashTable["clientId"];
        private GoodsCreateTeaser_Model _teaserModel;
        private IWebElement _webElement;
        private int _countElementsInList;
        private int _newCategory;
        private int _newCurrency;

        public List<string> Errors = new List<string>(); //список ошибок (в каждой строке - спарсенное со страницы описание ошибки)
        readonly Randoms _randoms = new Randoms();//класс генерации случайных строк
        public string TeaserId;
        public string ClientId;
        public string PkId;
        public string AllowedDomain;

        public void CreateTeaser(bool setNecessaryFields, bool setUnnecessaryFields)
        {
            GetDriver();
            SetUpFields(setNecessaryFields, setUnnecessaryFields);
            CreationIsSuccessful();
        }

        private void GetDriver()
        {
            _driver = (IWebDriver)Registry.hashTable["driver"];//забираем из хештаблицы сохраненный при создании РК драйвер
            AllowedDomain = GetAnyAllowedDomain();//сначала выбираем 1 из разрешенных доменов
            _driver.Navigate().GoToUrl(_baseUrl); //заходим по ссылке
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            //ставим ожидание в 10 сек на случай, если страница медленно грузится и нужные эл-ты появятся не сразу
        }

        private void SetUpFields(bool setNecessaryFields, bool setUnnecessaryFields)
        {
            _teaserModel = new GoodsCreateTeaser_Model();
            _teaserModel.driver = _driver;

            LogTrace.WriteInLog(Goods_View.tab1 + _driver.Url);

            #region Required fields
                if (setNecessaryFields) //выбрано заполнение обязательных полей
                {
                    Thread.Sleep(5000);

                    LogTrace.WriteInLog(Goods_View.tab1 + "...Заполняю обязательные поля...");

                    _teaserModel.Link = AllowedDomain;
                    LogTrace.WriteInLog(Goods_View.tab1 + "Заполняю поле Ссылка. Было введено: " + _teaserModel.Link);

                    _teaserModel.Title = _randoms.RandomString(10);
                    LogTrace.WriteInLog(Goods_View.tab1 + "Заполняю поле Заголовок. Было введено: " + _teaserModel.Title);


                    _countElementsInList = _teaserModel.QuantityItemsInList(_teaserModel.locatorCategory);
                    Random rand = new Random();
                    _newCategory = rand.Next(1, _countElementsInList);
                    _teaserModel.Category = _newCategory;
                    LogTrace.WriteInLog(Goods_View.tab1 + "Работаю с выпадающим списком Категория. Было выбрано: " + _teaserModel.chosenCategory);

                    _teaserModel.AdvertText = _randoms.RandomString(20);
                    LogTrace.WriteInLog(Goods_View.tab1 + "Заполняю поле Рекламный текст. Было введено: " + _teaserModel.AdvertText);

                    int price = rand.Next(5, 11);
                    _teaserModel.PriceForClick = price.ToString();
                    LogTrace.WriteInLog(Goods_View.tab1 + "Заполняю поле Цена за клик, центы. Было введено: " + _teaserModel.PriceForClick);

                    string dir = Directory.GetCurrentDirectory();
                    _teaserModel.AttachFile = dir + @"\hare.jpg";
                    Thread.Sleep(8000);
                    LogTrace.WriteInLog(Goods_View.tab1 + "Работаю с полем загрузки Фото. Было загружено: " + _teaserModel.AttachFile);
                }
            #endregion

            #region Unrequired fields
                if (setUnnecessaryFields) //выбрано заполнение необязательных полей
                {
                    LogTrace.WriteInLog(Goods_View.tab1 + "...Заполняю необязательные поля...");

                    if (needSet())
                    {
                        _teaserModel.ScrewBeginsAt = true;
                        //Thread.Sleep(5000);
                        LogTrace.WriteInLog(Goods_View.tab1 + "Заполняю всплывающее окно 'Открутка начнется с'");
                        _teaserModel.Date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                        LogTrace.WriteInLog(Goods_View.tab1 + "Заполняю поле Дата. Было введено: " + _teaserModel.Date);
                        //Thread.Sleep(5000);
                        _teaserModel.Time = DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                        _teaserModel.Time = DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                        LogTrace.WriteInLog(Goods_View.tab1 + "Заполняю поле Время. Было введено: " + _teaserModel.Time);
                        //Thread.Sleep(5000);
                        _teaserModel.ButtonApply = true;
                        LogTrace.WriteInLog(Goods_View.tab1 + "Жму кнопку Применить");
                    }

                    if (needSet())
                    {
                        _teaserModel.TeaserWomen = true;
                        LogTrace.WriteInLog(Goods_View.tab1 + "Выбран checkbox Тизер женской тематики (если таковая есть для выбранной Категории)");
                    }

                    if (needSet())
                    {
                        _countElementsInList = _teaserModel.QuantityItemsInList(_teaserModel.locatorCurrency);
                        Random rnd = new Random();
                        _newCurrency = rnd.Next(0, _countElementsInList);
                        _teaserModel.Currency = _newCurrency;
                        LogTrace.WriteInLog(Goods_View.tab1 + "Работаю с выпадающим списком Валюта. Было выбрано: " + _teaserModel.chosenCurrency);
                    }

                    if (needSet())
                    {
                        Random rnd = new Random();
                        int price = rnd.Next(1, 11);
                        _teaserModel.PriceForGoodsService = price.ToString();
                        LogTrace.WriteInLog(Goods_View.tab1 + "Заполняю поле Цена товара/услуги. Было введено: " + _teaserModel.PriceForGoodsService);
                    }
                }
            #endregion
        }

        private void CreationIsSuccessful()
        {
            string createTeaserUrl = _driver.Url; //запоминаем url страницы "Добавление тизера"
            _teaserModel.Submit(); //пытаемся сохранить форму
            LogTrace.WriteInLog(Goods_View.tab1 + "Нажал кнопку Сохранить");

            if (_driver.Url == createTeaserUrl)
                Errors.Add(_teaserModel.GetErrors().ToString()); //проверяем, появились ли на форме ошибки заполнения полей
                //Errors = _teaserModel.GetErrors(); //проверяем, появились ли на форме ошибки заполнения полей
            else
            {
                TeaserId = GetTeaserIdFromPage();
                Registry.hashTable["teaserId"] = TeaserId;
                ClientId = Registry.hashTable["clientId"].ToString(); //берется для вывода в listBox и логи
                PkId = Registry.hashTable["pkId"].ToString(); //берется для вывода в listBox и логи

                BlockTeaserById(TeaserId);
            }
            Registry.hashTable["driver"] = _driver;
            LogTrace.WriteInLog(Goods_View.tab1 + _driver.Url);
        }

        private string GetTeaserIdFromPage()
        {
            List<IWebElement> list = _driver.FindElements(By.CssSelector("tbody td.info-column")).ToList(); //список ячеек 1го столбца таблицы

            _webElement = list[0]; //взяли только 0й - только что созданный

            string[] split = new string[] { "\r\n" };
            string[] elements = _webElement.Text.Split(split, StringSplitOptions.RemoveEmptyEntries); //разбиваем строку по \r\n 

            string idFinder = "";
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].Contains("ID тизера")) //ищем элемент массива, содержащий подстроку "ID тизера"
                {
                    idFinder = elements[i]; //нашли например "ID тизера:32452"
                    break;
                }
            }
            split = new[] { ":" };
            string[] idFinderMas = idFinder.Split(split, StringSplitOptions.RemoveEmptyEntries);//разбиваем строку по :
            string  teaserId = idFinderMas[1].Trim(); //отбрасываем 0й эл-т (фразу "ID тизера") и берем только 1й эл-т (цифры)

            return teaserId;
        }

        private void BlockTeaserById(string teaserId)
        {
            //нужно заблокировать созданный тизер, чтоб он не пошел в открутку
            string ban = string.Concat("buttonBlock", TeaserId);
            ban = ban.Replace(" ", "");
            try
            {
                _webElement = _driver.FindElement(By.Id(ban)); //находим зеленый кружок
                _webElement.Click(); //блокируем кликом на зеленый кружок
                LogTrace.WriteInLog(Goods_View.tab1 + "Блокирую тизер. id элемента: " + ban);

                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5)); //явное ожидание
                string banned = string.Concat("buttonUnblock", TeaserId);
                banned = banned.Replace(" ", "");
                _webElement = wait.Until(ExpectedConditions.ElementExists(By.Id(banned)));
            }

            catch (Exception)
            {
                Errors.Add("ОШИБКА: Не удалось заблокировать тизер");
                LogTrace.WriteInLog(Goods_View.tab3 + "ОШИБКА:");
                LogTrace.WriteInLog(Goods_View.tab3 + "Не удалось заблокировать тизер");
            }
        }

        private bool needSet() //генерируем 0 или 1.  1 - заполняем необязательное поле, 0 - не заполняем
        {
            Random rnd = new Random();
            return rnd.Next(0, 2) == 1 ? true : false;
        }

        private string GetAnyAllowedDomain() //функция, возвращающая 1 из разрешенных доменов для данного клиента
        {
            string domain;
            _driver.Navigate().GoToUrl("https://admin.dt00.net/cab/goodhits/sites/client_id/" + Registry.hashTable["clientId"]); //идем на страницу с сайтами для данного клиента
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            List<IWebElement> listDomains = _driver.FindElements(By.CssSelector("td[nowrap='nowrap']>a")).ToList(); //получаем список элементов-доменов, из которых нужно извлечь ссылку

            Random rnd = new Random();
            int randomIndex = rnd.Next(0, listDomains.Count); //генерируем индекс для списка элементов-доменов
            domain = listDomains[randomIndex].GetAttribute("href"); //извлекаем ссылку
            return domain;
        }
    }
}
