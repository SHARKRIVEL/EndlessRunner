using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    PlayerCollision playerCollision;

    void Start()
    {
        playerCollision = FindFirstObjectByType<PlayerCollision>();
    }

    public void OnButtonClick()
    {
       playerCollision.ButtonPressedRef();
    }

    public void OnRestartButton()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene);
    }

    public void OnExitButton()
    {
       Application.Quit();
    }
}
