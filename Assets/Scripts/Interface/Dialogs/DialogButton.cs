using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Interface.Dialogs
{
    public class DialogButton : MonoBehaviour
    {
        public Sprite NormalButtonImage;
        public Sprite PressedButtonImage;
        public Image Source;
        public Button button;

        private void Awake()
        {
            Source.sprite = NormalButtonImage;
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonHoldPressed()
        {
            Source.sprite = PressedButtonImage;
        }

        private void OnButtonClick()
        {
            AudioManager.instance.PlaySelect();
        }
    }
}
