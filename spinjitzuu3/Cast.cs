using System.Drawing;
using System.Windows.Forms;

namespace spinjitzuu3
{
    class Cast
    {

        //Keys Declaration
        public static short fKey = 33;
        public static short dKey = 32;

        public static short qKey = 16;
        public static short wKey = 17;
        public static short eKey = 18;
        public static short rKey = 19;

        Input input = new Input();

        /// <summary>
        /// Cast Flash
        /// </summary>
        public void Flash(short flashKey)
        {
            KeyboardDirectInput.SendKey(flashKey);
        }

        /// <summary>
        /// Cast Q Ability
        /// </summary>
        public void Q()
        {
            KeyboardDirectInput.SendKey(qKey);
        }

        /// <summary>
        /// Cast W Ability
        /// </summary>
        public void W()
        {
            KeyboardDirectInput.SendKey(wKey);
        }

        /// <summary>
        /// Cast E Ability
        /// </summary>
        public void E()
        {
            KeyboardDirectInput.SendKey(eKey);
        }

        /// <summary>
        /// Cast R Ability
        /// </summary>
        public void R()
        {
            KeyboardDirectInput.SendKey(rKey);
        }



        //Champion Designed Scripts

        /// <summary>
        /// Cast Targeted W Ability
        /// </summary>
        public void targetedW(Point enemyPos)
        {
            Point Lastmovepos;
            Lastmovepos = Cursor.Position;
            Spin spin = new Spin();
            input.SetPosition(enemyPos.X + 65, enemyPos.Y + 95);
            W();
            input.SetPosition(Lastmovepos.X, Lastmovepos.Y);
        }


        /// <summary>
        /// Returns if Twitch's W Ability is ready to use
        /// </summary>
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

        /// <summary>
        /// Returns if Kindreds's E Ability is ready to use
        /// </summary>
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

        /// <summary>
        /// Returns if Kindreds's Q Ability is ready to use
        /// </summary>
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


        /// <summary>
        /// Returns if Kindreds's R Ability is ready to use
        /// </summary>
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

        /// <summary>
        /// Kindred's Auto-R cast script
        /// </summary>
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
