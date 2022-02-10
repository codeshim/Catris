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

public class BlockGroupClass : MonoBehaviour
{
    // Parent Information
    public GameObject Parent = null;
    public Unit Pos = new Unit();
    public BlockShape Shape = BlockShape.I_Shape;
    // Parent Falling Values
    public bool FallOn = false;
    float FallCount = 0.0f;
    float FallSec = 0.2f;

    // Child Component
    public BlockController[] SingleBlockCtrl = new BlockController[4];
    public Unit[] RelativePos = new Unit[4];

    public BlockGroupClass(BlockShape NewShape, Unit StartPos)
    {
        Pos.XPos = StartPos.XPos;
        Pos.YPos = StartPos.YPos;
        Shape = NewShape;
        FallOn = true;

        for (int i = 0; i < RelativePos.Length; i++)
        {
            RelativePos[i] = new Unit();
        }
        switch (Shape)
        {
            case BlockShape.I_Shape:
                RelativePos[0].NewPos(0, 0);
                RelativePos[1].NewPos(0, -1);
                RelativePos[2].NewPos(0, 1);
                RelativePos[3].NewPos(0, 2);
                break;

            case BlockShape.J_Shape:
                RelativePos[0].NewPos(0, 0);
                RelativePos[1].NewPos(-1, 0);
                RelativePos[2].NewPos(0, 1);
                RelativePos[3].NewPos(0, 2);
                break;

            case BlockShape.L_Shape:
                RelativePos[0].NewPos(0, 0);
                RelativePos[1].NewPos(1, 0);
                RelativePos[2].NewPos(0, 1);
                RelativePos[3].NewPos(0, 2);
                break;

            case BlockShape.O_Shape:
                RelativePos[0].NewPos(0, 0);
                RelativePos[1].NewPos(1, 0);
                RelativePos[2].NewPos(0, 1);
                RelativePos[3].NewPos(1, 1);
                break;

            case BlockShape.S_Shape:
                RelativePos[0].NewPos(0, 0);
                RelativePos[1].NewPos(-1, 0);
                RelativePos[2].NewPos(0, 1);
                RelativePos[3].NewPos(1, 1);
                break;

            case BlockShape.T_Shape:
                RelativePos[0].NewPos(0, 0);
                RelativePos[1].NewPos(0, -1);
                RelativePos[2].NewPos(-1, 0);
                RelativePos[3].NewPos(1, 0);
                break;

            case BlockShape.Z_Shape:
                RelativePos[0].NewPos(0, 0);
                RelativePos[1].NewPos(1, 0);
                RelativePos[2].NewPos(0, 1);
                RelativePos[3].NewPos(-1, 1);
                break;
        }
        Parent = new GameObject(Shape.ToString());
        GameObject ChildPrefab = Resources.Load<GameObject>("BlockPivot");
        for (int i = 0; i < SingleBlockCtrl.Length; i++)
        {
            GameObject Child = (GameObject)Instantiate(ChildPrefab,
            new Vector3(0, 0, 0), Quaternion.identity);
            SingleBlockCtrl[i] = Child.GetComponent<BlockController>();
            Child.transform.parent = Parent.transform;
        }
        SetPos();
    }

    public void SetPos()
    {
        for(int i = 0; i < SingleBlockCtrl.Length; i++)
        {
            SingleBlockCtrl[i].Pos.NewPos(Pos.XPos + RelativePos[i].XPos, Pos.YPos + RelativePos[i].YPos);
            SingleBlockCtrl[i].UpdatePos();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        //Debug.Log("Update");
        //if (FallOn)
        //{
        //    if (Input.GetKeyDown(KeyCode.LeftArrow) && Pos.XPos > 0)
        //    {
        //        if (!IsXBlocked((Pos.XPos - 1), Pos.YPos))
        //        {
        //            Pos.XPos--;
        //            UpdatePos();
        //        }
        //    }

        //    if (Input.GetKeyDown(KeyCode.RightArrow) && Pos.XPos < BlockManager.XSize)
        //    {
        //        if (!IsXBlocked((Pos.XPos + 1), Pos.YPos))
        //        {
        //            Pos.XPos++;
        //            UpdatePos();
        //        }
        //    }

        // Falling Over Time
        FallCount += Time.deltaTime;
        if (FallCount >= FallSec)
        {
            bool isBlocked = false;
            for (int i = 0; i < SingleBlockCtrl.Length; i++)
            {
                // Hit Bottom
                if ((Pos.YPos + RelativePos[i].YPos) == 0)
                {
                    isBlocked = true;
                    break;
                }

                // Hit Other Blocks
                if (SingleBlockCtrl[i].IsYBlocked((Pos.XPos + RelativePos[i].XPos),
                    (Pos.YPos + RelativePos[i].YPos) - 1))
                {
                    isBlocked = true;
                    break;
                }
            }

            if (isBlocked)
            {
                // Stacking On Block or Bottom
                for (int i = 0; i < SingleBlockCtrl.Length; i++)
                {
                    SingleBlockCtrl[i].SetBlocked();
                }
                FallOn = false;
                FallCount = 0.0f;
            }
            else
            {
                // Falling
                Pos.YPos--;
                SetPos();
                FallCount = 0.0f;
            }
        }
    }
}

