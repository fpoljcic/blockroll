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
    public AllLevels allLevels;
    int level = 4;
    bool isRendering = false;

    void Start() {
        renderLevel();
    }

    void renderLevel() {
        isRendering = true;

        movement.resetCube();

        cube.transform.position = new Vector3(0.5f, 1, 0.5f);

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
                    onLevelWin();
                    return;
                }

                if (firstPointBlock.type == BlockType.UNSTABLE_VERTICAL && movement.cube.isVertical()) {
                    GameObject.Find(firstPointBlock.getName()).GetComponent<BoxCollider>().enabled = false;
                    onLevelLose();
                    return;
                }

                if (movement.cube.isHorizontal() && (firstPointBlock.type == BlockType.UNSTABLE_HORIZONTAL || secondPointBlock.type == BlockType.UNSTABLE_HORIZONTAL)) {
                    GameObject.Find(firstPointBlock.getName()).GetComponent<BoxCollider>().enabled = false;
                    GameObject.Find(secondPointBlock.getName()).GetComponent<BoxCollider>().enabled = false;
                    onLevelLose();
                    return;
                }

                break;
            }
        }

        if (!isOnBoard) {
            onLevelLose();
        }
    }

    void onLevelLose() {
        movement.isMoving = true;
        cube.GetComponent<ConstantForce>().force = movement.cube.direction * 100;
        cube.GetComponent<Rigidbody>().useGravity = true;
        Debug.Log("LOSE!");
    }

    void onLevelWin() {
        level++;
        renderLevel();
        Debug.Log("WIN!");
    }
}
