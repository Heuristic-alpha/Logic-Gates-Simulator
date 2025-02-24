using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSceneObjectController : MonoBehaviour
{
    public void OnSceneStart()
    {
        gameObject.SetActive(true);
    }
    public void OnIntroSceneEnd()
    {
        gameObject.SetActive(false);
    }

}
