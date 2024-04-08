using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOut : MonoBehaviour, ChaosFactor
{
    // Start is called before the first frame update

    private Light sun;
    private Light dr;

    [SerializeField]
    public float timer;
    [SerializeField]
    private string CFname;

    public string Name { get { return CFname; } }

    private ParticleSystem fog;

    public float Timer { get { return timer; } }



    void Start()
    {
        sun = GameObject.Find("Sun").GetComponent<Light>();
        dr = GameObject.Find("Directional Light").GetComponent<Light>();
        fog = GameObject.Find("Pit Volume Fog/Particle System").GetComponent<ParticleSystem>();
        StartCoroutine(lightsOut());
    }

    IEnumerator lightsOut()
    {

        //flicker off
        PowerDownSound();
        sun.enabled = false;
        dr.enabled = false;
        


        //reduce intensity
        sun.intensity = 0.66f;
        dr.intensity = 0.66f;

        GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(1, 0.2f);
        yield return new WaitForSeconds(0.2f);
        //flicker on
        sun.enabled = true;
        dr.enabled = true;

        GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(1, 0.2f);
        yield return new WaitForSeconds(0.2f);
        //flciker off
        sun.enabled = false;
       // dr.enabled = false;

        //reduce intensity
        sun.intensity = 0.33f;
        dr.intensity = 0.33f;
        GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(1, 0.2f);
        yield return new WaitForSeconds(0.2f);
        //flicker on
        sun.enabled = true;
        // dr.enabled = true;
        GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(1, 0.2f);
        yield return new WaitForSeconds(0.2f);
        //Turn off final
        sun.enabled = false;
        dr.enabled = false;
        fog.gameObject.SetActive(false);
        

        yield return null;
    }


    private void OnDestroy()
    {
        sun.intensity = 1f;
        dr.intensity = 1f;

        sun.enabled = true;
        dr.enabled = true;
        fog.gameObject.SetActive(true);

    }

    public void OnEndOfChaosFactor(bool earlyEnd)
	{
		Destroy(gameObject);
	}

    //Plays the powering down sound
    private void PowerDownSound()
    {
        float randPitch = Random.Range(0.8f, 1.5f);
        AudioSource audioSource = AudioManager._Instance.CFAudioSource;
        if (audioSource != null)
        {
            audioSource.pitch = randPitch;
            AudioManager._Instance.PlaySoundFX(AudioManager._Instance.CFAudioList[6], audioSource);
        }

    }

}
