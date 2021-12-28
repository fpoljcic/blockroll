using UnityEngine;

public class AudioScript : MonoBehaviour {

    public AudioSource buttonClickSound;

    public void playButtonClickSound() {
        buttonClickSound.Play();
    }
}
