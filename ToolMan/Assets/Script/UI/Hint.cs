using UnityEngine;
using UnityEngine.UI;

namespace ToolMan.UI
{
    public class Hint : MonoBehaviour
    {
        public string _title;
        public Sprite _sprite;
        public string _content;
        public bool locked
        {
            get => _locked;
        }
        public int order {
            get => _order;
        }

        [SerializeField]
        private bool _locked = true;
        [SerializeField]
        private int _order;

        private void Awake()
        {
            _locked = true;
            Debug.Log("unlock:( locked, title = " + _title);
        }

        public void Unlock() { _locked = false; Debug.Log("unlock:( hint = " + _title + " locked = " + locked); }
    }
}
