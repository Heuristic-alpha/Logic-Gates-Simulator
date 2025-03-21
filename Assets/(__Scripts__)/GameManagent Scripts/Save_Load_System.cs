using HSCL.XML;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;
using Unity.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;

public class Save_Load_System : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    private static Save_Load_System _singeleton;
    [SerializeField] CameraController _cameraController;
    [SerializeField] BackGroundGridController _backGroundGridController;

    // C# Properties: //////////////////////////////////////////////////////////
    public bool TempSave { get { return _tempSave; } set { _tempSave = value; } }
    public static Save_Load_System Singeleton { get { return _singeleton; } }

    // C# Fields: //////////////////////////////////////////////////////////////
    [SerializeField] bool _tempSave = true;
    public bool GeneratePrettySave = true;

    private string _applicationPath;
    private string _tempFolderPath;
    private const string _logicGateSimFolder = "logicGatesSim";
    private const string _saveFolder = "saves";
    private const string _tempFolder = "temp";
    private const string _FILE_TYPE = ".lgs";

    // PlayerPrefs Key:
    private const string AUTO_SAVE = "AutoSave";

    public const string SPACE = " ";
    public const string NEXT_LINE = "\n";

    // Unity Main Events: //////////////////////////////////////////////////////

    private void Awake()
    {
        InitSingeleton();
        InitPath();
    }
    private void Start()
    {
        LoadAutoSaveKey();
        LoadTempSaveFile();
    }

    // Unity Other Events: /////////////////////////////////////////////////////

    private void OnApplicationQuit()
    {
        SaveAutoSaveKey();
        SaveTempSaveFile();
    }

    // C# Public Methods: //////////////////////////////////////////////////////
    public string GetSavePathFolder()
    {
        return _applicationPath;
    }
    public string[] GetPathToExistSaveFiles()
    {
        return Directory.GetFiles(_applicationPath, "*" + _FILE_TYPE);
    }
    public void CreateSaveFileFromName(string directoryName, string displyName)
    {
        string filePath = CreateSaveFileIfNotExistAndReturnPath(directoryName);
        SaveData saveData = new SaveData();
        saveData.Init(displyName, _backGroundGridController.Get_BackColor(), _backGroundGridController.Get_LineColor());
        StringBuilder saveContent = saveData.ToStringContent();
        WriteToSaveFile(filePath, saveContent);

    }
    public void CreateSaveFileFromPath(string filePath, string displyName)
    {
        SaveData saveData = new SaveData();
        saveData.Init(displyName, _backGroundGridController.Get_BackColor(), _backGroundGridController.Get_LineColor());
        StringBuilder saveContent = saveData.ToStringContent();
        WriteToSaveFile(filePath, saveContent);
    }
    public void LoadSaveFileFromPath(string path)
    {
        SaveData saveData = new SaveData();
        saveData.Init(path);
        LoadSaveData(saveData);
    }
    public void DeleteSaveFileFromPath(string path)
    {
        if (File.Exists(path)) File.Delete(path);
    }
    public bool GetSaveInfoFromSaveFilePath(string path, out string saveName, out string datetime)
    {
        XmlParser slightParser = new XmlParser(File.ReadAllText(path), SaveData.info_TAG);
        XmlTagInfo infoNode;
        if(slightParser.TryGetXMLTagInfoFromTag(SaveData.info_TAG,out infoNode))
        {
            saveName = infoNode.GetValueOfAttribute(SaveData.name_TAG);
            datetime = infoNode.GetValueOfAttribute(SaveData.dateTime_TAG);
            return true;
        }
        saveName = "Null";
        datetime = "Null";
        return false;
    }

    // C# Private Methods: /////////////////////////////////////////////////////
    private void InitSingeleton()
    {
        if (_singeleton == null) _singeleton = this;
        else
        {
            Destroy(this);
        }
    }
    private void InitPath()
    {
#if UNITY_STANDALONE_WIN
        _applicationPath = Path.GetFullPath(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments));
        _applicationPath = Path.Combine(_applicationPath, _logicGateSimFolder);
#else
        _applicationPath = Application.persistentDataPath;
#endif
        _applicationPath = Path.Combine(_applicationPath, _saveFolder);
        _tempFolderPath = Path.Combine(_applicationPath, _tempFolder);
        CreateDirectoryIfNotExist();
    }
    private string GetPathFromSaveFile(string name)
    {
        string path = _applicationPath + Path.DirectorySeparatorChar + name + _FILE_TYPE;
        if (File.Exists(path)) { return path; }
        else
        {
            throw new IOException($"<color=red>File with Name: <color=yellow>{name}</color> does not exist.</color>");
        }
    }
    private void CreateDirectoryIfNotExist()
    {
        if (!Directory.Exists(_applicationPath)) Directory.CreateDirectory(_applicationPath);
        if (!Directory.Exists(_tempFolderPath)) Directory.CreateDirectory(_tempFolderPath);
    }
    private void CreateSaveFileIfNotExist(string name)
    {
        string path = _applicationPath + Path.DirectorySeparatorChar + name + _FILE_TYPE;
        FileStream fs;
        if (!File.Exists(path)) fs = File.Create(path);
        else
        {
            File.Delete(path);
            fs = File.Create(path);
        }
        fs.Close();
    }
    private string CreateSaveFileIfNotExistAndReturnPath(string name)
    {
        string path = _applicationPath + Path.DirectorySeparatorChar + name + _FILE_TYPE;
        FileStream fs;
        if (!File.Exists(path)) fs = File.Create(path);
        else
        {
            File.Delete(path);
            fs = File.Create(path);
        }
        fs.Close();
        return path;
    }
    private string CreateTempSaveFileIfNotExistAndReturnPath()
    {
        string path = _tempFolderPath + Path.DirectorySeparatorChar + "temp" + _FILE_TYPE;
        FileStream fs;
        if (!File.Exists(path)) fs = File.Create(path);
        else fs = File.OpenRead(path);
        fs.Close();
        return path;
    }

    private string ReadFromSaveFile(string path)
    {
        return File.ReadAllText(path);
    }
    private void WriteToSaveFile(string path, StringBuilder content)
    {
        File.WriteAllText(path, content.ToString());
    }

    private void LoadTempSaveFile()
    {
        if (TempSave)
        {
#if UNITY_STANDALONE_WIN || UNITY_ANDROID
            string path = CreateTempSaveFileIfNotExistAndReturnPath();
            LoadSaveFileFromPath(path);
#endif
        }
    }
    private void SaveTempSaveFile()
    {
        if (TempSave)
        {
#if UNITY_STANDALONE_WIN || UNITY_ANDROID
            SaveData saveData = new SaveData();
            saveData.Init("temp", _backGroundGridController.Get_BackColor(), _backGroundGridController.Get_LineColor());
            StringBuilder saveContent = saveData.ToStringContent();
            WriteToSaveFile(CreateTempSaveFileIfNotExistAndReturnPath(), saveContent);
#endif
        }
    }
    private void LoadSaveData(SaveData saveData)
    {
        ItemManager.Singeleton.RemoveAllSpawned();
        ItemManager.Singeleton.AddItems(saveData.ItemInfos);
        ItemManager.Singeleton.AddWires(saveData.WireInfos);
        _cameraController.SetCameraSizeExactly(saveData.CameraPostion.z);
        _cameraController.SetCameraPos(saveData.CameraPostion);
        _backGroundGridController.Set_BackColor(saveData.BackColor);
        _backGroundGridController.Set_LineColor(saveData.LineColor);
    }

    private void SaveAutoSaveKey()
    {
        if (PlayerPrefs.HasKey(AUTO_SAVE))
        {
            PlayerPrefs.SetInt(AUTO_SAVE, Convert.ToInt32(_tempSave));
        }
    }
    private void LoadAutoSaveKey()
    {
        if (PlayerPrefs.HasKey(AUTO_SAVE))
        {
            _tempSave = Convert.ToBoolean(PlayerPrefs.GetInt(AUTO_SAVE));
        }
        else
        {
            PlayerPrefs.SetInt(AUTO_SAVE, Convert.ToInt32(_tempSave));
        }
    }

    // Debuging:
    private void ShowStringArray(string[] strings)
    {
        Debug.Log("string start ----------- >");
        for (int i = 0; i < strings.Length; i++)
        {
            Debug.Log(strings[i]);
        }
        Debug.Log("string end ---------- <");
    }

    // Json Serialization test:
    private void SaveTo_Json_File(SaveData saveData, string path, string fileName)
    {    
        string jsonString = JsonSerialization.ToJson(saveData);
        File.WriteAllText(Path.Combine(path, fileName + ".json"), jsonString);
    }

} // end of class

[Serializable]
public readonly struct ItemInfo
{
    public readonly int id;
    public readonly float x;
    public readonly float y;
    public readonly Item_Rotation item_Rotation;

    public ItemInfo(int id, float x, float y, Item_Rotation item_Rotation)
    {
        this.id = id;
        this.x = x;
        this.y = y;
        this.item_Rotation = item_Rotation;
    }

    public override string ToString()
    {
        string rot;
        if (item_Rotation == Item_Rotation.n) rot = "n";
        else if (item_Rotation == Item_Rotation.e) rot = "e";
        else if (item_Rotation == Item_Rotation.s) rot = "s";
        else if (item_Rotation == Item_Rotation.w) rot = "w";
        else rot = "E"; // E means Error

        string content = id.ToString() + Save_Load_System.SPACE + x.ToString() + Save_Load_System.SPACE + y.ToString() + Save_Load_System.SPACE + rot;
        return content;
    }

    public static ItemInfo ToItemInfoFrom(IItemable itemable)
    {
        Item item = itemable.GetItem();
        Transform t = (itemable as MonoBehaviour).transform;
        int id = item.Id;
        Item_Rotation ir = itemable.GetItem_Rotation();

        return new ItemInfo(id, t.position.x, t.position.y, ir);
    }

}

[Serializable]
public readonly struct WireInfo
{
    // for wire startPoint
    public readonly int localItemId_s;
    public readonly Trait pinType_s;
    public readonly byte pinIndex_s;

    // for wire endPoint
    public readonly int localItemId_e;
    public readonly Trait pinType_e;
    public readonly byte pinIndex_e;

    public WireInfo(int localItemId_s, Trait pinType_s, byte pinIndex_s, int localItemId_e, Trait pinType_e, byte pinIndex_e)
    {
        this.localItemId_s = localItemId_s;
        this.pinType_s = pinType_s;
        this.pinIndex_s = pinIndex_s;
        this.localItemId_e = localItemId_e;
        this.pinType_e = pinType_e;
        this.pinIndex_e = pinIndex_e;
    }

    public override string ToString()
    {
        string pinTypeStart;
        string pinTypeEnd;

        if (pinType_s == Trait.Input) pinTypeStart = "i";
        else if (pinType_s == Trait.Output) pinTypeStart = "o";
        else pinTypeStart = "E";  // Error

        if (pinType_e == Trait.Input) pinTypeEnd = "i";
        else if (pinType_e == Trait.Output) pinTypeEnd = "o";
        else pinTypeEnd = "E";    // Error

        string content = localItemId_s.ToString() + Save_Load_System.SPACE + pinTypeStart + Save_Load_System.SPACE + pinIndex_s.ToString() + Save_Load_System.SPACE +
                         localItemId_e.ToString() + Save_Load_System.SPACE + pinTypeEnd + Save_Load_System.SPACE + pinIndex_e.ToString();
        return content;
    }
}

[Serializable]
public readonly struct SaveInfo
{
    public readonly string version;
    public readonly string name;
    public readonly DateTime dateTime;

    public SaveInfo(string version, string name, DateTime dateTime)
    {
        this.version = version;
        this.name = name;
        this.dateTime = dateTime;
    }

    public override string ToString()
    {
        return $"Ver:{version} name:{name} dateTime:{dateTime}";
    }
}

/// <summary>
/// object that contained Game's data that should be saved
/// </summary>
[Serializable]
public class SaveData
{
    // Tags:
    public const string save_TAG = "save";
    public const string info_TAG = "info";
    public const string ver_TAG = "ver";
    public const string dateTime_TAG = "dateTime";
    public const string name_TAG = "name";
    public const string items_TAG = "items";
    public const string wires_TAG = "wires";
    public const string item_TAG = "item";
    public const string wire_TAG = "wire";
    public const string setting_TAG = "setting";
    public const string camera_TAG = "camera";
    public const string backGroundColor_TAG = "backGroundColor";
    public const string backGroundLineColor_TAG = "backGroundLineColor";

    public const string SPACE = " ";
    public const string NEXT_LINE = "\n";

    private XmlParser parser; // slightParser
    private bool _prettySave;

    // save info
    public string version;
    public DateTime dateTime;
    public string saveDisplayName;
    // setting:
    public Vector3 cameraPostion;
    // backGroundColor:
    public Color backColor;
    public Color lineColor;
    // Items:
    public ItemInfo[] itemInfos;
    // Wires:
    public  WireInfo[] wireInfos;

    // All properties:
    public string Version { get { return version; } }
    public DateTime DateTime { get { return dateTime; } }
    public string SaveDisplayName { get { return saveDisplayName; } }
    public Vector3 CameraPostion { get { return cameraPostion; } }
    public Color BackColor { get { return backColor; } }
    public Color LineColor { get { return lineColor; } }
    public ItemInfo[] ItemInfos { get { return itemInfos; } }
    public WireInfo[] WireInfos { get { return wireInfos; } }

    /// <summary>
    /// Load all data from file'directory path
    /// </summary>
    /// <param name="path">saveFile directory path</param>
    public void Init(string path)
    {
        parser = new XmlParser(File.ReadAllText(path));
        XmlTagInfo saveTag = parser.RootNode;

        XmlTagInfo infoTag = saveTag.GetChildNode(info_TAG); // infoTag is SelfClosedTag and has Attributes
        XmlTagInfo settingTag = saveTag.GetChildNode(setting_TAG);
        XmlTagInfo cameraTag = settingTag.GetChildNode(camera_TAG);
        XmlTagInfo backGroundColorTag = settingTag.GetChildNode(backGroundColor_TAG);
        XmlTagInfo backGroundLineColorTag = settingTag.GetChildNode(backGroundLineColor_TAG);

        XmlTagInfo itemsTag = saveTag.GetChildNode(items_TAG);
        XmlTagInfo[] itemTags = itemsTag.GetAllChildren();

        XmlTagInfo wiresTag = saveTag.GetChildNode(wires_TAG);
        XmlTagInfo[] wireTags = wiresTag.GetAllChildren();

        version = infoTag.GetValueOfAttribute(ver_TAG);
        dateTime = DateTime.Parse(infoTag.GetValueOfAttribute(dateTime_TAG), CultureInfo.InvariantCulture);
        saveDisplayName = infoTag.GetValueOfAttribute(name_TAG);

        cameraPostion = Vector3Extension.Parse(cameraTag.GetContent());
        Parse_ColorContentTo_Color(backGroundColorTag, out backColor);
        Parse_ColorContentTo_Color(backGroundLineColorTag, out lineColor);

        itemInfos = Parse_itemsContentTo_ItemInfo(itemTags);
        wireInfos = Parse_wireContentTo_WireInfo(wireTags);

        _prettySave = Save_Load_System.Singeleton.GeneratePrettySave;
    }
    public void Init(string saveName, Color backColor, Color lineColor)
    {
        this.backColor = backColor;
        this.lineColor = lineColor;
        saveDisplayName = saveName;

        _prettySave = Save_Load_System.Singeleton.GeneratePrettySave;
    }

    public SaveData() { }

    public StringBuilder ToStringContent()
    {
        return Write_GameCurrentStateTo_XmlStringContent();
    }

    // parsing methods:
    private void Parse_ColorContentTo_Color(XmlTagInfo backGroundColorTag, out Color color)
    {
        string backGroundColorContent = backGroundColorTag.GetContent().Trim();
        string[] colorSegments = backGroundColorContent.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        float red = float.Parse(colorSegments[0]);
        float green = float.Parse(colorSegments[1]);
        float blue = float.Parse(colorSegments[2]);
        color = new Color(red, green, blue);
    }
    private ItemInfo[] Parse_itemsContentTo_ItemInfo(XmlTagInfo[] items)
    {
        ItemInfo[] itemInfos = new ItemInfo[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            string[] segment = items[i].GetContent().Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int id = int.Parse(segment[0]);
            float x = float.Parse(segment[1]);
            float y = float.Parse(segment[2]);
            Item_Rotation rot;
            if (segment[3] == "n") rot = Item_Rotation.n;
            else if (segment[3] == "e") rot = Item_Rotation.e;
            else if (segment[3] == "s") rot = Item_Rotation.s;
            else if (segment[3] == "w") rot = Item_Rotation.w;
            else rot = Item_Rotation.n;

            itemInfos[i] = new ItemInfo(id, x, y, rot);
        }
        return itemInfos;
    }
    private WireInfo[] Parse_wireContentTo_WireInfo(XmlTagInfo[] wires)
    {
        WireInfo[] wireInfos = new WireInfo[wires.Length];

        for (int i = 0; i < wires.Length; i++)
        {
            string[] segment = wires[i].GetContent().Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            int localItemId_s = int.Parse(segment[0]);
            Trait pinType_s;
            byte pinIndex_s = byte.Parse(segment[2]);
            int localItemId_e = int.Parse(segment[3]);
            Trait pinType_e;
            byte pinIndex_e = byte.Parse(segment[5]);

            if (segment[1] == "i") pinType_s = Trait.Input;
            else if (segment[1] == "o") pinType_s = Trait.Output;
            else throw new Exception("Error");

            if (segment[4] == "i") pinType_e = Trait.Input;
            else if (segment[4] == "o") pinType_e = Trait.Output;
            else throw new Exception("Error");

            wireInfos[i] = new WireInfo(localItemId_s, pinType_s, pinIndex_s, localItemId_e, pinType_e, pinIndex_e);
        }
        return wireInfos;
    }

    // writing method:
    private StringBuilder Write_GameCurrentStateTo_XmlStringContent()
    {
        StringBuilder content = new StringBuilder();
        itemInfos = ItemManager.Singeleton.GetAllSpawnedItemsAsItemInfoes();
        wireInfos = ItemManager.Singeleton.GetAllSpawnedWiresAsWireInfoes();

        XmlElementDescriber saveNode = new (save_TAG, TagType.OpenClosed, null);
        XmlElementDescriber infoNode = new(info_TAG, TagType.SelfClosed, saveNode, null);
        infoNode.AddAttribute(ver_TAG, Application.version);
        infoNode.AddAttribute(dateTime_TAG, DateTime.Now.ToString(CultureInfo.InvariantCulture));
        infoNode.AddAttribute(name_TAG, saveDisplayName);
        XmlElementDescriber settingNode = new(setting_TAG, TagType.OpenClosed, saveNode, null);
        XmlElementDescriber cameraNode = new(camera_TAG, TagType.OpenClosed,settingNode ,Vector3Extension.ToString(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.orthographicSize)));
        XmlElementDescriber backGroundColorNode = new(backGroundColor_TAG, TagType.OpenClosed,settingNode, $"{backColor.r} {backColor.g} {backColor.b}");
        XmlElementDescriber backGroundLineColorNode = new(backGroundLineColor_TAG, TagType.OpenClosed,settingNode, $"{lineColor.r} {lineColor.g} {lineColor.b}");
        XmlElementDescriber itemsNode = new(items_TAG, TagType.OpenClosed, saveNode, null);
        for(int i = 0; i < itemInfos.Length; i++)
        {
            XmlElementDescriber itemNode = new(item_TAG, TagType.OpenClosed, itemInfos[i].ToString());
            itemNode.SetParent(itemsNode);
        }
        XmlElementDescriber wiresNode = new(wires_TAG, TagType.OpenClosed, saveNode, null);
        for (int i = 0; i < wireInfos.Length; i++)
        {
            XmlElementDescriber wireNode = new(wire_TAG, TagType.OpenClosed, wireInfos[i].ToString());
            wireNode.SetParent(wiresNode);
        }

        content = saveNode.ConvertToStringContent(_prettySave ? TagFormat.Indent : TagFormat.Inline, 0);
        return content;
    }

}// end of SaveData Class