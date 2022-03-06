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
}
