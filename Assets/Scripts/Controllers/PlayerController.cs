using UnityEngine;

public class PlayerController : MonoSingleton<PlayerController>
{
    private int _health;
	private int _maxHealth;

    private int _strength = 5;
    public int Strength
    {
        get { return _strength; }
    }

	void Awake()
    {
        LoadPlayerInfo();
    }

    public void AttackPlayer(int strength, string attackerName)
    {
        _health = Mathf.Max(0, _health - strength);

    }

	public void Heal(int amount)
	{
		_health = Mathf.Min(_maxHealth, _health + amount);
		MessageManager.Instance.SendPlayerHealedMessage(amount, _health);
	}

    private void LoadPlayerInfo()
    {
        //load from save file

        _health = 100;
		_maxHealth = 100;
    }
}
