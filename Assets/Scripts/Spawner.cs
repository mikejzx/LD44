using UnityEngine;
using System.Collections.Generic;

/* This class is responsible for instantiating all
 enemy prefabs. */

[System.Serializable]
public class Spawner {

    public static Spawner instance;

    private bool _ready = false;
    public bool ready {
        get { return _ready; }
        set {
            GUIManager.SetReady(value);
            _ready = value;
        }
    }
    private bool _doublespeed = false;
    public bool doublespeed {
        get {
            // Only double speed when the wave is running.
            if (!ready) {
                return false;
            }
            return _doublespeed;
        }
        set {
            GUIManager.SetDoubleSpeed(value);
            _doublespeed = value;
        }
    }

    public GameObject enemyPrefab;
    public Transform enemyParent;

    //private Pool<Enemy> pooledEnemies = new Pool<Enemy>();
    public List<Enemy> enemies = new List<Enemy>();
    private const int poolSize = 30;

    private int _wave = 1;
    public static int wave { get { return instance._wave; } set { GUIManager.SetWave(value); instance._wave = value; } } 
    private int enemiesRemainingThisWave = 12;
    private int enemiesToSpawnThisWaveTotal = 12;
    private int enemiesKilledThisWave = 0;

    private int enemiesToSpawn_basic,
        enemiesToSpawn_tough,
        enemiesToSpawn_tougher,
        enemiesToSpawn_insane;

    private float timer = 0.0f;
    private float spawnSpeed = 1.0f;

    public void Initialise () {
        /*List<Enemy> list = new List<Enemy>(poolSize);
        for (int i = 0; i < poolSize; ++i) { list.Add(new Enemy()); }
        pooledEnemies.Initialise(list);*/

        instance = this;
        enemies = new List<Enemy>();

        wave = 0;
        spawnSpeed = 1.0f;
        WaveIncrement();
        doublespeed = false;
    }

    public void Update () {
        Time.timeScale = doublespeed ? 2.0f : 1.0f;
        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.D)) {
            doublespeed ^= true;
        }

        if (!ready) {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                ready = true;
                if (wave == 1) {
                    // Remove msg telling user to start.
                    bool success;
                    GUIManager.RemoveInstructionMsg(out success);
                    // If the user tries to start before the fade finishes.
                    ready = success;
                }
            }
            return;
        }

        if (enemiesRemainingThisWave > 0) {
            timer += Time.deltaTime;
            if (timer > spawnSpeed) {
                timer = 0.0f;
                SpawnEnemy();
            }
        }
        else {
            timer = 0.0f;

            // If  a sufficient amount of enemies have died, then goto next wave
            if (enemiesKilledThisWave >= enemiesToSpawnThisWaveTotal) {
                WaveIncrement();

                // Give player more life
                Player.health += wave * GameManager.WAVE_BONUS_LIFE_MUL;

                // Wait for user input to continue.
                ready = false;
            }
        }
    }

    private void WaveIncrement () {
        ++wave;
        AudioHandler.PlayWaveFinishSound();

        // Calculate number of enemies to spawn next round.
        enemiesKilledThisWave = 0;
        enemiesToSpawnThisWaveTotal = wave * 10;
        enemiesRemainingThisWave = enemiesToSpawnThisWaveTotal;
        // Get the count of each enemy type
        WaveSpawnHandler.GetEnemyCountsTyped(wave, enemiesToSpawnThisWaveTotal,
            out enemiesToSpawn_basic, out enemiesToSpawn_tough,
            out enemiesToSpawn_tougher, out enemiesToSpawn_insane);
        // Had to fix this...
        enemiesToSpawnThisWaveTotal = enemiesToSpawn_basic + enemiesToSpawn_tough + enemiesToSpawn_tougher + enemiesToSpawn_insane;
        enemiesRemainingThisWave = enemiesToSpawnThisWaveTotal;
        spawnSpeed = Mathf.Max(spawnSpeed - 0.1f, 0.04f);
    }

    private void SpawnEnemy () {
        --enemiesRemainingThisWave;
        List<int> available = new List<int>();
        if (enemiesToSpawn_basic > 0) { available.Add(0); }
        if (enemiesToSpawn_tough > 0) { available.Add(1); }
        if (enemiesToSpawn_tougher > 0) { available.Add(2); }
        if (enemiesToSpawn_insane > 0) { available.Add(3); }
        int rand = Random.Range(0, available.Count);
        if (available.Count == 0) {
            Debug.Log("Available count was zero... o_O");
            enemiesRemainingThisWave = 0;
            return;
        }
        EnemyType type = EnemyType.BasicTypeA;
        switch (available[rand]) {
            // Basic
            case (0): {
                // Random basic
                int rand2 = Random.Range(0, 3);
                switch (rand2) {
                    case (0): { type = EnemyType.BasicTypeA; } break;
                    case (1): { type = EnemyType.BasicTypeB; } break;
                    case (2): { type = EnemyType.BasicTypeC; } break;
                }
                --enemiesToSpawn_basic;
            } break;
            // Tough
            case (1): {
                type = EnemyType.Tough;
                --enemiesToSpawn_tough;
            } break;

            // Tougher
            case (2): {
                type = EnemyType.Tougher;
                --enemiesToSpawn_tougher;
            } break;

            // Insane
            case (3): {
                type = EnemyType.Insane;
                --enemiesToSpawn_insane;
            } break;
            // Not very modular ... :(
        }

        GameObject enemy = Object.Instantiate(enemyPrefab, enemyParent);
        enemy.SetActive(true);
        Enemy e = enemy.GetComponent<Enemy>();
        e.type = type;
        enemies.Add(e);
        e.GetComponent<RectTransform>().anchoredPosition = GameManager.currentPath[0].apos;
    }

    public static void EnemyRemove(Enemy e) {
        instance.enemies.Remove(e);
        ++instance.enemiesKilledThisWave;
    }
}
