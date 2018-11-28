namespace Urq
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Utilities;


    /// <summary>
    /// Gets inputs for a particular kind of inputs when requested
    /// </summary>
    public static class InputManagerHelper
    {
        public enum ControllerType
        {
            LocalPlayer1,
            LocalPlayer2,
            ComputerEasy,
            ComputerNormal,
            ComputerHard,
        }

        public enum Buttons
        {
            Button1,
            Button2,
            Button3
        }

        public delegate void InputGetter(IInputRequester requester, bool useRaw);

        public static ControlInputs GetCurrentControlInputs(ControllerType type, IInputRequester requester, bool useRaw = false)
        {
            switch (type)
            {
                case ControllerType.LocalPlayer1:
                    return GetLocalPlayer1Inputs(requester, useRaw);
                case ControllerType.LocalPlayer2:
                    return GetLocalPlayer2Inputs(requester, useRaw);
                default:
                    Utilities.Debug.LogError("Hay we fell into a default controller type!");
                    return GetDefaultInputs(requester, useRaw);
            }
        }

        private static InputManagerHelper.ControlInputs GetDefaultInputs(IInputRequester requester, bool useRaw)
        {
            //No inputs here boss man
            return new InputManagerHelper.ControlInputs(0, 0, 0, 0, 0);
        }

        private static InputManagerHelper.ControlInputs GetLocalPlayer1Inputs(IInputRequester requester, bool useRaw)
        {
            if (useRaw)
            {
                float horizontalInput = Input.GetAxisRaw(InputManagerConsts.Player1Horizontal);
                float verticalInput = Input.GetAxisRaw(InputManagerConsts.Player1Vertical);
                float fire1 = Input.GetAxisRaw(InputManagerConsts.Player1Fire1);
                float fire2 = Input.GetAxisRaw(InputManagerConsts.Player1Fire2);
                float fire3 = Input.GetAxisRaw(InputManagerConsts.Player1Fire3);

                return new InputManagerHelper.ControlInputs(horizontalInput, verticalInput, fire1, fire2, fire3);
            }
            else
            {

                float horizontalInput = Input.GetAxis(InputManagerConsts.Player1Horizontal);
                float verticalInput = Input.GetAxis(InputManagerConsts.Player1Vertical);
                float fire1 = Input.GetAxis(InputManagerConsts.Player1Fire1);
                float fire2 = Input.GetAxis(InputManagerConsts.Player1Fire2);
                float fire3 = Input.GetAxis(InputManagerConsts.Player1Fire3);

                return new InputManagerHelper.ControlInputs(horizontalInput, verticalInput, fire1, fire2, fire3);
            }
        }

        private static InputManagerHelper.ControlInputs GetLocalPlayer2Inputs(IInputRequester requester, bool useRaw)
        {
            if (useRaw)
            {
                float horizontalInput = Input.GetAxisRaw(InputManagerConsts.Player2Horizontal);
                float verticalInput = Input.GetAxisRaw(InputManagerConsts.Player2Vertical);
                float fire1 = Input.GetAxisRaw(InputManagerConsts.Player2Fire1);
                float fire2 = Input.GetAxisRaw(InputManagerConsts.Player2Fire2);
                float fire3 = Input.GetAxisRaw(InputManagerConsts.Player2Fire3);

                return new InputManagerHelper.ControlInputs(horizontalInput, verticalInput, fire1, fire2, fire3);
            }
            else
            {
                float horizontalInput = Input.GetAxis(InputManagerConsts.Player2Horizontal);
                float verticalInput = Input.GetAxis(InputManagerConsts.Player2Vertical);
                float fire1 = Input.GetAxis(InputManagerConsts.Player2Fire1);
                float fire2 = Input.GetAxis(InputManagerConsts.Player2Fire2);
                float fire3 = Input.GetAxis(InputManagerConsts.Player2Fire3);

                return new InputManagerHelper.ControlInputs(horizontalInput, verticalInput, fire1, fire2, fire3);
            }
        }

        /// <summary>
        /// Represents all the inputs a ship can do
        /// </summary>
        public class ControlInputs
        {
            public ControlInputs(float horizontalInput, float verticalInput, float button1, float button2, float button3)
            {
                HorizontalInput = horizontalInput;
                VerticalInput = verticalInput;
                Button1 = button1;
                Button2 = button2;
                Button3 = button3;
            }

            public float GetButtonValue(Buttons targetButton)
            {
                switch (targetButton)
                {
                    case InputManagerHelper.Buttons.Button1:
                        return Button1;
                    case InputManagerHelper.Buttons.Button2:               
                        return Button2;
                    case InputManagerHelper.Buttons.Button3:
                        return Button3;
                }

                Utilities.Debug.LogError("Hay somehow we didnt get the button we wanted!");
                return 0;
            }

            public float HorizontalInput
            {
                get;
                private set;
            }

            public float VerticalInput
            {
                get;
                private set;
            }

            public float Button1
            {
                get;
                private set;
            }

            public float Button2
            {
                get;
                private set;
            }

            public float Button3
            {
                get;
                private set;
            }
        }


        /// <summary>
        /// Marks something that can request inputs - just to differentiate from everything else
        /// </summary>
        public interface IInputRequester
        {
           
        }

    }
}