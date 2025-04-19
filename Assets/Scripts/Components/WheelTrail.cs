using UnityEngine;

public class WheelTrail : MonoBehaviour
{
    [SerializeField] private TrailRenderer _fLeftTrail;
    [SerializeField] private TrailRenderer _rLeftTrail;
    [SerializeField] private TrailRenderer _fRightTrail;
    [SerializeField] private TrailRenderer _rRightTrail;
    [SerializeField] private float _trailMinVelocity = 0.5f;
    private PlayerCarController _carController;

    private void Start()
    {
        _carController = GetComponent<PlayerCarController>();
    }

    private void Update()
    {
        bool trailEnabled = _carController.IsHandbrakeActive || _carController.SlipVelocity.magnitude > _trailMinVelocity;
        _rLeftTrail.emitting = trailEnabled;
        _rRightTrail.emitting = trailEnabled;

        _fLeftTrail.emitting = _carController.IsBraking;
        _fRightTrail.emitting = _carController.IsBraking;
    }

    public bool IsTrailActive()
    {
        return GameManager.Instance.Timer > 0 && (_rLeftTrail.emitting || _rRightTrail.emitting || _fLeftTrail.emitting || _fRightTrail.emitting);
    }
}
