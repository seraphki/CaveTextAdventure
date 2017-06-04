using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoSingleton<UIController>
{
    [Header("Text Input/Output")]
    public Text TextOutput;
    public InputField TextInput;
    public ScrollRect TextOutputScrollRect;

    [Header("Groups")]
    public CanvasGroup ExploreMenu;
    public CanvasGroup BattleMenu;

    [Header("Explore Menu")]
    public Button InteractionButton;
    public Button NextLevelButton;
    public Button PreviousLevelButton;

    [Header("Battle Menu")]
    public CanvasGroup ActionMenu;
    public CanvasGroup TargetMenu;
    public Text PrimaryTargetButton;
    public InputField ActionInput;
    public InputField TargetInput;

    private const float _typingSpeed = 0.015f;

    private float _textCountdown = 0;
    private string _textQueue = "";

    private string _pendingAction;

    void Awake()
    {
        TextOutputScrollRect.normalizedPosition = new Vector2(0, 1);
    }

    void Update()
    {
        if (TextOutput.text.Length != _textQueue.Length)
        {
            if (_textCountdown > 0)
            {
                _textCountdown -= Time.deltaTime;
            }
            else
            {
                TextOutput.text += _textQueue.Substring(TextOutput.text.Length, 1);
                _textCountdown = _typingSpeed;
            }
        }
    }

    #region Text Output 

    public void TextOutputUpdate(string text)
    {
        _textQueue += "\n" + text;
    }

    public void TextOutputNewLine()
    {
        _textQueue += "\n";
    }

    #endregion

    #region BattleMenu

    public void SubmitAction()
    {
        _pendingAction = ActionInput.text;
        ActionInput.text = "";

        DisableGroup(ActionMenu);
        EnableGroup(TargetMenu);
    }

    public void SubmitTarget()
    {
        BattleManager.Instance.DoAction(_pendingAction, TargetInput.text);
        _pendingAction = "";
        TargetInput.text = "";

        DisableGroup(TargetMenu);
        EnableGroup(ActionMenu);
    }

    public void UpdateActionInput(Text textField)
    {
        ActionInput.text = textField.text;
    }

    public void UpdateTargetInput(Text textField)
    {
        TargetInput.text = textField.text;
    }

    #endregion

    #region Explore Menu

    public void SetChestButton(UnityAction onClick)
    {
        InteractionButton.GetComponentInChildren<Text>().text = "Open\nChest";
        SetInteractionButton(onClick);
    }

    public void SetEnemyButton(UnityAction onClick)
    {
        InteractionButton.GetComponentInChildren<Text>().text = "Engage\nEnemy";
        SetInteractionButton(onClick);
    }

    private void SetInteractionButton(UnityAction onClick)
    {
        DisableInteractionButtons();

        InteractionButton.interactable = true;
        InteractionButton.onClick.RemoveAllListeners();
        InteractionButton.onClick.AddListener(onClick);
    }

    public void EnableNextLevelButton()
    {
        Debug.Log("Enable Next Level Button");
        DisableInteractionButtons();
        NextLevelButton.interactable = true;
    }

    public void EnablePreviousLevelButton()
    {
        Debug.Log("Enable Previous Level Button");
        DisableInteractionButtons();
        PreviousLevelButton.interactable = true;
    }

    public void DisableInteractionButtons()
    {
        InteractionButton.interactable = false;
        NextLevelButton.interactable = false;
        PreviousLevelButton.interactable = false;
    }

    #endregion

    #region MenuSwitchers

    public void SwitchToBattleMenu(string enemyName)
    {
        DisableGroup(ExploreMenu);
        EnableGroup(BattleMenu);
        PrimaryTargetButton.text = enemyName;
        EnableGroup(ActionMenu);
        InteractionButton.interactable = false;
    }

    public void SwitchToExploreMenu()
    {
        DisableGroup(BattleMenu);
        EnableGroup(ExploreMenu);
    }

    #endregion

    #region Private Methods

    private void EnableGroup(CanvasGroup group)
    {
        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    private void DisableGroup(CanvasGroup group)
    {
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    #endregion
}
