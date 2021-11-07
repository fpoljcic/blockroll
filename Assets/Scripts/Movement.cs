using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public Cube cube = new Cube(Vector2.zero, Vector2.zero, Vector3.zero, Orientation.VERTICAL);
    public int speed = 300;
    public bool isMoving = false;

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
        if ((cube.isHorizontalX() && (direction == Vector3.left || direction == Vector3.right)) || (cube.isHorizontalY() && (direction == Vector3.forward || direction == Vector3.back))) {
            directionDivide = 1;
        }

        Vector3 rotationCenter = transform.position + direction / directionDivide + Vector3.down / (cube.isVertical() ? 1 : 2);
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        float remainingAngle = 90;
        while (remainingAngle > 0) {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }

        updateCubeInfo(direction);

        isMoving = false;
    }

    void updateCubeInfo(Vector3 direction) {
        bool isHorizontalMovement;
        if (direction == Vector3.right || direction == Vector3.left) {
            isHorizontalMovement = true;
        } else {
            isHorizontalMovement = false;
        }

        cube.direction = direction;

        if (cube.isVertical()) {
            // Iz vertikalnog polozaja u horizontalni
            if (isHorizontalMovement) {
                cube.orientation = Orientation.HORIZONTAL_X;
                cube.point1 = new Vector2(cube.point1.x + (direction == Vector3.left ? 2 : 1) * direction.x, cube.point1.y);
                cube.point2 = new Vector2(cube.point2.x + (direction == Vector3.right ? 2 : 1) * direction.x, cube.point2.y);
            } else {
                cube.orientation = Orientation.HORIZONTAL_Y;
                cube.point1 = new Vector2(cube.point1.x, cube.point1.y + (direction == Vector3.forward ? 2 : 1) * direction.z);
                cube.point2 = new Vector2(cube.point2.x, cube.point2.y + (direction == Vector3.back ? 2 : 1) * direction.z);
            }
        } else if (cube.isHorizontalX() && isHorizontalMovement || cube.isHorizontalY() && !isHorizontalMovement) {
            // Iz horizontalnog polozaja u vertikalni
            cube.orientation = Orientation.VERTICAL;
            if (isHorizontalMovement) {
                cube.point1 = new Vector2((direction == Vector3.right ? cube.point2.x : cube.point1.x) + direction.x, cube.point1.y);
                cube.point2 = cube.point1;
            } else {
                cube.point1 = new Vector2(cube.point1.x, (direction == Vector3.forward ? cube.point2.y : cube.point1.y) + direction.z);
                cube.point2 = cube.point1;
            }
        } else if (cube.isHorizontalY() && isHorizontalMovement) {
            // Iz horizontalnog Y polozaja u horizontalni Y
            cube.point1 = new Vector2(cube.point1.x + direction.x, cube.point1.y);
            cube.point2 = new Vector2(cube.point2.x + direction.x, cube.point2.y);
        } else if (cube.isHorizontalX() && !isHorizontalMovement) {
            // Iz horizontalnog X polozaja u horizontalni X
            cube.point1 = new Vector2(cube.point1.x, cube.point1.y + direction.z);
            cube.point2 = new Vector2(cube.point2.x, cube.point2.y + direction.z);
        }
    }

    public class Cube {
        public Vector2 point1;
        public Vector2 point2;
        public Vector3 direction;
        public Orientation orientation;

        public Cube(Vector2 point1, Vector2 point2, Vector3 direction, Orientation orientation) {
            this.point1 = point1;
            this.point2 = point2;
            this.direction = direction;
            this.orientation = orientation;
        }

        public bool isVertical() {
            return orientation == Orientation.VERTICAL;
        }

        public bool isHorizontal() {
            return orientation == Orientation.HORIZONTAL_X || orientation == Orientation.HORIZONTAL_Y;
        }

        public bool isHorizontalX() {
            return orientation == Orientation.HORIZONTAL_X;
        }

        public bool isHorizontalY() {
            return orientation == Orientation.HORIZONTAL_Y;
        }
    }

    public enum Orientation {
        VERTICAL,
        HORIZONTAL_X,
        HORIZONTAL_Y
    }
}
