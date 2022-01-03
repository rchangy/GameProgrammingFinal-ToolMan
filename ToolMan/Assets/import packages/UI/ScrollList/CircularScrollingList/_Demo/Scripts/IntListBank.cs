using UnityEngine;
using System.Collections.Generic;
using AirFishLab.ScrollingList.Demo;
using ToolMan.UI;
namespace AirFishLab.ScrollingList.Demo
{
    public class IntListBank : BaseListBank
    {
        public GameObject hintObj;
        public Hint lockedHint;
        [SerializeField]
        private List<ListBox> _boxes;

        private void Awake()
        {
            //_boxes = GetComponentsInChildren<IntListBox>();
            _boxes = GetComponent<CircularScrollingList>()._listBoxes;
            foreach (ListBox b in _boxes) { b.GetComponent<IntListBox>().OnStart(hintObj, lockedHint); }
        }
        private readonly int[] _contents = {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22
        };

        public override object GetListContent(int index)
        {
            return _contents[index];
        }

        public override int GetListLength()
        {
            return _contents.Length;
        }
    }
}
