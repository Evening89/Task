using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.View;

namespace Task.Utils
{
    public static class Paths
    {
        private static string _domain = "admin.dt00.net/cab/";
        
        private static string Authorization()
        {
            return Registry.hashTable["Login"] + ":" + Registry.hashTable["Password"] + "@";
        }

        public static string UrlCreateClient
        {
            get
            {
                return "https://" + _domain + "goodhits/clients-new";
            }
            set{}
        }

        public static string UrlCreatePk
        {
            get
            {
                return "https://" + Authorization() +
                          _domain + "goodhits/clients-new-campaign/client_id/" + Registry.hashTable["clientId"];
            }
            set { }
        }

        public static string UrlCreateSite
        {
            get
            {
                return "https://" + _domain + "goodhits/clients-new-site/client_id/" +
                            Registry.hashTable["clientId"];
            }
            set { }
        }

        public static string UrlCreateTeaser
        {
            get
            {
                return "https://" + Authorization() +
                              _domain + "goodhits/ghits-add/campaign_id/" + Registry.hashTable["pkId"] +
                              "/filters/client_id/" + Registry.hashTable["clientId"];
            }
            set { }
        }

        public static string UrlEditClient
        {
            get
            {
                return "https://" + _domain + "goodhits/clients-edit/id/" + Registry.hashTable["clientId"] +
                            "/filters/%252Fclient_id%252F" + Registry.hashTable["clientId"] + "%252Fsubnet%252Fall";
            }
            set { }
        }

        public static string UrlEditPk
        {
            get
            {
                return "https://" + _domain + "goodhits/campaigns-edit/id/" + Registry.hashTable["pkId"] +
                        "/filters/%252Fclient_id%252F" + Registry.hashTable["clientId"];
            }
            set { }
        }

        public static string UrlEditSite
        {
            get
            {
                return "https://" + _domain + "goodhits/sites-edit/id/" + Registry.hashTable["siteId"] +
                          "/filters/%252Fid%252F" + Registry.hashTable["siteId"];
            }
            set { }
        }

        public static string UrlEditTeaser
        {
            get
            {
                return "https://" + _domain + "goodhits/ghits-edit/id/" + Registry.hashTable["teaserId"] +
                            "/filters/%252Fcampaign_id%252F" + Registry.hashTable["pkId"];
            }
            set { }
        }

        public static string UrlPicClaimForTeaser
        {
            get
            {
                return "https://" + _domain + "goodhits/creative-add/id/" + Registry.hashTable["pkId"] +
                                   "/filters/%252Fclient_id%252F" + Registry.hashTable["clientId"];
            }
            set { }
        }

        public static string UrlPicHistory
        {
            get
            {
                return "https://" + _domain + "overall-log/show-log/table_id/" + Registry.hashTable["pkId"] +
                            "/table/g_partners_1";
            }
            set { }
        }

        public static string UrlPicStatistics
        {
            get
            {
                return "https://" + Authorization() +
                               _domain + "goodhits/campaigns-stat/campaign_id/" + Registry.hashTable["pkId"];
            }
            set { }
        }

        public static void SwitchPaths(string _place)
        {
            if (_place == "prod") _domain = "admin.dt00.net/cab/";
            if (_place == "beta") _domain = "beta.admin.dt00.net/cab/";
        }
    }
}
