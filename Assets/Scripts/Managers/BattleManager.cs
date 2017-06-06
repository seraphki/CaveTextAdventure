using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoSingleton<BattleManager>
{
    private int _enemyId = 0;
    public int EnemyId
    {
        get { return _enemyId; }
        set
        {
            _enemyId = value;
            _enemyData = EnemyLibrary.Instance.GetEnemyData(EnemyId);
            _enemyHealth = _enemyData.Health;
        }
    }

    private EnemyData _enemyData;
    private int _enemyHealth;

    public void StartBattle()
    {
		MessageManager.Instance.SendBeginBattleMessage(_enemyData.Name);
        UIController.Instance.SwitchToBattleMenu(_enemyData.Name);
    }

    public void DoAction(string action, string target)
    {
        UIController.Instance.NewLine();
        Debug.Log("Doing action " + action + ", " + target + "(" + _enemyData.Name + ")");

        //Check for Base Interactions
        if (action.Trim().ToLower() == "attack" && target.Trim().ToLower() == _enemyData.Name.ToLower())
        {
            //Basic Attack
            LandHit();
        }

        for (int i = 0; i < _enemyData.BasicInteractions.Length; i++)
        {
            EnemyData.BasicInteraction interaction = _enemyData.BasicInteractions[i];
            if (action.Trim().ToLower() == interaction.Action.ToString().ToLower())
            {
                if (string.IsNullOrEmpty(interaction.Target) || Helpers.StringLooseCompare(target, interaction.Target))
                {
					MessageManager.Instance.SendActionMessage(interaction.Action.ToString(), interaction.Target, _enemyData.Name);
                    ExecuteOutcome(interaction.Outcome);
                }
            }
        }

        for (int i = 0; i < _enemyData.SpecialInteractions.Length; i++)
        {
            EnemyData.SpecialInteraction interaction = _enemyData.SpecialInteractions[i];
            if (Helpers.StringLooseCompare(action, interaction.Action))
            {
                if (string.IsNullOrEmpty(interaction.Target) || Helpers.StringLooseCompare(target, interaction.Target))
                {
                    ExecuteOutcome(interaction.Outcome);
                }
            }
        }

        if (_enemyHealth > 0)
        {
			//Enemy Retaliates
			GameController.Instance.AddActionToQueue(new UnityAction(EnemyAttack));
        }
    }

    private void ExecuteOutcome(EnemyData.InteractionOutcome outcome)
    {
        //Attack Target
        if (outcome.AttackModifier > 0)
        {
            LandHit(outcome.AttackModifier);
        }

        //Cash Prizes
        if (outcome.CoinPrize > 0)
        {
            InventoryManager.Instance.AddCoins(outcome.CoinPrize);
        }

        //Item Prize
        if (outcome.ItemPrizeId > 0)
        {
            InventoryManager.Instance.AddItem(outcome.ItemPrizeId, 1);
        }

        //Message
        if (!string.IsNullOrEmpty(outcome.Message))
        {
			MessageManager.Instance.SendOutcomeMessage(outcome.Message);
        }
    }

    private void LandHit(float powerModifier = 1)
    {
        int healthLost = Mathf.CeilToInt(PlayerController.Instance.Strength * powerModifier);
        _enemyHealth -= healthLost;

		MessageManager.Instance.SendPlayerAttackedMessage(_enemyData.Name, healthLost, (_enemyHealth < _enemyData.Health * 0.25f));

		if (_enemyHealth <= 0)
        {
            BattleWon();
        }
    }

    private void EnemyAttack()
    {
        PlayerController.Instance.AttackPlayer(_enemyData.Strength, _enemyData.Name);
    }

    private void BattleWon()
    {
		MessageManager.Instance.SendBattleWonMessage(_enemyData.Name, _enemyData.PrizeMessage);
        
        if (_enemyData.CoinPrize > 0)
        {
            InventoryManager.Instance.AddCoins(_enemyData.CoinPrize);
        }
        
        if (_enemyData.ItemPrize > 0)
        {
            InventoryManager.Instance.AddItem(_enemyData.ItemPrize, 1);
        }

        UIController.Instance.SwitchToExploreMenu();
        CaveManager.Instance.GetCurrentRoom().EnemyId = 0;
        CaveManager.Instance.GetCurrentRoom().Look();
        UIController.Instance.NewLine();
    }
}
