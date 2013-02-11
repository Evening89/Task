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
    public class GoodsCreateClientController
    {
        private IWebDriver _driver;
        private GoodsCreateClient_Model _clientModel;
        private const string BaseUrl = "https://admin.dt00.net/cab/goodhits/clients-new";

        public List<string> Errors = new List<string>(); //список ошибок (в каждой строке - спарсенное со страницы описание ошибки)
        public string ClientId; //переменная для хранения ID только что созданного клиента
        public string Login; //переменная для хранения логина только что созданного клиента
        public string Password; //переменная для хранения пароля только что созданного клиента
        readonly Randoms _randoms = new Randoms();//класс генерации случайных строк
        
        private void InitDriver()
        {
            //FirefoxBinary FireBin = new FirefoxBinary("C:\\Program Files (x86)\\Mozilla Firefox\\firefox.exe");
            //FireBin.TimeoutInMilliseconds = 130000;
            //FirefoxProfile FireProfile = new FirefoxProfile(@"C:\Users\User\AppData\Roaming\Mozilla\Firefox\Profiles\d1j6msfx.FF");
            //FireProfile.Port = 9966;
            //driver = new FirefoxDriver(FireBin, FireProfile);

            _driver = new FirefoxDriver(); 
            _driver.Navigate().GoToUrl(BaseUrl); //заходим по ссылке
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));//ставим ожидание в 10 сек на случай, если страница медленно грузится и нужные эл-ты появятся не сразу
        }

        public void CloseDriver()
        {
            try
            {
                _driver.Close();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

        public void CreateClient(string fileName, bool setNecessaryFields, bool setUnnecessaryFields)
        {
            LoginAdminPanel(fileName);
            SetUpFields(setNecessaryFields, setUnnecessaryFields);
            CreationIsSuccessful();
        }

        private void LoginAdminPanel(string fileName)
        {
            InitDriver();
            Authorization(fileName); //проходим авторизацию (доступы берутся из файла)
        }

        private void Authorization(string fileName)
        {
            Credentials credentials = GetUserCredentialFromFile(fileName);//читаем доступы из файла
            Authorization auth = new Authorization(_driver, credentials);
            PushCredentialsToHash(credentials);
            auth.Submit();
        }

        private Credentials GetUserCredentialFromFile(string fileName)
        {
            List<FileData> csvStruct = FileData.ReadFile(fileName);
            Credentials credentials = new Credentials(csvStruct[0].item, csvStruct[1].item);
            return credentials;
        }

        private void PushCredentialsToHash(Credentials credentials)
        {
            Registry.hashTable["Login"] = credentials.Login;
            Registry.hashTable["Password"] = credentials.Password;
        }

        private void SetUpFields(bool setNecessaryFields, bool setUnnecessaryFields)
        {
            _clientModel = new GoodsCreateClient_Model();
            _clientModel.driver = _driver;
            LogTrace.WriteInLog(_driver.Url);

            #region Required fields
                if (setNecessaryFields) //выбрано заполнение обязательных полей
                {
                    LogTrace.WriteInLog("...Заполняю обязательные поля...");

                    Password = _randoms.RandomString(10);
                    _clientModel.Password = Password;
                    LogTrace.WriteInLog("Заполняю поле Пароль. Было введено: " + _clientModel.Password);

                    Login = "SOK_auto_aded_client_" + _randoms.RandomString(5);
                    _clientModel.Login = Login;
                    LogTrace.WriteInLog("Заполняю поле Логин. Было введено: " + _clientModel.Login);

                    _clientModel.UserName = _randoms.RandomString(10);
                    LogTrace.WriteInLog("Заполняю поле ФИО. Было введено: " + _clientModel.UserName);

                    _clientModel.Email = _randoms.RandomString(5) + "@" + _randoms.RandomString(5) + "." + "com";
                    LogTrace.WriteInLog("Заполняю поле E-Mail. Было введено: " + _clientModel.Email);

                    _clientModel.TestClient = true;
                    LogTrace.WriteInLog("Выбран checkbox Тестовый клиент");
                }
            #endregion

            #region Unrequired fields
                if (setUnnecessaryFields) //выбрано заполнение также необязательных полей
                {
                    LogTrace.WriteInLog("...Заполняю необязательные поля...");
                    Random rnd = new Random();

                    if (needSet())
                    {
                        _clientModel.Phone = _randoms.RandomNumber(10);
                        LogTrace.WriteInLog("Заполняю поле Телефон. Было введено: " + _clientModel.Phone);
                    }
                    if (needSet())
                    {
                        _clientModel.Skype = _randoms.RandomString(5) + _randoms.RandomNumber(5);
                        LogTrace.WriteInLog("Заполняю поле Skype. Было введено: " + _clientModel.Skype);
                    }
                    if (needSet())
                    {
                        _clientModel.Icq = _randoms.RandomNumber(5);
                        LogTrace.WriteInLog("Заполняю поле ICQ. Было введено: " + _clientModel.Icq);
                    }
                    if (needSet())
                    {
                        _clientModel.ExchangeInCabinet = true;
                        LogTrace.WriteInLog("Выбран checkbox Обмен в кабинете");
                    }
                    if (needSet())
                    {
                        _clientModel.NewsInCabinet = true;
                        LogTrace.WriteInLog("Выбран checkbox Новости в кабинете");
                    }

                    if (needSet())
                    {
                        _clientModel.LimitNumOfCampaigns = _randoms.RandomNumber(2);
                        LogTrace.WriteInLog("Заполняю поле Ограничение по количеству кампаний");
                    }
                    if (needSet())
                    {
                        _clientModel.AllowViewFilterByPlatform = true;
                        LogTrace.WriteInLog("Выбран checkbox Разрешен просмотр фильтра по площадкам");
                    }
                    if (needSet())
                    {
                        _clientModel.Comments = _randoms.RandomString(20);
                        LogTrace.WriteInLog("Заполняю поле Комментарий");
                    }
                }
                //пропущены поля "Ограничение на кол-во создаваемых тизеров в сутки каждой РК", "Отображение сводной статистики трат", "Подсеть"
            #endregion
        }

        private void CreationIsSuccessful()
        {
            string createClientUrl = _driver.Url; //запоминаем url страницы "Добавление клиента"
            _clientModel.Submit(); //пытаемся сохранить форму
            LogTrace.WriteInLog("Нажал кнопку Сохранить");

            //если текущий и createClientUrl не совпали - клиент создался и ошибки искать не надо
            if (_driver.Url == createClientUrl)
                Errors.Add(_clientModel.GetErrors().ToString()); //проверяем, появились ли на форме ошибки заполнения полей
            else
                GetClientIdFromUrl();

            Registry.hashTable["driver"] = _driver;
            LogTrace.WriteInLog(_driver.Url);
        }

        private void GetClientIdFromUrl()
        {
            char[] slash = new char[] { '/' };
            string[] url = _driver.Url.Split(slash); //разбиваем URL по /
            ClientId = url[url.Length - 1]; //берем последний элемент массива - это id нового клиента
            Registry.hashTable["clientId"] = ClientId; //глобально запомнили ID клиента
        }
        
        private bool needSet() //генерируем 0 или 1.  1 - заполняем необязательное поле, 0 - не заполняем
        {
            Random rnd = new Random();
            return rnd.Next(0, 2) == 1 ? true : false;
        }
    }
}
