using UnityEngine;

namespace ToolMan.UI
{
    public class HideNotification : Objective
    {
        [SerializeField]
        private Notification _noti1;
        [SerializeField]
        private Notification _noti2;

        private bool _isCompleted;

        protected override void Init()
        {
            _isCompleted = false;
        }

        public override void StartObjective()
        {
            //_p1.controlEnable = true;
            //_p2.controlEnable = true;
            //_uIController.SetControlEnable(true);

            // Display notifications
            _noti1.OnComplete();
            if (_noti2)
                _noti2.OnComplete();
            _isCompleted = true;
        }

        public override bool isCompleted()
        {
            return _isCompleted;
        }
    }
}
