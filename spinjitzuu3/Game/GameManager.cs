using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spinjitzuu3
{
    internal class GameManager
    {
        /// <summary>
        /// Checks game state
        /// </summary>
        /// <returns></returns>
        public static bool CheckIfInGame()
        {
            try
            {
                if (float.Parse(API.readAttackSpeed()) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
