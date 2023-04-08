using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField]
    private BaseCounter baseCounter;

    [SerializeField]
    private GameObject[] visualGameObjectArray;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += PlayerInstance_OnSelectedCounterChanged;
    }

    private void PlayerInstance_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach(GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }

    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
