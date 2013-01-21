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

namespace Task.Controller
{
    public class GoodsCreateTeaser_Controller
    {
        IWebDriver driver;
        public string baseUrl = "https://" + Registry.hashTable["Login"] + ":" + Registry.hashTable["Password"] + "@" + "admin.dt00.net/cab/goodhits/ghits-add/campaign_id/" + Registry.hashTable["pkId"] + "/filters/client_id/" + Registry.hashTable["clientId"];

        public List<string> errors = new List<string>(); //список ошибок (в каждой строке - спарсенное со страницы описание ошибки)
        Randoms randoms = new Randoms();//класс генерации случайных строк
        public string teaserId;
        public string clientId;
        public string pkId;
        public string allowedDomain;

        public void CreateTeaser(bool setNecessaryFields, bool setUnnecessaryFields)
        {
            driver = (IWebDriver) Registry.hashTable["driver"];//забираем из хештаблицы сохраненный при создании РК драйвер
            allowedDomain = GetAnyAllowedDomain();//сначала выбираем 1 из разрешенных доменов
            driver.Navigate().GoToUrl(baseUrl); //заходим по ссылке
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            //ставим ожидание в 10 сек на случай, если страница медленно грузится и нужные эл-ты появятся не сразу

            GoodsCreateTeaser_Model teaserModel = new GoodsCreateTeaser_Model();
            teaserModel.driver = driver;

            LogTrace.WriteInLog("     " + driver.Url);
            
            #region Required fields
                if (setNecessaryFields) //выбрано заполнение обязательных полей
                {
                    Thread.Sleep(5000);

                    LogTrace.WriteInLog("     ...Заполняю обязательные поля...");

                    teaserModel.Link = allowedDomain;
                    LogTrace.WriteInLog("     Заполняю поле Ссылка. Было введено: " + teaserModel.Link);

                    teaserModel.Title = randoms.RandomString(10);
                    LogTrace.WriteInLog("     Заполняю поле Заголовок. Было введено: " + teaserModel.Title);

                    Random rnd=new Random();
                    int category = rnd.Next(1, 31);
                    teaserModel.Category = category;
                    LogTrace.WriteInLog("     Работаю с выпадающим списком Категория. Было выбрано: " + teaserModel.chosenCategory);

                    teaserModel.AdvertText = randoms.RandomString(20);
                    LogTrace.WriteInLog("     Заполняю поле Рекламный текст. Было введено: " + teaserModel.AdvertText);

                    rnd = new Random();
                    int price = rnd.Next(5, 11);
                    teaserModel.PriceForClick = price.ToString();         
                    LogTrace.WriteInLog("     Заполняю поле Цена за клик, центы. Было введено: " + teaserModel.PriceForClick);

                    string dir = Directory.GetCurrentDirectory();
                    teaserModel.AttachFile = dir + @"\hare.jpg";
                    Thread.Sleep(8000);
                    LogTrace.WriteInLog("     Работаю с полем загрузки Фото. Было загружено: " + teaserModel.AttachFile);
                }
            #endregion

            #region Unrequired fields
                if(setUnnecessaryFields) //выбрано заполнение необязательных полей
                {
                    LogTrace.WriteInLog("     ...Заполняю необязательные поля...");

                    if (needSet())
                    {
                        teaserModel.ScrewBeginsAt = true;
                        //Thread.Sleep(5000);
                        LogTrace.WriteInLog("     Заполняю всплывающее окно 'Открутка начнется с'");
                        teaserModel.Date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                        LogTrace.WriteInLog("     Заполняю поле Дата. Было введено: " + teaserModel.Date);
                        //Thread.Sleep(5000);
                        teaserModel.Time = DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                        teaserModel.Time = DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                        LogTrace.WriteInLog("     Заполняю поле Время. Было введено: " + teaserModel.Time);
                        //Thread.Sleep(5000);
                        teaserModel.ButtonApply = true;
                        LogTrace.WriteInLog("     Жму кнопку Применить");
                    }

                    if (needSet())
                    {
                        teaserModel.TeaserWomen = true;
                        LogTrace.WriteInLog("     Выбран checkbox Тизер женской тематики (если таковая есть для выбранной Категории)");
                    }

                    if (needSet())
                    {
                        Random rnd = new Random();
                        int currency = rnd.Next(0, 6);
                        teaserModel.Currency = currency;
                        LogTrace.WriteInLog("     Работаю с выпадающим списком Валюта. Было выбрано: " + teaserModel.chosenCurrency);
                    }

                    if (needSet())
                    {
                        Random rnd = new Random();
                        int price = rnd.Next(1, 11);
                        teaserModel.PriceForGoodsService = price.ToString();
                        LogTrace.WriteInLog("     Заполняю поле Цена товара/услуги. Было введено: " + teaserModel.PriceForGoodsService);
                    }
                }
            #endregion

            string createTeaserUrl = driver.Url; //запоминаем url страницы "Добавление тизера"
            //Thread.Sleep(5000);
            teaserModel.Submit(); //пытаемся сохранить форму
            //Thread.Sleep(5000);
            LogTrace.WriteInLog("     Нажал кнопку Сохранить");
            string isCreatedTeaserUrl = driver.Url; //запоминаем url страницы, открывшейся после нажатия "Cохранить"
            //если createSitetUrl и isCreatedSiteUrl совпали - мы никуда не перешли и значит есть ошибки заполнения полей
            //если createSitetUrl и isCreatedSiteUrl не совпали - сайт создался и ошибки искать не надо
            if (createTeaserUrl == isCreatedTeaserUrl)
            {
                errors = teaserModel.GetErrors(); //проверяем, появились ли на форме ошибки заполнения полей
            }
            else
            {
                //если нет ошибок - значит тизер успешно создался и мы перешли на страницу с инфо о созданном тизере
                List<IWebElement> list = driver.FindElements(By.CssSelector("tbody td.info-column")).ToList(); //список ячеек 1го столбца таблицы

                IWebElement webElement = list[0]; //взяли только 0й - только что созданный

                string[] split = new string[] { "\r\n" }; 
                string[] elements = webElement.Text.Split(split, StringSplitOptions.RemoveEmptyEntries); //разбиваем строку по \r\n 

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
                teaserId = idFinderMas[1]; //отбрасываем 0й эл-т (фразу "ID тизера") и берем только 1й эл-т (цифры)
                clientId = Registry.hashTable["clientId"].ToString(); //берется для вывода в listBox и логи
                pkId = Registry.hashTable["pkId"].ToString(); //берется для вывода в listBox и логи

                //нужно заблокировать созданный тизер, чтоб он не пошел в открутку
                string ban = string.Concat("buttonBlock", teaserId);
                ban = ban.Replace(" ", "");
                try
                {
                    webElement = driver.FindElement(By.Id(ban)); //находим зеленый кружок
                    webElement.Click(); //блокируем кликом на зеленый кружок
                    LogTrace.WriteInLog("     Блокирую тизер. id элемента: " + ban);

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5)); //явное ожидание
                    string banned = string.Concat("buttonUnblock", teaserId);
                    banned = banned.Replace(" ", "");
                    webElement = wait.Until(ExpectedConditions.ElementExists(By.Id(banned)));
                }
                
                catch(Exception)
                {
                    LogTrace.WriteInLog("ОШИБКА:");
                    LogTrace.WriteInLog("     Не удалось заблокировать тизер");
                }
            }
            Registry.hashTable["driver"] = driver;
            LogTrace.WriteInLog("     " + driver.Url);
        }

        protected bool needSet() //генерируем 0 или 1.  1 - заполняем необязательное поле, 0 - не заполняем
        {
            Random rnd = new Random();
            return rnd.Next(0, 2) == 1 ? true : false;
        }

        protected string GetAnyAllowedDomain() //функция, возвращающая 1 из разрешенных доменов для данного клиента
        {
            string domain;
            driver.Navigate().GoToUrl("https://admin.dt00.net/cab/goodhits/sites/client_id/" + Registry.hashTable["clientId"]); //идем на страницу с сайтами для данного клиента
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            List<IWebElement> listDomains = driver.FindElements(By.CssSelector("td[nowrap='nowrap']>a")).ToList(); //получаем список элементов-доменов, из которых нужно извлечь ссылку

            Random rnd = new Random();
            int randomIndex = rnd.Next(0, listDomains.Count); //генерируем индекс для списка элементов-доменов
            domain = listDomains[randomIndex].GetAttribute("href"); //извлекаем ссылку
            return domain;
        }
    }
}
