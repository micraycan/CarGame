using UnityEngine;

public class ConeIndicator : MonoBehaviour
{
    private void Update()
    {
        if (transform.parent.gameObject.CompareTag("CollectedCone"))
        {
            Destroy(gameObject);
        }
    }
}
