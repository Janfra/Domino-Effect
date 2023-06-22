using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlignMainMenu : MonoBehaviour
{
    [SerializeField] private RawImage background;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject[] buttons; 
    // Start is called before the first frame update
    void Start()
    {
        Vector3 canvasScale = background.GetComponent<RectTransform>().lossyScale;
        background.GetComponent<RectTransform>().localScale = new Vector3(Screen.width / 100.0f / canvasScale.x, Screen.height / 100.0f / canvasScale.x, 1.0f);
    }
}
