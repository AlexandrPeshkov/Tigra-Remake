using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Interface.Dialogs
{
    class RetryDialog : MonoBehaviour
    {
        public DialogButton OkButton;
        public DialogButton CancellButton;

        private void Awake()
        {
            OkButton.button.onClick.AddListener(OnOkButtonClick);
            CancellButton.button.onClick.AddListener(OnOkButtonClick);
        }

        private void OnOkButtonClick()
        {
            Destroy(gameObject);
        }
    }
}
