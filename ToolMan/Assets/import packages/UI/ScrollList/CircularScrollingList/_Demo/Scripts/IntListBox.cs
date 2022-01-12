using AirFishLab.ScrollingList;
using UnityEngine;
using UnityEngine.UI;
using ToolMan.UI;
using System.Collections.Generic;

namespace AirFishLab.ScrollingList.Demo
{
    public class IntListBox : ListBox
    {
        public UIController uIController;
        [SerializeField]
        private Button button;
        public Hint hint;
        private GameObject _hintObj;
        private Hint[] _hints;
        private Hint _lockedHint;
        public string boxName;
        [SerializeField]
        private Text _contentText;

        public void OnStart(GameObject hintObj, Hint lockedHint)
        {
            _hintObj = hintObj;
            _lockedHint = lockedHint;
            _hints = _hintObj.GetComponentsInChildren<Hint>();
        }

        protected override void UpdateDisplayContent(object content)
        {
           boxName = (hint.locked)? _lockedHint._title : hint._title;
            _contentText.text = boxName;
            //Debug.Log("hint " + hint._title + "update content = " + boxName);
        }

        public void ButtonLoadHint() {
            if (uIController)
                uIController.hintPanel.LoadHint(hint._title);
        }
    }
}
