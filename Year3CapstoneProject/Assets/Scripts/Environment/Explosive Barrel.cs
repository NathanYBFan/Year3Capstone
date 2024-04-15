using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyedVersion;

    [SerializeField]
    private float respawnTime;

    [SerializeField]
	private GameObject radiusIndicator;

	[SerializeField]
    private float explosiveForce;

    [SerializeField]
    private float explosiveRadius;

    [SerializeField]
    private AudioSource audioSource;

    private Vector3 expPos;
    private bool exploded;
    //private Rigidbody rb;
    private Vector3 startPos;
    private quaternion startRot;


    private void Awake()
    {
        startPos = transform.position;

        startRot = transform.rotation;
    }
    private void Start()
    {


        //rb = destroyedVersion.GetComponent<Rigidbody>();
        expPos = transform.position;
        exploded = false;
        //Debug.Log(GetComponent<Rigidbody>().name);

    }

    private IEnumerator boom()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<BoxCollider>().enabled = false;

        StartCoroutine(BarrelReset());
        yield return null;
    }

    private IEnumerator BarrelReset()
    {

        GetComponentInParent<Platform>().fakeDestroy(respawnTime);


        yield return new WaitForSeconds(respawnTime+1);

        transform.rotation = startRot;
        transform.position = new Vector3(startPos.x, GetComponentInParent<Transform>().position.y, startPos.z);
        
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        exploded = false;
        radiusIndicator.SetActive(true);

    }

    public void ExplodeBarrel()
    {
		GameObject dest = Instantiate(destroyedVersion, transform.position, transform.rotation, transform.parent.parent);

		GetComponent<MeshRenderer>().enabled = false;
		//GetComponent
		//transform.position = new Vector3(transform.position.x, transform.position.y - 20, transform.position.z);
		GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(1, 0.5f);
		exploded = true;
		BlastSound();
		StartCoroutine(boom());

		radiusIndicator.SetActive(false);
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Collider>().CompareTag("Bullet") && !exploded)
        {
            GameObject dest = Instantiate(destroyedVersion, transform.position, transform.rotation, transform.parent.parent);

            GetComponent<MeshRenderer>().enabled = false;
            //GetComponent
            //transform.position = new Vector3(transform.position.x, transform.position.y - 20, transform.position.z);
            GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(1, 0.5f);
            exploded = true;
            BlastSound();
            StartCoroutine(boom());

            radiusIndicator.SetActive(false);
        }
		else if (other.transform.GetComponent<CapsuleCollider>() != null  && !exploded && other.GetComponentInParent<LaserLightShow>() != null)
        {
            GameObject dest = Instantiate(destroyedVersion, transform.position, transform.rotation, transform.parent.parent);

            GetComponent<MeshRenderer>().enabled = false;
            //GetComponent
            //transform.position = new Vector3(transform.position.x, transform.position.y - 20, transform.position.z);
            GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(1, 0.5f);
            exploded = true;
            BlastSound();
            StartCoroutine(boom());

            radiusIndicator.SetActive(false);
        }
    }

    //Plays the explosion sound
    private void BlastSound()
    {
        float randPitch = Random.Range(0.8f, 1.5f);
        audioSource.pitch = randPitch;
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.EnvAudioList[0], audioSource);
    }




}
