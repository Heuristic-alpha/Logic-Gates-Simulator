using UnityEngine;

public class CamPosViewer : MonoBehaviour
{
    // Unity GameObjects: //////////////////////////////////////////////////////
    // Unity Components: ///////////////////////////////////////////////////////
    [SerializeField] TMPro.TextMeshProUGUI _posText;

    // C# Properties: //////////////////////////////////////////////////////////
    // C# Fields: //////////////////////////////////////////////////////////////
    [SerializeField] int _updateAfterSomeFrame = 3;

    // Unity Main Events: //////////////////////////////////////////////////////
    private void Update()
    {
        if ((Time.frameCount % _updateAfterSomeFrame) == 0)
        {
            Vector2 pos = Camera.main.transform.position;
            Vector2 finalPos = new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
            _posText.text = $"<b><color=\"red\">X :</color></b> <color=\"yellow\">{finalPos.x}</color>   <b><color=\"red\">Y :</color></b> <color=\"yellow\">{finalPos.y}</color>";
        }
    }

    // Unity Other Events: /////////////////////////////////////////////////////
    // C# Public Methods: //////////////////////////////////////////////////////
    // C# Private Methods: /////////////////////////////////////////////////////

} // end of class