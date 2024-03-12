using NaughtyAttributes;
using UnityEngine;

public class FallZone : MonoBehaviour
{
    #region SerializeFields
    [SerializeField]
    [Foldout("Stats"), Tooltip("Damage to deal when falling off")]
    private int damageToDeal = 1;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameObject closestObject = null;
        float distance = float.PositiveInfinity;

        foreach (GameObject platform in GameManager._Instance.Platforms)
        {
            // If its not a crumbling block
            if (!CheckIfCrumblingBlock(platform) && !platform.GetComponent<Platform>().IsDropped)
            {
                var d = (other.transform.parent.parent.position - platform.transform.position).sqrMagnitude;
                if (d < distance)
                {
                    closestObject = platform;
                    distance = d;
                }
            }
        }

        // Get correct Location
        Vector3 temp = closestObject.transform.position;
        temp.y += 1f;

        // Teleport
        other.transform.parent.parent.position = temp;
        
        // Deal Damage
        PlayerStats stats = other.transform.parent.parent.GetComponent<PlayerStats>();
        stats.TakeDamage(damageToDeal, DamageType.Falling);
    }

    private bool CheckIfCrumblingBlock(GameObject platform)
    {
        CrumbleBlock crumble = platform.GetComponent<CrumbleBlock>();

        // is a crumble block and hasnt respawned return true
        if (crumble != null) return true;
        return false;
    }
}
