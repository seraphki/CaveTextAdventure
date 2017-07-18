using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoSingleton<UIController>
{
    [Header("Root")]
    public CanvasGroup MainCanvasGroup;

    [Header("Text Input/Output")]
    public Text TextOutput;
    public ScrollRect TextOutputScrollRect;
    public Text CurrentInput;

    [Header("Input")]
    public InputField ActionInput;
    public InputField TargetInput;
    public InputField SubjectInput;
    public InputField DirectionInput;

    [Header("Groups")]
    public CanvasGroup ActionMenu;
    public CanvasGroup TargetMenu;
    public CanvasGroup SubjectMenu;
    public CanvasGroup DirectionMenu;

    private const float _typingSpeed = 0.015f;

    //Text Output
    private float _textCountdown = 0;
    private string _textQueue = "";

    //Free Input Menu
    private string _pendingAction;

    //Explore Menu
    private UnityAction _pendingInteractionAction;

    //Inventory
    private string _selectedItemId = null;
    private Dictionary<string, GameObject> _itemButtons;
    private Dictionary<string, Text> _itemCountDisplays;

    void Awake()
    {
        TextOutputScrollRect.normalizedPosition = new Vector2(0, 1);
        EventSystem.current.SetSelectedGameObject(ActionInput.gameObject);
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

    public void EnableUI()
    {
        MainCanvasGroup.interactable = true;
    }


    #region Text Output 

    public void TextOutputUpdate(string text)
    {
        _textQueue += "\n" + text + "\n";
    }

    #endregion

    #region Input

    public void SubmitAction(string action)
    {
        //Check for single word commands
        if (Helpers.LooseCompare(action, "look"))
        {
            LookCommand();
        }
        else if (Helpers.LooseCompare(action, "inventory"))
        {
            InventoryCommand();
        }
        else
        {
            //Multi word commands
            _pendingAction = string.IsNullOrEmpty(action) ? ActionInput.text : action;
            DisableGroup(ActionMenu);

            if (Helpers.LooseCompare(_pendingAction, "go"))
            {
                EnableGroup(DirectionMenu);
                EventSystem.current.SetSelectedGameObject(DirectionInput.gameObject);
            }
            else
            {
                EnableGroup(TargetMenu);
                EventSystem.current.SetSelectedGameObject(TargetInput.gameObject);
            }

            CurrentInput.text = _pendingAction;
        }

        ActionInput.text = "";
    }

    public void SubmitTarget(string target)
    {
        target = string.IsNullOrEmpty(target) ? TargetInput.text : target;
		string pendingAction = _pendingAction;

		_pendingAction = "";
        TargetInput.text = "";

        UnityAction action = new UnityAction(() => { GameController.WorldManagerInst.SubmitAction(pendingAction, target); });
        GameController.Instance.AddActionToQueue(action);

        EnableGroup(ActionMenu);
        DisableGroup(TargetMenu);
        EventSystem.current.SetSelectedGameObject(ActionInput.gameObject);

        CurrentInput.text = "";
    }

    public void SubmitDirection(string direction)
    {
        direction = direction ?? DirectionInput.text;

        _pendingAction = "";
        DirectionInput.text = "";

        UnityAction action = new UnityAction(() => { GameController.WorldManagerInst.SubmitAction("Go", direction); });
        GameController.Instance.AddActionToQueue(action);

        EnableGroup(ActionMenu);
        DisableGroup(DirectionMenu);
        EventSystem.current.SetSelectedGameObject(ActionInput.gameObject);

        CurrentInput.text = "";
    }

    private void LookCommand()
    {
        UnityAction action = new UnityAction(() => { GameController.WorldManagerInst.Look(); });
        GameController.Instance.AddActionToQueue(action);
    }
    
    private void InventoryCommand()
    {
        UnityAction action = new UnityAction(() => { MessageManager.SendInventoryMessage(); });
        GameController.Instance.AddActionToQueue(action);
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
