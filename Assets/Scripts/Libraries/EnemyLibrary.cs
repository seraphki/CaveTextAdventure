using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyLibrary
{
    public static EnemyLibrary Instance;

    private Dictionary<int, EnemyData> _enemies;
    private const string _path = "Data/Enemies";

    public int EnemyCount
    {
        get { return _enemies.Count; }
    }

    public EnemyLibrary()
    {
        Instance = this;

        _enemies = new Dictionary<int, EnemyData>();
        EnemyData[] enemies = Resources.LoadAll<EnemyData>(_path);
        for (int i = 0; i < enemies.Length; i++)
        {
            _enemies.Add(enemies[i].EnemyId, enemies[i]);
        }
        Debug.Log(_enemies.Count + " Enemies Loaded");
    }

    public int[] GetAllEnemyIds()
    {
        return _enemies.Keys.ToArray();
    }

    public int[] GetEnemyIdsByLevel(int level)
    {
        int[] levelEnemies = _enemies.Where(i => i.Value.Difficulty <= level).Select(k => k.Key).ToArray();
        return levelEnemies;
    }

    public EnemyData GetEnemyData(int enemyId)
    {
        return _enemies[enemyId];
    }
}
