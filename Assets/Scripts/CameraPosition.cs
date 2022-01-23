using UnityEngine;
using static AllLevels;

public class CameraPosition : MonoBehaviour {
    private Vector3 centerPoint;
    private float targetAngle = 0;
    private readonly Vector3[] directions = { Vector3.right, Vector3.back, Vector3.left, Vector3.forward };
    private int offset = 0;
    private bool lockedCamera = false;

    void Update() {
        if (!lockedCamera) {
            if (Input.GetKeyDown(KeyCode.A)) {
                offset = (offset + 1) % 4;
                targetAngle -= 90.0f;
            } else if (Input.GetKeyDown(KeyCode.D)) {
                offset = offset - 1 < 0 ? 3 : (offset - 1);
                targetAngle += 90.0f;
            } else if (Input.GetKeyDown(KeyCode.S)) {
                ResetCamera();
            }
        }

        if (targetAngle != 0) {
            Rotate();
        }
    }

    public Vector3 GetDirection(int position) {
        return directions[(position + offset) % 4];
    }

    public void ResetCamera() {
        switch (offset) {
            case 1:
                targetAngle += 90.0f;
                break;
            case 2:
                targetAngle += 180.0f;
                break;
            case 3:
                targetAngle -= 90.0f;
                break;
        }
        offset = 0;
    }

    public void LockCamera() {
        lockedCamera = true;
        ResetCamera();
    }

    private void Rotate() {
        const float rotationAmount = 2f;

        if (targetAngle > 0) {
            transform.RotateAround(centerPoint, Vector3.up, -rotationAmount);
            targetAngle -= rotationAmount;
        } else if (targetAngle < 0) {
            transform.RotateAround(centerPoint, Vector3.up, rotationAmount);
            targetAngle += rotationAmount;
        }
    }

    public void UpdateCameraPosition(Block[] level, Vector3 cameraOverride) {
        float minX = float.MaxValue, maxX = float.MinValue, minZ = float.MaxValue, maxZ = float.MinValue;

        foreach (Block block in level) {
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

        centerPoint = new Vector3(finalX, finalY, minZ + ((maxZ - 8 - minZ) / 2));
        offset = 0;
        targetAngle = 0;
        Camera.main.transform.SetPositionAndRotation(new Vector3(finalX, finalY, finalZ), Quaternion.Euler(45, 0, 0));

        //Debug.Log("Camera set to x=" + Camera.main.transform.position.x + ", y=" + Camera.main.transform.position.y + ", z=" + Camera.main.transform.position.z);
    }
}
