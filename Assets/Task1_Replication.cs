using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task1_Replication : MonoBehaviour
{
    private void Update()
    {
        if (ExamplePongLogic.instance.playerCount < 6)
            return;
        Replikavimas();
    }
    public void Replikavimas()
    {
        // Žaidėjų, prisijungusių prie sistemos kiekis
        int clientCount = ExamplePongLogic.instance.GetClientCount();

        // Žaidėjų kiekis (įskaičiuoja tik žaidžiančius žaidėjus)
        int playerCount = ExamplePongLogic.instance.GetPlayerCount();

        // Kampas tarp žaidėjų platformų ir teigiamos X ašies laipsniais
        // (Laipsnių didinimas suka platformą prieš laikrodžio rodyklę)
        float[] playerPaddleAngles = ExamplePongLogic.instance.GetPaddleAngles();

        // Žaidėjų platformų sukimosi kryptys
        int[] playerPaddleRotationDirections = ExamplePongLogic.instance.GetPlayerPaddleRotationDirections();

        int ballCount = ExamplePongLogic.instance.GetBallCount();

        // Kamuoliukų pozicijos
        Vector2[] ballPositions = ExamplePongLogic.instance.GetBallPositions();

        // Kamuoliukų judėjimo kryptys
        Vector2[] ballDirections = ExamplePongLogic.instance.GetBallDirections();

        // Kamuoliukų greičiai
        float[] ballVelocities = ExamplePongLogic.instance.GetBallVelocities();

        // Aktyvūs blokai
        bool[] blocksActive = ExamplePongLogic.instance.GetBlocksActive();

        // Žaidimo taškai
        float score = ExamplePongLogic.instance.GetScore();

        int Player1 = 1;
        int Player2 = 2;

        // Užduoties pradžia


        Replicate(Player1, BALL_POSITIONS, ballPositions); // DELETE
        Replicate(Player2, BALL_POSITIONS, ballPositions); // DELETE


        // Užduoties pabaiga
    }












    //---------------- IGNORUOTI --- IGNORE -------------------
    //---------------------------------------------------------

    public void Replicate(int playerID, int dataID, System.Object data)
    {
        ExamplePongLogic logic = ExamplePongLogic.instance.GetPlayerLogicInstance(playerID);

        switch(dataID)
        {
            case CLIENT_COUNT:
                logic.SetClientCount((int)data);
                break;
            case PLAYER_COUNT:
                logic.SetPlayerCount((int)data);
                break;
            case PLAYER_PADDLE_ANGLES:
                logic.SetPaddleAngles((float[])data);
                break;
            case BALL_COUNT:
                logic.SetBallCount((int)data);
                break;
            case BALL_POSITIONS:
                logic.SetBallPositions((Vector2[])data, new Vector3(((playerID) % 3) * 30, -20 * (int)((playerID) / 3), 0));
                break;
            case BALL_DIRECTIONS:
                logic.SetBallDirections((Vector2[])data);
                break;
            case BALL_VELOCITIES:
                logic.SetBallVelocities((float[])data);
                break;
            case BLOCKS_ACTIVE:
                logic.SetBlocksActive((bool[])data);
                break;
            case SCORE:
                logic.SetScore((float)data);
                break;
        }
    }

    const int CLIENT_COUNT = 0;
    const int PLAYER_COUNT = 1;
    const int PLAYER_PADDLE_ANGLES = 2;
    const int BALL_POSITIONS = 3;
    const int BALL_DIRECTIONS = 4;
    const int BALL_VELOCITIES = 5;
    const int BLOCKS_ACTIVE = 6;
    const int SCORE = 7;
    const int BALL_COUNT = 8;

}
