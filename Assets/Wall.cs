using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool disabled = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (disabled)
            return;

        ExamplePongLogic.instance.score -= 5;
        ExamplePongLogic.instance.UpdateScoreUI();
    }
}
