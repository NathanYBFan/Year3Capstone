using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Deal Damage
        PlayerStats stats = other.transform.parent.parent.GetComponent<PlayerStats>();
        stats.TakeDamage(9999999, DamageType.Lethal);
    }
}
