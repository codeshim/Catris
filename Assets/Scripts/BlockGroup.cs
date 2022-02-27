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
    float FallSec = 0.8f;

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
                break;

            case BlockShape.J_Shape:
                SingleBlockCtrl[0].RelativePos.NewPos(0, 0);
                SingleBlockCtrl[1].RelativePos.NewPos(-1, 0);
                SingleBlockCtrl[2].RelativePos.NewPos(0, 1);
                SingleBlockCtrl[3].RelativePos.NewPos(0, 2);
                break;

            case BlockShape.L_Shape:
                SingleBlockCtrl[0].RelativePos.NewPos(0, 0);
                SingleBlockCtrl[1].RelativePos.NewPos(1, 0);
                SingleBlockCtrl[2].RelativePos.NewPos(0, 1);
                SingleBlockCtrl[3].RelativePos.NewPos(0, 2);
                break;

            case BlockShape.O_Shape:
                SingleBlockCtrl[0].RelativePos.NewPos(0, 0);
                SingleBlockCtrl[1].RelativePos.NewPos(1, 0);
                SingleBlockCtrl[2].RelativePos.NewPos(0, 1);
                SingleBlockCtrl[3].RelativePos.NewPos(1, 1);
                break;

            case BlockShape.S_Shape:
                SingleBlockCtrl[0].RelativePos.NewPos(0, 0);
                SingleBlockCtrl[1].RelativePos.NewPos(-1, 0);
                SingleBlockCtrl[2].RelativePos.NewPos(0, 1);
                SingleBlockCtrl[3].RelativePos.NewPos(1, 1);
                break;

            case BlockShape.T_Shape:
                SingleBlockCtrl[0].RelativePos.NewPos(0, 0);
                SingleBlockCtrl[1].RelativePos.NewPos(0, -1);
                SingleBlockCtrl[2].RelativePos.NewPos(-1, 0);
                SingleBlockCtrl[3].RelativePos.NewPos(1, 0);
                break;

            case BlockShape.Z_Shape:
                SingleBlockCtrl[0].RelativePos.NewPos(0, 0);
                SingleBlockCtrl[1].RelativePos.NewPos(1, 0);
                SingleBlockCtrl[2].RelativePos.NewPos(0, 1);
                SingleBlockCtrl[3].RelativePos.NewPos(-1, 1);
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
                Debug.Log("leftPressed");
                bool isBlocked = false;
                foreach (BlockController singleBlockCtrl in SingleBlockCtrl)
                {
                    // Hit Other Blocks or Sidewall
                    if ((Pos.XPos + singleBlockCtrl.RelativePos.XPos) <= 0 ||
                        singleBlockCtrl.IsXBlocked((Pos.XPos - 1), Pos.YPos))
                    {
                        isBlocked = true;
                        break;
                    }
                }

                if (!isBlocked)
                {
                    Debug.Log("left!");
                    Pos.XPos--;
                    Debug.Log(Pos.XPos);
                    foreach (BlockController singleBlockCtrl in SingleBlockCtrl)
                    {
                        singleBlockCtrl.UpdatePos();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Debug.Log("rightPressed");
                bool isBlocked = false;
                foreach (BlockController singleBlockCtrl in SingleBlockCtrl)
                {
                    // Hit Other Blocks or Sidewall
                    if ((Pos.XPos + singleBlockCtrl.RelativePos.XPos) >= BlockManager.XSize ||
                        singleBlockCtrl.IsXBlocked((Pos.XPos + 1), Pos.YPos))
                    {
                        isBlocked = true;
                        break;
                    }
                }

                if (!isBlocked)
                {
                    Debug.Log("right!");
                    Pos.XPos++;
                    Debug.Log(Pos.XPos);
                    foreach (BlockController singleBlockCtrl in SingleBlockCtrl)
                    {
                        singleBlockCtrl.UpdatePos();
                    }
                }
            }
        }

        // Falling Over Time
        FallCount += Time.deltaTime;
        if (FallCount >= FallSec)
        {
            bool isBlocked = false;
            foreach (BlockController singleBlockCtrl in SingleBlockCtrl)
            {
                // Hit Bottom
                if ((Pos.YPos + singleBlockCtrl.RelativePos.YPos) == 0)
                {
                    isBlocked = true;
                    break;
                }

                // Hit Other Blocks
                if (singleBlockCtrl.IsYBlocked((Pos.XPos + singleBlockCtrl.RelativePos.XPos),
                    (Pos.YPos + singleBlockCtrl.RelativePos.YPos) - 1))
                {
                    isBlocked = true;
                    break;
                }
            }

            if (isBlocked)
            {
                // Stacking On Block or Bottom
                foreach (BlockController singleBlockCtrl in SingleBlockCtrl)
                {
                    singleBlockCtrl.SetBlocked();
                }
                FallOn = false;
                FallCount = 0.0f;
            }
            else
            {
                // Falling
                Pos.YPos--;
                foreach (BlockController singleBlockCtrl in SingleBlockCtrl)
                {
                    singleBlockCtrl.UpdatePos();
                }
                FallCount = 0.0f;
            }
        }
    }
}

