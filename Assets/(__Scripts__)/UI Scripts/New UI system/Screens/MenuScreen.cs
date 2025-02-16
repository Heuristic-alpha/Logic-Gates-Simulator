using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : WindowScreenBase
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject backPanel;

    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] TMP_Text appVerText;
    RectTransform menuPanel_RectTransform;
    Image _backPanelImage;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    [SerializeField] float menuPanelMoveSpeed = 1500;
    [SerializeField] Color exitMenuPanel_normalColor = new Color(0, 0, 0, 0);
    [SerializeField] Color exitMenuPanel_highLightColor = new Color(0, 0, 0, 0.5f);

    // Unity Main Events: //////////////////////////////////////////////////////  
    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    public override void OnCreate()
    {
        base.OnCreate();
        menuPanel_RectTransform = menuPanel.transform as RectTransform;
        _backPanelImage = backPanel.GetComponent<Image>();
        _screenSample = HSCL.ScreenSample.MenuScreen;
        appVerText.text = $"Version \' {Application.version} \'";
        OpenMenu();
    }
    public override void OnUpdate()
    {
        if(IsInteractive && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    public void OpenMenu()
    {
        StartCoroutine(OpenMenuRoutine());
    }
    public void CloseMenu()
    {      
        StartCoroutine(CloseMenuRoutine());     
    }    
    public void OpenSettingButton()
    {
        UIManager.Singeleton.CreateScreen(HSCL.ScreenSample.SettingScreen);
    }
    public void OpenSaveAndLoadButton()
    {
        UIManager.Singeleton.CreateScreen(HSCL.ScreenSample.SaveLoadScreen);
    }
    public void DeleteAllButton()
    {
        ItemManager.Singeleton.RemoveAllSpawned();
    }
    public void HelpButton()
    {
        UIManager.Singeleton.CreateScreen(HSCL.ScreenSample.HelpScreen);
    }
    public void ExitButton()
    {
        UIManager.Singeleton.CreateScreen(HSCL.ScreenSample.ExitAppScreen);
    }


    // C# Private Methods: /////////////////////////////////////////////////////
    private IEnumerator OpenMenuRoutine()
    {
        SetWindowInteraction(false);

        // set start position:
        RectTransform canvasRect = _canvas.transform as RectTransform;
        float startPosX = canvasRect.rect.width / 2 + menuPanel_RectTransform.rect.width / 2;
        menuPanel_RectTransform.anchoredPosition = new Vector2(-startPosX, 0);

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
            _backPanelImage.color = Color.Lerp(exitMenuPanel_normalColor, exitMenuPanel_highLightColor, currentT);

            if (menuPanel_RectTransform.anchoredPosition.x >= openForm_distinationX)
            {
                menuPanel_RectTransform.anchoredPosition = new Vector2(openForm_distinationX, 0);
                break;
            }
            yield return null;
        }

        SetWindowInteraction(true);
    }
    private IEnumerator CloseMenuRoutine()
    {
        SetWindowInteraction(false);

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
            _backPanelImage.color = Color.Lerp(exitMenuPanel_highLightColor, exitMenuPanel_normalColor, currentT);

            if (menuPanel_RectTransform.anchoredPosition.x <= closeForm_distinationX)
            {
                menuPanel_RectTransform.anchoredPosition = new Vector2(closeForm_distinationX, 0);
                break;
            }
            yield return null;
        }

        SetWindowInteraction(true);
        GameManager.Instance.Enable_MoveCameraWithKeyboard(true);
        UIManager.Singeleton.DestroyTheFrontScreen();
    }

}
