using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    public GameObject levelsContainer;
    public GameObject optionsCube;
    public GameProgress gameProgress;
    public AudioMixer audioMixer;
    public TMP_Dropdown ResolutionDropdown;
    Resolution[] resolutions;
    public Texture[] textures;
    public Renderer cube;

    void Start() {
        Button[] levelButtons = levelsContainer.GetComponentsInChildren<Button>();
        int level = gameProgress.getLevel();

        foreach (Button button in levelButtons) {
            int buttonLevel = int.Parse(button.name);
            if (buttonLevel > level) {
                button.interactable = false;
            }
        }

        SetResolutions();
    }

    void Update() {
        optionsCube.transform.Rotate(Vector3.up * Time.deltaTime * 20);
    }

    private void SetResolutions() {
        int CurrentResolutionIndex = 0;
        resolutions = Screen.resolutions;

        ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++) {
            string Option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(Option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height) {
                CurrentResolutionIndex = i;
            }
        }

        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = CurrentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    public void PlayGame(int level) {
        gameProgress.setLevel(level);
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame() {
        Debug.Log("Exit game");
        Application.Quit();
    }
    public void SetResolution(int ResolutionIndex) {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume) {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    
    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    public void NextCubeTexture() {
        int texture = gameProgress.getCubeTexture();
        if (texture < 3) {
            cube.material.mainTexture = textures[texture + 1];
            gameProgress.changeCubeTexture(texture + 1);
        }
    }

    public void PreviousCubeTexture() {
        int texture = gameProgress.getCubeTexture();
        if (texture > 0) {
            cube.material.mainTexture = textures[texture - 1];
            gameProgress.changeCubeTexture(texture - 1);
        }
    }
}
