using UnityEngine;

public class GameProgress : MonoBehaviour {
    public void AdvanceLevel() {
        int level = PlayerPrefs.GetInt("Level") + 1;
        if (level > 15) {
            return;
        }

        PlayerPrefs.SetInt("LevelsUnlocked", level);
        SetLevel(level);
    }

    public void PreviousLevel() {
        int level = PlayerPrefs.GetInt("Level") - 1;
        if (level < 1) {
            return;
        }

        PlayerPrefs.SetInt("LevelsUnlocked", level);
        SetLevel(level);
    }

    public void SetLevel(int level) {
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.Save();
        Debug.Log("Level " + GetLevel() + " set");
    }

    public int GetLevel() {
        return PlayerPrefs.GetInt("Level", 1);
    }

    public int GetLevelsUnlocked() {
        return PlayerPrefs.GetInt("LevelsUnlocked", 1);
    }

    public void ResetGame() {
        PlayerPrefs.DeleteAll();
        Debug.Log("Data reset complete");
    }

    public int GetCubeTexture() {
        return PlayerPrefs.GetInt("CubeTexture", 1);
    }

    public void ChangeCubeTexture(int texture) {
        PlayerPrefs.SetInt("CubeTexture", texture);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(string isFullscreen) {
        PlayerPrefs.SetString("Fullscreen", isFullscreen);
        PlayerPrefs.Save();
    }

    public string GetFullscreen() {
        return PlayerPrefs.GetString("Fullscreen", "true");
    }

    public void SetQuality(int qualityIndex) {
        PlayerPrefs.SetInt("QualityIndex", qualityIndex);
        PlayerPrefs.Save();
    }

    public int GetQuality() {
        return PlayerPrefs.GetInt("QualityIndex", 3);
    }

    public void SetVolume(float volume) {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() {
        return PlayerPrefs.GetFloat("Volume", 0);
    }

    public void SetResolution(int width, int height) {
        PlayerPrefs.SetInt("ResolutionWidth", width);
        PlayerPrefs.SetInt("ResolutionHeight", height);
        PlayerPrefs.Save();
    }

    public int GetResolutionWidth() {
        return PlayerPrefs.GetInt("ResolutionWidth", -1);
    }

    public int GetResolutionHeight() {
        return PlayerPrefs.GetInt("ResolutionHeight", -1);
    }
}
