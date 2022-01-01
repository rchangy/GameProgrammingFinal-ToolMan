using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace ToolMan.UI
{
    public class ShowHint : Objective
    {
        [SerializeField]
        private Hint hintToShow;
        [SerializeField]
        private List<Hint> hintsToUnlock;

        private bool _isCompleted;

        protected override void Init()
        {
            _isCompleted = false;
        }

        public override void StartObjective()
        {
            // Unlock some hints
            Debug.Log("hi:)))");
            foreach (Hint h in hintsToUnlock) { uIController.hintPanel.UnlockHint(h._title); }
            Debug.Log("hi:)))");
            uIController.hintPanel.LoadHint(hintToShow._title);
            uIController.SetTutorialUI(true);
            //StartCoroutine(WaitForEsc());
        }

        public override bool isCompleted()
        {
            return _isCompleted;
        }

        IEnumerator WaitForEsc() {
            while (uIController.showingTutorialUI) { }
            _isCompleted = true;
            yield return null;
        }
    }
}
