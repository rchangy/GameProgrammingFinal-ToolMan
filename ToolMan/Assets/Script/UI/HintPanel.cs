using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace ToolMan.UI
{
    public class HintPanel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _title;
        [SerializeField]
        private Image _image;
        [SerializeField]
        private TextMeshProUGUI _content;

        [SerializeField]
        private GameObject hintsObj;
        [SerializeField]
        private List<Hint> _hints;
        [SerializeField]
        private GameObject buttonObj;

        [SerializeField]
        private Hint lockedHint;

        private void Awake()
        {
            Init();
        }

        private void Init() {
            _hints.Clear();

            Hint[] hs = hintsObj.transform.GetComponentsInChildren<Hint>();
            
            Array.Sort( hs, delegate (Hint h1, Hint h2){ return h1.order.CompareTo(h2.order); } );
            Button[] bs = buttonObj.transform.GetComponentsInChildren<Button>();
            Array.Sort(bs, delegate (Button b1, Button b2) { return b1.name.CompareTo(b2.name); });
            for (int i=0; i<hs.Length; i++)
            {
                Hint h = hs[i];
                _hints.Add(h);

                //Debug.Log("i = " + i);
                Button b = bs[i];
                b.onClick.AddListener(delegate { LoadHint(h._title); });

                //Debug.Log("h = " + h._title + " b = " + b.name);
            }

            //CheckpointManager.LoadCheckpoint();
            //int lastUnlockedHint =  CheckpointManager.GetCheckpointInfo().lastUnlockedHint;
            //for (int i = 0; i < lastUnlockedHint; i++) _hints[i].Unlock();
            for (int i = 0; i < _hints.Count; i++) _hints[i].Unlock();
        }

        public void LoadHint(string hintTitle) {
            Hint hint = _hints.Find(h => h._title == hintTitle);
            //Debug.Log("load hint = " + hint._title);
            if (hint.locked) {
                _title.text = lockedHint._title;
                _image.sprite = lockedHint._sprite;
                _content.text = lockedHint._content;
            }
            else
            {
                _title.text = hint._title;
                _image.sprite = hint._sprite;
                _content.text = hint._content;
            }   
        }

        public void UnlockHint(string hintTitle) { _hints.Find(h => h._title == hintTitle).Unlock(); }
    }
}
