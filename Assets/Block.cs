using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static int breakCount = 0;
    public bool disabled = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (disabled)
        {
            Destroy(gameObject);
            return;
        }

        breakCount++;
        ExamplePongLogic.instance.score += 10;
        ExamplePongLogic.instance.UpdateScoreUI();
        if (breakCount % ExamplePongLogic.instance.newBallCount == 0)
        {
            ExamplePongLogic.instance.NewBall();
        }
        Destroy(gameObject);
        
    }
}
