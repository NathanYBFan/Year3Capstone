using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMr20 : MonoBehaviour
{
    #region SerializeFields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("List of points to move to")]
    private List<GameObject> pointList;


    
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Speed value of platform & Mr. 20")]
    private float speed;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Rotation speed value of platform & Mr. 20")]
    private float rotSpeed;

    #endregion

    private Vector3 currPos;
    private int targetIndex;
    private bool rotating;
    private bool returning; // whether or not he is on the return trip back to the start
    private Vector3 rotVector;

    // Start is called before the first frame update
    void Start()
    {
        targetIndex = 1;
        rotating = false;
        returning = false;
    }

    // Update is called once per frame
    void Update()
    {
        currPos = gameObject.transform.position;
        float degrees = rotSpeed * Time.deltaTime;
        if (!returning)
        {
            rotVector = new Vector3(0, degrees, 0);
        }
        else
        {
            rotVector = new Vector3(0, -degrees, 0);
        }
        

        if(targetIndex == 2)
        {
            if (!returning && gameObject.transform.rotation.eulerAngles.y < 180)
            {
                //rotate clockwise until facing 180 degrees in world space
                gameObject.transform.Rotate(rotVector, Space.World);
            }
            else if(returning && gameObject.transform.rotation.eulerAngles.y > 180)
            {
                gameObject.transform.Rotate(rotVector, Space.World);
            }
        }
        if (targetIndex == 4)
        {
            if (!returning && gameObject.transform.rotation.eulerAngles.y < 270)
            {
                //rotate clockwise until facing 180 degrees in world space
                gameObject.transform.Rotate(rotVector, Space.World);
            }
        }

        if(targetIndex == 0 && gameObject.transform.rotation.eulerAngles.y > 90)
        {
            gameObject.transform.Rotate(rotVector, Space.World);
        }

        gameObject.transform.position = Vector3.MoveTowards(currPos, pointList[targetIndex].transform.position, speed * Time.deltaTime);
        
      
        if(currPos == pointList[targetIndex].transform.position)
        {
            if(targetIndex + 1 >= pointList.Count)
            {
                returning = true;
            }
            if(currPos == pointList[0].transform.position)
            {
                returning = false;
            }

            if (returning && targetIndex - 1 >= 0)
            {
                targetIndex--;
            }
            else if (targetIndex + 1 < pointList.Count)
            {
                targetIndex++;

            }
            
        }

    }

    

}
