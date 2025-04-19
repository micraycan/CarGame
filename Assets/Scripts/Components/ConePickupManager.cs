using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConePickupManager : MonoBehaviour
{
    private List<GameObject> _attachedBodies = new List<GameObject>();

    [SerializeField] private GameObject _startingPoint;

    private void OnEnable() { GameManager.Instance.OnGameStart += Reset; }
    private void OnDisable() { GameManager.Instance.OnGameStart -= Reset; }

    private void Start()
    {
        _attachedBodies.Add(_startingPoint);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cone"))
        {
            var obj = collision.gameObject;
            obj.tag = "CollectedCone";
            obj.GetComponent<ConeFollow>().SetFollowTarget(_attachedBodies.Last());
            _attachedBodies.Add(obj);
            GameManager.Instance.ResetTimer();
            ConeSpawner.OnConeCollected?.Invoke();
        }
    }

    private void Reset()
    {
        foreach (var obj in _attachedBodies)
        {
            if (obj.CompareTag("CollectedCone"))
            {
                Destroy(obj);
            }
        }
        _attachedBodies = new List<GameObject> { _startingPoint };
    }
}
