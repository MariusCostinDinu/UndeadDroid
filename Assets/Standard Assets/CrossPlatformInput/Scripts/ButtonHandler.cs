using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class ButtonHandler : MonoBehaviour
    {

        public string Name;
        public GameObject text;

        void OnEnable()
        {

        }

        private void Update()
        {
            ((RectTransform)transform).anchorMin = new Vector2(1 - 0.2f * Screen.height / Screen.width, 0.02f);
            //text.transform.localScale = Vector3.one * Screen.height / Screen.width;
            text.GetComponent<Text>().fontSize = 26 * Screen.height / Screen.width;
        }

        public void SetDownState()
        {
            CrossPlatformInputManager.SetButtonDown(Name);
        }


        public void SetUpState()
        {
            CrossPlatformInputManager.SetButtonUp(Name);
        }


        public void SetAxisPositiveState()
        {
            CrossPlatformInputManager.SetAxisPositive(Name);
        }


        public void SetAxisNeutralState()
        {
            CrossPlatformInputManager.SetAxisZero(Name);
        }


        public void SetAxisNegativeState()
        {
            CrossPlatformInputManager.SetAxisNegative(Name);
        }
    }
}
