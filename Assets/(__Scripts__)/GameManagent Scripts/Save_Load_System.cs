using HSCL;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;
using Unity.Serialization.Json;

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

    private string _applicationPath;
    private string _tempFolderPath;
    private const string _logicGateSimFolder = "logicGateSim";
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
        string saveContent = saveData.ToStringContent();
        WriteToSaveFile(filePath, in saveContent);

    }
    public void CreateSaveFileFromPath(string filePath, string displyName)
    {
        SaveData saveData = new SaveData();
        saveData.Init(displyName, _backGroundGridController.Get_BackColor(), _backGroundGridController.Get_LineColor());
        string saveContent = saveData.ToStringContent();
        WriteToSaveFile(filePath, in saveContent);
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
        if (slightParser.TryGetContentFromTag(SaveData.name_TAG, out saveName)
        && slightParser.TryGetContentFromTag(SaveData.dateTime_TAG, out datetime))
        {
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
    private void WriteToSaveFile(string path, in string content)
    {
        File.WriteAllText(path, content);
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
            string saveContent = saveData.ToStringContent();
            WriteToSaveFile(CreateTempSaveFileIfNotExistAndReturnPath(), in saveContent);
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

    public const string SPACE = " ";
    public const string NEXT_LINE = "\n";

    private XmlParser parser; // slightParser

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

    // disable prameters constructor for Serialization
    /*
    /// <summary>
    /// Load all data from file'directory path
    /// </summary>
    /// <param name="path">saveFile directory path</param>
    public SaveData(string path)
    {
        parser = new XmlParser(File.ReadAllText(path));
        XmlTagInfo saveTag = parser.rootNode;

        XmlTagInfo infoTag = saveTag.GetChildNode(info_TAG);
        XmlTagInfo verTag = infoTag.GetChildNode(ver_TAG);
        XmlTagInfo dateTimeTag = infoTag.GetChildNode(dateTime_TAG);
        XmlTagInfo nameTag = infoTag.GetChildNode(name_TAG);

        XmlTagInfo settingTag = saveTag.GetChildNode(setting_TAG);
        XmlTagInfo cameraTag = settingTag.GetChildNode(camera_TAG);
        XmlTagInfo backGroundColorTag = settingTag.GetChildNode(backGroundColor_TAG);

        XmlTagInfo itemsTag = saveTag.GetChildNode(items_TAG);
        XmlTagInfo[] itemTags = itemsTag.GetAllChildren();

        XmlTagInfo wiresTag = saveTag.GetChildNode(wires_TAG);
        XmlTagInfo[] wireTags = wiresTag.GetAllChildren();

        version = verTag.GetContent();
        dateTime = DateTime.Parse(dateTimeTag.GetContent(), CultureInfo.InvariantCulture);
        saveDisplayName = nameTag.GetContent();

        cameraPostion = Vector3Extension.Parse(cameraTag.GetContent());
        Parse_backGroundColorContentTo_Colors(backGroundColorTag, out backColor, out lineColor);

        itemInfos = Parse_itemsContentTo_ItemInfo(itemTags);
        wireInfos = Parse_wireContentTo_WireInfo(wireTags);
    }
    public SaveData(string saveName, Color backColor, Color lineColor)
    {
        this.backColor = backColor;
        this.lineColor = lineColor;
        saveDisplayName = saveName;
    }
    */

    /// <summary>
    /// Load all data from file'directory path
    /// </summary>
    /// <param name="path">saveFile directory path</param>
    public void Init(string path)
    {
        parser = new XmlParser(File.ReadAllText(path));
        XmlTagInfo saveTag = parser.RootNode;

        XmlTagInfo infoTag = saveTag.GetChildNode(info_TAG);
        XmlTagInfo verTag = infoTag.GetChildNode(ver_TAG);
        XmlTagInfo dateTimeTag = infoTag.GetChildNode(dateTime_TAG);
        XmlTagInfo nameTag = infoTag.GetChildNode(name_TAG);

        XmlTagInfo settingTag = saveTag.GetChildNode(setting_TAG);
        XmlTagInfo cameraTag = settingTag.GetChildNode(camera_TAG);
        XmlTagInfo backGroundColorTag = settingTag.GetChildNode(backGroundColor_TAG);

        XmlTagInfo itemsTag = saveTag.GetChildNode(items_TAG);
        XmlTagInfo[] itemTags = itemsTag.GetAllChildren();

        XmlTagInfo wiresTag = saveTag.GetChildNode(wires_TAG);
        XmlTagInfo[] wireTags = wiresTag.GetAllChildren();

        version = verTag.GetContent();
        dateTime = DateTime.Parse(dateTimeTag.GetContent(), CultureInfo.InvariantCulture);
        saveDisplayName = nameTag.GetContent();

        cameraPostion = Vector3Extension.Parse(cameraTag.GetContent());
        Parse_backGroundColorContentTo_Colors(backGroundColorTag, out backColor, out lineColor);

        itemInfos = Parse_itemsContentTo_ItemInfo(itemTags);
        wireInfos = Parse_wireContentTo_WireInfo(wireTags);
    }
    public void Init(string saveName, Color backColor, Color lineColor)
    {
        this.backColor = backColor;
        this.lineColor = lineColor;
        saveDisplayName = saveName;
    }

    public SaveData() { }

    public string ToStringContent()
    {
        return Write_GameCurrentStateTo_StringContent();
    }

    // parsing methods:
    private void Parse_backGroundColorContentTo_Colors(XmlTagInfo backGroundColorTag, out Color backColor, out Color lineColor)
    {
        string backGroundColorContent = backGroundColorTag.GetContent().Trim();
        string[] colorSegments = backGroundColorContent.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        float red1 = float.Parse(colorSegments[0]);
        float green1 = float.Parse(colorSegments[1]);
        float blue1 = float.Parse(colorSegments[2]);
        float red2 = float.Parse(colorSegments[3]);
        float green2 = float.Parse(colorSegments[4]);
        float blue2 = float.Parse(colorSegments[5]);
        backColor = new Color(red1, green1, blue1);
        lineColor = new Color(red2, green2, blue2);
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

    // writing methods:
    private string Write_SaveInfoTo_string(SaveInfo saveInfo)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(StartTag(info_TAG) + NEXT_LINE);
        sb.Append(StartTag(ver_TAG) + SPACE + saveInfo.version.ToString() + SPACE + EndTag(ver_TAG) + NEXT_LINE);
        sb.Append(StartTag(dateTime_TAG) + SPACE + saveInfo.dateTime.ToString(CultureInfo.InvariantCulture) + SPACE + EndTag(dateTime_TAG) + NEXT_LINE);
        sb.Append(StartTag(name_TAG) + saveInfo.name + EndTag(name_TAG) + NEXT_LINE);
        sb.Append(EndTag(info_TAG));

        return sb.ToString();
    }
    private string Write_CameraInfoTo_string()
    {
        string content = Vector3Extension.ToString(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.orthographicSize));
        return $"{StartTag(camera_TAG)} {content} {EndTag(camera_TAG)}";
    }
    private string Write_BackGroundColorTo_string()
    {
        string content = $"{backColor.r} {backColor.g} {backColor.b} {lineColor.r} {lineColor.g} {lineColor.b}";
        return $"{StartTag(backGroundColor_TAG)} {content} {EndTag(backGroundColor_TAG)}";
    }
    private string Write_SettingTo_string()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(StartTag(setting_TAG) + NEXT_LINE);
        sb.Append(Write_CameraInfoTo_string() + NEXT_LINE);
        sb.Append(Write_BackGroundColorTo_string() + NEXT_LINE);
        sb.Append(EndTag(setting_TAG));

        return sb.ToString();
    }
    private string Write_ItemInfoTo_string(ItemInfo itemInfo)
    {
        return StartTag(item_TAG) + SPACE + itemInfo.ToString() + SPACE + EndTag(item_TAG);
    }
    private string Write_WireInfoTo_string(WireInfo wireInfo)
    {
        return StartTag(wire_TAG) + SPACE + wireInfo.ToString() + SPACE + EndTag(wire_TAG);
    }
    private string Write_ItemsInfoTo_string(ItemInfo[] itemInfos)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(StartTag(items_TAG) + NEXT_LINE);
        for (int i = 0; i < itemInfos.Length; i++)
        {
            sb.Append(Write_ItemInfoTo_string(itemInfos[i]) + NEXT_LINE);
        }
        sb.Append(EndTag(items_TAG));

        return sb.ToString();
    }
    private string Write_WiresInfoTo_string(WireInfo[] wireInfos)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(StartTag(wires_TAG) + NEXT_LINE);
        for (int i = 0; i < wireInfos.Length; i++)
        {
            sb.Append(Write_WireInfoTo_string(wireInfos[i]) + NEXT_LINE);
        }
        sb.Append(EndTag(wires_TAG));

        return sb.ToString();
    }
    private string Write_GameCurrentStateTo_StringContent()
    {
        StringBuilder content = new StringBuilder();
        itemInfos = ItemManager.Singeleton.GetAllSpawnedItemsAsItemInfoes();
        wireInfos = ItemManager.Singeleton.GetAllSpawnedWiresAsWireInfoes();

        content.Append(StartTag(save_TAG) + NEXT_LINE + NEXT_LINE);
        content.Append(Write_SaveInfoTo_string(new SaveInfo(Application.version, saveDisplayName, DateTime.Now)) + NEXT_LINE + NEXT_LINE);
        content.Append(Write_SettingTo_string() + NEXT_LINE + NEXT_LINE);
        content.Append(Write_ItemsInfoTo_string(itemInfos) + NEXT_LINE + NEXT_LINE);
        content.Append(Write_WiresInfoTo_string(wireInfos) + NEXT_LINE + NEXT_LINE);
        content.Append(EndTag(save_TAG));

        return content.ToString();
    }

    private string StartTag(string tagName) => $"<{tagName}>";
    private string EndTag(string tagName) => $"</{tagName}>";


}