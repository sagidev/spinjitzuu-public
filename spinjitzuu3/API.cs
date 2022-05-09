using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace spinjitzuu3
{
    class API
    {
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
            catch
            {
                //MessageBox.Show(ex.Message);
                return "";
            }
        }
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
        public static string readActiveCurrentHealth()
        {
            string temp = getBetween(ScrapeActivePlayer(), "currentHealth\": ", ",");
            return temp.Replace(".", ",");
        }
        public static string readActiveMaxHealth()
        {
            string temp = getBetween(ScrapeActivePlayer(), "maxHealth\": ", ",");
            return temp.Replace(".", ",");
        }

        public static string readAttackSpeed()
        {
            string temp = getBetween(Scrape(), "attackSpeed\": ", ",");
            return temp.Replace(".", ",");
        }
        public static string readAttackRange()
        {
            return getBetween(Scrape(), "attackRange\": ", ",");
        }
        public static string readGameTime()
        {
            string temp = getBetween(Scrape(), "gameTime\": ", ",");
            return temp.Replace(".", ",");
        }
        public static string readChampionName()
        {
            string temp = getBetween(Scrape(), "championName\": \"", "\"");
            return temp;
        }
        public static string readCurrentHealth()
        {
            string temp = getBetween(Scrape(), "currentHealth\": ", ",");
            return temp.Replace(".", ","); ;
        }
        public static string readCurrentMana()
        {
            string temp = getBetween(Scrape(), "resourceValue\": ", ",");
            return temp.Replace(".", ","); ;
        }

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
