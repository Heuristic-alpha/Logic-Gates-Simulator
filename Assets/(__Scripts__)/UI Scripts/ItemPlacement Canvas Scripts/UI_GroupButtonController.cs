using UnityEngine;
using UnityEngine.UI;

public class UI_GroupButtonController : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] ItemSellectorController[] itemSellectorControllers;
    [SerializeField, Tooltip("Order is Important!")] Image[] selectedGroupImages;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    private int _currentSelectedItemGroupIndex = 0;
    private Color _transparentColor = new Color(1, 1, 1, 0);
    [SerializeField] Color selectedColor;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        OnSelectGroup(3);
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public void OnSelectGroup(int newGroupindex)
    {
        if (newGroupindex == 0)
        {
            DeActiveAll();
            _currentSelectedItemGroupIndex = 0;
        }
        else if(newGroupindex == _currentSelectedItemGroupIndex)
        {
            return;
        }
        else
        {
            OnlyActiveOneIndex(newGroupindex);
            _currentSelectedItemGroupIndex = newGroupindex;
        }
    }
    // ButtonEvents:
    public void OnClick_Input_Button()
    {
        OnSelectGroup(1);
    }
    public void OnClick_Output_Button()
    {
        OnSelectGroup(2);
    }
    public void OnClick_SimpleGates_Button()
    {
        OnSelectGroup(3);
    }
    public void OnClick_Chips_Button()
    {
        OnSelectGroup(4);
    }

    // C# Private Methods: /////////////////////////////////////////////////////


    private void DeActiveAll()
    {
        foreach (var item in itemSellectorControllers)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var img in selectedGroupImages)
        {
            img.color = _transparentColor;
        }
    }
    private void OnlyActiveOneIndex(int newIndex)
    {
        foreach (var item in itemSellectorControllers)
        {
            if (item.ItemGroupIndex == newIndex) item.gameObject.SetActive(true);
            else item.gameObject.SetActive(false);
        }
        for (int i = 1; i <= selectedGroupImages.Length; i++)
        {
            if (i == newIndex) selectedGroupImages[i - 1].color = selectedColor;
            else selectedGroupImages[i - 1].color = _transparentColor;
        }
    }

} // end of class