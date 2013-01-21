using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using Task.Model;
using Task.Utils;

namespace Task.Controller
{
    public class GoodsCreateClient_Controller
    {
        IWebDriver driver;
        public string baseUrl = "https://admin.dt00.net/cab/goodhits/clients-new";
        public List<string> errors = new List<string>(); //список ошибок (в каждой строке - спарсенное со страницы описание ошибки)
        public string clientId; //переменная для хранения ID только что созданного клиента
        public string login; //переменная для хранения логина только что созданного клиента
        public string password; //переменная для хранения пароля только что созданного клиента
        Randoms randoms = new Randoms();//класс генерации случайных строк

        public void InitDriver()
        {
            //FirefoxBinary FireBin = new FirefoxBinary("C:\\Program Files (x86)\\Mozilla Firefox\\firefox.exe");
            //FireBin.TimeoutInMilliseconds = 130000;
            //FirefoxProfile FireProfile = new FirefoxProfile(@"C:\Users\User\AppData\Roaming\Mozilla\Firefox\Profiles\d1j6msfx.FF");
            //FireProfile.Port = 9966;
            //driver = new FirefoxDriver(FireBin, FireProfile);

            driver = new FirefoxDriver();
        }

        public void CloseDriver()
        {
            try
            {
                driver.Close();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            //Assert.AreEqual("", verificationErrors.ToString());
        }

        public void CreateClient(string FileName, bool setNecessaryFields, bool setUnnecessaryFields)
        {
            List<FileData> CsvStruct = new List<FileData>();
            CsvStruct = FileData.ReadFile(FileName);

            InitDriver();
            driver.Navigate().GoToUrl(baseUrl); //заходим по ссылке
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            //ставим ожидание в 10 сек на случай, если страница медленно грузится и нужные эл-ты появятся не сразу

            Authorization auth = new Authorization();
            auth.driver = driver;
            auth.Login = CsvStruct[0].item;
            auth.Password = CsvStruct[1].item;
            Registry.hashTable["Login"] = CsvStruct[0].item;
            Registry.hashTable["Password"] = CsvStruct[1].item;
            auth.Submit();
            
            GoodsCreateClient_Model clientModel = new GoodsCreateClient_Model();
            clientModel.driver = driver;

            LogTrace.WriteInLog(driver.Url);

            #region Necessary fields
                if (setNecessaryFields) //выбрано заполнение обязательных полей
                {
                    LogTrace.WriteInLog("...Заполняю обязательные поля...");

                    password = randoms.RandomString(10);
                    clientModel.Password = password;
                    LogTrace.WriteInLog("Заполняю поле Пароль. Было введено: " + clientModel.Password);

                    login = "SOK_auto_aded_client_" + randoms.RandomString(5);
                    clientModel.Login = login;
                    LogTrace.WriteInLog("Заполняю поле Логин. Было введено: " + clientModel.Login);

                    clientModel.UserName = randoms.RandomString(10);
                    LogTrace.WriteInLog("Заполняю поле ФИО. Было введено: " + clientModel.UserName);

                    clientModel.Email = randoms.RandomString(5) + "@" + randoms.RandomString(5) + "." + "com";
                    LogTrace.WriteInLog("Заполняю поле E-Mail. Было введено: " + clientModel.Email);

                    clientModel.TestClient = true;
                    LogTrace.WriteInLog("Выбран checkbox Тестовый клиент");
                }

            #endregion

            #region Not necessary fields
                if (setUnnecessaryFields) //выбрано заполнение также необязательных полей
                {
                    LogTrace.WriteInLog("...Заполняю необязательные поля...");
                    Random rnd = new Random();

                    if (needSet())
                    {
                        clientModel.Phone = randoms.RandomNumber(10);
                        LogTrace.WriteInLog("Заполняю поле Телефон. Было введено: " + clientModel.Phone);
                    }
                    if(needSet())
                    {
                        clientModel.Skype = randoms.RandomString(5) + randoms.RandomNumber(5);
                        LogTrace.WriteInLog("Заполняю поле Skype. Было введено: " + clientModel.Skype);
                    }
                    if(needSet())
                    {
                        clientModel.Icq = randoms.RandomNumber(5);
                        LogTrace.WriteInLog("Заполняю поле ICQ. Было введено: " + clientModel.Icq);
                    }
                    if(needSet())
                    {
                        clientModel.ExchangeInCabinet = true;
                        LogTrace.WriteInLog("Выбран checkbox Обмен в кабинете");
                    }
                    if(needSet())
                    {
                        clientModel.NewsInCabinet = true;
                        LogTrace.WriteInLog("Выбран checkbox Новости в кабинете");
                    }
                    
                    if(needSet())
                    {
                        clientModel.LimitNumOfCampaigns = randoms.RandomNumber(2);
                        LogTrace.WriteInLog("Заполняю поле Ограничение по количеству кампаний");
                    }
                    if(needSet())
                    {
                        clientModel.AllowViewFilterByPlatform = true;
                        LogTrace.WriteInLog("Выбран checkbox Разрешен просмотр фильтра по площадкам");
                    }
                    if(needSet())
                    {
                        clientModel.Comments = randoms.RandomString(20);
                        LogTrace.WriteInLog("Заполняю поле Комментарий");
                    }
                }
                //пропущены поля "Ограничение на кол-во создаваемых тизеров в сутки каждой РК", "Отображение сводной статистики трат", "Подсеть"
            #endregion

            string createClientUrl = driver.Url; //запоминаем url страницы "Добавление клиента"
            clientModel.Submit(); //пытаемся сохранить форму
            LogTrace.WriteInLog("Нажал кнопку Сохранить");
            string isCreatedClientUrl = driver.Url; //запоминаем url страницы, открывшейся после нажатия "Завершить"
            //если createClientUrl и isCreatedClientUrl совпали - мы никуда не перешли и значит есть ошибки заполнения полей
            //если createClientUrl и isCreatedClientUrl не совпали - клиент создался и ошибки искать не надо
            if (createClientUrl == isCreatedClientUrl)
            {
                errors.Add(clientModel.GetErrors().ToString()); //проверяем, появились ли на форме ошибки заполнения полей
            }
            else
            {
                //if (errors.Count == 0)
                //если нет ошибок - значит клиент успешно создался и мы перешли на страницу с инфо о созданном клиенте
                //{
                char[] slash = new char[] { '/' };
                string[] url = driver.Url.Split(slash); //разбиваем URL по /
                clientId = url[url.Length - 1]; //берем последний элемент массива - это id нового клиента
                Registry.hashTable["clientId"] = clientId; //глобально запомнили ID клиента
                //}
            }
            //Registry.hashTable.Add("driver", driver); //записываем в хештаблицу driver и его состояние, чтобы потом извлечь и использовать его при создании сайта/РК
            Registry.hashTable["driver"] = driver;
            LogTrace.WriteInLog(driver.Url);
        }

        protected bool needSet() //генерируем 0 или 1.  1 - заполняем необязательное поле, 0 - не заполняем
        {
            Random rnd = new Random();
            return rnd.Next(0, 2) == 1 ? true : false;
        }
    }

}
