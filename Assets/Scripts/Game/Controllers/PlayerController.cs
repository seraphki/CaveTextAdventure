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

    public void ModifyHealth(int health)
    {
        _health += _health;
    }

    private void LoadPlayerInfo()
    {
        //TODO: load from save file
        _health = 100;
		_maxHealth = 100;
    }
}
