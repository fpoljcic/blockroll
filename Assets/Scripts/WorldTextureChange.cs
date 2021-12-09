using UnityEngine;

public class WorldTextureChange : MonoBehaviour {
    public Texture[] textures;
    public Renderer sphere;
    public GameProgress gameProgress;

    void Start() {
        if (gameProgress != null) {
            sphere.material.mainTexture = textures[gameProgress.GetWorldTexture()];
        }
    }
}
