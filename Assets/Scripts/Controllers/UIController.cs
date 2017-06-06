using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoSingleton<UIController>
{
    [Header("Text Input/Output")]
    public Text TextOutput;
    public ScrollRect TextOutputScrollRect;

    [Header("Groups")]
    public CanvasGroup ExploreMenu;
    public CanvasGroup BattleMenu;
	public CanvasGroup InventoryMenu;

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

	[Header("Inventory Window")]
	public Transform ItemContainer;
	public GameObject ItemButtonPrefab;
	public Image SelectedImage;
	public Text SelectedText;
	public Button UseButton;

    private const float _typingSpeed = 0.015f;

	//Text Output
    private float _textCountdown = 0;
    private string _textQueue = "";

	//Free Input Menu
    private string _pendingAction;

	//Explore Menu
	private UnityAction _pendingInteractionAction;

	//Inventory
	private int _selectedItemId = 0;
	private Dictionary<int, GameObject> _itemButtons;
	private Dictionary<int, Text> _itemCountDisplays;

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

    public void NewLine()
    {
        _textQueue += "\n";
    }

	#endregion

	#region InventoryWindow

	private void PopulateItemGrid()
	{
		for (int i = 0; i < ItemContainer.childCount; i++)
			Destroy(ItemContainer.GetChild(0).gameObject);

		_itemButtons = new Dictionary<int, GameObject>();
		_itemCountDisplays = new Dictionary<int, Text>();
		int[] itemIds = InventoryManager.Instance.GetOwnedItemIds();
		
		for (int i = 0; i < itemIds.Length; i++)
		{
			int itemId = itemIds[i];
			GameObject itemButton = (GameObject)Instantiate(ItemButtonPrefab, ItemContainer, false);

			Button buttonComp = itemButton.GetComponent<Button>();
			buttonComp.onClick.AddListener(() => SelectItem(itemId));

			Image buttonImage = itemButton.GetComponentInChildren<Image>();
			buttonImage.sprite = ItemLibrary.Instance.GetItemData(itemId).Icon;

			Text count = itemButton.GetComponentInChildren<Text>();
			count.text = InventoryManager.Instance.GetItemAmountOwned(itemId).ToString();
			_itemCountDisplays.Add(itemId, count);
		}

		SelectedImage.enabled = false;
		SelectedText.enabled = false;
	}

	private void UpdateItemCounts()
	{
		int[] itemIds = _itemCountDisplays.Keys.ToArray();
		for (int i = 0; i < itemIds.Length; i++)
		{
			_itemCountDisplays[itemIds[i]].text = InventoryManager.Instance.GetItemAmountOwned(itemIds[i]).ToString();
		}
	}

	private void SelectItem(int itemId)
	{
		_selectedItemId = itemId;
		UpdateSelectedItem();
	}

	private void UpdateSelectedItem()
	{
		SelectedImage.enabled = true;
		SelectedText.enabled = true;

		ItemData data = ItemLibrary.Instance.GetItemData(_selectedItemId);
		SelectedImage.sprite = data.Icon;

		int amountOwned = InventoryManager.Instance.GetItemAmountOwned(_selectedItemId);
		SelectedText.text = data.Name;

		UseButton.interactable = amountOwned > 0;
	}

	public void UseSelectedItem()
	{
		if (_selectedItemId > 0)
		{
			InventoryManager.Instance.UseItem(_selectedItemId);
		}
		UpdateItemCounts();
	}

	public void CloseInventoryMenu()
	{
		_selectedItemId = 0;
		SwitchToExploreMenu();
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
		string pendingAction = _pendingAction;
		string target = TargetInput.text;
		UnityAction action = new UnityAction(() => { BattleManager.Instance.DoAction(pendingAction, target); });
		GameController.Instance.AddActionToQueue(action);

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

	public void MoveButtonPressed(int direction)
	{
		GameController.Instance.AddActionToQueue(new UnityAction(() => { CaveManager.Instance.Move(direction); }));
	}

	public void MoveLevelButtonPressed(int direction)
	{
		GameController.Instance.AddActionToQueue(new UnityAction(() => { CaveManager.Instance.MoveLevel(direction); }));
	}

	public void InteractionButtonPressed()
	{
		GameController.Instance.AddActionToQueue(_pendingInteractionAction);
		_pendingInteractionAction = null;
	}

    public void SetChestButton(UnityAction onClick)
    {
        InteractionButton.GetComponentInChildren<Text>().text = "Open\nChest";
		InteractionButton.interactable = true;
		_pendingInteractionAction = onClick;
    }

    public void SetEnemyButton(UnityAction onClick)
    {
        InteractionButton.GetComponentInChildren<Text>().text = "Engage\nEnemy";
		InteractionButton.interactable = true;
		_pendingInteractionAction = onClick;
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
		DisableGroup(InventoryMenu);
		DisableGroup(ExploreMenu);
        EnableGroup(BattleMenu);
        PrimaryTargetButton.text = enemyName;
        EnableGroup(ActionMenu);
        InteractionButton.interactable = false;
    }

    public void SwitchToExploreMenu()
    {
		DisableGroup(InventoryMenu);
        DisableGroup(BattleMenu);
        EnableGroup(ExploreMenu);
    }

	public void SwitchToInventoryMenu()
	{
		PopulateItemGrid();

		DisableGroup(BattleMenu);
		DisableGroup(ExploreMenu);
		EnableGroup(InventoryMenu);
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
