using UnityEngine;
using System.Collections.Generic;

/* This class is responsible for instantiating all
 enemy prefabs. */

[System.Serializable]
public class Spawner {

    public static Spawner instance;

    public GameObject enemyPrefab;
    public Transform enemyParent;

    //private Pool<Enemy> pooledEnemies = new Pool<Enemy>();
    public List<Enemy> enemies = new List<Enemy>();
    private const int poolSize = 30;

    private int _wave = 1;
    public int wave { get { return _wave; } set { GUIManager.SetWave(value); _wave = value; } } 
    private int enemiesRemainingThisWave = 12;
    private int enemiesToSpawnThisWave = 12;
    private int enemiesKilledThisWave = 0;

    private float timer = 0.0f;
    private float spawnSpeed = 1.0f;

    public Spawner () {
        /*List<Enemy> list = new List<Enemy>(poolSize);
        for (int i = 0; i < poolSize; ++i) { list.Add(new Enemy()); }
        pooledEnemies.Initialise(list);*/

        instance = this;
        enemies = new List<Enemy>();
    }

    public void Update () {
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
            if (enemiesKilledThisWave >= enemiesToSpawnThisWave) {
                ++wave;

                // Calculate number of enemies to spawn next round.
                enemiesKilledThisWave = 0;
                enemiesToSpawnThisWave = wave * 15;
                enemiesRemainingThisWave = enemiesToSpawnThisWave;
                spawnSpeed = Mathf.Max(spawnSpeed - 0.1f, 0.04f);
            }
        }
    }

    private void SpawnEnemy () {
        --enemiesRemainingThisWave;

        GameObject enemy = Object.Instantiate(enemyPrefab, enemyParent);
        enemy.SetActive(true);
        Enemy e = enemy.GetComponent<Enemy>();
        enemies.Add(e);
        e.GetComponent<RectTransform>().anchoredPosition = GameManager.currentPath[0].apos;
    }

    public static void EnemyRemove(Enemy e) {
        instance.enemies.Remove(e);
        ++instance.enemiesKilledThisWave;
    }
}
