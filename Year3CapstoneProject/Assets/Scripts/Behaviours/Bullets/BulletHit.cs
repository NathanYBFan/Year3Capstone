using NaughtyAttributes;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    [SerializeField, ReadOnly]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject parentPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.transform.parent != parentPlayer)
        {
            // TODO NATHANF: Deal damage when health system script is done, make sure checking parent player is correct
            // Deal damage
        }
    }

    public void SetParent(GameObject parent) { parentPlayer = parent; }
}
