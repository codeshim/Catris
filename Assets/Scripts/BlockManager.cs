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
}

public class Space
{
    public Unit Position = new Unit();
    public bool IsTaken = false;
}

public class BlockManager : MonoBehaviour
{
    // Unit Information
    public static float UnitSize = 0.5f;
    public static int XSize = 9;
    public static int YSize = 18;
    public static Space[] Spaces = new Space[XSize * YSize];
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
        else
        {
            CurBlock.Update();
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
        int index = 0;
        for (int y = 0; y < YSize; y++)
        {
            for (int x = 0; x < XSize; x++)
            {
                Space inst = new Space();
                inst.Position.NewPos(x, y);
                inst.IsTaken = false;
                Spaces[index] = inst;
                index++;
            }
        }
    }
}
