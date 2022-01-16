using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public Unit Pos = new Unit();

    // Test Values
    public bool FallOn = false;
    float FallCount = 0.0f;
    float FallSec = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FallOn)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && Pos.XPos > 0)
            {
                if (!IsXBlocked((Pos.XPos - 1), Pos.YPos))
                {
                    Pos.XPos--;
                    UpdatePos();
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && Pos.XPos < BlockManager.XSize)
            {
                if (!IsXBlocked((Pos.XPos + 1), Pos.YPos))
                {
                    Pos.XPos++;
                    UpdatePos();
                }
            }

            // Falling Over Time
            FallCount += Time.deltaTime;
            if (FallCount >= FallSec)
            {
                if (IsYBlocked(Pos.XPos, (Pos.YPos - 1)) || Pos.YPos <= 0)
                {
                    // Stacking On Block or Bottom
                    FallCount = 0.0f;
                    SetBlocked(Pos);
                    FallOn = false;
                }
                else
                {
                    // Falling
                    Pos.YPos--;
                    UpdatePos();
                    FallCount = 0.0f;
                }
            }
        }
    }

    public void UpdatePos()
    {
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

    public void SetBlocked(Unit Pos)
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
