namespace Urq
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class TestButton : MonoBehaviour
    {
        [AutomaticEditorButton]
        public string test1 = "Test1";

        [AutomaticEditorButton]
        public string test2 = "Test2";

        [AutomaticEditorButton]
        public string test3 = "Test3";

        [AutomaticEditorButton]
        public string test4 = "Test4";

        [AutomaticEditorButton]
        public string test5 = "Test5";

        public void Test1()
        {
          Button b = this.GetComponent<Button>();
          b.FindSelectableOnDown().GetComponent<Image>().color = Color.blue;
        }

        public void Test2()
        {
            Button b = this.GetComponent<Button>();
            b.FindSelectableOnUp().GetComponent<Image>().color = Color.green;
        }

        public void Test3()
        {
            Button b = this.GetComponent<Button>();
            b.FindSelectableOnRight().GetComponent<Image>().color = Color.yellow;
        }

        public void Test4()
        {
            Button b = this.GetComponent<Button>();
            b.FindSelectableOnLeft().GetComponent<Image>().color = Color.magenta;
        }

        public void Test5()
        {
            List<int> fish = new List<int>();
            fish.Add(0);
            fish.Add(1);
            foreach (var s in fish)
            {
                fish.Remove(s);
            }
        }
    }
}