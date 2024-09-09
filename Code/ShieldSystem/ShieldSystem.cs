using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using static Godot.DisplayServer;

public partial class ShieldSystem : Node
{
    [Export] Godot.Collections.Array<PackedScene> shieldLevels = new();
    [Export] Node storageNode;
    [Export] Node globalStorageNode;
    [Export] Marker2D offsetXNode;

    const int shieldRepeatAngle = 30;
    const int maxShieldCount = 360 / shieldRepeatAngle;
    int maxShieldLevels = 0;
    float shieldChunkOffsetX;
    List<ShieldChunk> activeShields = new() {};
    List<Node2D> catchedObjects = new();

    // Attack mode vars (TODO: remove, not needed)
    float targetRotation = 0; // How much to rotate towards mouse, range [0, 1]
    float targetAngle = 180; // Distribution of pieces, range [0, 180] 

    public override void _Ready()
    {
        shieldChunkOffsetX = offsetXNode.Position.X;

        maxShieldLevels = shieldLevels.Count - 1;
        for (int i = 0; i < maxShieldCount; i++)
            activeShields.Add(null);

        for (int i = 0; i < 3; i++) // maxShieldCount
            UpgradeShield(i);

        //UpgradeShield(4);

        for (int i = 10; i < 12; i++) // maxShieldCount
            UpgradeShield(i);

        UpgradeShield(4);

        UpgradeShield(8);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEvent keyEvent && keyEvent.IsPressed())
        {
            if (@event.IsActionPressed(GameInput.Shoot))
            {
                OnShoot();
            }
        }
    }

    void UpgradeShield(int index)
    {
        var activeShield = activeShields[index];
        if (!CanUpgradeShield(activeShield, out int nextLevel))
            return;

        var instance = (ShieldChunk)shieldLevels[nextLevel].Instantiate();
        storageNode.AddChild(instance);
        instance.Rotation = Mathf.DegToRad(shieldRepeatAngle * index);
        GD.Print(instance.Transform.X);
        //instance.Position += instance.GlobalTransform.Y;
        instance.Position += instance.Transform.X * shieldChunkOffsetX; //new Vector2(shieldChunkOffsetX, 0);
        instance.ShieldSystem = this;

        activeShield?.QueueFree();
        activeShields[index] = instance;
    }

    bool CanUpgradeShield(ShieldChunk shieldChunk, out int nextLevel)
    {
        nextLevel = shieldChunk == null ? 0 : (shieldChunk.GetPenetrationLevel() + 1);
        return nextLevel <= maxShieldLevels;
    }

    public void AddBody(IShieldCollectible body)
    {
        var node = body as Node2D;

        catchedObjects.Add(node);

        Update();
        node.CallDeferred("reparent", storageNode);
        body.OnCatch();

        //rb.QueueFree();
        //rb.Sleeping = true;

        //body.ShootBack();



        //body.SetIsFiredFromShield(true);

        //rb.Reparent(storageNode);
        //storageNode.AddChild(rb);
        //node.ProcessMode = ProcessModeEnum.Disabled;

    }

    //void ChangeRotation(float targetRotation)
    //{
    //    this.targetRotation = targetRotation;
    //    //Update();
    //}

    //void ChangeMaxAngle(float targetAngle)
    //{
    //    this.targetAngle = targetAngle;
    //    //Update();
    //}

    void Update()
    {
        // TODO: implement changing positions and rotations
        //float step = (targetAngle * 2) / ((float)catchedObjects.Count - 1);
        //float currentAngle = 0;
        //for (int i = 0; i < 1; i++)
        //{
        //    Node2D body = catchedObjects[i];
        //    float finalAngle;
        //    if (i % 2 == 1)
        //    {
        //        currentAngle += step;
        //        finalAngle = currentAngle;
        //    }
        //    else
        //    {
        //        finalAngle = -currentAngle;
        //    }
        //    body.Rotation = Mathf.DegToRad(finalAngle);
        //    body.Position = body.Transform.X * shieldChunkOffsetX;

        //}
    }

    void OnShoot()
    {
        foreach (var rb in catchedObjects)
        {
            if (!IsInstanceValid(rb) || rb == null)
            {
                continue;
            }

            rb.CallDeferred("reparent", globalStorageNode);
            (rb as IShieldCollectible).ShootBack();
            //rb.ProcessMode = ProcessModeEnum.Inherit;
        }
        // TODO: implement shooting


        catchedObjects.Clear();
    }
}
