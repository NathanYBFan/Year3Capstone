using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokeBarrel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 6.0f);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            switch(hit.tag)
            {
                case "Player":
					hit.transform.parent.parent.GetComponent<PlayerStats>().TakeDamage(3, DamageType.Hazard);
					if (rb != null)
					{
						rb.AddExplosionForce(30f, transform.position, 6.0f, 1.0f, ForceMode.Force);
					}
					break;
                case "Bullet":
                    BulletObjectPoolManager._Instance.ExpiredBullet(hit.transform.parent.gameObject);
                    break;
            }
        }
        StartCoroutine(boom());
    }

    private IEnumerator boom()
    {
        yield return new WaitForSeconds(6f);
        Destroy(gameObject);
    }

}
