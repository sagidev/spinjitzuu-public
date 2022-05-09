using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spinjitzuu3
{
    class Cast
    {
        public static short fKey = 33;
        public static short dKey = 32;

        public static short qKey = 16;
        public static short wKey = 17;
        public static short eKey = 18;
        public static short rKey = 19;

        Input input = new Input();
        //Spin spin = new Spin();

        public void Flash(short flashKey)
        {
            KeyboardDirectInput.SendKey(flashKey);
        }
        public void Q()
        {
            KeyboardDirectInput.SendKey(qKey);
        }
        public void W()
        {
            KeyboardDirectInput.SendKey(wKey);
        }
        public void E()
        {
            KeyboardDirectInput.SendKey(eKey);
        }
        public void R()
        {
            KeyboardDirectInput.SendKey(rKey);
        }

        public void targetedW(Point enemyPos)
        {
            Point Lastmovepos;
            Lastmovepos = Cursor.Position;
            Spin spin = new Spin();
            input.SetPosition(enemyPos.X + 65, enemyPos.Y + 95);
            W();
            input.SetPosition(Lastmovepos.X, Lastmovepos.Y);
        }

        public bool isTwitchWReady()
        {
            PixelBot pxbot = new PixelBot();
            string twitchWReady = "#14260F";
            Color twitchWReadyColor = System.Drawing.ColorTranslator.FromHtml(twitchWReady);
            if (pxbot.GetPixelColor(887, 1027) == twitchWReadyColor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isKindredEReady()
        {
            PixelBot pxbot = new PixelBot();
            string skillReady = "#5033B3";
            Color skillReadyColor = System.Drawing.ColorTranslator.FromHtml(skillReady);
            if (pxbot.GetPixelColor(931, 1027) == skillReadyColor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isKindredQReady()
        {
            PixelBot pxbot = new PixelBot();
            string skillReady = "#2F4094";
            Color skillReadyColor = System.Drawing.ColorTranslator.FromHtml(skillReady);
            if (pxbot.GetPixelColor(843, 1027) == skillReadyColor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isKindredRReady()
        {
            PixelBot pxbot = new PixelBot();
            string skillReady = "#3A527A";
            Color skillReadyColor = System.Drawing.ColorTranslator.FromHtml(skillReady);
            if (pxbot.GetPixelColor(975, 1027) == skillReadyColor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void kindredAutoR()
        {
            float currentHealth = float.Parse(API.readActiveCurrentHealth());
            float maxHealth = float.Parse(API.readActiveMaxHealth());
            if ((currentHealth / maxHealth * 100) < 20)
            {
                if (isKindredRReady())
                {
                    R();
                }
            }
        }
    }
}
