using UnityEngine;
using UnityEngine.UI;

public class InformationGridEntry : MonoBehaviour
{
    public Text Label;

    [Header("Info Item Stuff")]
    public Button DeleteButton;

    [Header("Array Stuff")]
    public Button AddElementButton;
    public Button ExpandContractButton;
    public Transform Container;

    private bool _expanded = true;
    private Text _expandContractButtonText;

    private void Awake()
    {
        if (ExpandContractButton != null)
        {
            _expandContractButtonText = ExpandContractButton.GetComponentInChildren<Text>();
            ExpandContractButton.onClick.AddListener(ToggleContainer);
        }
    }

    public void ToggleContainer()
    {
        SetContainerToggle(!_expanded);
    }

    public void SetContainerToggle(bool toggle)
    {
        _expanded = toggle;
        _expandContractButtonText.text = _expanded ? "Hide" : "Show";
        Container.gameObject.SetActive(_expanded);
    }
}
