using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserLightShow : MonoBehaviour, ChaosFactor
{

    [SerializeField]
    private float rotSpeed;

    [SerializeField]
    private int damage;

    [SerializeField]
    public float timer;
    [SerializeField]
    private string CFname;

    public string Name { get { return CFname; } }
    public float Timer { get{ return timer; } }

    [SerializeField]
    private GameObject[] compatibleCFs;
    public GameObject[] CompatibleCFs { get { return compatibleCFs; } }

    // Start is called before the first frame update
    void Start()
    {
        ChaosFactorManager._Instance.activeCFCount++;
        Vector3 start = GameManager._Instance.Platforms[0].transform.position;
        Vector3 end = GameManager._Instance.Platforms.Last().transform.position;

        transform.position = new Vector3 ((start.x + end.x) /2, 5.67f, (start.z + end.z)/2);

    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(new Vector3(0, 0, rotSpeed) *Time.deltaTime);
        timer -= Time.deltaTime;


    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<CapsuleCollider>() != null && other.CompareTag("Player"))
        {
            other.gameObject.transform.parent.parent.GetComponent<PlayerStats>().TakeDamage(damage, DamageType.ChaosFactor);

        }

    }

	public void OnEndOfChaosFactor(bool earlyEnd)
	{
        ChaosFactorManager._Instance.activeCFCount--;
        Destroy(gameObject);
	}
}
