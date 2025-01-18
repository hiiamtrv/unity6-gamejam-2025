using UnityEngine;

public class ChangeSceneButton : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        UIManager.Instance.LoadSceneAsync(sceneName);
    }
}
