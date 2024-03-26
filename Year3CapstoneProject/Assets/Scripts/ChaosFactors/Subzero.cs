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

        foreach (GameObject p in GameManager._Instance.Players) 
        {
            p.GetComponent<PlayerBody>().OnIce = true;
        }

        foreach (GameObject q in GameManager._Instance.Platforms)
        {
            q.GetComponent<Platform>().toggleIce(true);


        }

        Debug.Log("Enable Function ran and finished");
    }

    private void OnDestroy()
    {
        material.dynamicFriction = hold;
        Debug.Log("On Destroy ran and finished");

        foreach (GameObject p in GameManager._Instance.Players)
        {
            p.GetComponent<PlayerBody>().OnIce = false;
        }
        foreach (GameObject q in GameManager._Instance.Platforms)
        {
            q.GetComponent<Platform>().toggleIce(false);
        }

    }
}
