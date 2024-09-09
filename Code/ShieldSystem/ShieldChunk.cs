using Godot;
using System;

public partial class ShieldChunk : Area2D
{
    [Export] ShieldSystem shieldSystem;
    [Export] int penetrationLevel;

    public ShieldSystem ShieldSystem {set => shieldSystem = value; }

    public int GetPenetrationLevel() { return penetrationLevel; }


    public override void _Ready()
    {
        Monitoring = true;
        BodyEntered += ShieldChunkBodyEntered;
    }

    private void ShieldChunkBodyEntered(Node2D body)
    {
        if (body is not IShieldCollectible shieldCollectable)
            return;

        if (shieldCollectable.GetPenetrationLevel() > GetPenetrationLevel() || shieldCollectable.IsFiredFromShield())
            return;
        
        shieldSystem.AddBody(shieldCollectable);
    }
}
