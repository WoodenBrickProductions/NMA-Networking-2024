using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task1_Replication : MonoBehaviour
{
    const bool task1_enabled = true; // įjungta pirmoji užduotis: Replikavimas
    const bool task2_enabled = false; // įjungta antroji užduotis: Nuspėjimai
    const bool task3_enabled = false; // įjungta pirmoji užduotis: Migracija
    const float ping_delay = 2; // Duomenų vėlinimas sekundėmis

    public void Replikavimas()
    {
        /////////////
        /// DUOTA
        /////////////

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


        //  -------- 1-os Užduoties pradžia -------------

        int Player2 = 1; // Antras žaidėjas
        int firstBall = 0; // Pirmas kamuoliukas
        Replicate(Player2, PLAYER_PADDLE_ANGLES, playerPaddleAngles); // Replikuoti antro žaidėjo visų žaidėjų platformas
        Replicate(Player2, BALL_POSITIONS, ballPositions, firstBall); // Replikuoti antro žaidėjo pirmo kamuoliuko poziciją

        // ---------- 1-os Užduoties pabaiga ------------



        // ---------- 3-os Užduoties pradžia ------------




        // ---------- 3-os Užduoties pabaiga ------------

    }


    public void Nuspejimai(int id, ExamplePongLogic logic, float delta)
    {
        /////////////
        /// DUOTA
        /////////////

        // Žaidėjo, kuris atlieka nuspėjimus ID
        int playerID = id; 

        // Žaidėjų, prisijungusių prie sistemos kiekis
        int clientCount = logic.GetClientCount();

        // Žaidėjų kiekis (įskaičiuoja tik žaidžiančius žaidėjus)
        int playerCount = logic.GetPlayerCount();

        // Kampas tarp žaidėjų platformų ir teigiamos X ašies laipsniais
        // (Laipsnių didinimas suka platformą prieš laikrodžio rodyklę)
        float[] playerPaddleAngles = logic.GetPaddleAngles();

        // Žaidėjų platformų sukimosi kryptys
        int[] playerPaddleRotationDirections = logic.GetPlayerPaddleRotationDirections();

        // Kamuoliukų kiekis
        int ballCount = logic.GetBallCount();

        // Kamuoliukų pozicijos
        Vector2[] ballPositions = logic.GetBallPositions();

        // Kamuoliukų judėjimo kryptys
        Vector2[] ballDirections = logic.GetBallDirections();

        // Kamuoliukų greičiai
        float[] ballVelocities = logic.GetBallVelocities();

        // Aktyvūs blokai
        bool[] blocksActive = logic.GetBlocksActive();


        // ---------- 2-os Užduoties pradžia ------------

        // Pastaba: šioje užduotyje negalima naudoti funkcijos: Replicate()
        

        // Laikas nuo paskutinio replikavimo iš host
        float timeSinceLastReplication = delta;

        // Testavimui
        //blocksActive[test++] = false;
        //blocksActive[test / 2] = true;
        //test = test % blocksActive.Length;
        //Simulate(playerID, BLOCKS_ACTIVE, blocksActive);
        // Testavimui

        // ---------- 2-os Užduoties pabaiga ------------
    }








    //---------------- IGNORUOTI --- IGNORE -------------------
    //---------------------------------------------------------

    static int test = 0;
    static float time = 0;

    private void Update()
    {
        if (ExamplePongLogic.instance.playerCount < 6)
            return;

        time += Time.deltaTime;

        for(int i = 1; i < 6; i++)
        {
            currentPlayerID = i;
            Nuspejimai(currentPlayerID, ExamplePongLogic.instance.GetPlayerLogicInstance(currentPlayerID), time);
        }

        if(task2_enabled && time < ping_delay)
        {
            return;
        }

        Replikavimas();
        time = 0;
        LogReplications();
    }

    public static int replicationDataCount = 0;

    public void LogReplications()
    {
        Debug.Log("Data amount replicated: " + replicationDataCount);
        replicationDataCount = 0;
    }

    static int currentPlayerID = 0;

    public void Simulate(int playerID, int dataID, System.Object data)
    {
        if(playerID != currentPlayerID)
        {
            Debug.LogError("Bandoma simuliuoti neteisingą žaidėją su ID: " + playerID + " kai šiuometinis žaidėjas: " + currentPlayerID);
            return;
        }

        switch (dataID)
        {
            case BALL_POSITIONS:
                ExamplePongLogic logic = ExamplePongLogic.instance.GetPlayerLogicInstance(playerID);
                logic.SetBallPositions((Vector2[])data, Vector3.zero);
                replicationDataCount += ((Vector2[])data).Length;
                break;

            default:
                Replicate(playerID, dataID, data);
                break;
        }
    }

    public void Replicate(int playerID, int dataID, System.Object data)
    {
        ExamplePongLogic logic = ExamplePongLogic.instance.GetPlayerLogicInstance(playerID);

        switch(dataID)
        {
            case CLIENT_COUNT:
                logic.SetClientCount((int)data);
                replicationDataCount += 1;
                break;
            case PLAYER_COUNT:
                logic.SetPlayerCount((int)data);
                replicationDataCount += 1;
                break;
            case PLAYER_PADDLE_ANGLES:
                logic.SetPaddleAngles((float[])data);
                replicationDataCount += ((float[])data).Length;
                break;
            case BALL_COUNT:
                logic.SetBallCount((int)data);
                replicationDataCount += 1;
                break;
            case BALL_POSITIONS:
                logic.SetBallPositions((Vector2[])data, new Vector3(((playerID) % 3) * 30, -20 * (int)((playerID) / 3), 0));
                replicationDataCount += ((Vector2[])data).Length;
                break;
            case BALL_DIRECTIONS:
                logic.SetBallDirections((Vector2[])data);
                replicationDataCount += ((Vector2[])data).Length;
                break;
            case BALL_VELOCITIES:
                logic.SetBallVelocities((float[])data);
                replicationDataCount += ((float[])data).Length;
                break;
            case BLOCKS_ACTIVE:
                logic.SetBlocksActive((bool[])data);
                replicationDataCount += ((bool[])data).Length;
                break;
            case SCORE:
                logic.SetScore((float)data);
                replicationDataCount += 1;
                break;
            case PLAYER_PADDLE_ROTATION_DIRECTIONS:
                logic.SetPlayerPaddleRotationDirections((int[])data);
                replicationDataCount += ((int[])data).Length;
                break;
        }
    }

    public void Replicate(int playerID, int dataID, System.Object data, int index)
    {
        ExamplePongLogic logic = ExamplePongLogic.instance.GetPlayerLogicInstance(playerID);

        switch (dataID)
        {
            case CLIENT_COUNT:
                logic.SetClientCount((int)data);
                replicationDataCount += 1;
                break;
            case PLAYER_COUNT:
                logic.SetPlayerCount((int)data);
                replicationDataCount += 1;
                break;
            case PLAYER_PADDLE_ANGLES:
                logic.SetPaddleAngle(index, (float[])data);
                replicationDataCount += 1;
                break;
            case BALL_COUNT:
                logic.SetBallCount((int)data);
                replicationDataCount += 1;
                break;
            case BALL_POSITIONS:
                logic.SetBallPosition(index, (Vector2[])data, new Vector3(((playerID) % 3) * 30, -20 * (int)((playerID) / 3), 0));
                replicationDataCount += 1;
                break;
            case BALL_DIRECTIONS:
                logic.SetBallDirection(index, (Vector2[])data);
                replicationDataCount += 1;
                break;
            case BALL_VELOCITIES:
                logic.SetBallVelocity(index, (float[])data);
                replicationDataCount += 1;
                break;
            case BLOCKS_ACTIVE:
                logic.SetBlockActive(index, (bool[])data);
                replicationDataCount += 1;
                break;
            case SCORE:
                logic.SetScore((float)data);
                replicationDataCount += 1;
                break;
            case PLAYER_PADDLE_ROTATION_DIRECTIONS:
                logic.SetPlayerPaddleRotationDirection(index, (int[])data);
                replicationDataCount += 1;
                break;
        }
    }

    const int CLIENT_COUNT = 0;
    const int PLAYER_COUNT = 1;
    const int PLAYER_PADDLE_ANGLES = 2;
    const int PLAYER_PADDLE_ROTATION_DIRECTIONS = 9;
    const int BALL_POSITIONS = 3;
    const int BALL_DIRECTIONS = 4;
    const int BALL_VELOCITIES = 5;
    const int BLOCKS_ACTIVE = 6;
    const int SCORE = 7;
    const int BALL_COUNT = 8;

}
