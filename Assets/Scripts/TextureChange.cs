using UnityEngine;

public class TextureChange : MonoBehaviour { 

    public Texture[] textures;
    public Renderer cube;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            cube.material.mainTexture = textures[0];
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            cube.material.mainTexture = textures[1];
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            cube.material.mainTexture = textures[2];
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            cube.material.mainTexture = textures[3];
        }
    }
}
