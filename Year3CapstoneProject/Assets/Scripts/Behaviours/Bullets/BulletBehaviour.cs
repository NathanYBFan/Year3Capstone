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

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("")]
    private int originalPlayerIndex;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private float lifeTime = 5f;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private float movementSpeed = 2.5f;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int damageToDeal = 1;

    private void OnEnable()
    {
        StartCoroutine(LifetimeClock());
    }

    private void Update()
    {
        // make bullet move direction
        bulletRootObject.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    private IEnumerator LifetimeClock()
    {
        yield return new WaitForSeconds(lifeTime);
        BulletObjectPoolManager._Instance.ExpiredBullet(bulletRootObject.gameObject);
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            BulletObjectPoolManager._Instance.ExpiredBullet(bulletRootObject.gameObject);
            return;
        }

        if (other.transform.parent.parent.GetComponent<PlayerBody>().PlayerIndex == originalPlayerIndex) return;
        
        other.transform.parent.parent.GetComponent<PlayerStats>().TakeDamage(damageToDeal);
    }

    public void ResetPlayerIndex(int newIndex) { originalPlayerIndex = newIndex; }
}
