using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    public int XPos = 0;
    public int YPos = 0;

    public void NewPos(Unit newPos)
    {
        XPos = newPos.XPos;
        YPos = newPos.YPos;
    }

    public void NewPos(int newXPos, int newYPos)
    {
        XPos = newXPos;
        YPos = newYPos;
    }

    public static string GetUnitKey(int XPos, int YPos)
    {
        return XPos.ToString() + "_" + YPos.ToString();
    }
}

public class Space
{
    public BlockController singleBlockControl = null;
}


public class BlockManager : MonoBehaviour
{
    // Unit Information
    public static float UnitSize = 0.5f;
    public static int XSize = 9;
    public static int YSize = 18;
    public static Dictionary<string, Space> Spaces = new Dictionary<string, Space>();
    //public static ArrayList Spaces = new ArrayList();

    //public GameObject BlockPrefab = null;
    BlockGroup CurBlock = null;

    Unit StartUnit = new Unit();

    BlockGroup Test = null;

    // Start is called before the first frame update
    void Start()
    {
        StartUnit.NewPos(XSize / 2, YSize);
        SpacesInit();
        SpawnBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if (!CurBlock.FallOn)
        {
            SpawnBlock();
        }
    }

    void SpawnBlock()
    {
        int ranVal = Random.Range(0, (int)BlockShape.Size);
        // Factory Method pattern
        CurBlock = BlockGroup.Create(gameObject, (BlockShape)ranVal, StartUnit);
    }


    void SpacesInit()
    {
        for (int y = -1; y < YSize + 1; y++)
        {
            for (int x = -1; x < XSize + 1; x++)
            {
                Space space = new Space();
                Spaces.Add(Unit.GetUnitKey(x, y), space);
            }
        }
    }

    public static void ClearLines(List<int> ClearedLineYPoses)
    {
        int ClearedLineCount = 0;
        foreach (int yPos in ClearedLineYPoses)
        {
            for (int x = 0; x < XSize + 1; x++)
            {
                string key = Unit.GetUnitKey(x, yPos - ClearedLineCount);
                BlockController block = Spaces[key].singleBlockControl;
                Spaces[key].singleBlockControl = null;
                Destroy(block.gameObject);
            }
            for (int y = yPos - ClearedLineCount + 1; y < BlockManager.YSize; y++)
            {
                for (int x = 0; x < XSize + 1; x++)
                {
                    string key = Unit.GetUnitKey(x, y);
                    if (Spaces[key].singleBlockControl != null)
                    {
                        Debug.Log(key + "-movedown");
                        BlockController block = Spaces[key].singleBlockControl;
                        Spaces[key].singleBlockControl = null;
                        block.MoveDown();
                        key = Unit.GetUnitKey(x, y - 1);
                        Spaces[key].singleBlockControl = block;
                    }
                }
            }
        }
    }
}
