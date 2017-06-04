using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    public enum EnemyState { Sleeping, Passive, Agressive }
    public enum BasicAction { Attack }

    [Header("Basic Info")]
    public int EnemyId;
    public string Name;
    public int Difficulty;
    public int Health;
    public int Strength;
    public EnemyState DefaultState;

    [Header("Interactions")]
    public BasicInteraction[] BasicInteractions;
    public SpecialInteraction[] SpecialInteractions;

    [Header("Prizes")]
    public int CoinPrize;
    public int ItemPrize;
    public string PrizeMessage;

    [System.Serializable]
    public class BasicInteraction
    {
        public BasicAction Action;
        public string Target;
        public InteractionOutcome Outcome;
    }

    [System.Serializable]
    public class SpecialInteraction
    {
        public string Action;
        public string Target;
        public InteractionOutcome Outcome;
    }

    [System.Serializable]
    public class InteractionOutcome
    {
        public int ItemPrizeId;
        public float AttackModifier;
        public int CoinPrize;
        public string Message;
    }
}

