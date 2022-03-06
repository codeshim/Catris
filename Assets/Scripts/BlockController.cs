using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockController : MonoBehaviour
{
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
        this.transform.localPosition = new Vector3(
            (Group.Pos.XPos + RelativePos.XPos) * BlockManager.UnitSize,
            (Group.Pos.YPos + RelativePos.YPos) * BlockManager.UnitSize,
            0
        );
    }

    public void RotateLeft()
    {
        RelativePos.NewPos(-RelativePos.YPos, RelativePos.XPos);
    }

    public void RotateRight()
    {
        RelativePos.NewPos(RelativePos.YPos, -RelativePos.XPos);
    }

    public void SetBlocked()
    {
        BlockManager.Spaces[
            Unit.GetUnitKey(
                Group.Pos.XPos + RelativePos.XPos,
                Group.Pos.YPos + RelativePos.YPos
            )
        ].singleBlockControl = this;
    }
}
