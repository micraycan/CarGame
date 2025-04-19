using System.Collections.Generic;
using UnityEngine;

public class TrafficCarController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed = 5f;

    public enum CarDirection { Left, Right }
    private CarDirection _direction;
    private bool _isActive;

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameActive) { gameObject.SetActive(false); }

        AutoDrive();
    }

    public void InitializeCar(Sprite sprite, Vector2 spawnPos, CarDirection direction)
    {
        _spriteRenderer.sprite = sprite;
        _direction = direction;
        transform.position = spawnPos;
        transform.rotation = Quaternion.Euler(0, 0, direction == CarDirection.Left ? 0 : 180);
        _isActive = true;
    }

    private void AutoDrive()
    {
        if (!_isActive) { return; }

        Vector3 moveDirection = _direction == CarDirection.Left ? Vector2.left : Vector2.right;
        transform.position += moveDirection * _speed * Time.fixedDeltaTime;
    }
}
