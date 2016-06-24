using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
namespace Battle
{
    public class EnterBattlePanel : MonoBehaviour
    {
        private Button fightButton;
        private Button quitButton;

        private Text text;

        private Action call;

        private Action call2;

        void Awake()
        {
            text = this.transform.FindChild("Text").GetComponent<Text>();
            quitButton = this.transform.FindChild("QuitButton").GetComponent<Button>();
            fightButton = this.transform.FindChild("FightButton").GetComponent<Button>();

        }

        public void Open(List<EnemyData> _list, Action _call, Action _call2)
        {
            AddEvent();
            call = _call;
            call2 = _call2;
            text.text = "你遭遇了";
            Dictionary<string, int> nameDic = new Dictionary<string, int>();
            foreach (EnemyData data in _list)
            {
                if (nameDic.ContainsKey(data.Name))
                {
                    nameDic[data.Name]++;
                }
                else
                {
                    nameDic.Add(data.Name, 1);
                }
            }

            foreach (KeyValuePair<string, int> kvp in nameDic)
            {
                text.text += kvp.Key + "*" + kvp.Value.ToString();
            }
            text.text += ",你决定。。。";
        }

        private void AddEvent()
        {
            quitButton.onClick.AddListener(OnQuit);
            fightButton.onClick.AddListener(OnFight);
        }

        private void OnQuit()
        {
            if (call2 != null)
            {
                call2();
                Close();
            }
        }

        private void OnFight()
        {
            if (call != null)
            {
                call();
                Close();
            }
        }

        private void Close()
        {
            quitButton.onClick.RemoveListener(OnQuit);
            fightButton.onClick.RemoveListener(OnFight);
            this.gameObject.SetActive(false);
        }
    }

}