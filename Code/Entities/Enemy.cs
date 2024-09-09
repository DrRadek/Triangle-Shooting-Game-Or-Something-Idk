using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy : RigidBody2D
{
    public delegate void OnDeathDelegate();
    public OnDeathDelegate OnDeath;

    [Export] Node2D playerTarget;
    [Export] Node weapon;
    IUsable weaponInstance;

    int health = 3;

    public override void _Ready()
    {
        playerTarget = (Node2D)GetParent().GetParent().GetNode("Player");

        BodyEntered += EnemyBodyEntered;
        
        weaponInstance = (IUsable)weapon;
    }

    private void EnemyBodyEntered(Node body)
    {
        if (body is not IShieldCollectible shieldCollictible || shieldCollictible.IsFiredFromShield() == false || body.IsQueuedForDeletion())
            return;

        body.QueueFree();

        health--;
        if(health == 0)
        {
            OnDeath?.Invoke();
            QueueFree();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        weaponInstance.Use();

        var direction = (playerTarget.GlobalPosition - GlobalPosition) * 2;

        var lookAtVector = Position + direction.Normalized();
        Rotation = Mathf.LerpAngle(Rotation, Rotation + GetAngleTo(lookAtVector), 0.1f);

        ApplyCentralForce(direction);

        //ApplyCentralForce((playerTarget.GlobalPosition - GlobalPosition) * 2);
        //LookAt(playerTarget.GlobalPosition);
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        if (LinearVelocity.Length() > 500)
            LinearVelocity *= 0.5f;
    }
}
