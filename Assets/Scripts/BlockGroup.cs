using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockShape
{
    I_Shape,
    J_Shape,
    L_Shape,
    O_Shape,
    S_Shape,
    T_Shape,
    Z_Shape,
    Size,
}

public class BlockGroup : MonoBehaviour
{
    public Unit Pos = new Unit();
    public BlockShape Shape = BlockShape.I_Shape;
    // Parent Falling Values
    public bool FallOn = false;
    float FallCount = 0.0f;
    float FallSec = 3.0f;

    // Child Component
    public BlockController[] SingleBlockCtrl = new BlockController[4];

    public static BlockGroup Create(GameObject gameObject, BlockShape NewShape, Unit StartPos)
    {
        BlockGroup obj = gameObject.AddComponent<BlockGroup>();
        // BlockGroup Property
        obj.Pos.XPos = StartPos.XPos;
        obj.Pos.YPos = StartPos.YPos;
        obj.Shape = NewShape;

        // SingleBlockCtrl Property
        obj.SetSingleBlockCtrls();

        // BlockGroup Activity
        obj.FallOn = true;
        return obj;
    }

    public void SetSingleBlockCtrls()
    {
        GameObject ChildPrefab = Resources.Load<GameObject>("BlockPivot");
        for (int i = 0; i < SingleBlockCtrl.Length; i++)
        {
            GameObject Child = (GameObject)Instantiate(ChildPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            SingleBlockCtrl[i] = Child.GetComponent<BlockController>();
            SingleBlockCtrl[i].Group = this;
        }
        switch (Shape)
        {
            case BlockShape.I_Shape:
                SingleBlockCtrl[0].RelativePos.NewPos(0, 0);
                SingleBlockCtrl[1].RelativePos.NewPos(0, -1);
                SingleBlockCtrl[2].RelativePos.NewPos(0, 1);
                SingleBlockCtrl[3].RelativePos.NewPos(0, 2);
                Pos.YPos -= 3;
                break;

            case BlockShape.J_Shape:
                SingleBlockCtrl[0].RelativePos.NewPos(0, 0);
                SingleBlockCtrl[1].RelativePos.NewPos(-1, 0);
                SingleBlockCtrl[2].RelativePos.NewPos(0, 1);
                SingleBlockCtrl[3].RelativePos.NewPos(0, 2);
                Pos.YPos -= 3;
                break;

            case BlockShape.L_Shape:
                SingleBlockCtrl[0].RelativePos.NewPos(0, 0);
                SingleBlockCtrl[1].RelativePos.NewPos(1, 0);
                SingleBlockCtrl[2].RelativePos.NewPos(0, 1);
                SingleBlockCtrl[3].RelativePos.NewPos(0, 2);
                Pos.YPos -= 3;
                break;

            case BlockShape.O_Shape:
                SingleBlockCtrl[0].RelativePos.NewPos(0, 0);
                SingleBlockCtrl[1].RelativePos.NewPos(1, 0);
                SingleBlockCtrl[2].RelativePos.NewPos(0, 1);
                SingleBlockCtrl[3].RelativePos.NewPos(1, 1);
                Pos.YPos -= 2;
                break;

            case BlockShape.S_Shape:
                SingleBlockCtrl[0].RelativePos.NewPos(0, 0);
                SingleBlockCtrl[1].RelativePos.NewPos(-1, 0);
                SingleBlockCtrl[2].RelativePos.NewPos(0, 1);
                SingleBlockCtrl[3].RelativePos.NewPos(1, 1);
                Pos.YPos -= 2;
                break;

            case BlockShape.T_Shape:
                SingleBlockCtrl[0].RelativePos.NewPos(0, 0);
                SingleBlockCtrl[1].RelativePos.NewPos(0, -1);
                SingleBlockCtrl[2].RelativePos.NewPos(-1, 0);
                SingleBlockCtrl[3].RelativePos.NewPos(1, 0);
                Pos.YPos -= 1;
                break;

            case BlockShape.Z_Shape:
                SingleBlockCtrl[0].RelativePos.NewPos(0, 0);
                SingleBlockCtrl[1].RelativePos.NewPos(1, 0);
                SingleBlockCtrl[2].RelativePos.NewPos(0, 1);
                SingleBlockCtrl[3].RelativePos.NewPos(-1, 1);
                Pos.YPos -= 2;
                break;
        }
        foreach (BlockController singleBlockCtrl in SingleBlockCtrl)
        {
            singleBlockCtrl.UpdatePos();
        }
    }


    // Update is called once per frame
    public void Update()
    {
        if (FallOn)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveLeft();
                CheckLeftBlocked();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveRight();
                CheckRightBlocked();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveDown();
                CheckYBlocked();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                RotateLeft();
                CheckLeftBlocked();
                CheckRightBlocked();
                CheckYBlocked();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                RotateRight();
                CheckLeftBlocked();
                CheckRightBlocked();
                CheckYBlocked();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                while (FallOn)
                {
                    MoveDown();
                    CheckYBlocked();
                }
            }
        }

        // Falling Over Time
        FallCount += Time.deltaTime;
        if (FallCount >= FallSec)
        {
            MoveDown();
            CheckYBlocked();
            FallCount = 0.0f;
        }

        foreach (BlockController singleBlockCtrl in SingleBlockCtrl)
        {
            singleBlockCtrl.UpdatePos();
        }
    }

    public void MoveLeft()
    {
        Pos.XPos--;
    }

    public void MoveRight()
    {
        Pos.XPos++;
    }

    public void MoveDown()
    {
        Pos.YPos--;
    }


    public void RotateLeft()
    {
        foreach (BlockController singleBlockCtrl in SingleBlockCtrl)
        {
            singleBlockCtrl.RotateLeft();
        }
    }

    public void RotateRight()
    {
        foreach (BlockController singleBlockCtrl in SingleBlockCtrl)
        {
            singleBlockCtrl.RotateRight();
        }
    }

    public void CheckLeftBlocked()
    {
        foreach (BlockController singleBlockCtrl in SingleBlockCtrl)
        {
            int XPos = Pos.XPos + singleBlockCtrl.RelativePos.XPos;
            int YPos = Pos.YPos + singleBlockCtrl.RelativePos.YPos;
            // wall check
            if (XPos < 0)
            {
                Pos.XPos = Pos.XPos + 1;
            }
            // block check
            if (BlockManager.Spaces[Unit.GetUnitKey(XPos, YPos)].singleBlockControl != null &&
                BlockManager.Spaces[Unit.GetUnitKey(XPos, YPos)].singleBlockControl.Group != this)
            {
                Pos.XPos = Pos.XPos + 1;
            }
        }
    }

    public void CheckRightBlocked()
    {
        foreach (BlockController singleBlockCtrl in SingleBlockCtrl)
        {
            int XPos = Pos.XPos + singleBlockCtrl.RelativePos.XPos;
            int YPos = Pos.YPos + singleBlockCtrl.RelativePos.YPos;
            // wall check
            if (XPos > BlockManager.XSize)
            {
                Pos.XPos = Pos.XPos - 1;
            }
            // block check
            if (BlockManager.Spaces[Unit.GetUnitKey(XPos, YPos)].singleBlockControl != null &&
                BlockManager.Spaces[Unit.GetUnitKey(XPos, YPos)].singleBlockControl.Group != this)
            {
                Pos.XPos = Pos.XPos - 1;
            }
        }
    }

    public void CheckYBlocked()
    {
        bool isBlocked = false;
        foreach (BlockController singleBlockCtrl in SingleBlockCtrl)
        {
            int XPos = Pos.XPos + singleBlockCtrl.RelativePos.XPos;
            int YPos = Pos.YPos + singleBlockCtrl.RelativePos.YPos;
            // wall check
            if (YPos < 0)
            {
                Pos.YPos = Pos.YPos + 1;
                isBlocked = true;
            }
            // block check
            if (BlockManager.Spaces[Unit.GetUnitKey(XPos, YPos)].singleBlockControl != null &&
                BlockManager.Spaces[Unit.GetUnitKey(XPos, YPos)].singleBlockControl.Group != this)
            {
                Debug.Log("Blocked");

                Pos.YPos = Pos.YPos + 1;
                isBlocked = true;
            }
        }

        if (isBlocked)
        {
            FallOn = false;
            foreach (BlockController singleBlockCtrl in SingleBlockCtrl)
            {
                singleBlockCtrl.SetBlocked();
            }
        }
    }
}

