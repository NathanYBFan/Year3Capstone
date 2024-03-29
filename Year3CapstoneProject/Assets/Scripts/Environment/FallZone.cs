using NaughtyAttributes;
using UnityEngine;

public class FallZone : MonoBehaviour
{
    #region SerializeFields
    [SerializeField]
    [Foldout("Stats"), Tooltip("Damage to deal when falling off")]
    private int damageToDeal = 1;
	#endregion
	public float nudgeAmount = 1f; // The amount to nudge the player if the space is occupied
	public Vector3 closestPoint; // The amount to nudge the player if the space is occupied
	private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameObject closestObject = null;
        float distance = float.PositiveInfinity;
        PlayerStats stats = other.transform.parent.parent.GetComponent<PlayerStats>();
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
        temp.y += 2f;

        closestPoint = temp;
        if (IsPositionOccupied(temp))
        {
            Debug.Log("Yes");
			Vector3 nudgeDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
			temp = temp + (nudgeDirection * nudgeAmount);
		}

        // Teleport
        if (!stats.Booted) { other.transform.parent.parent.position = temp; }
        
        
        // Deal Damage
        stats.TakeDamage(damageToDeal, DamageType.Falling);
    }

    private bool IsPositionOccupied(Vector3 position)
	{
		// Check if there are any colliders overlapping the player's collider at the respawn position
		Collider[] hitColliders = Physics.OverlapSphere(position, 0.5f);
		foreach (var hitCollider in hitColliders)
		{
			if (hitCollider.gameObject.CompareTag("Player")) // Make sure not to count the player's own collider
			{
				return true; // The position is occupied
			}
		}
		return false; // The position is not occupied
	}
	private bool CheckIfCrumblingBlock(GameObject platform)
    {
        CrumbleBlock crumble = platform.GetComponent<CrumbleBlock>();

        // is a crumble block and hasnt respawned return true
        if (crumble != null) return true;
        return false;
    }
}
