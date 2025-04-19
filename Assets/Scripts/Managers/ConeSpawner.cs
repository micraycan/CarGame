using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ConeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _conePrefab;
    [SerializeField] private List<Transform> _noSpawnZone;
    [SerializeField] private Vector2 _spawnArea;

    private GameObject _currentCone;
    private bool _coneSpawned = false;

    public static Action OnConeCollected;

    private void OnEnable() => OnConeCollected += SpawnNewCone;
    private void OnDisable() => OnConeCollected -= SpawnNewCone;

    private void Update()
    {
        if (_coneSpawned || !GameManager.Instance.IsGameActive) { return; }

        _coneSpawned = true;
        _currentCone = Instantiate(_conePrefab, GetSpawnPos(), Quaternion.identity);
    }

    private Vector2 GetSpawnPos()
    {
        Vector2 pos = new Vector2(Random.Range(-_spawnArea.x, _spawnArea.x), Random.Range(-_spawnArea.y, _spawnArea.y));
        if (CanSpawnAtPos(pos))
        {
            return pos;
        }
        else
        {
            return GetSpawnPos();
        }
    }

    private bool CanSpawnAtPos(Vector2 pos)
    {
        return Physics2D.OverlapPoint(pos) == null;
    }

    private void SpawnNewCone()
    {
        _coneSpawned = false;
        _currentCone = null;
    }

    public void ClearCones()
    {
        if (_currentCone == null) { return; }
        Destroy(_currentCone);
    }

    public void Reset()
    {
        SpawnNewCone();
    }
}
