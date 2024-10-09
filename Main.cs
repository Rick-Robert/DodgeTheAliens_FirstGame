using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene EnemyScene {get;set;}
	[Export]
	public int incrSpeed = 50;
	private int _score;
	private int diff;
	// Called when the node enters the scene tree for the first time.
	
	
	

	public void NewGame()
	{
		var music = GetNode<AudioStreamPlayer2D>("Music");
		music.VolumeDb = 0; music.PitchScale = 1; music.Play(); 

		GetTree().CallGroup("Enemies", Node.MethodName.QueueFree);
		var hud = GetNode<Hud>("HUD");
		_score = 0;
		hud.UpdateScore(_score);
		hud.ShowMessage("Prepare To Die");
		
		var player = GetNode<Player>("Player");
		player.Rotation = 0;
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("StartTimer").Start();
	}
	public void GameOver()
	{
		var music = GetNode<AudioStreamPlayer2D>("Music");
		music.VolumeDb = -10; music.PitchScale = (float)0.5;

		GetNode<AudioStreamPlayer2D>("DeathSound").Play();
		GetNode<Timer>("EnemyTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<Hud>("HUD").ShowGameOver();
	}
	private void OnScoreTimerTimeout()
	{
		_score++;
		GetNode<Hud>("HUD").UpdateScore(_score);
	}
	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("EnemyTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	private void OnEnemyTimerTimeout()
	{
		Enemy enemy = EnemyScene.Instantiate<Enemy>();
		var enemySpawnLocation = GetNode<PathFollow2D>("EnemyPath/EnemySpawnLocation");
		enemySpawnLocation.ProgressRatio = GD.Randf();
		
		float direction = enemySpawnLocation.Rotation + Mathf.Pi/2;

		
		int intervalDiff = 15;
		

		enemy.Position = enemySpawnLocation.Position;

		direction += (float)GD.RandRange(-Mathf.Pi/4, Mathf.Pi/4);
		enemy.Rotation = direction;

		diff = Mathf.FloorToInt(_score/intervalDiff)*incrSpeed;
		var vel = new Vector2((float)GD.RandRange(150.0+diff*0.2, 250.0+diff),0);
		enemy.LinearVelocity = vel.Rotated(direction);

		AddChild(enemy);
	}
	public override void _Ready()
	{
		GetNode<Player>("Player").Hide();
		//NewGame();	
		GD.Print("Start");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
