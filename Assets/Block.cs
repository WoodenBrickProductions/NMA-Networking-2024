using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static int breakCount = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        breakCount++;
        if(breakCount % ExamplePongLogic.instance.newBallCount == 0)
        {
            ExamplePongLogic.instance.NewBall();
        }
        Destroy(gameObject);
        
    }
}
