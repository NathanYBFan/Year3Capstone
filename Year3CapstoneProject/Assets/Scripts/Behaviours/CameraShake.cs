using UnityEngine;
using Cinemachine;
using NaughtyAttributes;

public class CameraShake : MonoBehaviour
{
	private CinemachineVirtualCamera CinemachineVirtualCamera;
	[SerializeField]
	[Foldout("CameraBehaviour"), Tooltip("Strength of shake")]
	private float ShakeIntensity1;
	[SerializeField]
	[Foldout("CameraBehaviour"), Tooltip("Duration of Camera shake")]
	private float ShakeDuration1;

	private float timer;
	private CinemachineBasicMultiChannelPerlin clamp;
	void Awake()
	{
		CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
	}

	private void Start()
	{
		EndShake();
	}

	public void ShakeCamera(float ShakeIntensity, float ShakeDuration)
	{


		CinemachineBasicMultiChannelPerlin clamp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		clamp.m_AmplitudeGain = ShakeIntensity;
		timer = ShakeDuration;
	}

	public void ShakeCamera(float ShakeIntensity, float ShakeDuration, GeneratesRumble rumble, GameObject targPlayer, Vector3 source)
	{
		CinemachineBasicMultiChannelPerlin clamp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		clamp.m_AmplitudeGain = ShakeIntensity;
		timer = ShakeDuration;

		for (int i = 0; i < 4; i++)
		{
			if (i == targPlayer.GetComponent<PlayerBody>().PlayerIndex)
				StartCoroutine(GameManager._Instance.CreateRumble(rumble.RumbleDuration, rumble.LeftIntensity * 2, rumble.RightIntensity * 2, i, false));
			else
			{
				float distanceFromImpact = Vector3.Distance(source, GameManager._Instance.Players[i].transform.position);
				float increasedRumble = ((rumble.LeftIntensity + rumble.RightIntensity) * 0.5f) * (1 - Mathf.Clamp01(distanceFromImpact / 32));
				StartCoroutine(GameManager._Instance.CreateRumble(rumble.RumbleDuration, increasedRumble, increasedRumble, i, false));
			}
		}
	}

	void EndShake()
	{
		CinemachineBasicMultiChannelPerlin clamp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		clamp.m_AmplitudeGain = 0f;
		timer = 0;
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			ShakeCamera(ShakeIntensity1, ShakeDuration1);
		}
		if (timer > 0)
		{
			timer -= Time.deltaTime;

			if (timer <= 0)
			{
				EndShake();
			}
		}
	}
}
