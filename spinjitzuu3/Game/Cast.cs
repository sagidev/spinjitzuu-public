using System.Drawing;
using System.Windows.Forms;

namespace spinjitzuu3
{
    /// <summary>
    /// Handles ability casting and related functionality
    /// </summary>
    public class Cast
    {
        #region Key Bindings
        // Summoner Spells
        private const short FlashKey = 33;
        private const short IgniteKey = 32;

        // Abilities
        private const short QKey = 16;
        private const short WKey = 17;
        private const short EKey = 18;
        private const short RKey = 19;
        #endregion

        private readonly Input _input = new Input();
        private readonly PixelBot _pixelBot = new PixelBot();

        #region Basic Ability Casting
        /// <summary>
        /// Casts Flash summoner spell
        /// </summary>
        public void Flash() => KeyboardDirectInput.SendKey(FlashKey);

        /// <summary>
        /// Casts Ignite summoner spell
        /// </summary>
        public void Ignite() => KeyboardDirectInput.SendKey(IgniteKey);

        /// <summary>
        /// Casts Q ability
        /// </summary>
        public void Q() => KeyboardDirectInput.SendKey(QKey);

        /// <summary>
        /// Casts W ability
        /// </summary>
        public void W() => KeyboardDirectInput.SendKey(WKey);

        /// <summary>
        /// Casts E ability
        /// </summary>
        public void E() => KeyboardDirectInput.SendKey(EKey);

        /// <summary>
        /// Casts R ability
        /// </summary>
        public void R() => KeyboardDirectInput.SendKey(RKey);
        #endregion

        #region Advanced Ability Casting
        /// <summary>
        /// Casts targeted W ability at enemy position
        /// </summary>
        /// <param name="enemyPos">The enemy's position</param>
        public void CastTargetedW(Point enemyPos)
        {
            Point lastCursorPos = Cursor.Position;

            // Move cursor to target position with offset
            _input.SetPosition(enemyPos.X + 65, enemyPos.Y + 95);

            // Cast W ability
            W();

            // Return cursor to original position
            _input.SetPosition(lastCursorPos.X, lastCursorPos.Y);
        }
        #endregion

        #region Ability Status Checks
        /// <summary>
        /// Checks if Twitch's W ability is ready to use
        /// </summary>
        /// <returns>True if ability is ready, false otherwise</returns>
        public bool IsTwitchWReady()
        {
            const string readyColorHex = "#14260F";
            Color readyColor = ColorTranslator.FromHtml(readyColorHex);

            return _pixelBot.GetPixelColor(887, 1027) == readyColor;
        }

        /// <summary>
        /// Checks if Kindred's Q ability is ready to use
        /// </summary>
        /// <returns>True if ability is ready, false otherwise</returns>
        public bool IsKindredQReady()
        {
            const string readyColorHex = "#2F4094";
            Color readyColor = ColorTranslator.FromHtml(readyColorHex);

            return _pixelBot.GetPixelColor(843, 1027) == readyColor;
        }

        /// <summary>
        /// Checks if Kindred's E ability is ready to use
        /// </summary>
        /// <returns>True if ability is ready, false otherwise</returns>
        public bool IsKindredEReady()
        {
            const string readyColorHex = "#5033B3";
            Color readyColor = ColorTranslator.FromHtml(readyColorHex);

            return _pixelBot.GetPixelColor(931, 1027) == readyColor;
        }

        /// <summary>
        /// Checks if Kindred's R ability is ready to use
        /// </summary>
        /// <returns>True if ability is ready, false otherwise</returns>
        public bool IsKindredRReady()
        {
            const string readyColorHex = "#3A527A";
            Color readyColor = ColorTranslator.FromHtml(readyColorHex);

            return _pixelBot.GetPixelColor(975, 1027) == readyColor;
        }
        #endregion

        #region Champion-Specific Logic
        /// <summary>
        /// Automatically casts Kindred's R ability when health falls below threshold
        /// </summary>
        public void KindredAutoR()
        {
            const float healthThresholdPercent = 20.0f;

            float currentHealth = float.Parse(API.readActiveCurrentHealth());
            float maxHealth = float.Parse(API.readActiveMaxHealth());
            float healthPercent = (currentHealth / maxHealth) * 100;

            if (healthPercent < healthThresholdPercent && IsKindredRReady())
            {
                R();
            }
        }
        #endregion
    }
}