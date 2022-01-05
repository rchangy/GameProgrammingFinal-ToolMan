using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolMan.UI
{
    public class HintNotRead : MonoBehaviour
    {
        [SerializeField]
        private List<string> unreadHint = new List<string>();
        UIController uIController;
       
        public void UnreadHint(string title) {
            unreadHint.Add(title);
            if (unreadHint.Count == 1)
                GetComponent<ShowNotification>().StartObjective();
        }

        public void ReadHint(string title) {
            int cnt = unreadHint.Count;
            unreadHint.Remove(title);
            if (unreadHint.Count == 0 && cnt > 0)
                GetComponent<HideNotification>().StartObjective();
        }
    }
}
