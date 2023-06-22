
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    private static PauseHandler instance;
    DominoMovement playerDomino;

    [Header("Dependencies")]
    [SerializeField]
    GameObject pauseContainer;


    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one pause menu");
        }
        else
        {
            instance = this;
            if(pauseContainer == null)
            {
                Debug.LogError("Set the pause container for disabling");
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseContainer.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseContainer.SetActive(false);
        Time.timeScale = 1.0f;
        SetPlayerInputTo(true);
    }

    public void GoToMenu()
    {
        if(SceneLoader.Instance == null)
        {
            Debug.LogError("No scene loader in scene");
            return;
        }
        SceneLoader.Instance.LoadScene(0);
    }

    public void QuitGame()
    {
        SceneLoader.OnQuitButton();
    }

    private void OnDisable()
    {
        instance = null;
    }

    private void PauseGame()
    {
        pauseContainer.SetActive(true);
        Time.timeScale = 0.0f;
        SetPlayerInputTo(false);
    }

    private void SetPlayerInputTo(bool isEnable)
    {
        if (playerDomino)
        {
            playerDomino.isInputEnable = isEnable;
        }
    }

    public static void SetPlayersDominoForPausing(DominoMovement domino)
    {
        if(instance == null)
        {
            Debug.LogWarning("No pause menu in scene");
        }
        else
        {
            instance.playerDomino = domino;
        }
    }
}
