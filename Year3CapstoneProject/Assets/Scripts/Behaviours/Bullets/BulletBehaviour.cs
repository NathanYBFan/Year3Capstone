using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    // TODO NATHANF: FILL IN BEHAVIOURS

    // Serialize Fields
    [SerializeField, Required]
    [Foldout("Dependencies"), Tooltip("")]
    private Transform bulletRootObject;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private float lifeTime = 5f;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private float movementSpeed = 2.5f;

    private void OnEnable()
    {
        StartCoroutine(LifetimeClock());
    }

    private void Update()
    {
        // make bullet move direction
        bulletRootObject.position += Vector3.forward * movementSpeed * Time.deltaTime;
    }

    private IEnumerator LifetimeClock()
    {
        yield return new WaitForSeconds(lifeTime);
        BulletObjectPoolManager._Instance.ExpiredBullet(this.gameObject);
        yield break;
    }
}
