using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System.IO;
using System;

public class ExamplePongLogic : MonoBehaviour {

	public static ExamplePongLogic instance;
	public Dictionary<int, ExamplePongLogic> playerLogics = new Dictionary<int, ExamplePongLogic>();

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

    public int GetClientCount()
    {
		return clientCount;
    }

	public void SetClientCount(int data)
	{
		clientCount = data;
	}

    public int[] GetPlayerPaddleRotationDirections()
    {
		return playerPaddleRotationDirections;
    }

	public void SetPlayerPaddleRotationDirections(int[] rotations)
	{
		for(int i = 0; i < playerCount; i++)
        {
			SetPlayerPaddleRotationDirection(i, rotations);
        }
	}

	public void SetPlayerPaddleRotationDirection(int index, int[] rotations)
	{
		playerPaddleRotationDirections[index] = rotations[index];
	}

	public int GetPlayerCount()
    {
		return playerCount;
    }
	
	public void SetPlayerCount(int data)
	{
		playerCount = data;
	}

	public Vector2[] GetBallPositions()
    {
		return ballPositions;
    }

    public int GetBallCount()
    {
		return ballCount;
    }

	public void SetBallCount(int count)
    {
		ballCount = count;
    }

    public void SetBallPositions(Vector2[] positions, Vector2 offset)
	{
		for (int i = 0; i < ballCount; i++)
		{
			SetBallPosition(i, positions, offset);
		}
	}

	public void SetBallPosition(int index, Vector2[] positions, Vector2 offset)
    {
		ballPositions[index] = positions[index] + offset;
		var ball = balls[index];
		ball.position = ballPositions[index];

	}

	public float[] GetPaddleAngles()
    {
		return playerPaddleAngles;
    }

	public void SetPaddleAngles(float[] angles)
	{
		for(int i = 0; i < playerCount; i++)
        {
			SetPaddleAngle(i, angles);
        }
	}

	public void SetPaddleAngle(int index, float[] angles)
    {
		playerPaddleAngles[index] = angles[index];

	}

	public Vector2[] GetBallDirections()
    {
		return ballDirections;
    }

	public void SetBallDirections(Vector2[] directions)
	{
		for(int i = 0; i < ballDirections.Length; i++)
        {
			SetBallDirection(i, directions);
        }
	}

	public void SetBallDirection(int index, Vector2[] directions)
    {
		ballDirections[index] = directions[index];
		balls[index].velocity = directions[index] * balls[index].velocity.magnitude;

	}

	const string largeCircle =
		"0 0 0 0 0 1 1 1 1 1 0 0 0 0 0\n" +
		"0 0 0 1 1 1 1 1 1 1 1 1 0 0 0\n" +
		"0 0 1 1 1 1 1 1 1 1 1 1 1 0 0\n" +
		"0 1 1 1 1 1 1 1 1 1 1 1 1 1 0\n" +
		"0 1 1 1 1 1 1 1 1 1 1 1 1 1 0\n" +
		"1 1 1 1 1 1 1 1 1 1 1 1 1 1 1\n" +
		"1 1 1 1 1 1 1 1 1 1 1 1 1 1 1\n" +
		"1 1 1 1 1 1 1 1 1 1 1 1 1 1 1\n" +
		"1 1 1 1 1 1 1 1 1 1 1 1 1 1 1\n" +
		"1 1 1 1 1 1 1 1 1 1 1 1 1 1 1\n" +
		"0 1 1 1 1 1 1 1 1 1 1 1 1 1 0\n" +
		"0 1 1 1 1 1 1 1 1 1 1 1 1 1 0\n" +
		"0 0 1 1 1 1 1 1 1 1 1 1 1 0 0\n" +
		"0 0 0 1 1 1 1 1 1 1 1 1 0 0 0\n" +
		"0 0 0 0 0 1 1 1 1 1 0 0 0 0 0\n";

    public bool[] GetBlocksActive()
    {
		return blocksActive;
    }

	public void SetBlocksActive(bool[] blocks)
	{
		for(int i = 0; i < blocks.Length; i++)
        {
			SetBlockActive(i, blocks);
		}
	}

	public void SetBlockActive(int index, bool[] blocks)
	{
		blocksActive[index] = blocks[index];
		var block = this.blocks[index];
		block.enabled = blocks[index];
		block.gameObject.SetActive(blocks[index]);
	}

	public ExamplePongLogic GetPlayerLogicInstance(int playerID)
    {
		if (this != instance)
        {
			Debug.LogError("Ši operacija negalima su klientu!");
			return null;
        }

		if(!playerLogics.ContainsKey(playerID))
        {
			Debug.LogError("Paduotas neteisingas žaidėjo kodas: " + playerID);
			return null;
        }

		return playerLogics[playerID];
    }

	public void SetHostInstance(int playerID)
    {
		instance = playerLogics[playerID];
    }

    public float GetScore()
    {
		return score;
    }

	public void SetScore(float score)
	{
		this.score = score;
	}


	public float[] GetBallVelocities()
    {
		return ballVelocities;
    }

	public void SetBallVelocity(int index, float[] velocities)
	{
		ballVelocities[index] = velocities[index];
		balls[index].velocity = velocities[index] * balls[index].velocity.normalized;
	}

	public void SetBallVelocities(float[] velocities)
	{
		for (int i = 0; i < velocities.Length; i++)
        {
			SetBallVelocity(i, velocities);
        }
	}

	public void NewBall()
    {
		for (int i = 0; i < playerCount; i++)
		{
			var logic = playerLogics[i];
			var newBall = Instantiate(ballPrefab);
			newBall.transform.parent = logic.transform;
			var paddle = logic.paddles[(Block.breakCount / newBallCount % playerCount)];
			newBall.transform.position = paddle.transform.GetChild(0).position - paddle.transform.right;
			newBall.velocity = -paddle.transform.right * logic.baseBallSpeed;
			logic.balls.Add(newBall);
			newBall.gameObject.SetActive(true);



			logic.ballCount++;
			Array.Resize(ref logic.ballDirections, ballCount);
			Array.Resize(ref logic.ballPositions, ballCount);
			Array.Resize(ref logic.ballVelocities, ballCount);

			logic.ballPositions[ballCount - 1] = newBall.position;
			logic.ballDirections[ballCount - 1] = newBall.velocity.normalized;
			logic.ballVelocities[ballCount - 1] = newBall.velocity.magnitude;
		}

		{
			int i = 5;
			var logic = playerLogics[i];
			var newBall = Instantiate(ballPrefab);
			newBall.transform.parent = logic.transform;
			var paddle = logic.paddles[(Block.breakCount / newBallCount % playerCount)];
			newBall.transform.position = paddle.transform.GetChild(0).position - paddle.transform.right;
			newBall.velocity = -paddle.transform.right * logic.baseBallSpeed;
			logic.cheatBalls.Add(newBall);
			newBall.gameObject.SetActive(true);
		}
	}

	public Text uiText;
	
	public float score;
	int clientCount;
	public int playerCount;
	float[] playerPaddleAngles = new float[0];
	int[] playerPaddleRotationDirections = new int[0];
	int ballCount;
	Vector2[] ballPositions = new Vector2[0];
	Vector2[] ballDirections = new Vector2[0];
	float[] ballVelocities = new float[0];
	public bool[] blocksActive;

	float[] base_angles = new float[0];

	public GameObject paddlePrefab;
	private Dictionary<int, GameObject> paddles = new Dictionary<int, GameObject>();
	private List<Rigidbody2D> balls = new();
	private List<Rigidbody2D> cheatBalls = new();

	public float baseBallSpeed = 10;

	public GameObject blockPrefab;
	public Rigidbody2D ballPrefab;
	public int newBallCount = 20;

    void Awake () {
		if (instance != null)
			return;
		instance = this;
		AirConsole.instance.onMessage += OnMessage;
		AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onDisconnect += OnDisconnect;
	}

	void CreateInstances()
    {
		playerLogics[0] = this;
		for(int i = 0; i < 5; i++)
        {
			var logic = Instantiate(gameObject).GetComponent<ExamplePongLogic>();
			var walls = logic.GetComponentsInChildren<Wall>(true);
			foreach (var wall in walls)
            {
				wall.disabled = true;
            }
			logic.transform.position = new Vector3(((i + 1) % 3) * 30, -20 * (int) ((i + 1) / 3), 0);
			playerLogics.Add(i + 1, logic);
			logic.StartGame(true);
        }
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
				StartGame (false);
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
				StartGame (false);
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
			playerPaddleRotationDirections[active_player] = (int)data["move"];
		}
	}

	void StartGame (bool client) {
		playerCount = 6;
		clientCount = 6;
		if(!client)
        {
			AirConsole.instance.SetActivePlayers (6);
			CreateInstances();
        }
		base_angles = new float[6];
		playerPaddleAngles = new float[6];
		playerPaddleRotationDirections = new int[6];

		for (int i = 0; i < 6; i++)
        {
			var paddle = Instantiate(paddlePrefab);
			paddle.transform.GetChild(0).GetComponent<Racket>().logic = this;
			paddle.SetActive(true);
			paddles.Add(i, paddle);
			paddle.transform.parent = this.transform;
			paddle.transform.localPosition = Vector3.zero;
			paddle.transform.rotation = Quaternion.Euler(0, 0, 360 / 6 * i);
			base_angles[i] = paddle.transform.rotation.eulerAngles.z;

		}
		if(!client)
        {
			NewBall();
			ResetBall (true);
			UpdateScoreUI ();
        }
		MakeBlocks(this);
	}

	void ResetBall (bool move) {
		
		for(int i = 0; i < ballCount; i++)
        {
			var ball = balls[i];
			var paddle = paddles[i % 6];
			ball.position = paddle.transform.GetChild(i % 6).position - paddle.transform.right;
			
			if(move)
            {
				ball.velocity = -paddle.transform.right * this.baseBallSpeed;
            }
			else
            {
				ball.velocity = Vector3.zero;
			}

			ballPositions[ballCount - 1] = ball.position;
			ballDirections[ballCount - 1] = ball.velocity.normalized;
			ballVelocities[ballCount - 1] = ball.velocity.magnitude;

        }

	}

	void MakeBlocks(ExamplePongLogic logic)
    {
		using(var reader = new StringReader(largeCircle))
        {
			int dimension = 15;
			float scale = 0.3f;

			int counter = 0;

			for(int i = 0; i < dimension; i++)
            {
				var line = reader.ReadLine();
				var split = line.Split(" ");
				for(int j = 0; j < split.Length; j++)
                {
					if (split[j] == "0")
						continue;

					var block = Instantiate(blockPrefab);
					var blockScript = block.GetComponent<Block>();

					blockScript.id = counter++;
					blocks.Add(blockScript);

					block.transform.parent = logic.transform;
					block.transform.localPosition = new Vector2((i - (dimension / 2.0f - 0.5f)) * scale, (j - (dimension / 2.0f - 0.5f)) * scale);
					block.GetComponent<Block>().disabled = logic != ExamplePongLogic.instance;
					block.SetActive(true);
                }
            }
			Array.Resize(ref blocksActive, blocks.Count);
			for(int i = 0; i < blocks.Count; i++)
            {
				blocksActive[i] = true;
            }
        }
    }

	public List<Block> blocks = new List<Block>();

	public void UpdateScoreUI () {
		// update text canvas
		uiText.text = "" + score;
	}

    private void Update()
    {
		for (int i = 0; i < playerCount; i++)
        {
			var rotation = paddles[i].transform.rotation;
			var euler = rotation.eulerAngles;
			euler.z = Mathf.Clamp((euler.z + playerPaddleRotationDirections[i] * 5 * Time.deltaTime), (base_angles[i] - 180 / playerCount + 5), (base_angles[i] + 180 / playerCount - 5));
			rotation.eulerAngles = euler;
			paddles[i].transform.rotation = rotation;
		}

		for(int i = 0; i < ballCount; i++)
        {
			var ball = balls[i];
			ballPositions[i] = ball.position;
			ballDirections[i] = ball.velocity.normalized;
			ballVelocities[i] = ball.velocity.magnitude;
        }
	}

	void FixedUpdate () {
	}

	void OnDestroy () {

		// unregister airconsole events on scene change
		if (AirConsole.instance != null) {
			AirConsole.instance.onMessage -= OnMessage;
		}
	}
}
