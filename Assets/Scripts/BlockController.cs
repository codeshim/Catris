using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockController : MonoBehaviour
{
    public Unit Pos = new Unit();
    public BlockGroup Group;
    public Unit RelativePos = new Unit();

    // Test Values
    public bool FallOn = false;
    float FallCount = 0.0f;
    float FallSec = 0.2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePos()
    {
        Pos.NewPos(Group.Pos.XPos + RelativePos.XPos, Group.Pos.YPos + RelativePos.YPos);
        this.transform.localPosition = new Vector3(Pos.XPos * BlockManager.UnitSize, Pos.YPos * BlockManager.UnitSize, 0);
    }

    public bool IsXBlocked(int nextXPos, int curYPos)
    {
        bool isBlocked = false;
        for (int i = 0; i < BlockManager.Spaces.Length; i++)
        {
            if (BlockManager.Spaces[i].Position.YPos == nextXPos &&
                BlockManager.Spaces[i].Position.XPos == curYPos)
            {
                if (BlockManager.Spaces[i].IsTaken)
                {
                    isBlocked = true;
                }
                break;
            }
        }

        return isBlocked;
    }

    public bool IsYBlocked(int curXPos, int nextYPos)
    {
        bool isBlocked = false;
        for (int i = 0; i < BlockManager.Spaces.Length; i++)
        {
            if (BlockManager.Spaces[i].Position.YPos == curXPos &&
                BlockManager.Spaces[i].Position.XPos == nextYPos)
            {
                if (BlockManager.Spaces[i].IsTaken)
                {
                    isBlocked = true;
                }
                break;
            }
        }

        return isBlocked;
    }

    public void SetBlocked()
    {
        for (int i = 0; i < BlockManager.Spaces.Length; i++)
        {
            if (BlockManager.Spaces[i].Position.YPos == Pos.XPos &&
                BlockManager.Spaces[i].Position.XPos == Pos.YPos)
            {
                BlockManager.Spaces[i].IsTaken = true;
            }
        }
    }
}
