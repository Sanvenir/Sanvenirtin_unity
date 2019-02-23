using System;
using System.Collections.Generic;
using ObjectScripts.CharSubstance;
using ObjectScripts.ItemScripts;

namespace ObjectScripts.StyleScripts.ActStyleScripts
{
    [Serializable]
    public class ActStyleCategory
    {
        public List<BaseActStyle> ActStyleList;

        /// <summary>
        ///     For each style category, character's fetch dictionary need to occupy this list of weapon type
        ///     e.p. Style Category for Two-handed long sword need the character have one hand with long sword and
        ///     other hand empty
        /// </summary>
        public List<int> WeaponTypeList;

        /// <summary>
        ///     Check whether the fetch list of character meet the requirement of weapon type list
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool Check(Character character)
        {
            var buff = new List<int>(WeaponTypeList);
            foreach (var fetchObject in character.FetchDictionary.Values)
            {
                int weaponType;
                if (fetchObject == null)
                {
                    weaponType = 0;
                }
                else
                {
                    var weapon = fetchObject as Weapon;
                    if (weapon == null) weaponType = -1;
                    else weaponType = weapon.WeaponType;
                }

                if (!buff.Contains(weaponType)) return false;
                buff.Remove(weaponType);
            }

            return true;
        }
    }
}