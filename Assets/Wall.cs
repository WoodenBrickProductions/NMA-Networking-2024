using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ExamplePongLogic.instance.score -= 5;
        ExamplePongLogic.instance.UpdateScoreUI();
    }
}
