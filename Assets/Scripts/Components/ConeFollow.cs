using UnityEngine;

public class ConeFollow : MonoBehaviour
{
    private GameObject _followTarget;
    private Vector2 _currentVelocity;

    [SerializeField] private float _followSpeed;
    [SerializeField] private float _followDistance;

    private void Start()
    {
        _followTarget = null;
    }

    public void SetFollowTarget(GameObject target)
    {
        _followTarget = target;
    }

    private void Update()
    {
        if (_followTarget == null) { return; }

        if (Vector2.Distance(_followTarget.transform.position, transform.position) > _followDistance)
        {
            transform.position = Vector2.SmoothDamp(transform.position, _followTarget.transform.position, ref _currentVelocity, _followSpeed);
        }
    }
}
