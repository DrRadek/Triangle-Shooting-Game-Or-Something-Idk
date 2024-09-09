using Godot;
using System;

public partial class Weapon : Node, IUsable
{
    [Export] Node storageNode;
    [Export] PackedScene ammoInstance;

    bool isReady = true;
    float cooldown = 0.3f;

    Timer timer = new Timer();

    public override void _Ready()
    {
        timer.OneShot = true;
        AddChild(timer);

        storageNode = GetParent().GetParent();
    }

    public void Use()
    {
        if (timer.TimeLeft > 0)
            return;

        var instance = ammoInstance.Instantiate();
        AddChild(instance);
        instance.Reparent(storageNode);

        // Start cooldown
        timer.Start(cooldown);
    }
}
