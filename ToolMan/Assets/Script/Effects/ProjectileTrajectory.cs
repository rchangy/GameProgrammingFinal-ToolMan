using UnityEngine;
using System.Collections.Generic;
using ToolMan.Combat.Skills.Normal;

public class ProjectileTrajectory : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    private int _lineSegmentCount = 28;
    private List<Vector3> _linePoints = new List<Vector3>();

    [SerializeField] private PlayerController _toolController;
    private GameObject _man;
    [SerializeField] private FlashBombSkill _flashBomb;

    private float _force;
    private float _range;

    private float _rbMass = 1f;

    #region Singleton
    public static ProjectileTrajectory Instance;
    private void Awake()
    {
        Instance = this;
        _force = _flashBomb.Force;
        _range = _flashBomb.ExplosionRange;
        _man = _toolController.GetAnotherPlayer().gameObject;
    }
    #endregion

    private void FixedUpdate()
    {
        if (!(_toolController.IsGrabbed() && _toolController.getTool().getName() == "FlashBomb"))
        {
            ProjectileTrajectory.Instance.HideLine();
            return;
        }
        UpdateTrajectory();
    }

    public void UpdateTrajectory()
    {
        Vector3 dir = _man.transform.forward;
        dir.y = 0;
        dir = Vector3.Normalize(dir);
        dir.y = 1;
        dir *= _force;

        Vector3 toolPos = _toolController.transform.position;
        Vector3 vel = (dir / _rbMass) * Time.fixedDeltaTime;
        float flightDuration = (2 * vel.y) / Physics.gravity.y;
        float stepTime = flightDuration / _lineSegmentCount;

        _linePoints.Clear();

        for(int i = 0; i < _lineSegmentCount; i++)
        {
            float stepTimePassed = stepTime * i;
            Vector3 moveVec = new Vector3
            (
                vel.x * stepTimePassed,
                vel.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                vel.z * stepTimePassed
            );

            _linePoints.Add(toolPos - moveVec);
        }

        _line.positionCount = _linePoints.Count;
        _line.SetPositions(_linePoints.ToArray());
    }

    private void HideLine()
    {
        _line.positionCount = 0;
    }
}
