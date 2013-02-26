﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using OpenQA.Selenium;
using Task.Model;
using Task.Model.Pictograms;
using Task.Utils;
using Cookie = OpenQA.Selenium.Cookie;

namespace Task.Controller.Pictograms
{
    public class PicStatisticsController
    {
        private IWebDriver _driver;
        private PicStatisticsModel _statisticsModel;
        private readonly string _baseUrl = "https://" + Registry.hashTable["Login"] + ":" + Registry.hashTable["Password"] + "@" + "admin.dt00.net/cab/goodhits/campaigns-stat/campaign_id/" + Registry.hashTable["pkId"];
        private readonly string _pkUrl = "https://admin.dt00.net/cab/goodhits/campaigns/client_id/" + Registry.hashTable["clientId"];

        public bool ViewStatistics()
        {
            GetDriver();
            return SetUpFields();
        }

        private void GetDriver()
        {
            _driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный ранее драйвер
            _driver.Navigate().GoToUrl(_baseUrl); //заходим по ссылке
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
        }

        private bool SetUpFields()
        {
            _statisticsModel = new PicStatisticsModel();
            _statisticsModel.driver = _driver;

            LogTrace.WriteInLog(_driver.Url);

            #region Заполнение полей
            //string parentWindow = _driver.CurrentWindowHandle;

            //string css = "a[href *= '/cab/goodhits/campaigns-stat/id/" + Registry.hashTable["pkId"] + "'] > img[src *= '/cab/public/images/stat.png']";
            //_driver.FindElement(By.CssSelector(css)).Click();

            //IReadOnlyCollection<string> windows = _driver.WindowHandles;
            //foreach (var window in windows)
            //{
            //    _driver.SwitchTo().Window(window);
            //    if (_driver.Title.Contains("Товары > Кампании > Статистика")) break;
            //}
            
            //_statisticsModel.ButtonExportData = true;
            bool result = false;
            if (_driver.PageSource.Contains("403 | Forbidden | Exception"))
                return result = true;

            string filepath = string.Concat(Directory.GetCurrentDirectory(), "\\Экспорт_Статистики_" + DateTime.Now.Day + ".xls");
            DownloadFile("https://" + Registry.hashTable["Login"] + ":" + Registry.hashTable["Password"] + "@" + "admin.dt00.net/cab/goodhits/campaigns-stat-export/campaign_id/" + Registry.hashTable["pkId"], filepath);
            
            return result;

            #endregion
        }

        public int DownloadFile(String remoteFilename, String localFilename)
        {
            // Function will return the number of bytes processed
            // to the caller. Initialize to 0 here.
            int bytesProcessed = 0;

            // Assign values to these objects here so that they can
            // be referenced in the finally block
            Stream remoteStream = null;
            Stream localStream = null;
            HttpWebResponse response = null;

            // Use a try/catch/finally block as both the WebRequest and Stream
            // classes throw exceptions upon error
            try
            {
                // Create a request for the specified remote file name
                ICookieJar cookieJar = _driver.Manage().Cookies;
                string sessId = cookieJar.GetCookieNamed("PHPSESSID").Value;
                HttpWebRequest request = HttpWebRequest.CreateHttp(remoteFilename);
                string cookie = "PHPSESSID=" + sessId;
                request.Headers.Add(HttpRequestHeader.Cookie, cookie);

                if (request != null)
                {
                    // Send the request to the server and retrieve the
                    // WebResponse object
                    response = (HttpWebResponse)request.GetResponse();
                    if (response != null)
                    {
                        // Once the WebResponse object has been retrieved,
                        // get the stream object associated with the response's data
                        remoteStream = response.GetResponseStream();

                        // Create the local file
                        localStream = File.Create(localFilename);

                        // Allocate a 1k buffer
                        byte[] buffer = new byte[1024];
                        int bytesRead;

                        // Simple do/while loop to read from stream until
                        // no bytes are returned
                        do
                        {
                            // Read data (up to 1k) from the stream
                            bytesRead = remoteStream.Read(buffer, 0, buffer.Length);

                            // Write the data to the local file
                            localStream.Write(buffer, 0, bytesRead);

                            // Increment total bytes processed
                            bytesProcessed += bytesRead;
                        } while (bytesRead > 0);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // Close the response and streams objects here
                // to make sure they're closed even if an exception
                // is thrown at some point
                if (response != null) response.Close();
                if (remoteStream != null) remoteStream.Close();
                if (localStream != null) localStream.Close();
            }

            // Return total bytes processed to caller.
            return bytesProcessed;
        }
    }
}
