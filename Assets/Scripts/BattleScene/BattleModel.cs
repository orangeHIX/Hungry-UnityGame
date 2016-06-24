using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Battle
{
    public class BattleModel
    {
        public static BattleModel Instance = new BattleModel();

        private Dictionary<int, SkillData> skillDataDic = new Dictionary<int, SkillData>();

        private Dictionary<int, ItemData> itemDataDic = new Dictionary<int, ItemData>();

        public List<PlayerData> PlayList = new List<PlayerData>();

        public List<List<EnemyData>> EnemyDataList = new List<List<EnemyData>>();

        public int CurrIndex;

        public float PlayerAllHp;

        public List<int> EnemyAllHpList;

        /// <summary>
        /// 回复间隔
        /// </summary>
        public const int CureTime = 5;

        /// <summary>
        /// 回复量
        /// </summary>
        public const int CureValue = 100;

        private BattleModel()
        {
            InitSkillData();
            InitItemData();
        }

        public void Init()
        {
            InitPlayerData();
            InitEnemyData();
        }

        /// <summary>
        /// 设置玩家
        /// </summary>
        /// <param name="_list"></param>
        public void SetPlayer(List<PlayerData> _list)
        {
            PlayList = _list;
        }

        /// <summary>
        /// 设置敌人
        /// </summary>
        /// <param name="_data"></param>
        public void SetEnemy(List<List<EnemyData>> _list)
        {
            EnemyDataList = _list;
        }

        private void InitSkillData()
        {
            string[] skillName = { "斩击", "斩杀", "拳击", "射击", "小治疗", "大治疗" };
            EnumSkill[] enumS = { EnumSkill.Attack, EnumSkill.Attack, EnumSkill.Attack, EnumSkill.Attack, EnumSkill.Cure, EnumSkill.Cure };

            for (int i = 0; i < skillName.Length; i++)
            {
                SkillData skilldata = new SkillData();
                skilldata.Name = skillName[i];
                skilldata.SkillType = enumS[i];
                skillDataDic.Add(i + 1, skilldata);
            }
        }

        private void InitItemData()
        {
            string[] itemName = { "破布", "麻绳", "罐子" };

            for (int i = 0; i < itemName.Length; i++)
            {
                ItemData itemData = new ItemData();
                itemData.Name = itemName[i];
                itemData.Id = i + 1;
                itemDataDic.Add(itemData.Id, itemData);
            }
        }

        private void InitPlayerData()
        {
            List<PlayerData> list = new List<PlayerData>();
            string[] name = { "士兵", "持剑者", "开拓者", "射手", "小牧师", "大牧师" };
            int[] hp = { 50, 190, 10, 30, 50, 100 };
            int[] dam = { 2000, 90, 4, 15, 50, 80 };
            float[] speed = { 2.5f, 2.5f, 2, 3, 1.5f, 1 };
            int[] skillid = { 1, 2, 3, 4, 5, 6 };
            PlayerAllHp = 0;
            for (int i = 0; i < name.Length; i++)
            {
                PlayerData playerdata = new PlayerData();
                playerdata.Damage = dam[i];
                playerdata.Speed = speed[i];
                playerdata.Name = name[i];
                playerdata.HP = hp[i];
                playerdata.SkillId = skillid[i];
                PlayerAllHp += playerdata.HP;
                list.Add(playerdata);
            }
            SetPlayer(list);
        }

        private void InitEnemyData()
        {
            List<List<EnemyData>> enemyDataList = new List<List<EnemyData>>();

            string[] name1 = { "虚空魔龙", "啮齿兽", "虚空魔龙" };
            string[] name2 = { "啮齿兽", "啮齿兽", "啮齿兽" };
            List<string[]> nameList = new List<string[]>();
            nameList.Add(name1);
            nameList.Add(name2);

            int[] HP1 = { 4000, 1000, 4000 };
            int[] HP2 = { 1000, 1000, 1000 };
            List<int[]> hpList = new List<int[]>();
            hpList.Add(HP1);
            hpList.Add(HP2);

            float[] speed1 = { 1, 2, 1 };
            float[] speed2 = { 2, 2, 2 };
            List<float[]> speedList = new List<float[]>();
            speedList.Add(speed1);
            speedList.Add(speed2);

            int[] damage1 = { 100, 50, 100 };
            int[] damage2 = { 50, 50, 50 };
            List<int[]> damageList = new List<int[]>();
            damageList.Add(damage1);
            damageList.Add(damage2);

            int[] item1 = { 3, 1, 3 };
            int[] item2 = { 1, 1, 2 };
            List<int[]> itemList = new List<int[]>();
            itemList.Add(item1);
            itemList.Add(item2);

            int[] itemCount1 = { 1, 3, 1 };
            int[] itemCount2 = { 3, 3, 3 };
            List<int[]> itemCountList = new List<int[]>();
            itemCountList.Add(itemCount1);
            itemCountList.Add(itemCount2);

            EnemyAllHpList = new List<int>();

            for (int i = 0; i < nameList.Count; i++)
            {
                List<EnemyData> tempList = new List<EnemyData>();
                int hp = 0;
                for (int j = 0; j < nameList[i].Length; j++)
                {
                    EnemyData tempData = new EnemyData();
                    tempData.HP = hpList[i][j];
                    tempData.Damage = damageList[i][j];
                    tempData.Speed = speedList[i][j];
                    tempData.Name = nameList[i][j];
                    tempData.ItemCount = itemCountList[i][j];
                    tempData.ItemId = itemList[i][j];
                    hp += tempData.HP;
                    tempList.Add(tempData);
                }
                EnemyAllHpList.Add(hp);
                enemyDataList.Add(tempList);
            }
            SetEnemy(enemyDataList);
        }

        public SkillData GetSkillData(int _skillId)
        {
            return skillDataDic[_skillId];
        }

        public ItemData GetItemData(int _itemId)
        {
            return itemDataDic[_itemId];
        }

        public float GetEnemySpeed(int _index)
        {
            float s = 10 / EnemyDataList[CurrIndex][_index].Speed;
            return s;
        }

        public float GetPlayerSpeed(int _index)
        {
            return 10 / PlayList[_index].Speed;
        }

        public List<EnemyData> GetCurrEnemyData()
        {
            return EnemyDataList[CurrIndex];
        }
    }

}