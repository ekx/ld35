using UnityEngine;
using System.Collections;

public class FlashAnimation : MonoBehaviour
{
    public void OnScreenWhite()
    {
        GameController.Instance.OnScreenWhite();
    }
}