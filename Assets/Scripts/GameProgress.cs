using UnityEngine;

public class GameProgress : MonoBehaviour {

    public void advanceLevel() {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        PlayerPrefs.Save();
        Debug.Log("Level " + getLevel() + " set");
    }

    public void previousLevel() {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") - 1);
        PlayerPrefs.Save();
        Debug.Log("Level " + getLevel() + " set");
    }

    public int getLevel() {
        if (PlayerPrefs.HasKey("Level")) {
            return PlayerPrefs.GetInt("Level");
        } else {
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.Save();
            return 1;
        }
    }

    public void resetGame() {
        PlayerPrefs.DeleteAll();
        Debug.Log("Data reset complete");
    }
}
