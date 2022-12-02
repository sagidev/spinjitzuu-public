using System;
using System.Net;

namespace spinjitzuu3
{
    class API
    {
        /// <summary>
        /// Scrape liveclientdata website info
        /// </summary>
        public static string Scrape()
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                string url = Uri.EscapeUriString("https://127.0.0.1:2999/liveclientdata/allgamedata");
                string doc = "";
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    doc = client.DownloadString(url);
                }
                return doc;
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return "";
            }
        }

        /// <summary>
        /// Read LocalPlayer's Basic Info
        /// </summary>
        public static string ScrapeActivePlayer()
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                string url = Uri.EscapeUriString("https://127.0.0.1:2999/liveclientdata/activeplayer");
                string doc = "";
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    doc = client.DownloadString(url);
                }
                return doc;
            }
            catch
            {
                //MessageBox.Show(ex.Message);
                return "";
            }
        }

        /// <summary>
        /// Read LocalPlayer's Current Health
        /// </summary>
        public static string readActiveCurrentHealth()
        {
            string temp = getBetween(ScrapeActivePlayer(), "currentHealth\": ", ",");
            return temp.Replace(".", ",");
        }

        /// <summary>
        /// Read LocalPlayer's Max Health
        /// </summary>
        public static string readActiveMaxHealth()
        {
            string temp = getBetween(ScrapeActivePlayer(), "maxHealth\": ", ",");
            return temp.Replace(".", ",");
        }

        /// <summary>
        /// Read LocalPlayer's Attack Speed
        /// </summary>
        public static string readAttackSpeed()
        {
            string temp = getBetween(Scrape(), "attackSpeed\": ", ",");
            return temp.Replace(".", ",");
        }

        /// <summary>
        /// Read LocalPlayer's Attack Range
        /// </summary>
        public static string readAttackRange()
        {
            return getBetween(Scrape(), "attackRange\": ", ",");
        }

        /// <summary>
        /// Read Game Time
        /// </summary>
        public static string readGameTime()
        {
            string temp = getBetween(Scrape(), "gameTime\": ", ",");
            return temp.Replace(".", ",");
        }

        /// <summary>
        /// Read LocalPlayer's Champion Name
        /// </summary>
        public static string readChampionName()
        {
            string temp = getBetween(Scrape(), "championName\": \"", "\"");
            return temp;
        }

        /// <summary>
        /// Read LocalPlayer's Health
        /// </summary>
        public static string readCurrentHealth()
        {
            string temp = getBetween(Scrape(), "currentHealth\": ", ",");
            return temp.Replace(".", ","); ;
        }

        /// <summary>
        /// Read LocalPlayer's Mana
        /// </summary>
        public static string readCurrentMana()
        {
            string temp = getBetween(Scrape(), "resourceValue\": ", ",");
            return temp.Replace(".", ","); ;
        }

        /// <summary>
        /// Gets the value between two strings
        /// </summary>
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            return "";
        }
    }
}
