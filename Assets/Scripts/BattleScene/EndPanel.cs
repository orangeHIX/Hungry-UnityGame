using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
namespace Battle
{
    public class EndPanel : MonoBehaviour
    {

        private Button quitButton;
        private Action call;


        void Awake()
        {
            quitButton = this.transform.FindChild("Button").GetComponent<Button>();
            AddEvent();
        }

        private void AddEvent()
        {
            quitButton.onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            if (call != null)
            {
                call();
                Close();
            }
        }

        public void Open(Action _call)
        {
            call = _call;
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