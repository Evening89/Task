using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Task.Utils;
using Task.Model;

namespace Task.Controller
{
    public class GoodsEditPk_Controller
    {
        //берем сохраненный ранее 
        //(при создании клиента Task.Controller.GoodsCreateClient_Controller) ID клиента
        //и дописываем в URL
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


    }
}
