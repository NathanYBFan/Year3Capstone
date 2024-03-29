using UnityEngine;

public class ExplosiveTag : MonoBehaviour, ChaosFactor
{
	[SerializeField]
	private GameObject UnSpawnedbelt;

	[SerializeField]
	private int beltRotSpeed;

	[SerializeField]
	private int damage;

	[SerializeField]
	private int playerSpeed;

	[SerializeField]
	private float timer;


	private int targetSpeed;

	private float[] holdSpeeds;
	private GameObject Spawnedbelt;
	private GameObject targetPlayer;



	public float Timer { get { return timer; } }


	private void OnEnable()
	{
		Random.InitState((int)System.DateTime.Now.TimeOfDay.TotalSeconds);
		holdSpeeds = new float[GameManager._Instance.Players.Count];
		//get random player from the gamemanager player list, called target player
		for (int i = 0; i < GameManager._Instance.Players.Count; i++)
		{
			holdSpeeds[i] = GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed;
			GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed = playerSpeed;
			GameManager._Instance.Players[i].GetComponent<PlayerStats>().ChaosFactorCanShoot = false;
		}


		bool loop = true;
		while (loop)
		{
            int random = Random.Range(0, 4);
			Debug.Log(random);

			targetPlayer = GameManager._Instance.Players[random];

			if (targetPlayer.GetComponent<PlayerStats>().IsDead == true) { print("Dead player selected, trying agian"); }

			else
			{
				loop = false;
			}


		}

		targetSpeed = playerSpeed + 2;
		targetPlayer.GetComponent<PlayerStats>().MovementSpeed = targetSpeed;
		//spawn bomb belt on player
		Spawnedbelt = Instantiate(UnSpawnedbelt, new Vector3(targetPlayer.transform.position.x, targetPlayer.transform.position.y + 2, targetPlayer.transform.position.z), Quaternion.Euler(new Vector3(-90, 0, 0)), this.transform);
	}


	// Update is called once per frame
	void Update()
	{

		//move with target player
		Spawnedbelt.transform.position = new Vector3(targetPlayer.transform.position.x, targetPlayer.transform.position.y + 2, targetPlayer.transform.position.z);
		Spawnedbelt.transform.Rotate(new Vector3(0, 0, beltRotSpeed) * Time.deltaTime);

		if (targetPlayer.GetComponent<PlayerStats>().IsDead == true)
		{
			Destroy(gameObject);
		}

	}


	public void swapTarget(GameObject p)
	{

		targetPlayer.GetComponent<PlayerStats>().MovementSpeed = playerSpeed;
		targetPlayer = p;
		targetPlayer.GetComponent<PlayerStats>().MovementSpeed = targetSpeed;




	}



	private void OnDestroy()
	{
		
	}

	public void OnEndOfChaosFactor(bool earlyEnd)
	{
		for (int i = 0; i < GameManager._Instance.Players.Count; i++)
		{
			GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed = holdSpeeds[i];
			GameManager._Instance.Players[i].GetComponent<PlayerStats>().CanShoot = true;
			GameManager._Instance.Players[i].GetComponent<PlayerStats>().ChaosFactorCanShoot = true;
		}
		if (!earlyEnd)
		{
			targetPlayer.GetComponent<PlayerStats>().TakeDamage(damage, DamageType.ChaosFactor);
			GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(1, 0.5f);
		}	

		Destroy(gameObject);
	}
}
