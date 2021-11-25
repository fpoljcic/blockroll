using UnityEngine;
using static AllLevels;

public class CameraPosition : MonoBehaviour {

    public void UpdateCameraPosition(Block[] level, Vector3 cameraOverride) {
        float minX = float.MaxValue, maxX = float.MinValue, minZ = float.MaxValue, maxZ = float.MinValue;

        foreach(Block block in level) {
            if (block.x < minX) {
                minX = block.x;
            }

            if (block.x > maxX) {
                maxX = block.x;
            }

            if (block.y < minZ) {
                minZ = block.y;
            }

            if (block.y > maxZ) {
                maxZ = block.y;
            }
        }

        maxX++;
        minZ--;
        maxZ += 10;

        float distanceZ = Mathf.Abs(maxZ - minZ);
        float y = 0.28867513459f * distanceZ;
        float z = minZ - (0.07735026918f * distanceZ);

        float distanceX = Mathf.Abs(maxX + 1 - (minX - 1));
        float y2 = 0.389963f * distanceX;
        float z2 = -0.353068f * distanceX;

        float finalX = cameraOverride.x != 0 ? cameraOverride.x : (float)(minX + maxX) / 2;
        float finalY = cameraOverride.y != 0 ? cameraOverride.y : Mathf.Max(y, y2);
        float finalZ = cameraOverride.z != 0 ? cameraOverride.z : Mathf.Min(z, z2);

        Camera.main.transform.position = new Vector3(finalX, finalY, finalZ);

        //Debug.Log("Camera set to x=" + Camera.main.transform.position.x + ", y=" + Camera.main.transform.position.y + ", z=" + Camera.main.transform.position.z);
    }
}
