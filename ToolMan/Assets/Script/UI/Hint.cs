using UnityEngine;
using UnityEngine.UI;

namespace ToolMan.UI
{
    public class Hint : MonoBehaviour
    {
        public string _title;
        public Image _image;
        public string _content;
        public bool locked
        {
            get => _locked;
        }
        public int order {
            get => _order;
        }

        [SerializeField]
        private bool _locked;
        [SerializeField]
        private int _order;

        public void Unlock() { _locked = false; }
    }
}
