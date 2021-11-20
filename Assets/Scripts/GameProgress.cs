using UnityEngine;

public class GameProgress : MonoBehaviour {

    public void advanceLevel() {
        setLevel(PlayerPrefs.GetInt("Level") + 1);
    }

    public void previousLevel() {
        setLevel(PlayerPrefs.GetInt("Level") - 1);
    }

    public void setLevel(int level) {
        PlayerPrefs.SetInt("Level", level);
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
