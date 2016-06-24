using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
namespace Battle
{
    public class BattlePanel : MonoBehaviour
    {
        public GameObject FloatTextPrefab;

        private GameObject enemy;
        private GameObject player;

        private GameObject effect;

        private Text enemyHpText;
        private Text playerHpText;

        private List<Button> enemySkillButtonList = new List<Button>();
        private List<Button> playerSkillButtonList = new List<Button>();

        private Button cureButton;
        private Image cureBar;

        private Button runButton;

        private Image playHpBar;
        private Image enemyHpBar;

        //private Text endText;

        private List<Image> enemySkillAttackBarList = new List<Image>();
        private List<Image> playerSkillAttackBarList = new List<Image>();

        private float cureTime = 0;

        private List<float> enemyAttackTime = new List<float>();
        private List<float> playerAttackTime = new List<float>();


        private float playerHp;
        private float enemyHp;


        private const float hpBarLengthP = 420;
        private const float hpBarLengthE = 672;
        private const float skillBarLength = 222;
        private const float cureBarLength = 116.3f;

        private Vector3 playerTextP = new Vector3(-24, 88, 0);
        private Vector3 enemyTextP = new Vector3(-2, -79, 0);

        private bool isOver;

        private AlertPanel alertPanel;

        private List<GameObject> floatTextList;

        private Action<bool> call;

        void Awake()
        {
            enemy = this.transform.FindChild("Enemy").gameObject;
            player = this.transform.FindChild("Player").gameObject;
            effect = this.transform.FindChild("Effect").gameObject;

            cureButton = player.transform.FindChild("CureButton").GetComponent<Button>();
            cureBar = player.transform.FindChild("CureButton/MaskImage/Image").GetComponent<Image>();

            runButton = player.transform.FindChild("RunButton").GetComponent<Button>();

            alertPanel = this.transform.parent.FindChild("AlertPanelPanel").GetComponent<AlertPanel>();

            enemyHpText = enemy.transform.FindChild("HPText").GetComponent<Text>();
            playerHpText = player.transform.FindChild("HPText").GetComponent<Text>();

            //endText = this.transform.FindChild("EndText").GetComponent<Text>();
            //endText.gameObject.SetActive(false);

            playHpBar = player.transform.FindChild("HPProgress").FindChild("MaskImage").FindChild("BarImage").GetComponent<Image>();
            enemyHpBar = enemy.transform.FindChild("HPProgress").FindChild("MaskImage").FindChild("BarImage").GetComponent<Image>();

            //enemyAttackBar = enemy.transform.FindChild("AttackProgress").FindChild("MaskImage").FindChild("BarImage").GetComponent<Image>();
            //enemyAttackBar.transform.localPosition = Vector3.zero;
            GameObject skillP = player.transform.FindChild("Skill").gameObject;
            GameObject skillE = enemy.transform.FindChild("Skill").gameObject;

            for (int i = 0; i < 6; i++)
            {
                Button btnP = skillP.transform.FindChild("SkillButton" + (i + 1).ToString()).GetComponent<Button>();
                playerSkillButtonList.Add(btnP);
            }

            for (int j = 0; j < 3; j++)
            {
                Button btnE = skillE.transform.FindChild("SkillButton" + (j + 1).ToString()).GetComponent<Button>();
                enemySkillButtonList.Add(btnE);
            }
        }

        public void Open(Action<bool> _call)
        {
            call = _call;
            isOver = false;
            floatTextList = new List<GameObject>();
            foreach (Button btn in enemySkillButtonList)
            {
                btn.gameObject.SetActive(false);
            }

            foreach (Button btn in playerSkillButtonList)
            {
                btn.gameObject.SetActive(false);
            }
            playHpBar.transform.localPosition = Vector3.zero;
            enemyHpBar.transform.localPosition = Vector3.zero;
            cureBar.transform.localPosition = Vector3.zero;

            cureButton.onClick.AddListener(OnClickCure);
            runButton.onClick.AddListener(OnClickRun);

            SetInfo();
        }

        private void OnClickCure()
        {
            if (cureTime < BattleModel.CureTime || isOver)
            {
                return;
            }
            cureTime = 0;

            float tempHp = playerHp + BattleModel.CureValue;
            playerHp = tempHp > BattleModel.Instance.PlayerAllHp ? BattleModel.Instance.PlayerAllHp : tempHp;
            CreatFloatText(false, BattleModel.CureValue);
            playHpBar.transform.localPosition = -hpBarLengthP * (1 - (playerHp / BattleModel.Instance.PlayerAllHp)) * Vector3.right;
        }

        private void OnClickRun()
        {
            alertPanel.gameObject.SetActive(true);
            alertPanel.Open(AlertCallBack, false, true);
        }

        void Update()
        {
            if (isOver)
            {
                return;
            }

            for (int i = 0; i < enemyAttackTime.Count; i++)
            {
                enemyAttackTime[i] += Time.deltaTime;

                enemySkillAttackBarList[i].transform.localPosition = skillBarLength * (enemyAttackTime[i] / BattleModel.Instance.GetEnemySpeed(i)) * Vector3.right;
                if (enemyAttackTime[i] >= BattleModel.Instance.GetEnemySpeed(i))
                {
                    EnemyAttack(i);
                    enemyAttackTime[i] = 0;
                }
            }

            for (int i = 0; i < playerAttackTime.Count; i++)
            {
                playerAttackTime[i] += Time.deltaTime;

                playerSkillAttackBarList[i].transform.localPosition = skillBarLength * (playerAttackTime[i] / BattleModel.Instance.GetPlayerSpeed(i)) * Vector3.right;
                if (playerAttackTime[i] >= BattleModel.Instance.GetPlayerSpeed(i))
                {
                    PlayerAttack(i);
                    playerAttackTime[i] = 0;
                }
            }

            enemyHpText.text = enemyHp.ToString() + "/" + BattleModel.Instance.EnemyAllHpList[BattleModel.Instance.CurrIndex].ToString();
            playerHpText.text = playerHp.ToString() + "/" + BattleModel.Instance.PlayerAllHp.ToString();

            if (enemyHp <= 0)
            {
                //endText.gameObject.SetActive(true);
                //endText.text = "胜利";
                alertPanel.gameObject.SetActive(true);
                alertPanel.Open(AlertCallBack, true);
                isOver = true;
            }

            if (playerHp <= 0)
            {
                //endText.gameObject.SetActive(true);
                //endText.text = "失败";
                alertPanel.gameObject.SetActive(true);
                alertPanel.Open(AlertCallBack, false);
                isOver = true;
            }

            if (cureTime < BattleModel.CureTime)
            {
                cureTime += Time.deltaTime;
                cureBar.transform.localPosition = cureBarLength * (cureTime / BattleModel.CureTime) * Vector3.right * -1;
            }


        }

        private void SetInfo()
        {
            enemyAttackTime = new List<float>();
            for (int i = 0; i < BattleModel.Instance.GetCurrEnemyData().Count; i++)
            {
                enemySkillButtonList[i].gameObject.SetActive(true);
                enemySkillButtonList[i].transform.FindChild("Text").GetComponent<Text>().text = BattleModel.Instance.GetCurrEnemyData()[i].Name;
                enemyAttackTime.Add(0);
                enemySkillAttackBarList.Add(enemySkillButtonList[i].transform.FindChild("MaskImage").FindChild("Image").GetComponent<Image>());
                enemySkillAttackBarList[i].transform.localPosition = Vector3.zero;
            }
            playerAttackTime = new List<float>();
            for (int i = 0; i < BattleModel.Instance.PlayList.Count; i++)
            {
                playerSkillButtonList[i].gameObject.SetActive(true);
                playerSkillButtonList[i].transform.FindChild("Text").GetComponent<Text>().text = BattleModel.Instance.PlayList[i].Name;
                playerAttackTime.Add(0);
                playerSkillAttackBarList.Add(playerSkillButtonList[i].transform.FindChild("MaskImage").FindChild("Image").GetComponent<Image>());
                playerSkillAttackBarList[i].transform.localPosition = Vector3.zero;
            }

            playerHp = BattleModel.Instance.PlayerAllHp;
            enemyHp = BattleModel.Instance.EnemyAllHpList[BattleModel.Instance.CurrIndex];
        }

        private void EnemyAttack(int _index)
        {
            if (BattleModel.Instance.GetCurrEnemyData()[_index].Damage > playerHp)
            {
                playerHp = 0;
            }
            else
            {
                playerHp -= BattleModel.Instance.GetCurrEnemyData()[_index].Damage;
            }
            playHpBar.transform.localPosition = -hpBarLengthP * (1 - (playerHp / BattleModel.Instance.PlayerAllHp)) * Vector3.right;
            CreatFloatText(false, -BattleModel.Instance.GetCurrEnemyData()[_index].Damage);
        }

        private void PlayerAttack(int _index)
        {
            switch (BattleModel.Instance.GetSkillData(BattleModel.Instance.PlayList[_index].SkillId).SkillType)
            {
                case EnumSkill.Attack:
                    if (BattleModel.Instance.PlayList[_index].Damage > enemyHp)
                    {
                        enemyHp = 0;
                    }
                    else
                    {
                        enemyHp -= BattleModel.Instance.PlayList[_index].Damage;
                    }
                    enemyHpBar.transform.localPosition = -hpBarLengthE * (1 - (enemyHp / BattleModel.Instance.EnemyAllHpList[BattleModel.Instance.CurrIndex])) * Vector3.right;
                    CreatFloatText(true, -BattleModel.Instance.PlayList[_index].Damage);
                    break;
                case EnumSkill.Cure:
                    float tempHp = playerHp + BattleModel.Instance.PlayList[_index].Damage;
                    playerHp = tempHp > BattleModel.Instance.PlayerAllHp ? BattleModel.Instance.PlayerAllHp : tempHp;
                    playHpBar.transform.localPosition = -hpBarLengthP * (1 - (playerHp / BattleModel.Instance.PlayerAllHp)) * Vector3.right;
                    CreatFloatText(false, BattleModel.Instance.PlayList[_index].Damage);
                    break;
            }
            effect.GetComponent<EffectControl>().ShowEffect(BattleModel.Instance.GetSkillData(BattleModel.Instance.PlayList[_index].SkillId).SkillType);
        }

        private void CreatFloatText(bool _isUp, float _count)
        {
            //GameObject text = GameObject.Instantiate(Resources.Load("Prefabs/BattleSceneUI/FloatText")) as GameObject;
            GameObject text = GameObject.Instantiate(FloatTextPrefab) as GameObject;
            text.GetComponent<FloatText>().IsUp = _isUp;
            string sign = _count > 0 ? "+" : "";
            text.GetComponent<FloatText>().SetText(sign + _count.ToString());
            if (_isUp)
            {
                text.transform.parent = enemy.transform;
                text.transform.localScale = Vector3.one;
                text.transform.localPosition = enemyTextP;
            }
            else
            {
                text.transform.parent = player.transform;
                text.transform.localScale = Vector3.one;
                text.transform.localPosition = playerTextP;
            }
            floatTextList.Add(text);
        }

        private void AlertCallBack(bool _isWin)
        {
            if (call != null)
            {
                call(_isWin);
            }
            Close();
        }

        private void Close()
        {
            this.gameObject.SetActive(false);

            foreach (GameObject g in floatTextList)
            {
                GameObject.Destroy(g);
            }
            cureButton.onClick.RemoveListener(OnClickCure);
            runButton.onClick.RemoveListener(OnClickRun);
        }
    }

}