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

    public GameObject BlockPrefab = null;
    BlockController CurBlock = null;

    Unit StartUnit = new Unit();

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
        if (CurBlock != null && !CurBlock.FallOn)
        {
            SpawnBlock();
        }
    }

    void SpawnBlock()
    {
        if (BlockPrefab == null) return;

        GameObject tempBlock = (GameObject)Instantiate(BlockPrefab,
            new Vector3(0, 0, 0), Quaternion.identity);
        BlockController tempBlockCtlr = tempBlock.GetComponent<BlockController>();
        tempBlockCtlr.Pos.NewPos(StartUnit);
        tempBlockCtlr.UpdatePos();
        tempBlockCtlr.FallOn = true;
        CurBlock = tempBlockCtlr;
        //Blocks.Add(tempBlockCtlr);
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

        //Debug.Log(index);

        //for (int i = 0; i < Spaces.Length; i++)
        //{
        //    Debug.Log("(" + Spaces[i].Position.XPos + ", " + Spaces[i].Position.YPos + ")");
        //}
    }
}
