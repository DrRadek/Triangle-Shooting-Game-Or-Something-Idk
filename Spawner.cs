using Godot;
using System;

public partial class Spawner : Node2D
{
    [Export] PackedScene enemyScene;
    Timer timer = new Timer();
    float timeBetweenEnemies = 10;

    [Export] Player player;
    [Export] Label label;
    float score = 0;

    public override void _Ready()
    {
        AddChild(timer);
        //timer.Start(timeBetweenEnemies);
        timer.Timeout += TimerTimeout;
        TimerTimeout();


    }

    private void TimerTimeout()
    {
        var instance = (Enemy)enemyScene.Instantiate();
        AddChild(instance);

        timeBetweenEnemies *= 0.9f;
        timeBetweenEnemies = Mathf.Max(timeBetweenEnemies, 1);
        timer.Start(timeBetweenEnemies);

        if (player.health > 0)
            score++;

        label.Text = $"Score: {score}";
    }
}
