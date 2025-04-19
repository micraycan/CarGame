using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarController : MonoBehaviour
{
    public bool IsHandbrakeActive => _isHandbrakeActive;
    public Vector2 SlipVelocity => transform.right * Vector2.Dot(_rb.linearVelocity, transform.right);
    public float Speed => _rb.linearVelocity.magnitude;
    public bool IsBraking => Vector2.Dot(_rb.linearVelocity, transform.up) > 0 ? _input.y < 0 : _input.y > 0;

    private Rigidbody2D _rb;
    
    [Header("Car Settings")]
    [Range(0, 1)][SerializeField] private float _slipFactor;
    [SerializeField] private float _accelerationFactor;
    [SerializeField] private float _turnRadius;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _handbrakeForce;

    private Vector2 _input;
    private float _rotation;
    private float _defaultSlipFactor;
    private bool _isHandbrakeActive;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _defaultSlipFactor = _slipFactor;
    }

    private void FixedUpdate()
    {
        ApplyDrive();
        ApplySlip();
        ApplySteering();
        ApplyDrag();
    }

    public void OnMoveInput(InputAction.CallbackContext c)
    {
        if (GameManager.Instance.Timer <= 0) { return; }
        if (c.performed) { _input = c.ReadValue<Vector2>(); }
        else if (c.canceled) { _input = Vector2.zero; }
    }

    public void OnHandbrakeInput(InputAction.CallbackContext c)
    {
        if (GameManager.Instance.Timer <= 0) { return; }
        if (c.performed) { _isHandbrakeActive = true; }
        else if (c.canceled) { _isHandbrakeActive = false; }
    }

    private void ApplyDrive()
    {
        if (_input.y == 0 || _rb.linearVelocity.magnitude > _maxSpeed || _isHandbrakeActive) { return; }
        _rb.AddForce(transform.up * _input.y * _accelerationFactor, ForceMode2D.Force);
    }

    private void ApplySlip()
    {
        Vector2 forwardVel = transform.up * Vector2.Dot(_rb.linearVelocity, transform.up);
        Vector2 rightVel = transform.right * Vector2.Dot(_rb.linearVelocity, transform.right);

        float targetSlip = _isHandbrakeActive ? 1f : _defaultSlipFactor;
        _slipFactor = Mathf.Lerp(_slipFactor, targetSlip, Time.fixedDeltaTime * _handbrakeForce);

        Vector2 velocity = forwardVel + rightVel * _slipFactor;

        if (_isHandbrakeActive)
        {
            velocity = Vector2.Lerp(_rb.linearVelocity, Vector2.zero, Time.fixedDeltaTime * _handbrakeForce);
        }

        _rb.linearVelocity = velocity;
    }

    private void ApplySteering()
    {
        if (_rb.linearVelocity.magnitude == 0) { return; }

        float speedFactor = _rb.linearVelocity.magnitude / 8;
        _rotation -= _input.x * _turnRadius * speedFactor * GetSteeringDirectionSign();
        _rb.MoveRotation(_rotation);
    }

    private void ApplyDrag()
    {
        if (_rb.linearVelocity.magnitude > _maxSpeed)
        {
            _rb.linearVelocity = Vector2.Lerp(_rb.linearVelocity, _rb.linearVelocity.normalized, Time.fixedDeltaTime);
        }
    }

    private float GetSteeringDirectionSign()
    {
        float rVel = Vector2.Dot(_rb.linearVelocity, transform.right);
        float fVel = Vector2.Dot(_rb.linearVelocity, transform.up);

        if (Mathf.Abs(rVel) > Mathf.Abs(fVel))
        {
            return 1f;
        }

        return Mathf.Sign(fVel);
    }

    public void ResetCar()
    {
        transform.position = Vector3.zero;
        if (_rb == null) { return; }
        float rotation = Vector3.Angle(Vector3.up, transform.up) - 90;
        _rotation = 0;
    }

    public void StopCar()
    {
        _input = Vector2.zero;
        _rb.linearVelocity = Vector2.zero;
    }
}
