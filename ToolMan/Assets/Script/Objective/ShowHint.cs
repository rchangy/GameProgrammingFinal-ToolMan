using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;

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
            _p1.controlEnable = false;
            _p2.controlEnable = false;
            _uIController.SetControlEnable(true);

            // Unlock some hints
            foreach (Hint h in hintsToUnlock) {
                uIController.hintPanel.UnlockHint(h._title);
                if (h._title != hintToShow._title)
                    uIController.hintNotRead.UnreadHint(h._title);
            }
            uIController.hintPanel.LoadHint(hintToShow._title);
            uIController.SetHintUI(true);
            StartCoroutine(WaitForEsc());
        }

        public override bool isCompleted()
        {
            return _isCompleted;
        }

        IEnumerator WaitForEsc() {
            while (uIController.showingTutorialUI) { yield return null; }
            _isCompleted = true;
            yield return null;
        }
    }
}
