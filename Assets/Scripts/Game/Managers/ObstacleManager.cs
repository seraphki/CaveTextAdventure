using UnityEngine;
using UnityEngine.Events;

public class ObstacleManager
{
    private string _obstacleId;
    public string ObstacleId
    {
        get { return _obstacleId; }
        set
        {
            _obstacleId = value;
            _obstacleData = ObstacleDatabase.GetObstacleData(ObstacleId);
            _obstacleHealth = _obstacleData.Health;
        }
    }

    private ObstacleData _obstacleData;
    private int _obstacleHealth;

    public ObstacleManager()
    {

    }

    public void DoAction(string action, string target)
    {
        Debug.Log("Doing action " + action + ", " + target + "(" + _obstacleData.Name + ")");

        //Check for Base Interactions
        if (action.Trim().ToLower() == "attack" && target.Trim().ToLower() == _obstacleData.Name.ToLower())
        {
            //Basic Attack
            LandHit();
        }

        for (int i = 0; i < _obstacleData.InteractionIds.Length; i++)
        {
            //Interaction interaction = _obstacleData.EnemyInteractions[i];
     //       if (action.Trim().ToLower() == interaction.Action.ToString().ToLower())
     //       {
     //           if (string.IsNullOrEmpty(interaction.Target) || Helpers.StringLooseCompare(target, interaction.Target))
     //           {
					//MessageManager.SendActionMessage(interaction.Action.ToString(), interaction.Target, _obstacleData.Name);
     //               ExecuteOutcome(interaction.InteractionOutcome);
     //           }
     //       }
        }

        if (_obstacleHealth > 0)
        {
			//Enemy Retaliates
			GameController.Instance.AddActionToQueue(new UnityAction(EnemyAttack));
        }
    }

    private void LandHit(float powerModifier = 1)
    {
        int healthLost = Mathf.CeilToInt(PlayerController.Instance.Strength * powerModifier);
        _obstacleHealth -= healthLost;

		MessageManager.SendPlayerAttackedMessage(_obstacleData.Name, healthLost, (_obstacleHealth < _obstacleData.Health * 0.25f));

		if (_obstacleHealth <= 0)
        {
            BattleWon();
        }
    }

    private void EnemyAttack()
    {
        PlayerController.Instance.ModifyHealth(-_obstacleData.Strength);
    }

    private void BattleWon()
    {
        _obstacleData.CompletedOutcome.ExecuteOutcome();
        GameController.WorldManagerInst.GetCurrentRoom().Look();
    }
}
