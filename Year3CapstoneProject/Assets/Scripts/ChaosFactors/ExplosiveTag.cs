using System.Data;
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

    [SerializeField]
    private string CFname;


    private int targetSpeed;

	private float[] holdSpeeds;
	private GameObject Spawnedbelt;
	private GameObject targetPlayer;
	private GeneratesRumble rumble;

	private GameObject[] playerHolder;

    public string Name { get { return CFname; } }
    public float Timer { get { return timer; } }


	private void OnEnable()
	{
		playerHolder = new GameObject[4];
		rumble = GetComponent<GeneratesRumble>();
		bool loop = false;
		Random.InitState((int)System.DateTime.Now.TimeOfDay.TotalSeconds);
		holdSpeeds = new float[GameManager._Instance.Players.Count];
		//get random player from the gamemanager player list, called target player
		for (int i = 0; i < GameManager._Instance.Players.Count; i++)
		{
			holdSpeeds[i] = GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed;
			GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed = playerSpeed;
			GameManager._Instance.Players[i].GetComponent<PlayerStats>().ChaosFactorCanShoot = false;
			if (!GameManager._Instance.Players[i].GetComponent<PlayerStats>().IsDead) loop = true;
		}

		if (!loop)
		{
			OnEndOfChaosFactor(true);
			return;
		}

		int livingPlayerCount = 0;
		foreach(GameObject p in GameManager._Instance.Players)
		{

			if (!p.GetComponent<PlayerStats>().IsDead)
			{
				livingPlayerCount++;

            }


        }

        playerHolder = new GameObject[livingPlayerCount];
		int k = 0;
        foreach (GameObject p in GameManager._Instance.Players)
        {

            if (!p.GetComponent<PlayerStats>().IsDead)
            {
				playerHolder[k] = p;
				k++;
            }


        }

        int random = Random.Range(0, livingPlayerCount);

		targetPlayer = playerHolder[random];



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
			OnEndOfChaosFactor(true);
		}

	}


	public void swapTarget(GameObject p)
	{

		targetPlayer.GetComponent<PlayerStats>().MovementSpeed = playerSpeed;
		targetPlayer = p;
		targetPlayer.GetComponent<PlayerStats>().MovementSpeed = targetSpeed;

		StartCoroutine(GameManager._Instance.CreateRumble(rumble.RumbleDuration, rumble.LeftIntensity, rumble.RightIntensity, targetPlayer.GetComponent<PlayerBody>().PlayerIndex, false));



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
			Instantiate(Spawnedbelt.GetComponent<BombBelt>().ExplosionFX, Spawnedbelt.transform.position, Quaternion.identity);
			targetPlayer.GetComponent<PlayerStats>().TakeDamage(damage, DamageType.ChaosFactor);
			GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(1, 0.5f, rumble, targetPlayer, Spawnedbelt.transform.position);
		}
		else
		{
			ChaosFactorManager._Instance.CurrentRunningChaosFactors.Remove(gameObject);
			for (int i = 0; i < 4; i++)
				StartCoroutine(GameManager._Instance.StopRumble(i));
		}
		 
		Destroy(gameObject);
	}

	//Plays the burst sound
	private void BurstSound()
	{
		float randPitch = Random.Range(0.8f, 1.5f);
		AudioSource audioSource = gameObject.GetComponent<AudioSource>();
		if (audioSource != null)
		{
			audioSource.pitch = randPitch;
			AudioManager._Instance.PlaySoundFX(AudioManager._Instance.CFAudioList[4], audioSource);
		}

	}
}
