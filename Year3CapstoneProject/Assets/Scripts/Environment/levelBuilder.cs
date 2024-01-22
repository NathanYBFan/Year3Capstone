using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelBuilder : MonoBehaviour
{



    [SerializeField]
    TextAsset[] levels;
    [SerializeField]
    GameObject[] levelAssets;

    [SerializeField]
    int startX;
    [SerializeField]
    int startY;
    [SerializeField]
    int startZ;
    [SerializeField]
    int tileSize;


    int[,] levelInfo;
    int rowCount;
    int columnCount;

    //temp start method for testing, final version will be called in gamemanager
    void Start()
    {
        buildLevel(0);
    }


    public void buildLevel(int lev)
    {
        int xVal = startX;
        int yVal = startY;
        int zVal = startZ;
        toIntArray(lev);

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                if (levelInfo[i, j] - 1 != -1) 
                { 
                    if (levelInfo[i, j] > 20)
                    {
                        Instantiate(levelAssets[levelInfo[i, j] - 21], new Vector3(xVal, yVal + tileSize, zVal), Quaternion.identity);
                        
                    }

                    else
                    {
                        Instantiate(levelAssets[levelInfo[i, j] - 1], new Vector3(xVal, yVal, zVal), Quaternion.identity);
                        
                    }

                }
                zVal = zVal + tileSize;
            }
            zVal = startZ;
            xVal = xVal+tileSize;
        }




    }


    public void toIntArray(int lev)
    {
        int i = 0;//row tracker
        int j = 0;//column tracker


        string level = levels[lev].ToString();

        Debug.Log(level);

        string[] rSplit = level.Split('\n');
        rowCount = rSplit.Length;
        string[] temp = rSplit[0].Split("\t");
        columnCount = temp.Length;

        levelInfo = new int[rowCount, columnCount];


        foreach (string s in rSplit) 
        {
            string[] t = s.Split("\t");

            foreach (string r in t) 
            {

                levelInfo[i,j] = int.Parse(r);

                j++;
            }

            i++;
            j = 0;
        }

        Debug.Log("int arr complete");


    }



}
