using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCollapse : MonoBehaviour
{

    [SerializeField]
    private int numberOfBlocks;
    [SerializeField]
    private float dropInterval;
    [SerializeField]
    private float endDelay;


    int randomNum;

    GameObject[] droppedPlatforms;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Stage collapse started");
        droppedPlatforms = new GameObject[numberOfBlocks];
        StartCoroutine(collapse());

    }


    public IEnumerator collapse()
    {
        for (int i = 0; i < numberOfBlocks; i++)
        {
            randomNum = Random.Range(0, GameManager._Instance.Platforms.Count);


            GameObject dropping = GameManager._Instance.Platforms[randomNum];
            droppedPlatforms[i] = dropping;
            Color c = dropping.GetComponent<Renderer>().material.color;
            dropping.GetComponent<Renderer>().material.color = Color.clear;

            yield return new WaitForSeconds(0.85f);

            
            dropping.GetComponent<Platform>().collapse();

            dropping.GetComponent<Renderer>().material.color = c;

            yield return new WaitForSeconds(dropInterval);
        }

        yield return new WaitForSeconds(endDelay);
        c();

    }


    private void c()
    {
        for (int j = 0; j < numberOfBlocks; j++)
        {
            Debug.Log("going up");
            //StartCoroutine(droppedPlatforms[j].GetComponent<Platform>().Up());
            droppedPlatforms[j].GetComponent<Platform>().fakeRespawn();
        }
        
    }


}
