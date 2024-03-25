using Cinemachine;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Meteor : MonoBehaviour, ChaosFactor
{
    [SerializeField]
    private int damage;

    [SerializeField]
    private int spawnHeight;
    //[SerializeField]
    //private int maxSpawnX;
    //[SerializeField]
    //private int minSpawnX;
    //[SerializeField]
    //private int maxSpawnZ;
    //[SerializeField]
    //private int minSpawnZ;

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

    private GameObject markerInstance;


    public float Timer { get { return timer; } }
    void Awake()
    {

        Vector3 start = GameManager._Instance.Platforms[0].transform.position;
        Vector3 end = GameManager._Instance.Platforms.Last().transform.position;
        float maxSpawnX = end.x;
        float minSpawnX = start.x;
        float maxSpawnZ = start.z;
        float minSpawnZ = end.z;

        transform.position = new Vector3(Random.Range(minSpawnX, maxSpawnX), spawnHeight, Random.Range(minSpawnZ, maxSpawnZ));
        markerInstance = Instantiate(fallMarker, new Vector3(transform.position.x, markerSpawnHeight+4, transform.position.z), transform.rotation);
        rb = GetComponent<Rigidbody>();
        HissSound();
    }





    // Update is called once per frame
    void Update()
    {
        rb.AddForce(transform.up*fallForce);
        
    }


    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInChildren<CapsuleCollider>() != null && collision.gameObject.GetComponentInChildren<CapsuleCollider>().CompareTag("Player")) 
        {
            collision.gameObject.transform.GetComponent<PlayerStats>().TakeDamage(damage, DamageType.ChaosFactor);

        }

        StartCoroutine(boom());
    }*/



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("player detected by meteor");
            other.transform.parent.parent.GetComponent<PlayerStats>().TakeDamage(damage, DamageType.ChaosFactor);
        }
        Platform platform = other.GetComponent<Platform>();
        if (platform != null)
        {
			platform.gameObject.GetComponent<Collider>().enabled = false;
			platform.fakeDestroy();
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
        }
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        if (GameObject.Find("VCam").GetComponent<CameraShake>() != null)
        {
            GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(2, 0.75f);
        }
        Destroy(gameObject);
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
        AudioSource audioSource = AudioManager._Instance.ChooseEnvAudioSource();
        if (audioSource != null)
        {
            audioSource.pitch = randPitch;
            AudioManager._Instance.PlaySoundFX(AudioManager._Instance.CFAudioList[0], audioSource);
        }

    }

}
