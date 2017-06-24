using UnityEngine;
using UnityEngine.UI;

public class InformationGridEntry : MonoBehaviour
{
    public Text Label;
    public Button DeleteButton;

    public virtual void Setup(string label)
    {
        Label.text = label;
    }

    public virtual void UpdateInput(object obj)
    {

    }

    public virtual void UpdateInput(string data)
    {
        //To be overridden by child
    }

    public virtual void UpdateInput(int data)
    {
        //To be overridden by child
    }

    public virtual void UpdateInput(bool data)
    {
        //To be overridden by child
    }

    public virtual object GetValue()
    {
        return null;
    }
}
