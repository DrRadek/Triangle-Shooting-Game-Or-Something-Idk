using Godot;
using System;
using System.Collections.Generic;

public partial class WaveHandler : Node
{
    Label playerHealthLabel;
    Label totalEnemiesLeftLabel;

    List<Label> enemiesLeftLabels = new();


    int currentWave = 1;
    int totalEnemiesLeft;
    float delayBetweenSpawns = 1;

    Godot.Collections.Array<float> enemiesToSpawn = new() { 3, 0 };
    Godot.Collections.Array<int> enemiesLeftToSpawn = new() { 3, 0 };
    Godot.Collections.Array<int> enemiesLeft = new() { 3, 0 };

    [Export] Godot.Collections.Array<PackedScene> enemyTypes = new();
    [Export] Godot.Collections.Array<PackedScene> enemyNames = new();
    [Export] Player player;
    [Export] Node enemyStorage;

    bool isActiveWave = false;
    Timer timer = new();

    Random random = new Random();

    public int TotalEnemiesLeft { get => totalEnemiesLeft; 
        set {
            totalEnemiesLeft = value; 
            if(totalEnemiesLeft == 0)
            {
                isActiveWave = false;
                OpenUpgradeMenu();
                AdvanceToNextWave();
            }
        }
    }

    private void OpenUpgradeMenu()
    {
        // throw new NotImplementedException();
    }

    private void OnEnemySpawn()
    {
        if (!isActiveWave)
            return;

        int enemyIndex = random.Next(enemiesLeftToSpawn.Count);

        if (enemiesLeftToSpawn[enemyIndex] <= 0)
        {
            enemyIndex = -1;

            for (int i = 0; i < enemiesLeftToSpawn.Count;i++)
            {
                var enemyCount = enemiesLeftToSpawn[i];
                if (enemyCount <= 0)
                {
                    enemyIndex = i;
                    break;
                }
            }

            if(enemyIndex == -1)
            {
                return;
            }

            
        }

        enemiesLeftToSpawn[enemyIndex]--;
        enemiesLeft[enemyIndex]++;
        var enemy = (Enemy)enemyTypes[enemyIndex].Instantiate();
        enemyStorage.AddChild(enemy);
        enemy.OnDeath += () => OnEnemyDeath(enemyIndex);

    }

    void OnEnemyDeath(int enemyIndex)
    {
        enemiesLeft[enemyIndex]--;
        TotalEnemiesLeft--;
    }

    public override void _Ready()
    {
        AddChild(timer);
        timer.Timeout += OnEnemySpawn;
        StartWave();
    }

    void AdvanceToNextWave()
    {
        currentWave++;
        if (currentWave == 8)
        {
            enemiesToSpawn[1] = 1;
        }
        enemiesToSpawn[0] *= 1.2f;
        enemiesToSpawn[1] *= 1.15f;
        if (enemiesToSpawn[0] > 100)
            enemiesToSpawn[0] = 100;

        delayBetweenSpawns *= 0.9f;

        isActiveWave = true;

        TotalEnemiesLeft = 0;
        for (int i = 0; i < enemiesToSpawn.Count; i++)
        {
            enemiesLeftToSpawn[i] = (int)enemiesToSpawn[i];
            enemiesLeft[i] = 0;
            TotalEnemiesLeft += enemiesLeftToSpawn[i];
        }

        StartWave();
    }

    void StartWave()
    {
        PlayerStats.currentHealth = PlayerStats.maxHealth;
        timer.Start(delayBetweenSpawns);

    }
}
