using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    public static bool fromLevel = false;
    public GameObject levelsContainer;
    public GameObject optionsCube;
    public GameProgress gameProgress;
    public AudioMixer audioMixer;
    public Slider slider;
    public TMP_Dropdown GraphicsDropdown;
    public TMP_Dropdown ResolutionDropdown;
    public Toggle fullscreenToggle;
    Resolution[] resolutions;
    public Texture[] textures;
    public GameObject mainMenu;
    public GameObject levelsMenu;
    public Renderer cube;

    void Start() {
        if (fromLevel) {
            mainMenu.SetActive(false);
            SetLevelButtons();
            levelsMenu.SetActive(true);
        }

        ResolutionDropdown.onValueChanged.AddListener((e) => {
            ResolutionDropdown.GetComponentInChildren<TextMeshProUGUI>().text = ResolutionDropdown.options[ResolutionDropdown.value].text;
        });

        GraphicsDropdown.onValueChanged.AddListener((e) => {
            GraphicsDropdown.GetComponentInChildren<TextMeshProUGUI>().text = GraphicsDropdown.options[GraphicsDropdown.value].text;
        });

        SetDefaultValues();
        if (!fromLevel) {
            SetLevelButtons();
        }
        SetResolutions();
    }

    void Update() {
        optionsCube.transform.Rotate(Vector3.up * Time.deltaTime * 20);
    }

    private void SetLevelButtons() {
        Button[] levelButtons = levelsContainer.GetComponentsInChildren<Button>();
        int level = gameProgress.GetLevelsUnlocked();

        foreach (Button button in levelButtons) {
            int buttonLevel = int.TryParse(button.name, out buttonLevel) ? buttonLevel : -1;
            if (buttonLevel != -1 && buttonLevel > level) {
                button.interactable = false;
            }
        }
    }

    private void SetDefaultValues() {
        float volume = gameProgress.GetVolume();
        audioMixer.SetFloat("volume", volume);
        slider.value = volume;

        int quality = gameProgress.GetQuality();
        QualitySettings.SetQualityLevel(quality);
        GraphicsDropdown.value = quality;
        GraphicsDropdown.RefreshShownValue();

        int resolutionWidth = gameProgress.GetResolutionWidth();
        int resolutionHeight = gameProgress.GetResolutionHeight();
        if (resolutionWidth != -1) {
            Screen.SetResolution(resolutionWidth, resolutionHeight, Screen.fullScreen);
        }

        bool isFullScreen = bool.Parse(gameProgress.GetFullscreen());
        Screen.fullScreen = isFullScreen;
        fullscreenToggle.isOn = isFullScreen;

        int texture = gameProgress.GetCubeTexture();
        cube.material.mainTexture = textures[texture];
    }

    private void SetResolutions() {
        int CurrentResolutionIndex = 0;
        resolutions = Screen.resolutions;

        ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++) {
            string Option = resolutions[i].width + " x " + resolutions[i].height + " | " + resolutions[i].refreshRate + "Hz";
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
        gameProgress.SetLevel(level);
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame() {
        Debug.Log("Exit game");
        Application.Quit();
    }

    public void SetResolution(int ResolutionIndex) {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        gameProgress.SetResolution(resolution.width, resolution.height);
    }

    public void SetVolume(float volume) {
        audioMixer.SetFloat("volume", volume);
        gameProgress.SetVolume(volume);
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
        gameProgress.SetQuality(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
        gameProgress.SetFullscreen(isFullscreen.ToString());
    }

    public void NextCubeTexture() {
        int texture = gameProgress.GetCubeTexture();
        if (texture < 3) {
            cube.material.mainTexture = textures[texture + 1];
            gameProgress.ChangeCubeTexture(texture + 1);
        }
    }

    public void PreviousCubeTexture() {
        int texture = gameProgress.GetCubeTexture();
        if (texture > 0) {
            cube.material.mainTexture = textures[texture - 1];
            gameProgress.ChangeCubeTexture(texture - 1);
        }
    }
}
