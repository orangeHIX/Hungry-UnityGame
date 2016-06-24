using UnityEngine;
using System.Collections;
namespace Battle
{
    public class PlayerData
    {
        /// <summary>
        /// 当前护甲护甲
        /// </summary>
        public int HP;

        /// <summary>
        /// 伤害或是治疗量 取决于技能
        /// </summary>
        public int Damage;
        /// <summary>
        /// 次/10秒
        /// </summary>
        public float Speed;

        public int SkillId;

        public string Name;

    }
}
