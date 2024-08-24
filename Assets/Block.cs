using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static int breakCount = 0;
    public bool disabled = false;
    public int id = -1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (disabled)
        {
            return;
        }

        breakCount++;
        ExamplePongLogic.instance.score += 10;
        ExamplePongLogic.instance.UpdateScoreUI();
        if (breakCount % ExamplePongLogic.instance.newBallCount == 0)
        {
            ExamplePongLogic.instance.NewBall();
        }

        ExamplePongLogic.instance.blocksActive[id] = false;
        enabled = false;
        gameObject.SetActive(false);
    }
}
