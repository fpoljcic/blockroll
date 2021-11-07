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
    int level = 0;

    void Start() {
        foreach (Block block in allLevels.levels[level]) {
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

            Instantiate(blockToRender, new Vector3(block.x + 0.5f, -0.05f, block.y + 0.5f), Quaternion.identity);
        }
    }

    void Update() {
        StartCoroutine(checkCubeOnBoard());
    }

    IEnumerator checkCubeOnBoard() {
        bool isOnBoard = false;
        bool firstPointOnBoard = false;
        bool secondPointOnBoard = false;

        foreach (Block block in allLevels.levels[level]) {
            if (block.x == movement.cube.point1.x && block.y == movement.cube.point1.y) {
                firstPointOnBoard = true;
                if (block.type == BlockType.END && movement.cube.isVertical()) {
                    onLevelWin();
                }
            }
            if (block.x == movement.cube.point2.x && block.y == movement.cube.point2.y) {
                secondPointOnBoard = true;
            }
            if (firstPointOnBoard && secondPointOnBoard) {
                isOnBoard = true;
                break;
            }
        }

        if (!isOnBoard) {
            onLevelLose();
        }

        yield return null;
    }

    void onLevelLose() {
        movement.isMoving = true;
        cube.GetComponent<ConstantForce>().force = movement.cube.direction * 100;
        cube.GetComponent<Rigidbody>().useGravity = true;
    }

    void onLevelWin() {
        // level++;
        Debug.Log("WIN!");
    }
}
