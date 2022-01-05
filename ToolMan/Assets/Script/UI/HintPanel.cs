using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using AirFishLab.ScrollingList.Demo;

namespace ToolMan.UI
{
    public class HintPanel : MonoBehaviour
    {
        [SerializeField]
        private UIController _uIController;
        [SerializeField]
        private TextMeshProUGUI _title;
        [SerializeField]
        private Image _image;
        [SerializeField]
        private TextMeshProUGUI _content;

        [SerializeField]
        private GameObject hintsObj;
        public List<Hint> _hints;
        [SerializeField]
        private GameObject buttonObj;

        [SerializeField]
        private Hint lockedHint;

        [SerializeField]
        private HintNotRead hintNotRead;

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            CheckpointUnlockHints();
            //for (int i = 0; i < _hints.Count; i++) _hints[i].Unlock();
        }

        private void Init() {
            //_hints.Clear();

            //Hint[] hs = _hints.ToArray();
            //Hint[] hs = hintsObj.transform.GetComponentsInChildren<Hint>(true);
            //Debug.Log("hs len =" + hs.Length );

            //Array.Sort( hs, delegate (Hint h1, Hint h2){ return h1.order.CompareTo(h2.order); } );
            IntListBox[] boxes = buttonObj.transform.GetComponentsInChildren<IntListBox>(true);
            Button[] bs = buttonObj.transform.GetComponentsInChildren<Button>(true);
            Array.Sort(bs, delegate (Button b1, Button b2) { return b1.name.CompareTo(b2.name); });

            //Debug.Log("bs len = " + bs.Length);

            for (int i=0; i<bs.Length; i++)
            {
                //Hint h = hs[i];
                //_hints.Add(h);
                //Debug.Log("add hint " + h.name);

                //Debug.Log("bs i = " + i);
                IntListBox box = boxes[i];
                box.uIController = _uIController;
                Button b = bs[i];
                b.onClick.AddListener(delegate {  box.ButtonLoadHint(); });
                b.onClick.AddListener(delegate { Read(box.hint._title); });

                //CheckpointUnlockHints();
                //Debug.Log("h = " + h._title + " b = " + b.name);
            }

            
            //for (int i = 0; i < _hints.Count; i++) _hints[i].Unlock();
        }

        public void CheckpointUnlockHints() {
            CheckpointManager.LoadCheckpoint();
            Debug.Log("check unlock " + CheckpointManager.GetCheckpointInfo().level);
            int lastUnlockedHint = CheckpointManager.GetCheckpointInfo().lastUnlockedHint;
            for (int i = 0; i < lastUnlockedHint; i++) { _hints[i].Unlock(); Debug.Log("last = " + lastUnlockedHint + "unlock hint " + _hints[i]); }
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

        public void Read(string title) { hintNotRead.ReadHint(title); }
    }
}
