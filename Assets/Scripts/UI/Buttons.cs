using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public GameObject deathScene;
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO menuScene;
    public Vector3 menuPos;

    public void RestartGame()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        deathScene.SetActive(false);
        AudioManager.Instance.PlayMusic("bgm", true);
        // Time.timeScale = 1f;

        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPos, true);
    }

    public void QuitGame()
    {
        Application.Quit();
        
    }
}
