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
    public float Timer { get { return timer; } }



    void Start()
    {
        

        sun = GameObject.Find("Sun").GetComponent<Light>();
        dr = GameObject.Find("Directional Light").GetComponent<Light>();
        StartCoroutine(lightsOut());
    }

    IEnumerator lightsOut()
    {

        //flicker off
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


        yield return null;
    }


    private void OnDestroy()
    {
        sun.intensity = 1f;
        dr.intensity = 1f;

        sun.enabled = true;
        dr.enabled = true;
    }

}
