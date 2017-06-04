using UnityEngine;

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
        UIController.Instance.TextOutputNewLine();
        UIController.Instance.TextOutputUpdate("You've begun a battle with " + _enemyData.Name + "!");
        UIController.Instance.SwitchToBattleMenu(_enemyData.Name);
    }

    public void DoAction(string action, string target)
    {
        UIController.Instance.TextOutputNewLine();
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
                    string targetName = string.IsNullOrEmpty(interaction.Target) || Helpers.StringLooseCompare(target, interaction.Target) ?
                        _enemyData.Name : _enemyData.Name + " in the " + interaction.Target;

                    UIController.Instance.TextOutputUpdate("You " + interaction.Action + " the " + targetName);
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
            EnemyAttack();
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
            UIController.Instance.TextOutputUpdate(outcome.Message);
        }
    }

    private void LandHit(float powerModifier = 1)
    {
        int healthLost = Mathf.CeilToInt(PlayerController.Instance.Strength * powerModifier);
        _enemyHealth -= healthLost;
        UIController.Instance.TextOutputUpdate("You hit the " + _enemyData.Name + " for " + healthLost + "!");

        if (_enemyHealth <= 0)
        {
            BattleWon();
        }
        else if (_enemyHealth < _enemyData.Health * 0.2f)
        {
            UIController.Instance.TextOutputUpdate("The " + _enemyData.Name + " is severely wounded");
        }
    }

    private void EnemyAttack()
    {
        UIController.Instance.TextOutputNewLine();
        UIController.Instance.TextOutputUpdate("The " + _enemyData.Name + " is preparing to attack!");
        PlayerController.Instance.Hit(_enemyData.Strength);
    }

    private void BattleWon()
    {
        UIController.Instance.TextOutputUpdate("You defeated the " + _enemyData.Name + "!");
        
        if (_enemyData.CoinPrize > 0)
        {
            InventoryManager.Instance.AddCoins(_enemyData.CoinPrize);
        }
        
        if (_enemyData.ItemPrize > 0)
        {
            InventoryManager.Instance.AddItem(_enemyData.ItemPrize, 1);
        }

        if (!string.IsNullOrEmpty(_enemyData.PrizeMessage))
        {
            UIController.Instance.TextOutputUpdate(_enemyData.PrizeMessage);
        }

        UIController.Instance.SwitchToExploreMenu();
        CaveManager.Instance.GetCurrentRoom().EnemyId = 0;
        CaveManager.Instance.GetCurrentRoom().Look();
        UIController.Instance.TextOutputNewLine();
    }
}
