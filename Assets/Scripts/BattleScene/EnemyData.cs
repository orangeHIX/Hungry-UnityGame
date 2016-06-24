using UnityEngine;
using System.Collections;
namespace Battle
{
    public class EnemyData
    {
        /// <summary>
        /// 当前护甲
        /// </summary>
        public int HP;

        public string Name;

        public int Damage;
        /// <summary>
        /// 次/10秒
        /// </summary>
        public float Speed;

        ///// <summary>
        ///// 怪物描述
        ///// </summary>
        //public string Des;

        /// <summary>
        /// 掉落物体id
        /// </summary>
        public int ItemId;

        /// <summary>
        /// 掉落物体数量
        /// </summary>
        public int ItemCount;
    }
}
