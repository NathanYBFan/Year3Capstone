using NaughtyAttributes;
using UnityEngine;

public class levelBuilder : MonoBehaviour
{
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private TextAsset[] levels;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject[] levelAssets;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Transform levelBuilderParent;

    [SerializeField]
    int levelToBuildl;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int startX;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int startY;
    
    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int startZ;
    
    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int tileSize;


    private int[,] levelInfo;
    private int rowCount;
    private int columnCount;

    //temp start method for testing, final version will be called in gamemanager
    void Start()
    {
        buildLevel(levelToBuildl);
    }

    public void buildLevel(int lev)
    {
        int xVal = startX;
        int yVal = startY;
        int zVal = startZ;
        toIntArray(lev);

        GameObject temp;

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                temp = null;
                if (levelInfo[i, j] != 0)
                {
                    
                    if (levelInfo[i, j] > 20)
                    {

                        temp = Instantiate(levelAssets[levelInfo[i, j] - 21], new Vector3(xVal, yVal + tileSize, zVal), levelAssets[levelInfo[i, j] - 21].transform.rotation, this.transform);

                    }

                    else if (levelInfo[i, j] < 0)
                    {

                        if (levelInfo[i, j] < -20)
                        {
                            Debug.Log(levelAssets[levelInfo[i, j] * -1 - 21].transform.rotation.x);
                            Vector3 rot = new Vector3(levelAssets[levelInfo[i, j] *-1 - 21].transform.rotation.x, levelAssets[levelInfo[i, j] * -1 - 21].transform.rotation.y, levelAssets[levelInfo[i, j] * -1 - 21].transform.rotation.z);
                            temp = Instantiate(levelAssets[levelInfo[i, j] * -1 - 21], new Vector3(xVal, yVal+tileSize, zVal), Quaternion.Euler(new Vector3(rot.x-90, rot.y, rot.z + 180)), this.transform);

                        }


                        else
                        {
                            Vector3 rot = new Vector3(levelAssets[levelInfo[i, j] * -1 - 1].transform.rotation.x, levelAssets[levelInfo[i, j] * -1 - 1].transform.rotation.y, levelAssets[levelInfo[i, j] * -1 - 1].transform.rotation.z);
                            temp = Instantiate(levelAssets[levelInfo[i, j] * -1 - 1], new Vector3(xVal, yVal, zVal), Quaternion.Euler(new Vector3(rot.x-90, rot.y,rot.z+180)), this.transform);
                        }
                
                    }


                    else
                    {
                        temp = Instantiate(levelAssets[levelInfo[i, j] - 1], new Vector3(xVal, yVal, zVal), levelAssets[levelInfo[i, j] - 1].transform.rotation, this.transform);

                    }

                    if (temp.GetComponent<Platform>() != null)
                    {
                        GameManager._Instance.Platforms.Add(temp); 
                    }
                }
                
                xVal = xVal + tileSize;
            }
            xVal = startX;
            zVal = zVal - tileSize;
        }
    }


    public void toIntArray(int lev)
    {
        int i = 0;//row tracker
        int j = 0;//column tracker

        string level = levels[lev].ToString();

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
    }
}
