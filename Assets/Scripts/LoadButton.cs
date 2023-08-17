using UnityEngine.UI;
using UnityEngine;

public class LoadButton : MonoBehaviour
{
    public Button loadButton;
    
    void Start()
    {
        loadButton.onClick.AddListener(OnLoadButtonPressed);
    }

    void OnLoadButtonPressed()
    {
        // TODO: Open a file selector prompt
    }
}
