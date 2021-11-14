using UnityEngine;

public class GameProgress : MonoBehaviour {

    public void advanceLevel() {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        PlayerPrefs.Save();
        Debug.Log("Game data saved!");
    }

    public int getLevel() {
        if (PlayerPrefs.HasKey("Level")) {
            return PlayerPrefs.GetInt("Level");
        } else {
            return 1;
        }
    }

    public void ResetGame() {
        PlayerPrefs.DeleteAll();
        Debug.Log("Data reset complete");
    }
}
