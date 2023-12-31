using NaughtyAttributes;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    // TODO NATHANF: FILL IN BEHAVIOURS
    
    // Serialize Fields
    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private float lifeTime;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private float movementSpeed;

    private void Update()
    {
        // make bullet move direction

    }

    private void OnDisable()
    {
        BulletObjectPoolManager._Instance.ExpiredBullet(this.gameObject);
    }
}
