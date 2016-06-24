using UnityEngine;
using System.Collections;
namespace Battle
{
    public class EffectControl : MonoBehaviour
    {
        private GameObject effect1;
        private GameObject effect2;
        void Awake()
        {
            effect1 = this.transform.FindChild("Effect1").gameObject;
            effect2 = this.transform.FindChild("Effect2").gameObject;
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //if (animateInfo.nameHash == Animator.StringToHash("Base Layer.start"))
            //{
            //    if (animateInfo.normalizedTime >= 1.0f)
            //    {
            //        isCanStart = false;
            //        Controller.Instance.SendNotification(SceneManager.SCENEMANAGER_SCENE_START_CHANGE, null, null);
            //    }
            //}

        }

        public void ShowEffect(EnumSkill _enum)
        {
            switch (_enum)
            {
                case EnumSkill.Attack:
                    effect1.gameObject.SetActive(true);
                    effect1.GetComponent<Animator>().Play("play", 0, 0);
                    effect1.GetComponent<Effect>().EndTime = 1;
                    effect1.GetComponent<Effect>().IsStart = true;
                    break;
                case EnumSkill.Cure:
                    effect2.gameObject.SetActive(true);
                    effect2.GetComponent<Animator>().Play("play", 0, 0);
                    effect2.GetComponent<Effect>().EndTime = 1;
                    effect2.GetComponent<Effect>().IsStart = true;
                    break;
            }
        }
    }

}