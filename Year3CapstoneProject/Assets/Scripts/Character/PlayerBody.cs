using NaughtyAttributes;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    // Serialize Fields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject pivot;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private PlayerStats stats;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Rigidbody rb;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int playerIndex = -1; //Which number this player is.



    // Getters
    public int PlayerIndex { get { return playerIndex; } }

    // Private Variables
    private Vector2 moveDir, aimDir; //The current movement direction of this player.


    /// <summary>
    /// Sets the directional vector for movement.
    /// </summary>
    /// <param name="dir">The direction vector.</param>
    public void SetMovementVector(Vector2 dir)
	{
		moveDir = dir;
	}
	public void SetFiringDirection(Vector2 dir)
	{
		if (dir.x != 0 && dir.y != 0)
			aimDir = dir;
	}
	private void Start()
	{
		stats = GetComponent<PlayerStats>();
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	private void Update()
	{
		Vector3 moveDirection = new Vector3(moveDir.x, 0, moveDir.y);
		if (moveDirection.magnitude > 1)
		{
			moveDirection.Normalize();
		}

		rb.AddForce((moveDirection * stats.MovementSpeed) - rb.velocity, ForceMode.Acceleration);
		var angle = Mathf.Atan2(aimDir.x, aimDir.y) * Mathf.Rad2Deg;
		pivot.transform.rotation = Quaternion.Euler(0, angle, 0);
	}
}



