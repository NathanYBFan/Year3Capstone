using UnityEngine;
using Cinemachine;
using NaughtyAttributes;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera CinemachineVirtualCamera;
    [SerializeField]
    [Foldout ("CameraBehaviour"), Tooltip("Strength of shake")]
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
