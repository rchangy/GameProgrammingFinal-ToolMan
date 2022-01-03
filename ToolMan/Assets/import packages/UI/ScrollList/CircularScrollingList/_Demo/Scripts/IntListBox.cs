using AirFishLab.ScrollingList;
using UnityEngine;
using UnityEngine.UI;
using ToolMan.UI;
using System.Collections.Generic;

namespace AirFishLab.ScrollingList.Demo
{
    public class IntListBox : ListBox
    {
        //[SerializeField]
        //private List<Hint> _hints;
        private string[] _names = {"提示", "移動、跳躍", "變身", "十字鎬", "閃光彈", "轉視角", "水晶", "治療水晶", "抓取", "攻擊",
        "記憶水晶", "光劍", "工具選擇", "鍵位——人型", "鍵位——工具", "龜龜", "迴力鏢", "大雞、小雞", "史萊姆兔", "護盾", "狗勾將軍", "族邸鯨魔閻",};
        [SerializeField]
        private Text _contentText;

        protected override void UpdateDisplayContent(object content)
        {
            //_contentText.text = ((int) content).ToString();
            _contentText.text = _names[(int)content-1];
        }
    }
}
