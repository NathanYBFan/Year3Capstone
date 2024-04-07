using NaughtyAttributes;
using UnityEngine;

public class Subzero : MonoBehaviour, ChaosFactor
{
    [SerializeField]
    private float timer;
    [SerializeField]
    private string CFname;

    [SerializeField] 
    private float speed;
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Physics material for the floor")]
    PhysicMaterial material;

    private float hold;

    public string Name { get { return CFname; } }
    public float Timer { get { return timer; } }


    private void Start()
    {

        name = "SubZero";

        hold = material.dynamicFriction;
        material.dynamicFriction = 0;
        FreezeSound();

        foreach (GameObject p in GameManager._Instance.Players) 
        {
            p.GetComponent<PlayerBody>().OnIce = true;
        }

        foreach (GameObject q in GameManager._Instance.Platforms)
        {
            q.GetComponent<Platform>().toggleIce(true);


        }

    }

    private void OnDestroy()
    {
        material.dynamicFriction = hold;

        foreach (GameObject p in GameManager._Instance.Players)
        {
            p.GetComponent<PlayerBody>().OnIce = false;
        }
        foreach (GameObject q in GameManager._Instance.Platforms)
        {
            q.GetComponent<Platform>().toggleIce(false);
        }

    }

	public void OnEndOfChaosFactor(bool earlyEnd)
	{
		Destroy(gameObject);
	}

    //Plays the feeze sound
    private void FreezeSound()
    {
        float randPitch = Random.Range(0.8f, 1.5f);
        AudioSource audioSource = AudioManager._Instance.ChooseEnvAudioSource();
        if (audioSource != null)
        {
            audioSource.pitch = randPitch;
            AudioManager._Instance.PlaySoundFX(AudioManager._Instance.CFAudioList[2], audioSource);
        }

    }
}
