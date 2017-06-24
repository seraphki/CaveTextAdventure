﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ObstacleDatabase
{
    public static ObstacleArray GetObstacleArray()
    {
        string raw = Helpers.LoadGameFile(InformationSet.ObstacleInformation);
        Debug.Log(raw);
        return JsonUtility.FromJson<ObstacleArray>(raw);
    }

    private static Dictionary<string, ObstacleData> _obstacleData;
    private static Dictionary<string, ObstacleData> _obstacles
    {
        get
        {
            if (_obstacleData == null)
            {
                LoadObstacleData();
            }
            return _obstacleData;
        }
    }

    public static bool LoadObstacleData()
    {
        _obstacleData = new Dictionary<string, ObstacleData>();
        ObstacleArray obstacleArray = GetObstacleArray();
        if (obstacleArray != null)
        {
            for (int i = 0; i < obstacleArray.Obstacles.Length; i++)
            {
                ObstacleData enemy = obstacleArray.Obstacles[i];
                _obstacles.Add(enemy.ObstacleId, enemy);
            }
            return true;
        }

        return false;
    }

    public static int ObstacleCount
    {
        get { return _obstacles.Count; }
    }

    public static string[] GetAllObstacleIds()
    {
        return _obstacles.Keys.ToArray();
    }

    public static ObstacleData GetObstacleData(string enemyId)
    {
        return _obstacles[enemyId];
    }
}

[System.Serializable]
public class ObstacleData : IEditable
{
    public string ObstacleId;
    public string Name;
    public string Description;
    public int Health;
    public int Strength;
    public string[] InteractionIds;
    public Outcome CompletedOutcome;
}

[System.Serializable]
public class ObstacleArray : IEditable
{
    public ObstacleData[] Obstacles;
}