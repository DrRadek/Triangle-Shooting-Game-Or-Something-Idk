using Godot;
using System;

public partial class Bullet : RigidBody2D, IShieldCollectible
{
    [Export] CanvasItem canvasItem;

    bool isFiredFromShield = false;

    double maxLifetime = 4f;
    Timer timer;

    public override void _Ready()
    {
        timer = new Timer();
        AddChild(timer);
        timer.Start(maxLifetime);
        timer.Timeout += TimerTimeout;

        LinearVelocity = GlobalTransform.X * 800;
        //AddConstantCentralForce(Vector2.Right * 500);
    }

    private void TimerTimeout()
    {
        QueueFree();
    }

    // IShieldCollectable interface
    public int GetPenetrationLevel()
    {
        return 0;
    }

    public void OnCatch()
    {
        ProcessMode = ProcessModeEnum.Disabled;
        Rotate(Mathf.Pi);
        isFiredFromShield = true;
        var color = canvasItem.Modulate;
        color.A = 0.4f;
        canvasItem.Modulate = color;


    }

    public void ShootBack(float boostLevel = 1.5f)
    {
        timer.Start(maxLifetime);
        LinearVelocity = GlobalTransform.X * LinearVelocity.Length() * boostLevel;
        ProcessMode = ProcessModeEnum.Inherit;
    }

    public bool IsFiredFromShield()
    {
        return isFiredFromShield;
    }

    //public void SetIsFiredFromShield(bool value)
    //{
    //    isFiredFromShield = value;
    //}
}
