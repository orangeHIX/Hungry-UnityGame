using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
namespace Battle
{
    public class AlertPanel : MonoBehaviour
    {
        private Button quitButton;
        private Text resultText;
        private Text rewardText;

        private Action<bool> call;

        private bool isWin;
        private bool isRun;

        void Awake()
        {
            quitButton = this.transform.FindChild("QuitButton").GetComponent<Button>();
            resultText = this.transform.FindChild("ResultText").GetComponent<Text>();
            rewardText = this.transform.FindChild("RewardText").GetComponent<Text>();
        }

        public void OnClick()
        {
            if (call != null)
            {
                call(isWin);
                Close();
            }
        }

        public void Open(Action<bool> _call, bool _isWin, bool _isRun = false)
        {
            call = _call;
            isWin = _isWin;
            isRun = _isRun;

            this.transform.FindChild("QuitButton").GetComponent<Button>();
            quitButton.onClick.AddListener(OnClick);

            if (isRun)
            {
                resultText.text = "逃跑成功!";
                rewardText.text = string.Empty;
            }
            else
            {
                resultText.text = _isWin ? "战斗胜利!" : "战斗失败！";
                rewardText.text = "获得战利品:";
                foreach (EnemyData data in BattleModel.Instance.GetCurrEnemyData())
                {
                    rewardText.text += BattleModel.Instance.GetItemData(data.ItemId).Name + "*" + data.ItemCount.ToString() + "";
                }
            }
        }

        private void Close()
        {
            quitButton.onClick.RemoveListener(OnClick);
            this.gameObject.SetActive(false);
        }

        void Update()
        {

        }
    }

}