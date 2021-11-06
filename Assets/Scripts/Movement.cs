using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public int speed = 300;
    bool isMoving = false;
    CubePosition cubePosition = CubePosition.VERTICAL;

    enum CubePosition {
        VERTICAL,
        HORIZONTAL_X,
        HORIZONTAL_Y
    }

    void Update() {
        if (isMoving) {
            return;
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            StartCoroutine(Roll(Vector3.right));
        } else if (Input.GetKey(KeyCode.LeftArrow)) {
            StartCoroutine(Roll(Vector3.left));
        } else if (Input.GetKey(KeyCode.UpArrow)) {
            StartCoroutine(Roll(Vector3.forward));
        } else if (Input.GetKey(KeyCode.DownArrow)) {
            StartCoroutine(Roll(Vector3.back));
        }
    }

    IEnumerator Roll(Vector3 direction) {
        isMoving = true;

        int directionDivide = 2;
        if ((isHorizontalX() && (direction == Vector3.left || direction == Vector3.right)) || (isHorizontalY() && (direction == Vector3.forward || direction == Vector3.back))) {
            directionDivide = 1;
        }

        Vector3 rotationCenter = transform.position + direction / directionDivide + Vector3.down / (isVertical() ? 1 : 2);
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        float remainingAngle = 90;
        while (remainingAngle > 0) {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }

        updateCubePosition(direction);

        isMoving = false;
    }

    void updateCubePosition(Vector3 direction) {
        bool isHorizontalMovement;
        if (direction == Vector3.right || direction == Vector3.left) {
            isHorizontalMovement = true;
        } else {
            isHorizontalMovement = false;
        }

        if (isVertical()) {
            if (isHorizontalMovement) {
                cubePosition = CubePosition.HORIZONTAL_X;
            } else {
                cubePosition = CubePosition.HORIZONTAL_Y;
            }
        } else if (isHorizontalX() && isHorizontalMovement || isHorizontalY() && !isHorizontalMovement) {
            cubePosition = CubePosition.VERTICAL;
        }
    }

    bool isVertical() {
        return cubePosition == CubePosition.VERTICAL;
    }

    bool isHorizontal() {
        return cubePosition == CubePosition.HORIZONTAL_X || cubePosition == CubePosition.HORIZONTAL_Y;
    }

    bool isHorizontalX() {
        return cubePosition == CubePosition.HORIZONTAL_X;
    }

    bool isHorizontalY() {
        return cubePosition == CubePosition.HORIZONTAL_Y;
    }
}
