using UnityEngine;

public class GameProgress : MonoBehaviour {


	public void SaveGame(int level) {
		PlayerPrefs.SetInt("Level", level);
		PlayerPrefs.Save();
		Debug.Log("Game data saved!");
	}

	public int LoadGame() {
		if (PlayerPrefs.HasKey("Level")) {
			return PlayerPrefs.GetInt("Level");
		} else
			return 1;
	}

	public void ResetGame() {
		PlayerPrefs.DeleteAll();
		Debug.Log("Data reset complete");
	}
}



