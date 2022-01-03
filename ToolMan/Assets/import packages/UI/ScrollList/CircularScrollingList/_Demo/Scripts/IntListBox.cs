using AirFishLab.ScrollingList;
using UnityEngine;
using UnityEngine.UI;
using ToolMan.UI;
using System.Collections.Generic;

namespace AirFishLab.ScrollingList.Demo
{
    public class IntListBox : ListBox
    {
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
            //_contentText.text = ((int) content).ToString();
            Hint hint = _hints[(int)content - 1];
            boxName = (hint.locked)? _lockedHint._title : _hints[(int)content-1]._title;
            _contentText.text = boxName;
        }
    }
}
