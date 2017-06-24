using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum InformationSet { WorldInformation, LevelInformation, RoomInformation, InteractableInformation, ObstacleInformation, ItemInformation }
public class CartridgeCreatorController : MonoBehaviour
{
    [Header("UI Hookup")]
    public Transform InfoContent;
    public Dropdown InformationSetType;
    public Text MessageUI;

    [Header("Prefabs")]
    public GameObject ArrayEntryPrefab;
    public GameObject ClassEntryPrefab;
    public GameObject StringEntryPrefab;
    public GameObject IntEntryPrefab;
    public GameObject BoolEntryPrefab;

    private List<Type> _supportedTypes;
    private InformationSet _currentInformationSet = InformationSet.WorldInformation;
    private object _rootObject;
    private bool _dirty = false;


    #region Unity Methods

    private void Awake()
    {
        //Populate Dropdown
        List<InformationSet> info = Enum.GetValues(typeof(InformationSet)).Cast<InformationSet>().ToList();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < info.Count; i++)
        {
            options.Add(new Dropdown.OptionData(info[i].ToString()));
        }
        InformationSetType.AddOptions(options);

        //Fill out supported data types
        _supportedTypes = new List<Type>() { typeof(string), typeof(int), typeof(bool) };
    }

    private void Start()
    {
        //Populate Form
        PopulateFromJson();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Dropdown has changed - Update Information.
    /// </summary>
    public void InformationSetChanged()
    {
        MessageUI.text = "";
        string value = InformationSetType.options[InformationSetType.value].text;
        _currentInformationSet = (InformationSet)Enum.Parse(typeof(InformationSet), value);
        PopulateFromJson();
    }

    /// <summary>
    /// Save Data to JSON
    /// </summary>
    public void SaveData()
    {
        string json = JsonUtility.ToJson(_rootObject);
        bool success = Helpers.SaveGameFile(json, _currentInformationSet);

        if (success)
            MessageUI.text = "Save Successful";
        else
            MessageUI.text = "Save Unsuccessful!";
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Entry point for populating screen with information from JSON
    /// Based on Current Information Set as dictated by dropdown.
    /// </summary>
    private void PopulateFromJson()
    {
        for (int i = 0; i < InfoContent.childCount; i++)
            Destroy(InfoContent.GetChild(i).gameObject);

        if (_currentInformationSet == InformationSet.WorldInformation)
            _rootObject = WorldDatabase.GetWorldData() ?? Activator.CreateInstance(typeof(WorldData));
        else if (_currentInformationSet == InformationSet.LevelInformation)
            _rootObject = LevelDatabase.GetLevelArrayData() ?? Activator.CreateInstance(typeof(LevelArray));
        else if (_currentInformationSet == InformationSet.RoomInformation)
            _rootObject = RoomDatabase.GetRoomArayData() ?? Activator.CreateInstance(typeof(RoomArray));
        else if (_currentInformationSet == InformationSet.InteractableInformation)
            _rootObject = InteractableDatabase.GetInteractableArray() ?? Activator.CreateInstance(typeof(InteractableArray));
        else if (_currentInformationSet == InformationSet.ObstacleInformation)
            _rootObject = ObstacleDatabase.GetObstacleArray() ?? Activator.CreateInstance(typeof(ObstacleArray));
        else if (_currentInformationSet == InformationSet.ItemInformation)
            _rootObject = ItemDatabase.GetItemArray() ?? Activator.CreateInstance(typeof(ItemArray));

        PopulateViewGroup(_rootObject, InfoContent);
        MessageUI.text = _currentInformationSet.ToString() + " loaded.";
    }

    /// <summary>
    /// Populate view with information from given object.
    /// </summary>
    private void PopulateViewGroup(object obj, Transform container)
    {
        FieldInfo[] fields = obj.GetType().GetFields();
        for (int i = 0; i < fields.Length; i++)
        {
            PopulateViewSingle(obj, fields[i], container);
        }
    }

    /// <summary>
    /// Populate view with information from a single object field
    /// </summary>
    private void PopulateViewSingle(object obj, FieldInfo field, Transform container)
    {
        Type fieldType = field.FieldType;
        if (fieldType.IsArray)
        {
            CreateAndBindArrayField(obj, field, container);
        }
        else
        {
            CreateAndBindField(obj, field, container);
        }
    }

    /// <summary>
    /// Create a UI field and bind its data to the Input
    /// For use with non-array objects
    /// </summary>
    private void CreateAndBindField(object obj, FieldInfo field, Transform container)
    {
        GameObject lineItem = CreateField(field, container);
        Type type = field.FieldType;
        object value = field.GetValue(obj);

        if (_supportedTypes.Contains(field.FieldType))
        {
            if (field.FieldType == typeof(string))
            {
                InputField input = lineItem.GetComponentInChildren<InputField>();
                input.onValueChanged.AddListener(new UnityAction<string>((val) => { UpdateDataObject(obj, field, val); }));
                input.text = (string)value;
            }
            else if (field.FieldType == typeof(int))
            {
                InputField input = lineItem.GetComponentInChildren<InputField>();
                input.onValueChanged.AddListener(new UnityAction<string>((val) => { UpdateDataObject(obj, field, val); }));
                input.text = value.ToString();
            }
            else if (field.FieldType == typeof(bool))
            {
                Toggle input = lineItem.GetComponentInChildren<Toggle>();
                input.onValueChanged.AddListener(new UnityAction<bool>((val) => { UpdateDataObject(obj, field, val); }));
                input.isOn = (bool)value;
            }
        }
        else if (field.FieldType.GetInterface("IEditable") != null)
        {
            object classObject = field.GetValue(obj);
            InformationGridContainer_Class entry = lineItem.GetComponent<InformationGridContainer_Class>();
            PopulateViewGroup(classObject, entry.Container);
        }
    }

    /// <summary>
    /// Create a UI field and bind its data to the Input
    /// For use with array objects
    /// </summary>
    private void CreateAndBindArrayField(object obj, FieldInfo field, Transform container)
    {
        GameObject lineItem = Instantiate(ArrayEntryPrefab, container, false);
        InformationGridContainer_Array entry = lineItem.GetComponent<InformationGridContainer_Array>();
        entry.Label.text = field.Name;

        object arrayObj = field.GetValue(obj);
        if (arrayObj == null)
        {
            arrayObj = Array.CreateInstance(field.FieldType.GetElementType(), 0);
            field.SetValue(obj, arrayObj);
        }

        entry.Expander.onClick.AddListener(() => { ExpandArray(obj, field, entry.Container); });

        if (field.FieldType.GetElementType().IsArray)
        {
            //TODO: Nested Arrays
        }
        else
        {
            //Create UI for the elements
            CreateAndBindArrayElements(obj, field, entry.Container);
        }
    }

    /// <summary>
    /// Create a UI field and bind its data to the Input 
    /// For use with array elements
    /// </summary>
    private void CreateAndBindArrayElements(object obj, FieldInfo field, Transform container)
    {
        IList fieldEntries = (IList)field.GetValue(obj);
        Type elementType = field.FieldType.GetElementType();

        if (fieldEntries != null)
        {
            for (int i = 0; i < fieldEntries.Count; i++)
            {
                int index = i;

                //Create or get gameobject
                GameObject elementLineItem = null;
                if (container.childCount > i)
                    elementLineItem = container.GetChild(i).gameObject;
                else
                    elementLineItem = CreateField(elementType, container, "Element " + index);

                //Get entry component
                InformationGridEntry entry = elementLineItem.GetComponent<InformationGridEntry>();
                entry.DeleteButton.onClick.RemoveAllListeners();
                entry.DeleteButton.onClick.AddListener(() => { RemoveArrayElement(obj, field, container, index); });
                entry.DeleteButton.gameObject.SetActive(true);

                //Populate and bind field based on type
                if (_supportedTypes.Contains(elementType))
                {
                    if (elementType == typeof(string))
                    {
                        InputField input = elementLineItem.GetComponentInChildren<InputField>();
                        input.onValueChanged.RemoveAllListeners();
                        input.onValueChanged.AddListener(new UnityAction<string>((val) => { UpdateArrayDataObject(obj, field, index, val); }));
                        input.text = (string)fieldEntries[index];
                    }
                    else if (elementType == typeof(int))
                    {
                        InputField input = elementLineItem.GetComponentInChildren<InputField>();
                        input.onValueChanged.RemoveAllListeners();
                        input.onValueChanged.AddListener(new UnityAction<string>((val) => { UpdateArrayDataObject(obj, field, index, val); }));
                        input.text = fieldEntries[index].ToString();
                    }
                    else if (elementType == typeof(bool))
                    {
                        Toggle input = elementLineItem.GetComponentInChildren<Toggle>();
                        input.onValueChanged.RemoveAllListeners();
                        input.onValueChanged.AddListener(new UnityAction<bool>((val) => { UpdateArrayDataObject(obj, field, index, val); }));
                        input.isOn = (bool)fieldEntries[index];
                    }
                }
                //Custom Classes
                else if (elementType.GetInterface("IEditable") != null)
                {
                    //If object in array is null, create a new one
                    if (fieldEntries[index] == null)
                        fieldEntries[index] = Activator.CreateInstance(elementType);

                    object classObject = fieldEntries[index];
                    InformationGridContainer_Class classEntry = elementLineItem.GetComponent<InformationGridContainer_Class>();

                    //Clear container of previous entries
                    for (int c = 0; c < classEntry.Container.childCount; c++)
                        Destroy(classEntry.Container.GetChild(c).gameObject);

                    PopulateViewGroup(classObject, classEntry.Container);
                }
            }

            //Destroy any extra lines
            for (int i = fieldEntries.Count; i < container.childCount; i++)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// Expand the given array - adds an element to the end
    /// </summary>
    private void ExpandArray(object obj, FieldInfo field, Transform container)
    {
        Array array = (Array)field.GetValue(obj);
        Type elementType = array.GetType().GetElementType();
        Array newArray = Array.CreateInstance(elementType, array.Length + 1);
        Array.Copy(array, newArray, Math.Min(array.Length, newArray.Length));
        field.SetValue(obj, newArray);

        CreateAndBindArrayElements(obj, field, container);
    }

    /// <summary>
    /// Removes an element of the given array at given index
    /// </summary>
    private void RemoveArrayElement(object obj, FieldInfo field, Transform container, int index)
    {
        Array array = (Array)field.GetValue(obj);
        Type elementType = array.GetType().GetElementType();
        Array newArray = Array.CreateInstance(elementType, array.Length - 1);

        if (index > 0)
            Array.Copy(array, 0, newArray, 0, index);
        if (index < array.Length - 1)
            Array.Copy(array, index + 1, newArray, index, array.Length - index - 1);

        field.SetValue(obj, newArray);

        CreateAndBindArrayElements(obj, field, container);
    }

    /// <summary>
    /// Instantiate UI Prefab for given Type. 
    /// Uses field name for label.
    /// </summary>
    private GameObject CreateField(FieldInfo field, Transform container)
    {
        return CreateField(field.FieldType, container, field.Name);
    }

    /// <summary>
    /// Instantiate UI Prefab for given Type.
    /// Overloaded to include desired label.
    /// </summary>
    private GameObject CreateField(Type type, Transform container, string labelText)
    {
        GameObject prefab = GetPrefabFromType(type);
        GameObject lineItem = Instantiate(prefab, container, false);

        Text label = lineItem.GetComponentInChildren<Text>();
        label.text = labelText;

        return lineItem;
    }

    /// <summary>
    /// Update an Array Element
    /// </summary>
    private void UpdateArrayDataObject(object obj, FieldInfo field, int index, object value)
    {
        IList fieldEntries = (IList)field.GetValue(obj);
        fieldEntries[index] = value;
        _dirty = true;
    }

    /// <summary>
    /// Update a Data Object
    /// </summary>
    private void UpdateDataObject(object obj, FieldInfo field, object value)
    {
        //TODO: Check if value actually changed
        field.SetValue(obj, Convert.ChangeType(value, field.FieldType));
        _dirty = true;
    }

    private GameObject GetPrefabFromType(Type type)
    {
        if (type.GetInterface("IEditable") != null)
            return ClassEntryPrefab;
        else if (type == typeof(string))
            return StringEntryPrefab;
        else if (type == typeof(int))
            return IntEntryPrefab;
        else if (type == typeof(bool))
            return BoolEntryPrefab;

        Debug.LogWarning("No prefab was found for entry type: " + type.Name);
        return null;
    }

    #endregion
}
