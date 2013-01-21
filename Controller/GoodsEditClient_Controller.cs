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

namespace Task.Controller
{
    public class GoodsEditClient_Controller
    {
        IWebDriver driver;
        public string baseUrl = "https://admin.dt00.net/cab/goodhits/clients-edit/id/" + Registry.hashTable["clientId"] + "/filters/%252Fclient_id%252F" + Registry.hashTable["clientId"] + "%252Fsubnet%252FFall";
        Randoms randoms = new Randoms();//класс генерации случайных строк
        public List<string> errors = new List<string>(); //список ошибок

        public string emailByDirection;
        public string emailAdditionalByDirection;
        public string phoneByDirection;
        public string skypeByDirection;
        public string icqByDirection;
        public string nameByDirection;

        public string emailCommon;
        public string emailAdditionalCommon;
        public string phoneCommon;
        public string skypeCommon;
        public string icqCommon;
        public string nameCommon;
        public string aliasCommon;
        public int startSectionCommon;

        public string requisitesOfPayment;

        public int sendPassword;

        public int newCuratorExternal;
        public int newCuratorInternal;
        public int attractorClient;

        public string limitPkQuantity;
        public string limitTeasersQuantity;
        public string comments;
        
        GoodsEditClient_Model clientEditModel = new GoodsEditClient_Model();

        public void EditClient()
        {
            driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный ранее драйвер
            driver.Navigate().GoToUrl(baseUrl); //заходим по ссылке
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            
            clientEditModel.driver = driver;

            LogTrace.WriteInLog(driver.Url);

            #region Заполнение
                #region Контакты по направлению

                    LogTrace.WriteInLog("...Контакты по направлению...");

                    emailByDirection = randoms.RandomString(5) +"@" + randoms.RandomString(5) + "." + "com";
                    clientEditModel.ContactsByDirectionEmail = emailByDirection;
                    LogTrace.WriteInLog("     Заполняю поле E-Mail по направлению. Было введено: " + clientEditModel.ContactsByDirectionEmail);

                    emailAdditionalByDirection = randoms.RandomString(5) + "@" + randoms.RandomString(5) + "." + "com";
                    clientEditModel.ContactsByDirectionEmailAdditional = emailAdditionalByDirection;
                    LogTrace.WriteInLog("     Заполняю поле Дополнительный E-Mail по направлению. Было введено: " + clientEditModel.ContactsByDirectionEmailAdditional);

                    phoneByDirection = randoms.RandomNumber(10);
                    clientEditModel.ContactsByDirectionPhone = phoneByDirection;
                    LogTrace.WriteInLog("     Заполняю поле Телефон по направлению. Было введено: " + clientEditModel.ContactsByDirectionPhone);

                    skypeByDirection = randoms.RandomString(5) + randoms.RandomNumber(5);
                    clientEditModel.ContactsByDirectionSkype = skypeByDirection;
                    LogTrace.WriteInLog("     Заполняю поле Skype по направлению. Было введено: " + clientEditModel.ContactsByDirectionSkype);

                    icqByDirection = randoms.RandomNumber(5);
                    clientEditModel.ContactsByDirectionIcq = icqByDirection;
                    LogTrace.WriteInLog("     Заполняю поле ICQ по направлению. Было введено: " + clientEditModel.ContactsByDirectionIcq);

                    nameByDirection = randoms.RandomString(10);
                    clientEditModel.ContactsByDirectionName = nameByDirection;
                    LogTrace.WriteInLog("     Заполняю поле ФИО по направлению. Было введено: " + clientEditModel.ContactsByDirectionName);
                #endregion

                #region Основные контакты

                    LogTrace.WriteInLog("...Основные контакты...");

                    emailCommon = randoms.RandomString(5) + "@" + randoms.RandomString(5) + "." + "com";
                    clientEditModel.ContactsCommonEmail = emailCommon;
                    LogTrace.WriteInLog("     Заполняю поле E-Mail по основным контактам. Было введено: " + clientEditModel.ContactsCommonEmail);

                    emailAdditionalCommon = randoms.RandomString(5) + "@" + randoms.RandomString(5) + "." + "com";
                    clientEditModel.ContactsCommonEmailAdditional = emailAdditionalCommon;
                    LogTrace.WriteInLog("     Заполняю поле Дополнительный E-Mail по основным контактам. Было введено: " + clientEditModel.ContactsCommonEmailAdditional);

                    phoneCommon = randoms.RandomNumber(10);
                    clientEditModel.ContactsCommonPhone = phoneCommon;
                    LogTrace.WriteInLog("     Заполняю поле Телефон по основным контактам. Было введено: " + clientEditModel.ContactsCommonPhone);

                    skypeCommon = randoms.RandomString(5) + randoms.RandomNumber(5);
                    clientEditModel.ContactsCommonSkype = skypeCommon;
                    LogTrace.WriteInLog("     Заполняю поле Skype по основным контактам. Было введено: " + clientEditModel.ContactsCommonSkype);

                    icqCommon = randoms.RandomNumber(5);
                    clientEditModel.ContactsCommonIcq = icqCommon;
                    LogTrace.WriteInLog("     Заполняю поле ICQ по основным контактам. Было введено: " + clientEditModel.ContactsCommonIcq);

                    nameCommon = randoms.RandomString(10);
                    clientEditModel.ContactsCommonName = nameCommon;
                    LogTrace.WriteInLog("     Заполняю поле ФИО по основным контактам. Было введено: " + clientEditModel.ContactsCommonName);

                    aliasCommon = randoms.RandomString(10);
                    clientEditModel.ContactsCommonAlias = aliasCommon;
                    LogTrace.WriteInLog("     Заполняю поле 'Псевдоним для креатива' по основным контактам. Было введено: " + clientEditModel.ContactsCommonAlias);

                    int countElementsInList = clientEditModel.quantityItemsInList(clientEditModel.locatorStartSection);
                    Random rnd = new Random();
                    startSectionCommon = rnd.Next(0, countElementsInList);
                    clientEditModel.StartSectionProfile = startSectionCommon;
                    LogTrace.WriteInLog("     Выбираю в выпадающем списке 'Стартовый раздел кабинета' по основным контактам: " + clientEditModel.chosenStartSectionProfile);
                #endregion

                #region Платёжные реквизиты

                    LogTrace.WriteInLog("...Платёжные реквизиты...");

                    clientEditModel.KindOfPayment = true;
                    LogTrace.WriteInLog("     Выбираю в выпадающем списке 'Форма оплаты' по платежным реквизитам: " + clientEditModel.chosenKindOfPayment);

                    requisitesOfPayment = randoms.RandomNumber(16);
                    clientEditModel.Requisites = requisitesOfPayment;
                    LogTrace.WriteInLog("     Заполняю поле Реквизиты по платежным реквизитам. Было введено: " + clientEditModel.Requisites);

                    clientEditModel.ButtonAdd = true;
                    LogTrace.WriteInLog("     Нажимаю кнопку 'Добавить' по платежным реквизитам");
                #endregion

                #region Управление паролем

                    LogTrace.WriteInLog("...Управление паролем...");

                    clientEditModel.Password = true;
                    LogTrace.WriteInLog("     Нажимаю кнопку генерации пароля");

                    rnd = new Random();
                    sendPassword = rnd.Next(0, 3);
                    clientEditModel.SendPassword = sendPassword;
                    LogTrace.WriteInLog("     Выбираю radiobutton 'Отправить пароль?'. Выбрано: " + clientEditModel.chosenSendPassword);
                #endregion

                #region Взаимодействие

                    LogTrace.WriteInLog("...Взаимодействие...");

                    countElementsInList = clientEditModel.quantityItemsInList(clientEditModel.locatorExtCurator);
                    rnd = new Random();
                    newCuratorExternal = rnd.Next(0, countElementsInList);
                    clientEditModel.NewCuratorExternal = newCuratorExternal;
                    LogTrace.WriteInLog("     Выбираю в выпадающем списке 'Выбрать нового куратора' (внешний): " + clientEditModel.chosenNewCuratorExternal);

                    countElementsInList = clientEditModel.quantityItemsInList(clientEditModel.locatorIntCurator);
                    rnd = new Random();
                    newCuratorInternal = rnd.Next(0, countElementsInList);
                    clientEditModel.NewCuratorInternal = newCuratorInternal;
                    LogTrace.WriteInLog("     Выбираю в выпадающем списке 'Выбрать нового куратора' (внутренний): " + clientEditModel.chosenNewCuratorInternal);

                    countElementsInList = clientEditModel.quantityItemsInList(clientEditModel.locatorAttractor);
                    rnd = new Random();
                    attractorClient = rnd.Next(0, countElementsInList);
                    clientEditModel.AttractorClient = attractorClient;
                    LogTrace.WriteInLog("     Выбираю в выпадающем списке 'Привлекатель клиента': " + clientEditModel.chosenAttractorClient);
                #endregion

                #region Настройки

                    LogTrace.WriteInLog("...Настройки...");

                    clientEditModel.ExchangeInProfile = true;
                    LogTrace.WriteInLog("     Выбран checkbox Обмен в кабинете");

                    clientEditModel.NewsInProfile = true;
                    LogTrace.WriteInLog("     Выбран checkbox Новости в кабинете");

                    limitPkQuantity = randoms.RandomNumber(3);
                    clientEditModel.LimitPkQuantity = limitPkQuantity;
                    LogTrace.WriteInLog("     Заполняю поле 'Ограничение по количеству кампаний'. Было введено: " + clientEditModel.LimitPkQuantity);

                    clientEditModel.AllowViewFilterByPlatforms = true;
                    LogTrace.WriteInLog("     Выбран checkbox Разрешен просмотр фильтра по площадкам");

                    limitTeasersQuantity = randoms.RandomNumber(3);
                    clientEditModel.LimitTeasersQuantityPerDay = limitTeasersQuantity;
                    LogTrace.WriteInLog("     Заполняю поле 'Ограничение на кол-во создаваемых тизеров в сутки каждой РК'. Было введено: " + clientEditModel.LimitTeasersQuantityPerDay);

                    clientEditModel.SwitchOffTabAdvBlocks = true;
                    LogTrace.WriteInLog("     Выбран checkbox 'Отключить вкладку Рекламные блоки'");
                    
                    //clientEditModel.ReflectionStatisticsSpending = true;
                    //LogTrace.WriteInLog("     Выбран checkbox 'Отображение сводной статистики трат'");

                    comments = randoms.RandomString(30);
                    clientEditModel.Comments = comments;
                    LogTrace.WriteInLog("     Заполняю поле Комментарий. Было введено: " + clientEditModel.Comments);
                #endregion
            #endregion
            
            string editClientUrl = driver.Url; //запоминаем url страницы

            clientEditModel.Submit(); //пытаемся сохранить форму
            LogTrace.WriteInLog("Нажал кнопку Применить");
            LogTrace.WriteInLog(driver.Url);
            LogTrace.WriteInLog("");
            
            string isEditedClientUrl = driver.Url; //запоминаем url страницы, открывшейся после нажатия "Завершить"
            //если createClientUrl и isCreatedClientUrl совпали - мы никуда не перешли и значит есть ошибки заполнения полей
            //если createClientUrl и isCreatedClientUrl не совпали - клиент отредактировался и ошибки искать не надо
            if (editClientUrl == isEditedClientUrl)
            {
                errors = clientEditModel.GetErrors(); //проверяем, появились ли на форме ошибки заполнения полей
            }
            else
            {
                //CheckEditingClient();
            }
            //Registry.hashTable.Add("driver", driver); //записываем в хештаблицу driver и его состояние, чтобы потом извлечь и использовать его при создании сайта/РК
            Registry.hashTable["driver"] = driver;
            //LogTrace.WriteInLog(driver.Url);
        }

        public bool wasMismatch = false;

        public void CheckEditingClient()
        {
            driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный при создании клиента драйвер
            driver.Navigate().GoToUrl(baseUrl); //заходим по ссылке
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            LogTrace.WriteInLog(driver.Url);
            
            #region Проверка заполнения
               
                #region Контакты по направлению
                    LogTrace.WriteInLog("...Проверка: Контакты по направлению...");

                    if (emailByDirection == clientEditModel.GetContactsByDirectionEmail) { LogTrace.WriteInLog("Совпадают: содержимое поля E-Mail и введенное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("    НЕ СОВПАДАЮТ: содержимое поля E-Mail и введенное при редактировании");
                        wasMismatch = true;
                    }

                    if (emailAdditionalByDirection == clientEditModel.GetContactsByDirectionEmailAdditional) { LogTrace.WriteInLog("Совпадают: содержимое поля Дополнительный E-Mail и введенное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: содержимое поля Дополнительный E-Mail и введенное при редактировании");
                        wasMismatch = true;
                    }

                    if (phoneByDirection == clientEditModel.GetContactsByDirectionPhone) { LogTrace.WriteInLog("Совпадают: содержимое поля Телефон и введенное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: содержимое поля Телефон и введенное при редактировании");
                        wasMismatch = true;
                    }

                    if (skypeByDirection == clientEditModel.GetContactsByDirectionSkype) { LogTrace.WriteInLog("Совпадают: содержимое поля Skype и введенное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: содержимое поля Skype и введенное при редактировании");
                        wasMismatch = true;
                    }

                    if (icqByDirection == clientEditModel.GetContactsByDirectionIcq) { LogTrace.WriteInLog("Совпадают: содержимое поля ICQ и введенное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: содержимое поля ICQ и введенное при редактировании");
                        wasMismatch = true;
                    }

                    if (nameByDirection == clientEditModel.GetContactsByDirectionName) { LogTrace.WriteInLog("Совпадают: содержимое поля ФИО и введенное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: содержимое поля ФИО и введенное при редактировании");
                        wasMismatch = true;
                    }
                #endregion

                #region Основные контакты
                    LogTrace.WriteInLog("...Проверка: Основные контакты...");

                    if (emailCommon == clientEditModel.GetContactsCommonEmail) { LogTrace.WriteInLog("Совпадают: содержимое поля E-Mai и введенное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: содержимое поля E-Mai и введенное при редактировании");
                        wasMismatch = true;
                    }

                    if (emailAdditionalCommon == clientEditModel.GetContactsCommonEmailAdditional) { LogTrace.WriteInLog("Совпадают: содержимое поля Дополнительный E-Mail и введенное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: содержимое поля Дополнительный E-Mail и введенное при редактировании");
                        wasMismatch = true;
                    }

                    if (phoneCommon == clientEditModel.GetContactsCommonPhone) { LogTrace.WriteInLog("Совпадают: содержимое поля Телефон и введенное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: содержимое поля Телефон и введенное при редактировании");
                        wasMismatch = true;
                    }

                    if (skypeCommon == clientEditModel.GetContactsCommonSkype) { LogTrace.WriteInLog("Совпадают: содержимое поля Skype и введенное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: содержимое поля Skype и введенное при редактировании");
                        wasMismatch = true;
                    }

                    if (icqCommon == clientEditModel.GetContactsCommonIcq) { LogTrace.WriteInLog("Совпадают: содержимое поля ICQ и введенное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: содержимое поля ICQ и введенное при редактировании");
                        wasMismatch = true;
                    }

                    if (nameCommon == clientEditModel.GetContactsCommonName) { LogTrace.WriteInLog("Совпадают: содержимое поля ФИО и введенное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: содержимое поля ФИО и введенное при редактировании");
                        wasMismatch = true;
                    }

                    if (aliasCommon == clientEditModel.GetContactsCommonAlias) { LogTrace.WriteInLog("Совпадают: содержимое поля 'Псевдоним для креатива' и введенное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: содержимое поля 'Псевдоним для креатива' и введенное при редактировании");
                        wasMismatch = true;
                    }

                    if (clientEditModel.chosenStartSectionProfile == clientEditModel.GetStartSectionProfile) { LogTrace.WriteInLog("Совпадают: пункт списка 'Стартовый раздел кабинета' и выбранный при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: пункт списка 'Стартовый раздел кабинета' и выбранный при редактировании");
                        wasMismatch = true;
                    }       
                #endregion

                #region Платёжные реквизиты
                    LogTrace.WriteInLog("...Проверка: Платёжные реквизиты...");

                    string concatenated = clientEditModel.chosenKindOfPayment + " " + requisitesOfPayment;
                    if (concatenated == clientEditModel.GetRequisites) { LogTrace.WriteInLog("Совпадают: форма оплаты + реквизиты и введенные при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: форма оплаты + реквизиты и введенные при редактировании");
                        wasMismatch = true;
                    }
                #endregion

                #region Взаимодействие
                    LogTrace.WriteInLog("...Проверка: Взаимодействие...");

                    if (clientEditModel.GetCurrentCurator) { LogTrace.WriteInLog("Совпадают: текущий куратор и выбранный при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: текущий куратор и выбранный при редактировании");
                        wasMismatch = true;
                    }

                    if (clientEditModel.chosenAttractorClient == clientEditModel.GetAttractorClient) { LogTrace.WriteInLog("Совпадают: пункт списка 'Привлекатель клиента' и выбранный при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: пункт списка 'Привлекатель клиента' и выбранный при редактировании");
                        wasMismatch = true;
                    }
                #endregion

                #region Настройки
                    LogTrace.WriteInLog("...Проверка: Настройки...");

                    if (clientEditModel.GetClientIsActive) { LogTrace.WriteInLog("Совпадают: состояние checkbox 'Клиент активен' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: состояние checkbox 'Клиент активен' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (clientEditModel.GetExchangeInProfile) { LogTrace.WriteInLog("Совпадают: состояние checkbox 'Обмен в кабинете' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: состояние checkbox 'Обмен в кабинете' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (clientEditModel.GetNewsInProfile) { LogTrace.WriteInLog("Совпадают: состояние checkbox 'Новости в кабинете' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: состояние checkbox 'Новости в кабинете' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (clientEditModel.GetTestClient) { LogTrace.WriteInLog("Совпадают: состояние checkbox 'Тестовый клиент' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: состояние checkbox 'Тестовый клиент' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (limitPkQuantity == clientEditModel.GetLimitPkQuantity) { LogTrace.WriteInLog("Совпадают: содержимое поля 'Ограничение по количеству кампаний' и введенное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: содержимое поля 'Ограничение по количеству кампаний' и введенное при редактировании");
                        wasMismatch = true;
                    }

                    if (clientEditModel.GetAllowViewFilterByPlatforms) { LogTrace.WriteInLog("Совпадают: состояние checkbox 'Разрешен просмотр фильтра по площадкам' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: состояние checkbox 'Разрешен просмотр фильтра по площадкам' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (limitTeasersQuantity == clientEditModel.GetLimitTeasersQuantityPerDay) { LogTrace.WriteInLog("Совпадают: содержимое поля 'Ограничение на кол-во создаваемых тизеров в сутки каждой РК' и введенное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: содержимое поля 'Ограничение на кол-во создаваемых тизеров в сутки каждой РК' и введенное при редактировании");
                        wasMismatch = true;
                    }

                    if (clientEditModel.GetSwitchOffTabAdvBlocks) { LogTrace.WriteInLog("Совпадают: состояние checkbox 'Отключить вкладку Рекламные блоки' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: состояние checkbox 'Отключить вкладку Рекламные блоки' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (clientEditModel.GetReflectionStatisticsSpending) { LogTrace.WriteInLog("Совпадают: состояние checkbox 'Отображение сводной статистики трат' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: состояние checkbox 'Отображение сводной статистики трат' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (comments == clientEditModel.GetComments) { LogTrace.WriteInLog("Совпадают: содержимое поля Комментарий и введенное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("     НЕ СОВПАДАЮТ: содержимое поля Комментарий и введенное при редактировании");
                        wasMismatch = true;
                    } 
                #endregion
            #endregion
        }
    }
}
