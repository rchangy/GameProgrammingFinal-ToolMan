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
        private string[] _names = {"提示", "移動、跳躍", "變身", "十字鎬", "閃光彈", "轉視角", "水晶", "治療水晶", "抓取", "攻擊",
        "記憶水晶", "光劍", "工具選擇", "鍵位——人型", "鍵位——工具", "龜龜", "迴力鏢", "大雞、小雞", "史萊姆兔", "護盾", "狗勾將軍", "族邸鯨魔閻",};
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
