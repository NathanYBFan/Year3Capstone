using NaughtyAttributes;
using UnityEngine;

public class Subzero : MonoBehaviour, ChaosFactor
{
    [SerializeField]
    private float timer;
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Physics material for the floor")]
    PhysicMaterial material;

    private float hold;


    public float Timer { get { return timer; } }
    private void Start()
    {
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

        //Debug.Log("Enable Function ran and finished");
    }

    private void OnDestroy()
    {
        material.dynamicFriction = hold;
        //Debug.Log("On Destroy ran and finished");

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

    //Plays the impact sound
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
