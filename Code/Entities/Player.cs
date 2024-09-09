using Godot;
using System;

public partial class Player : RigidBody2D
{
    [Export] Node2D rotatedNode;

    [Export] float speed = 5000;
    [Export] float steeringAngle = 20;

    public int maxHealth = 3;
    public int health = 3;

    [Export] Label label1;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        //GD.Print("?");

        float forward = (Input.IsActionPressed(GameInput.MovementBackward) ? 1 : 0) - (Input.IsActionPressed(GameInput.MovementForward) ? 1 : 0);
        float right = (Input.IsActionPressed(GameInput.MovementRight) ? 1 : 0) - (Input.IsActionPressed(GameInput.MovementLeft) ? 1 : 0);


        //GD.Print(forward);

        var directionVector = new Vector2(right, forward).Normalized();
        ApplyCentralForce(directionVector * speed); // Transform.X * 
        //ApplyForce(GlobalTransform.Y * forward * speed); // , GlobalTransform.Y * directionVector

        var mouseDirection = GetGlobalMousePosition() - GlobalPosition;
        //GD.Print(mouseDirection.Normalized());

        var lookAtVector = Position + mouseDirection.Normalized();
        rotatedNode.Rotation = Mathf.LerpAngle(rotatedNode.Rotation, rotatedNode.Rotation + rotatedNode.GetAngleTo(lookAtVector), 0.1f);
        //rotatedNode.LookAt(Position + mouseDirection.Normalized());
    }



    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        LinearVelocity *= 0.9f;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is not IShieldCollectible shieldCollictible || shieldCollictible.IsFiredFromShield() == true || body.IsQueuedForDeletion())
            return;

        body.QueueFree();

        health--;
        if (health == 0)
        {
            GD.Print("GAME OVER");
        }

        
        label1.Text = $"Lives: {health}";
    }
}
