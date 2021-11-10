using System.Collections;
using UnityEngine;
using static AllLevels;

public class Main : MonoBehaviour {
    public Movement movement;
    public GameObject cube;
    public GameObject startBlock;
    public GameObject standardBlock;
    public GameObject endBlock;
    public GameObject disappieringBlock;
    public GameObject unstableHorizontalBlock;
    public GameObject unstableVerticalBlock;
    public GameObject removedColliderBlock;
    public AllLevels allLevels;
    int level = 1;
    bool isRendering = false;

    void Start() {
        renderLevel();
    }

    void renderLevel() {
        isRendering = true;

        resetCubePosition();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Board")) {
            Destroy(obj);
        }

        foreach (Block block in allLevels.levels[level - 1]) {
            GameObject blockToRender;

            switch (block.type) {
                case BlockType.START:
                    blockToRender = startBlock;
                    break;
                case BlockType.STANDARD:
                    blockToRender = standardBlock;
                    break;
                case BlockType.UNSTABLE_HORIZONTAL:
                    blockToRender = unstableHorizontalBlock;
                    break;
                case BlockType.UNSTABLE_VERTICAL:
                    blockToRender = unstableVerticalBlock;
                    break;
                case BlockType.DISAPPIERING:
                    blockToRender = disappieringBlock;
                    break;
                case BlockType.END:
                    blockToRender = endBlock;
                    break;
                default:
                    blockToRender = standardBlock;
                    break;
            }

            GameObject instantiatedBlock = Instantiate(blockToRender, new Vector3(block.x + 0.5f, -0.05f, block.y + 0.5f), Quaternion.identity);

            instantiatedBlock.name = block.getName();
            instantiatedBlock.tag = "Board";
        }

        isRendering = false;
    }

    void resetCubePosition() {
        movement.resetCube();
        cube.GetComponent<Rigidbody>().useGravity = false;
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cube.transform.rotation = Quaternion.identity;
        cube.transform.position = new Vector3(0.5f, 1, 0.5f);
    }

    public void checkCubePositionOnBoard() {
        bool isOnBoard = false;
        bool firstPointOnBoard = false;
        bool secondPointOnBoard = false;
        Block firstPointBlock = null;
        Block secondPointBlock = null;

        foreach (Block block in allLevels.levels[level - 1]) {
            if (block.x == movement.cube.point1.x && block.y == movement.cube.point1.y) {
                firstPointOnBoard = true;
                firstPointBlock = block;
            }

            if (block.x == movement.cube.point2.x && block.y == movement.cube.point2.y) {
                secondPointOnBoard = true;
                secondPointBlock = block;
            }

            if (firstPointOnBoard && secondPointOnBoard) {
                isOnBoard = true;

                if (firstPointBlock.type == BlockType.END && movement.cube.isVertical()) {
                    GameObject.Find(firstPointBlock.getName()).GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(onLevelWin());
                    return;
                }

                if (firstPointBlock.type == BlockType.UNSTABLE_VERTICAL && movement.cube.isVertical()) {
                    GameObject.Find(firstPointBlock.getName()).GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(onLevelLose());
                    return;
                }

                if (movement.cube.isHorizontal() && (firstPointBlock.type == BlockType.UNSTABLE_HORIZONTAL || secondPointBlock.type == BlockType.UNSTABLE_HORIZONTAL)) {
                    GameObject.Find(firstPointBlock.getName()).GetComponent<BoxCollider>().enabled = false;
                    GameObject.Find(secondPointBlock.getName()).GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(onLevelLose());
                    return;
                }

                break;
            }
        }

        if (!isOnBoard) {
            Vector3 push = determinePushDirection(firstPointOnBoard, secondPointOnBoard, firstPointBlock, secondPointBlock);
            StartCoroutine(onLevelLose(push));
        }
    }

    IEnumerator onLevelLose() {
        return onLevelLose(Vector3.zero);
    }

    IEnumerator onLevelLose(Vector3 direction) {
        movement.isMoving = true;

        if (!movement.cube.isVertical() && !direction.Equals(Vector3.zero)) {
            Vector3 rotationCenter = cube.transform.position + Vector3.down / (movement.cube.isVertical() ? 1 : 2);
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

            float remainingAngle = 90;
            while (remainingAngle > 0) {
                float rotationAngle = Mathf.Min(Time.deltaTime * 800, remainingAngle);
                cube.transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
                remainingAngle -= rotationAngle;
                yield return new WaitForSeconds(0.01f);
            }
        }
        
        cube.GetComponent<Rigidbody>().useGravity = true;

        yield return new WaitForSeconds(3f);

        if (removedColliderBlock != null) {
            removedColliderBlock.GetComponent<BoxCollider>().enabled = true;
            removedColliderBlock = null;
        }

        resetCubePosition();
        movement.isMoving = false;
    }

    Vector3 determinePushDirection(bool firstPointOnBoard, bool secondPointOnBoard, Block firstPointBlock, Block secondPointBlock) {
        Block block = null;
        Vector3 pushDirection = Vector3.zero;
        if (movement.cube.isHorizontalY()) {
            if (!firstPointOnBoard && secondPointOnBoard) {
                block = new Block(secondPointBlock.x, secondPointBlock.y + 2);
                pushDirection = Vector3.forward;
            } else if (firstPointOnBoard && !secondPointOnBoard) {
                block = new Block(firstPointBlock.x, firstPointBlock.y - 2);
                pushDirection = Vector3.back;
            }
        } else if (movement.cube.isHorizontalX()) {
            if (!firstPointOnBoard && secondPointOnBoard) {
                block = new Block(secondPointBlock.x - 2, secondPointBlock.y);
                pushDirection = Vector3.left;
            } else if (firstPointOnBoard && !secondPointOnBoard) {
                block = new Block(firstPointBlock.x + 2, firstPointBlock.y);
                pushDirection = Vector3.right;
            }
        }

        if (block != null) {
            GameObject gameObject = GameObject.Find(block.getName());
            if (gameObject != null) {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                removedColliderBlock = gameObject;
            }
        }

        return pushDirection;
    }

    IEnumerator onLevelWin() {
        movement.isMoving = true;
        cube.GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForSeconds(3f);
        level++;
        renderLevel();
        movement.isMoving = false;
    }
}
