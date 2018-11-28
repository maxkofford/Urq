namespace Urq
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Represents one players selection stuff
    /// </summary>
    public class MultiSelect : MonoBehaviour , InputManagerHelper.IInputRequester
    {
        public Selectable startingSelection;       
        public InputManagerHelper.ControllerType myController;
        public float biggerAmount = 1;

        private Selectable currentSelection;
        private float setHorizontal = 0;
        private float setVertical = 0;

        public void Select(Selectable sel)
        {
            if (sel != null)
            {
                this.transform.position = sel.transform.position;

                RectTransform myRect = this.GetComponent<RectTransform>();
                RectTransform theirRect = sel.GetComponent<RectTransform>();
                myRect.sizeDelta = theirRect.sizeDelta + new Vector2(biggerAmount, biggerAmount);
                currentSelection = sel;
            }
        }


        private void Start()
        {
            //currentSelection = startingSelection;
            Select(startingSelection);
        }
       

        private void Update()
        {
           var inputs = InputManagerHelper.GetCurrentControlInputs(myController, this, true);
            if (setHorizontal == 0)
            {
                //move the stuff around
                if (inputs.HorizontalInput > 0)
                {
                    Select(currentSelection.FindSelectableOnRight());
                }
                else if (inputs.HorizontalInput < 0)
                {
                    Select(currentSelection.FindSelectableOnLeft());
                }
            }

            if (setVertical == 0)
            {
                //move the stuff around
                if (inputs.VerticalInput > 0)
                {
                    Select(currentSelection.FindSelectableOnUp());
                }
                else if (inputs.VerticalInput < 0)
                {
                    Select(currentSelection.FindSelectableOnDown());
                }
            }

            setHorizontal = inputs.HorizontalInput;
            setVertical = inputs.VerticalInput;
        }
    }
}
