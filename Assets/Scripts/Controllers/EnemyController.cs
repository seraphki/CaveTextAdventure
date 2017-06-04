using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    private EnemyData _data
    {
        get { return _data; }
        set
        {
            _data = value;
            parseData();
        }
    }

    private int _health = 0;

    public void Interact(string action, string target)
    {
        if (action == "attack")
        {
            attack(target);
        }
    }

    private void parseData()
    {
        _health = _data.Health;
    }

    private void attack(string target)
    {

    }

    private void specialInteractions(string action, string target)
    {

    }
}
