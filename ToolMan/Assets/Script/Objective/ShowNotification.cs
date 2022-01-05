using UnityEngine;

namespace ToolMan.UI
{
    public class ShowNotification : Objective
    {
        [SerializeField]
        private Notification _noti1;
        [SerializeField]
        private Notification _noti2;
        [SerializeField]
        string content;

        private bool _isCompleted;

        protected override void Init()
        {
            _isCompleted = false;
        }

        public override void StartObjective()
        {
            if (uIController)
                uIController.SetNotificationUI(true);
            //_p1.controlEnable = true;
            //_p2.controlEnable = true;
            //_uIController.SetControlEnable(true);

            // Display notifications
            _noti1.OnStart(content);
            if (_noti2)
                _noti2.OnStart(content);
            _isCompleted = true;
        }

        public override bool isCompleted()
        {
            return _isCompleted;
        }
    }
}
