using System.Collections;
using UnityEngine;

public class Levels : MonoBehaviour {
    public Movement movement;
    public GameObject cube;
    public GameObject startBlock;
    public GameObject standardBlock;
    public GameObject endBlock;

    readonly Block[] level1 = new Block[] { new Block(0, 0, BlockType.START),
                                            new Block(6, -5, BlockType.END),
                                            new Block(-1, -1),
                                            new Block(0, -1),
                                            new Block(1, -1),
                                            new Block(-1, 0),
                                            new Block(-1, 1),
                                            new Block(0, 1),
                                            new Block(1, 1),
                                            new Block(1, 0),
                                            new Block(2, 0),
                                            new Block(3, 0),
                                            new Block(4, 0),
                                            new Block(5, 0),
                                            new Block(6, 0),
                                            new Block(7, 0),
                                            new Block(8, 0),
                                            new Block(7, -1),
                                            new Block(8, -1),
                                            new Block(7, -2),
                                            new Block(8, -2),
                                            new Block(7, -3),
                                            new Block(8, -3),
                                            new Block(7, -4),
                                            new Block(8, -4),
                                            new Block(7, -5),
                                            new Block(8, -5),
                                            new Block(7, -6),
                                            new Block(8, -6),
                                            new Block(5, -4),
                                            new Block(6, -4),
                                            new Block(5, -5),
                                            new Block(5, -6),
                                            new Block(6, -6),
    };

    void Start() {
        foreach(Block block in level1) {
            GameObject blockToRender;

            switch (block.type) {
                case BlockType.START:
                    blockToRender = startBlock;
                    break;
                case BlockType.STANDARD:
                    blockToRender = standardBlock;
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

        foreach (Block block in level1) {
            if (block.x == movement.cube.point1.x && block.y == movement.cube.point1.y) {
                firstPointOnBoard = true;
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

    class Block {
        public int x;
        public int y;
        public BlockType type;

        public Block(int x, int y) {
            this.x = x;
            this.y = y;
            type = BlockType.STANDARD;
        }

        public Block(int x, int y, BlockType type) {
            this.x = x;
            this.y = y;
            this.type = type;
        }
    }

    enum BlockType {
        START,
        STANDARD,
        END
    }
}
