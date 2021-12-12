using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMove : MonoBehaviour
{
    private Robber robb;

    private void Awake()
    {
        robb = GetComponent<Robber>();
    }

    private void FixedUpdate()
    {
        if (robb.dirHeld == -1) return;
        int facing = robb.GetFacing();

        Vector2 rPos = transform.position;
        Vector2 rPosGrid = robb.GetRoomPosOnGrid();

        float delta = 0;
        if (facing == 0 || facing == 2)
        {
            delta = rPosGrid.y - rPos.y;
        }
        else
        {
            delta = rPosGrid.x - rPos.x;
        }

        if (delta == 0) return;
        float move = robb.GetSpeed() * Time.fixedDeltaTime;
        move = Mathf.Min(move, Mathf.Abs(delta));
        if (delta < 0) move = -move;

        if (facing == 0 || facing == 2)
        {
            rPos.y += move;
        }
        else
        {
            rPos.x += move;
        }
        transform.position = rPos;
    }
}
