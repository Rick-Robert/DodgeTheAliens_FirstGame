using Godot;
using System;

public partial class Hud : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

	public void ShowMessage(String text)
	{
		var message = GetNode<Label>("Message");
		message.Text = text;
		GetNode<Timer>("MessageTimer").Start();
	}

	async public void ShowGameOver()
	{
		GetNode<Timer>("MessageTimer").Stop();
		await ToSignal(GetTree().CreateTimer(0.5), SceneTreeTimer.SignalName.Timeout);
		
		//messageTimer, Timer.SignalName.Timeout
		var message = GetNode<Label>("Message");
		message.Text = "Game Over!";
		message.Show();

		//await ToSignal(GetTree().CreateTimer(0.5), SceneTreeTimer.SignalName.Timeout);
		GetNode<Button>("StartButton").Text = "REPLAY?";
		GetNode<Button>("StartButton").Show();
	}

	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}	
	
	public void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide();
		EmitSignal(SignalName.StartGame);
		
	}

	public void OnMessageTimerTimeout()
	{
		GetNode<Label>("Message").Hide();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
