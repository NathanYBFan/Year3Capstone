using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class BombBelt : MonoBehaviour
{
	#region Serialize Fields
	[Foldout("Bomb Belt Stats"), Tooltip("The time (in seconds) the flash takes to get from min to max intensity.")]
	[SerializeField]
	private GameObject explosionFX;
	[Foldout("Bomb Belt Stats"), Tooltip("")]
	[SerializeField]
	private Color lightColour;
	[Foldout("Bomb Belt Stats"), Tooltip("")]
	[SerializeField]
	private Material bombBeltMat;
	[Foldout("Bomb Belt Stats"), Tooltip("")]
    [SerializeField]
    private float minEmissionIntensity = -4;
    [Foldout("Bomb Belt Stats"), Tooltip("")]
    [SerializeField]
    private float maxEmissionIntensity = 1.5f;
    [Foldout("Bomb Belt Stats"), Tooltip("The delay (in seconds) that the bomb belt will remain at x intensity before it changes.")]
    [SerializeField]
    private float delay = 1f;
    [Foldout("Bomb Belt Stats"), Tooltip("The time (in seconds) the flash takes to get from min to max intensity.")]
    [SerializeField]
    private float flashSpeed = 1f;
	[Foldout("Bomb Belt Stats"), Tooltip("The time (in seconds) the flash takes to get from min to max intensity.")]
	[SerializeField]
	private AudioClip tickSFX;
	[SerializeField]
	private AudioSource audioSource;
	#endregion
	public GameObject ExplosionFX { get { return explosionFX; } }
	
	#region Private Variables
	private ExplosiveTag bombTagData;
	private float currentTime = 0;
	private float baseFlashSpeed = 0;
	#endregion
	private void Awake()
	{
		bombTagData = gameObject.transform.parent.GetComponent<ExplosiveTag>();
		baseFlashSpeed = flashSpeed;
		bombBeltMat.EnableKeyword("_EMISSION");
		bombBeltMat.SetColor("_EmissionColor", lightColour * minEmissionIntensity);
		StartCoroutine(FlashOn());
	}
    private IEnumerator FlashOn()
    {
        float elapsedTime = 0;
		while (elapsedTime < flashSpeed)
		{
            float intensity = Mathf.Lerp(minEmissionIntensity, maxEmissionIntensity, elapsedTime / flashSpeed);
			bombBeltMat.EnableKeyword("_EMISSION");
			bombBeltMat.SetColor("_EmissionColor", lightColour * intensity); 
			elapsedTime += Time.deltaTime;
            yield return null;
		}
        yield return new WaitForSeconds(delay);
		currentTime += elapsedTime;
		audioSource.PlayOneShot(tickSFX);
		flashSpeed = baseFlashSpeed - ((currentTime / bombTagData.Timer) * baseFlashSpeed);
		StartCoroutine(FlashOff());
	}
    private IEnumerator FlashOff()
    {
        float elapsedTime = 0;
		while (elapsedTime < flashSpeed)
		{
            float intensity = Mathf.Lerp(maxEmissionIntensity, minEmissionIntensity, elapsedTime / flashSpeed);
			bombBeltMat.EnableKeyword("_EMISSION");
			bombBeltMat.SetColor("_EmissionColor", lightColour * intensity); 
			elapsedTime += Time.deltaTime;
            yield return null;
		}
        yield return new WaitForSeconds(delay);
		currentTime += elapsedTime;
		flashSpeed = baseFlashSpeed - ((currentTime / bombTagData.Timer) * baseFlashSpeed);
		StartCoroutine(FlashOn());
	}
	private void OnTriggerEnter(Collider other)
    {
        if (other != transform.parent && other.CompareTag("Player"))
        {
            transform.parent.GetComponent<ExplosiveTag>().swapTarget(other.transform.parent.parent.gameObject);

        }
    }

    /*private void OnDestroy()
    {
		BurstSound();
    }

	//Plays the burst sound
	private void BurstSound()
	{
		float randPitch = Random.Range(0.8f, 1.5f);
		if (audioSource != null)
		{
			audioSource.pitch = randPitch;
			AudioManager._Instance.PlaySoundFX(AudioManager._Instance.CFAudioList[5], audioSource);
		}

	}*/
}
