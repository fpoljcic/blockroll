using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    public GameObject levelsContainer;
    public GameProgress gameProgress;

    void Start() {
        Button[] levelButtons = levelsContainer.GetComponentsInChildren<Button>();
        int level = gameProgress.getLevel();

        foreach (Button button in levelButtons) {
            int buttonLevel = int.Parse(button.name);
            if (buttonLevel > level) {
                button.interactable = false;
            }
        }
    }

    public void PlayGame(int level) {
        gameProgress.setLevel(level);
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame() {
        Debug.Log("Exit game");
        Application.Quit();
    }
}
