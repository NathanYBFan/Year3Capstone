using NaughtyAttributes;
using UnityEngine;

public class newLevelBuilder : MonoBehaviour
{
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Text files to read from")]
    private TextAsset[] levels;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Drag in prefabs to generate in text file, starting from 0-X")]
    private GameObject[] levelAssets;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Transform levelBuilderParent;

    [SerializeField]
    [Tooltip("Starting from 0-X")]
    int levelToBuild;

    [SerializeField]
    [Foldout("Stats"), Tooltip("Starting dementions")]
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


    private string[,,] levelInfo;
    private int rowCount;
    private int columnCount;

    public int RowCount {  get { return rowCount; } }
    public int ColumnCount { get { return columnCount; } }
    public int TileSize { get { return tileSize; } }

    public string CurrentLevelName
    {
        get
        {
            return levels[levelToBuild].name;
        }
    }

    //temp start method for testing, final version will be called in gamemanager
    void Start()
    {
        GameManager._Instance.LevelBuilder = this;
        GameManager._Instance.StartNewGame();
	}

    public void buildLevel(int lev)
    {
        int xVal = startX;
        int yVal = startY;
        int zVal = startZ;
        Transform tr;
        toIntArray(lev);

        GameObject temp;
        GameObject temp2;
        levelToBuild = lev;
        AudioManager._Instance.PlayMusic(lev + 1);
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
               
                if (levelInfo[i, j, 2] != null)
                { 
                int prefab = int.Parse(levelInfo[i, j, 2]);

                //decide rotation

                Vector3 rot = new Vector3(levelAssets[prefab - 1].transform.rotation.x, levelAssets[prefab - 1].transform.rotation.y, levelAssets[prefab - 1].transform.rotation.z);
                int r = 0;
                switch (levelInfo[i,j,0])
                {
                    case "n":
                       
                        r = 270;
                        break;
                    case "e":
                       
                        r = 0;
                        break;
                    case "s":
                        
                        r = 90;
                        break;
                    case "w":
                        
                        r = 180;
                        break;
                }
                //decide eleavation
                int t = int.Parse(levelInfo[i, j, 1]);

                int sph = (t-1) * tileSize;

                    //decide prefab and spawn 
                    if (prefab == 6)
                    {
                        //temp = Instantiate(levelAssets[prefab - 1], new Vector3(xVal, yVal + sph, zVal), Quaternion.Euler(new Vector3(rot.x, r, rot.z)), this.transform);
                        temp = Instantiate(levelAssets[prefab - 1], new Vector3(xVal, yVal + sph - 0.25f, zVal), Quaternion.Euler(new Vector3(rot.x - 90, 0, rot.z + r)), this.transform);
                    }
                    else
                    {
                        temp = Instantiate(levelAssets[prefab - 1], new Vector3(xVal, yVal + sph, zVal), Quaternion.Euler(new Vector3(rot.x - 90, 0, rot.z + r)), this.transform);
                    }
                    //decide if it has something on top and if so spawn it

                
                    int topping = int.Parse(levelInfo[i, j, 3]);
                    if (topping > 0)
                    {
                        tr = temp.transform;
                        //tr.localScale = new Vector3(1, 1, 1);
                        temp2 = Instantiate(levelAssets[topping - 1], new Vector3(xVal, yVal + sph + 4, zVal), Quaternion.Euler(new Vector3(rot.x - 90, 0, rot.z + r)), temp.transform);
                        temp2.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
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


    //new format: direction,elevation,prefabIndex,hasSomethingOnTop
    //e.g. n,2,1,0

    private void toIntArray(int lev)
    {
        int i = 0;//row tracker
        int j = 0;//column tracker
        int k = 0;

        string level = levels[lev].ToString();


        string[] rSplit = level.Split('\n');
        rowCount = rSplit.Length;

        //
        string[] temp = rSplit[0].Split("\t");
        columnCount = temp.Length;



        levelInfo = new string[rowCount, columnCount, 4];

        foreach (string s in rSplit)
        {
            string[] t = s.Split("\t");

            foreach (string r in t)
            {

                string[] c = r.Split(",");

                foreach (string u in c)
                {

                    //Debug.Log(u);
                    levelInfo[i, j, k] = u;

                    k++;
                }




                k = 0;
                j++;

            }

            i++;
            j = 0;
        }


    }
 }

