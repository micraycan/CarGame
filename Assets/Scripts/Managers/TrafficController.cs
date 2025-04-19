using System.Collections.Generic;
using UnityEngine;

public class TrafficController : MonoBehaviour
{
    [SerializeField] private GameObject _carPrefab;
    [SerializeField] private float _spawnTimer;
    [SerializeField] private float _carLifetime;
    [SerializeField] private List<Sprite> _carSprites;
    [SerializeField] private List<Vector2> _spawnPositions;

    private bool _isActive;

    private void Start()
    {
        InvokeRepeating("SpawnCar", 0, _spawnTimer);
    }

    private void SpawnCar()
    {
        if (!_isActive) { return; }

        GameObject carObj = Instantiate(_carPrefab, transform);

        TrafficCarController.CarDirection direction = GetDirection();
        Vector2 spawnPos = GetSpawnPos(direction);

        carObj.GetComponent<TrafficCarController>().InitializeCar(GetSprite(), spawnPos, direction);
        Destroy(carObj, _carLifetime);
    }

    private Sprite GetSprite()
    {
        return _carSprites[Random.Range(0, _carSprites.Count)];
    }

    private Vector2 GetSpawnPos(TrafficCarController.CarDirection direction)
    {
        return direction == TrafficCarController.CarDirection.Left ? _spawnPositions[Random.Range(0, 2)] : _spawnPositions[Random.Range(2, 4)];
    }

    private TrafficCarController.CarDirection GetDirection()
    {
        return (TrafficCarController.CarDirection)Random.Range(0, 2);
    }

    public void StartTraffic()
    {
        _isActive = true;
    }

    public void StopTraffic()
    {
        _isActive = false;
    }
}
