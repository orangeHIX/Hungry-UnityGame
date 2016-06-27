using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Battle
{
    public class BattleScript : MonoBehaviour
    {
        private EnterBattlePanel enterPanel;
        private EndPanel endPanel;
        private BattlePanel battlePanel;

        private int battleIndex = -1;
        private int battleMax;


        void Awake()
        {
            BattleModel.Instance.Init();
            battleMax = BattleModel.Instance.EnemyDataList.Count;
            enterPanel = this.transform.FindChild("EnterBattlePanel").GetComponent<EnterBattlePanel>();
            endPanel = this.transform.FindChild("EndPanel").GetComponent<EndPanel>();
            battlePanel = this.transform.FindChild("BattlePanel").GetComponent<BattlePanel>();
            EndABattle(true);
        }


        private void EnterABattle()
        {
            battlePanel.gameObject.SetActive(true);
            battlePanel.Open(EndABattle);
        }

        private void EndABattle(bool _isWin)
        {
            if (_isWin)
            {
                if (battleIndex + 1 < battleMax)
                {
                    battleIndex++;
                    BattleModel.Instance.CurrIndex = battleIndex;
                    enterPanel.gameObject.SetActive(true);
                    enterPanel.Open(BattleModel.Instance.GetCurrEnemyData(), EnterABattle, BackToMap);
                }
                else
                {
                    endPanel.gameObject.SetActive(true);
                    endPanel.Open(BackToMap);
                }
            }
            else
            {
                BackToMap();
            }
        }

        private void BackToMap()
        {
            Debug.Log("返回地图");
            SceneManager.LoadScene("Main");
        }


        void Update()
        {

        }
    }
}