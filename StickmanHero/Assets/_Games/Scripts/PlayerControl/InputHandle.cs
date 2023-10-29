using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperFight
{
    public class InputHandle : MonoBehaviour
    {
        [SerializeField] InputType inputType;
        public TouchButton jumpButton;
        public TouchButton comboButton;
        public TouchButton skillButton;
        public TouchButton dashButton;
        public Joystick thumbStick;
        public StaticThumb staticThumb;
        public float horizontal;
        public bool jumpPressed;
        public bool comboPressed;
        public bool skillPressed;
        public bool dashPressed;
        float attackTimer;
        bool readyToClear;
        bool isActive;
        private void Start()
        {
            // if (Application.isMobilePlatform)
            // {
#if UNITY_STANDALONE || UNITY_EDITOR
            inputType = InputType.KEYBOARD;
#else
            inputType = InputType.TOUCH;
#endif
            // }
            // else
            // {
            //}
        }
        public void ResetInput()
        {
            if (staticThumb != null)
            {
                staticThumb.InputExit();
            }
            horizontal = 0;
            jumpPressed = false;
            readyToClear = false;
            skillPressed = false;
        }
        public void ActiveInput()
        {
            isActive = true;
        }
        public void Deactive()
        {
            isActive = false;
        }
        private void Update()
        {
            if (GameplayCtrl.Instance.gameState != GAME_STATE.GS_PLAYING || !isActive)
            {
                ClearInputs();
                return;
            }
            ClearInputs();
            ProcessInput();
        }
        void ProcessInput()
        {
            if (inputType == InputType.KEYBOARD)
            {
                horizontal = Input.GetAxisRaw("Horizontal");
                jumpPressed = jumpPressed || Input.GetKeyDown(KeyCode.Space);
                skillPressed = skillPressed || Input.GetKeyDown(KeyCode.R);
                dashPressed = dashPressed || Input.GetKeyDown(KeyCode.LeftShift);
                comboPressed = comboPressed || Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0);
                
            }
            else if (inputType == InputType.TOUCH)
            {
                horizontal = staticThumb.horizontal;
                jumpPressed = jumpPressed || jumpButton.GetButtonDown();
                skillPressed = skillPressed || skillButton.GetButtonDown();
                dashPressed = dashPressed || dashButton.GetButtonDown();
                comboPressed = comboPressed || comboButton.GetButtonDown();
            }
        }
        public void ClearInputs()
        {
            if (!readyToClear)
            {
                return;
            }
            horizontal = 0;
            comboPressed = false;
            jumpPressed = false;
            readyToClear = false;
            skillPressed = false;
            dashPressed = false;
        }
        private void FixedUpdate()
        {
            readyToClear = true;
        }
    }

    public enum InputType
    {
        KEYBOARD, TOUCH
    }
}

