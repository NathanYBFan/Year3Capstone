using Cinemachine;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Meteor : MonoBehaviour, ChaosFactor
{
    [SerializeField]
    private int damage;

    [SerializeField] 
    private float platformRespwnDelay;

    [SerializeField]
    private int spawnHeight;

    [SerializeField]
    private int fallForce;

    [SerializeField]
    private MeshRenderer MeteorVisual;



    private Rigidbody rb;

    [SerializeField]
    private ParticleSystem explosion;

    [SerializeField]
    private GameObject fallMarker;

    [SerializeField]
    private float markerSpawnHeight;

    [SerializeField]
    private float timer;
    [SerializeField]
    private string CFname;

    private GameObject markerInstance;
    private GeneratesRumble rumble;

    public string Name { get { return CFname; } }
    public float Timer { get { return timer; } }
    void Awake()
    {
        rumble = GetComponent<GeneratesRumble>();
        Vector3 start = GameManager._Instance.Platforms[0].transform.position;
        Vector3 end = GameManager._Instance.Platforms.Last().transform.position;
        float maxSpawnX = end.x;
        float minSpawnX = start.x;
        float maxSpawnZ = start.z;
        float minSpawnZ = end.z;

        transform.position = new Vector3(Random.Range(minSpawnX, maxSpawnX), spawnHeight, Random.Range(minSpawnZ, maxSpawnZ));
        markerInstance = Instantiate(fallMarker, new Vector3(transform.position.x, markerSpawnHeight+4, transform.position.z), fallMarker.transform.rotation);
        rb = GetComponent<Rigidbody>();
        HissSound();
    }





    // Update is called once per frame
    void Update()
    {
        rb.AddForce(transform.up*fallForce);
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
			other.transform.parent.parent.GetComponent<PlayerStats>().TakeDamage(damage, DamageType.ChaosFactor);
        }
        Platform platform = other.GetComponent<Platform>();
        if (platform != null)
        {
			platform.gameObject.GetComponent<Collider>().enabled = false;
			platform.fakeDestroy(platformRespwnDelay);
		}

		StartCoroutine(boom());
	}


    private IEnumerator boom()
    {
        BoomSound();
        explosion.Play();
        Destroy(markerInstance);
        MeteorVisual.enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;
        if (GameObject.Find("VCam").GetComponent<CameraShake>() != null)
        {
            GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(2, 0.75f);
            for (int i = 0; i < 4; i++)
            {
                float distanceFromImpact = Vector3.Distance(GameManager._Instance.Players[i].transform.position, transform.position);
                float increasedRumble = ((rumble.LeftIntensity + rumble.RightIntensity) * 0.5f) * (1 - Mathf.Clamp01(distanceFromImpact / 20));
                StartCoroutine(GameManager._Instance.CreateRumble(rumble.RumbleDuration, rumble.LeftIntensity + increasedRumble, rumble.RightIntensity + increasedRumble, i, false));
            }
        }
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        if (GameObject.Find("VCam").GetComponent<CameraShake>() != null)
        {
            GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(2, 0.75f);
		}
        GetComponent<ChaosFactor>().OnEndOfChaosFactor(true);
        yield break;
    }


    //Plays the impact sound
    private void BoomSound()
    {
        float randPitch = Random.Range(0.8f, 1.5f);
        AudioSource audioSource = AudioManager._Instance.ChooseEnvAudioSource();
        if (audioSource != null)
        {
            audioSource.pitch = randPitch;
            AudioManager._Instance.PlaySoundFX(AudioManager._Instance.CFAudioList[1], audioSource);
        }

    }

    //Plays the whistle/warning sound
    private void HissSound()
    {
        float randPitch = Random.Range(0.8f, 1.5f);
        AudioSource audioSource = AudioManager._Instance.CFAudioSource;
        if (audioSource != null)
        {
            audioSource.pitch = randPitch;
            AudioManager._Instance.PlaySoundFX(AudioManager._Instance.CFAudioList[0], audioSource);
        }

    }

	public void OnEndOfChaosFactor(bool earlyEnd)
	{
        StopAllCoroutines();
		GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(0, 1);
		for (int i = 0; i < 4; i++)
			StartCoroutine(GameManager._Instance.StopRumble(i));
		Destroy(gameObject);
	}
}
