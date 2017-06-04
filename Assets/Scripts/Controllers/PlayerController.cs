using UnityEngine;

public class PlayerController : MonoSingleton<PlayerController>
{
    private int _health;

    private int _strength = 5;
    public int Strength
    {
        get { return _strength; }
    }

	void Awake()
    {
        LoadPlayerInfo();
    }

    public void Hit(int strength)
    {
        UIController.Instance.TextOutputUpdate("You've been hit for " + strength + "!");
        _health = Mathf.Max(0, _health - strength);
        UIController.Instance.TextOutputUpdate("You have " + _health + " health remaining.");
    }

    private void LoadPlayerInfo()
    {
        //load from save file

        _health = 100;
    }
}
