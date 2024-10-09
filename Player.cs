using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
	public int Speed {get;set;} = 300;
	
	public Vector2 ScreenSize;
	public bool stop = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		var vel = Vector2.Zero;
		
		//if(Input.IsKeyPressed(Key.Enter)) {Show(); stop = true;}

		if(Input.IsActionPressed("move_up")){
			vel.Y += (float)-1;
		}
		if(Input.IsActionPressed("move_down")){
			vel.Y += (float)1;
		}
		if(Input.IsActionPressed("move_left")){
			vel.X += (float)-1;
			
		}
		if(Input.IsActionPressed("move_right")){
			vel.X += (float)1;
		}
		/*if(Input.IsKeyPressed(Key.Z)){
			Speed += 1;
		}else if (Input.IsKeyPressed(Key.X)){
			Speed -= 1;
		}*/
		
		if(vel.X != 0){
			animatedSprite2D.Animation = "walk";
		}else if(vel.Y != 0){
			animatedSprite2D.Animation = "up";
		}
		if(vel.Length()>0){
			vel = vel.Normalized()*Speed;
			//vel.angle() gets the angle of a vector with respect to the X axis (can be represented with (1,0) vector)
			/*var cos = vel.Dot(Vector2.Up)/(vel.Length()*Vector2.Up.Length());
			if(vel.X >= 0){
				Rotation = Mathf.Acos(cos);
			}else{
				Rotation = -Mathf.Acos(cos);
			}*/
			Rotation = vel.Angle() + Mathf.Pi/2; //counter angles in clock wise direction (e.g. up is -90 degrees, down is 90 degrees)
			animatedSprite2D.Play();
		}else{
			animatedSprite2D.Stop();
		}
		Position += vel*(float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X,30,ScreenSize.X-30),
			y: Mathf.Clamp(Position.Y,30,ScreenSize.Y-30)
		);
		
	}
	[Signal]
	public delegate void HitEventHandler();

	public void OnBodyEntered(Node2D body){
		Hide();
		EmitSignal(SignalName.Hit);
		GetNode <CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled,true);
	}

	public void Start(Vector2 position){
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
}

