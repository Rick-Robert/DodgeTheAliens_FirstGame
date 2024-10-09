using Godot;
using System;

public partial class Enemy : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[GD.Randi(/*N*/)%mobTypes.Length]); //GD.Randi Random 32 bit integer until N-1, N is smaller than 2^32
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
    	QueueFree();
	}
}
