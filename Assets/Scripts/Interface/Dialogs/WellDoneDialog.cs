using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Interface.Dialogs
{
    public class WellDoneDialog : MonoBehaviour
    {
        public DialogButton OkButton;

        private void Awake()
        {
            OkButton.button.onClick.AddListener(OnOkButtonClick);
        }

        private void OnOkButtonClick()
        {
            Destroy(gameObject);
        }
    }
}
