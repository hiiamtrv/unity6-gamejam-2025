using UnityEngine;

public class BackToMainMenu : MonoBehaviour
{
    public UIManager uiManager;
    void Start()
    {
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }
    public void Click()
    {
        uiManager.LoadSceneAsync("MainMenuScene");
    }

}
