using UnityEngine;

public class ObstacleController : MonoSingleton<ObstacleController>
{
    private string _obstacleId;
    private int _health = 0;

    public void SetObstacle(string obstacleId)
    {
        _obstacleId = obstacleId;
        ObstacleData data = ObstacleDatabase.GetObstacleData(obstacleId);
        MessageManager.SendStringMessage(data.EncounterMessage);

        _health = data.Health;
    }

    public void ExeuteObstacleActions()
    {
        if (ObstaclePresent())
        {
            ObstacleData data = ObstacleDatabase.GetObstacleData(_obstacleId);
            if (data.ObstacleActions != null && data.ObstacleActions.Length > 0)
            {
                //Get random action and execute
                int outcomeIndex = Random.Range(0, data.ObstacleActions.Length);
                data.ObstacleActions[outcomeIndex].ExecuteOutcome();
            }
        }
    }

    public void DamageObstacle(int damage)
    {
        if (!string.IsNullOrEmpty(_obstacleId))
        {
            _health -= damage;
            if (_health <= 0)
            {
                //Execute final outcome
                ObstacleData data = ObstacleDatabase.GetObstacleData(_obstacleId);
                data.CompletedOutcome.ExecuteOutcome();

                _obstacleId = null;
            }
        }
    }

    public bool ObstaclePresent()
    {
        return (!string.IsNullOrEmpty(_obstacleId) && _health > 0);
    }
}
