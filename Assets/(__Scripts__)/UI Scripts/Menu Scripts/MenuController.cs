using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    [SerializeField] GameObject openMenuButton; 
    [SerializeField] GameObject menuHolder;
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject exitMenuPanel;
    [SerializeField] GameObject SettingPanel;
    [SerializeField] GameObject saveAndLoadPanel;
    [SerializeField] GameObject blockerMenuPanel;

    // Unity Components: ///////////////////////////////////////////////////////
    RectTransform menuPanel_RectTransform;
    Image exitMenuPanel_Image;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    [SerializeField] float menuPanelMoveSpeed = 1500;
    [SerializeField] Color exitMenuPanel_normalColor = new Color(0, 0, 0, 0);
    [SerializeField] Color exitMenuPanel_highLightColor = new Color(0, 0, 0, 0.5f);
    private bool menuPanelIsInTransition = false;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Awake()
    {
        menuPanel_RectTransform = menuPanel.transform as RectTransform;
        exitMenuPanel_Image = exitMenuPanel.GetComponent<Image>();
        menuHolder.SetActive(false);
        SettingPanel.SetActive(false);
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public void OpenMenu()
    {
        if (menuPanelIsInTransition) return;

        GameManager.Instance.Enable_MoveCameraWithKeyboard(false);
        StartCoroutine(OpenMenuRoutine());
    }
    public void CloseMenu()
    {
        if (menuPanelIsInTransition) return;

        GameManager.Instance.Enable_MoveCameraWithKeyboard(true);
        StartCoroutine(CloseMenuRoutine());
    }
    public void OnPointerDownOnBlockerPanel()
    {
       // Debug.Log("Menu Blocker Panel has clicked.");
    }

    public void OpenSettingButton()
    {
        SettingPanel.SetActive(true);
    }
    public void OpenSaveAndLoadButton()
    {
        saveAndLoadPanel.SetActive(true);
    }
    public void DeleteAllButton()
    {
        ItemManager.Singeleton.RemoveAllSpawned();
    }
    public void ExitButton()
    {
        Application.Quit();
    }


    // C# Private Methods: /////////////////////////////////////////////////////
    private IEnumerator OpenMenuRoutine()
    {
        openMenuButton.SetActive(false);
        menuHolder.SetActive(true);
        blockerMenuPanel.SetActive(true);
        menuPanelIsInTransition= true;

        // calculate menuPanel position.X on Open form:
        float openForm_distinationX = menuPanel_RectTransform.anchoredPosition.x + menuPanel_RectTransform.rect.width;

        float currentX = 0;

        while (true)
        {   
            // set menuPanel Position:
            float step = Time.deltaTime * menuPanelMoveSpeed;
            menuPanel_RectTransform.anchoredPosition += new Vector2(step, 0);

            // set exitPanel Color:
            currentX += step;
            float currentT = currentX / menuPanel_RectTransform.rect.width;
            exitMenuPanel_Image.color = Color.Lerp(exitMenuPanel_normalColor, exitMenuPanel_highLightColor, currentT);

            if (menuPanel_RectTransform.anchoredPosition.x >= openForm_distinationX)
            {
                menuPanel_RectTransform.anchoredPosition = new Vector2(openForm_distinationX, 0);
                break;
            }
            yield return null;
        }

        blockerMenuPanel.SetActive(false);
        menuPanelIsInTransition = false;
    }
    private IEnumerator CloseMenuRoutine()
    {
        blockerMenuPanel.SetActive(true);
        menuPanelIsInTransition = true;

        // calculate menuPanel position.X on Close form:
        float closeForm_distinationX = menuPanel_RectTransform.anchoredPosition.x - menuPanel_RectTransform.rect.width;

        float currentX = 0;

        while (true)
        {
            // set menuPanel Position:
            float step = Time.deltaTime * menuPanelMoveSpeed;
            menuPanel_RectTransform.anchoredPosition += new Vector2(-step, 0);

            // set exitPanel Color:
            currentX += step;
            float currentT = currentX / menuPanel_RectTransform.rect.width;
            exitMenuPanel_Image.color = Color.Lerp(exitMenuPanel_highLightColor, exitMenuPanel_normalColor, currentT);

            if (menuPanel_RectTransform.anchoredPosition.x <= closeForm_distinationX)
            {
                menuPanel_RectTransform.anchoredPosition = new Vector2(closeForm_distinationX, 0);
                break;
            }
            yield return null;
        }

        openMenuButton.SetActive(true);
        menuHolder.SetActive(false);
        blockerMenuPanel.SetActive(false);

        menuPanelIsInTransition = false;
    }

} // end of class