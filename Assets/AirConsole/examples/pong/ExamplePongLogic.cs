﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System.IO;

public class ExamplePongLogic : MonoBehaviour {

	public static ExamplePongLogic instance;

	const string smallCircle =
		"0 0 1 1 1 1 1 0 0\n" +
		"0 1 1 1 1 1 1 1 0\n" +
		"1 1 1 1 1 1 1 1 1\n" +
		"1 1 1 1 1 1 1 1 1\n" +
		"1 1 1 1 1 1 1 1 1\n" +
		"1 1 1 1 1 1 1 1 1\n" +
		"1 1 1 1 1 1 1 1 1\n" +
		"0 1 1 1 1 1 1 1 0\n" +
		"0 0 1 1 1 1 1 0 0\n";

    public void NewBall()
    {
		var newBall = Instantiate(ball);
		var paddle = paddles[(Block.breakCount / newBallCount % 6)];
		newBall.position = paddle.transform.GetChild(0).position - paddle.transform.right;
		newBall.velocity = -paddle.transform.right * this.ballSpeed;
    }

    public Rigidbody2D racketLeft;
	public Rigidbody2D racketRight;
	public Rigidbody2D ball;
	public float ballSpeed = 10f;
	public Text uiText;
#if !DISABLE_AIRCONSOLE 
	private int scoreRacketLeft = 0;
	private int scoreRacketRight = 0;

	float turnLeft = 0;
	float turnRight = 0;
	float[] turns = new float[0];
	float[] base_angles = new float[0];

	public GameObject paddlePrefab;
	private Dictionary<int, GameObject> paddles = new Dictionary<int, GameObject>();

	public GameObject blockPrefab;
    public int newBallCount = 20;

    void Awake () {
		instance = this;
		AirConsole.instance.onMessage += OnMessage;
		AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onDisconnect += OnDisconnect;
	}

	/// <summary>
	/// We start the game if 2 players are connected and the game is not already running (activePlayers == null).
	/// 
	/// NOTE: We store the controller device_ids of the active players. We do not hardcode player device_ids 1 and 2,
	///       because the two controllers that are connected can have other device_ids e.g. 3 and 7.
	///       For more information read: http://developers.airconsole.com/#/guides/device_ids_and_states
	/// 
	/// </summary>
	/// <param name="device_id">The device_id that connected</param>
	void OnConnect (int device_id) {
		if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0) {
			if (AirConsole.instance.GetControllerDeviceIds ().Count >= 6) {
				StartGame ();
			} else {
				uiText.text = "NEED MORE PLAYERS";
			}
		}
	}

	/// <summary>
	/// If the game is running and one of the active players leaves, we reset the game.
	/// </summary>
	/// <param name="device_id">The device_id that has left.</param>
	void OnDisconnect (int device_id) {
		int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber (device_id);
		if (active_player != -1) {
			if (AirConsole.instance.GetControllerDeviceIds ().Count >= 6) {
				StartGame ();
			} else {
				AirConsole.instance.SetActivePlayers (0);
				ResetBall (false);
				uiText.text = "PLAYER LEFT - NEED MORE PLAYERS";
			}
		}
	}

	/// <summary>
	/// We check which one of the active players has moved the paddle.
	/// </summary>
	/// <param name="from">From.</param>
	/// <param name="data">Data.</param>
	void OnMessage (int device_id, JToken data) {
		int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber (device_id);
		if (active_player != -1) {
			turns[active_player] = 5 * (float)data["move"];
			Debug.Log("We have smth");
			if (active_player == 0) {
				turnLeft = 5 * (float)data ["move"];
			}
			if (active_player == 1) {
				turnRight = 5 * (float)data ["move"];
			}
		}
	}

	void StartGame () {
		AirConsole.instance.SetActivePlayers (6);
		turns = new float[6];
		base_angles = new float[6];
		for (int i = 0; i < 6; i++)
        {
			var paddle = Instantiate(paddlePrefab);
			paddle.SetActive(true);
			paddles.Add(i, paddle);
			paddle.transform.rotation = Quaternion.Euler(0, 0, 360 / 6 * i);
			base_angles[i] = paddle.transform.rotation.eulerAngles.z;

		}			
		ResetBall (true);
		MakeBlocks();
		scoreRacketLeft = 0;
		scoreRacketRight = 0;
		UpdateScoreUI ();
	}

	void ResetBall (bool move) {

		// place ball at center
		this.ball.position = paddles[0].transform.GetChild(0).position - paddles[0].transform.right ;
		
		// push the ball in a random direction
		if (move) {
			Vector3 startDir = new Vector3 (Random.Range (-1, 1f), Random.Range (-0.1f, 0.1f), 0);
			this.ball.velocity = startDir.normalized * this.ballSpeed;
		} else {
			this.ball.velocity = Vector3.zero;
		}
	}

	void MakeBlocks()
    {
		using(var reader = new StringReader(smallCircle))
        {
			int dimension = 9;
			float scale = 0.4f;
			for(int i = 0; i < dimension; i++)
            {
				var line = reader.ReadLine();
				var split = line.Split(" ");
				for(int j = 0; j < split.Length; j++)
                {
					if (split[j] == "0")
						continue;

					var block = Instantiate(blockPrefab);
					block.transform.position = new Vector2((i - (dimension / 2.0f - 0.5f)) * scale, (j - (dimension / 2.0f - 0.5f)) * scale);
					block.SetActive(true);
                }
            }
        }
    }

	void UpdateScoreUI () {
		// update text canvas
		uiText.text = scoreRacketLeft + ":" + scoreRacketRight;
	}

    private void Update()
    {
		for (int i = 0; i < turns.Length; i++)
        {
			var rotation = paddles[i].transform.rotation;
			var euler = rotation.eulerAngles;
			euler.z = Mathf.Clamp((euler.z + turns[i] * Time.deltaTime), (base_angles[i] - 180 / turns.Length + 5), (base_angles[i] + 180 / turns.Length - 5));
			rotation.eulerAngles = euler;
			paddles[i].transform.rotation = rotation;
		}
	}

	void FixedUpdate () {

		// check if ball reached one of the ends
		if (this.ball.position.x < -9f) {
			scoreRacketRight++;
			UpdateScoreUI ();
			ResetBall (true);
		}

		if (this.ball.position.x > 9f) {
			scoreRacketLeft++;
			UpdateScoreUI ();
			ResetBall (true);
		}
	}

	void OnDestroy () {

		// unregister airconsole events on scene change
		if (AirConsole.instance != null) {
			AirConsole.instance.onMessage -= OnMessage;
		}
	}
#endif
}
