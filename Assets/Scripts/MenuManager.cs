using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settings;

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        _mainMenu.SetActive(false);
        _settings.SetActive(true);
    }

    public void ExitSettings()
    {
        _mainMenu.SetActive(true);
        _settings.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
