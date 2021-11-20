using UnityEngine;
using static AllLevels;

public class CameraPosition : MonoBehaviour {

    public void UpdateCameraPosition(Block[] level) {
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
        float z = 0.07735026918f * distanceZ;

        float distanceX = Mathf.Abs(maxX + 1 - (minX - 1));
        if (distanceX > distanceZ) {
            y = 0.28867513459f * distanceX;
            z = 0.08735026918f * distanceX;
        }

        Camera.main.transform.position = new Vector3((float) (minX + maxX) / 2, y, minZ - z);

        //Debug.Log("Camera set to x=" + Camera.main.transform.position.x + ", y=" + Camera.main.transform.position.y + ", z=" + Camera.main.transform.position.z);
    }
}
