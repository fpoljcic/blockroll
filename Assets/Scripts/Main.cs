using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static AllLevels;

public class Main : MonoBehaviour {
    public Movement movement;
    public GameProgress gameProgress;
    public GameObject cube;
    public GameObject startBlock;
    public GameObject standardBlock;
    public GameObject endBlock;
    public GameObject disappieringBlock;
    public GameObject unstableHorizontalBlock;
    public GameObject unstableVerticalBlock;
    public AllLevels allLevels;
    public CameraPosition cameraPosition;
    public TextMeshProUGUI levelText;
    public Block[] level;
    public AudioSource winSound;
    public AudioSource loseSound;
    public AudioSource moveVerticalSound;
    public AudioSource moveHorizontalSound;
    public AudioSource gameWinSound;
    public AudioSource backgroundMusic;
    public Stopwatch stopwatch;
    public GameObject portalEffect;
    public GameObject newLevelEffect;
    public GameObject endGameEffect;
    public GameObject congratulationsText;
    GameObject removedColliderBlock;
    int? prevFirstPointBlockIndex;
    int? prevSecondPointBlockIndex;
    List<Block> disappearedBlocks = new List<Block>();

    void Start() {
        renderLevel();
        stopwatch.StartStopwatch();
    }

    void Update() {
        if (!movement.isMoving && Input.GetKeyDown(KeyCode.R)) {
            renderLevel();
        } else if (!movement.isMoving && Input.GetKeyDown(KeyCode.L)) {
            gameProgress.AdvanceLevel();
            renderLevel();
        } else if (!movement.isMoving && Input.GetKeyDown(KeyCode.J)) {
            gameProgress.PreviousLevel();
            renderLevel();
        } else if (Input.GetKeyDown(KeyCode.X) && !congratulationsText.activeSelf) {
            gameProgress.ResetGame();
            stopwatch.SetElapsedText("ELAPSED");
            stopwatch.ResetStopwatch();
            renderLevel();
        }
    }

    void renderLevel() {
        prevFirstPointBlockIndex = null;
        prevSecondPointBlockIndex = null;

        if (level != null) {
            resetLevel(level);
        }

        level = allLevels.levels[gameProgress.GetLevel() - 1];

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Board")) {
            Destroy(obj);
        }

        Vector3 startBlockPosition = Vector3.zero;
        foreach (Block block in level) {
            GameObject blockToRender;

            switch (block.type) {
                case BlockType.START:
                    startBlockPosition = new Vector3(block.x + 0.5f, 0.1f, block.y + 0.5f);
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
                case BlockType.DISAPPEARING:
                    blockToRender = disappieringBlock;
                    break;
                case BlockType.END:
                    portalEffect.transform.position = new Vector3(block.x + 0.5f, 0.05f, block.y + 0.5f);
                    blockToRender = endBlock;
                    break;
                default:
                    blockToRender = standardBlock;
                    break;
            }

            GameObject instantiatedBlock = Instantiate(blockToRender, new Vector3(block.x + 0.5f, -0.05f, block.y + 0.5f), Quaternion.identity);

            instantiatedBlock.name = block.getName();
            instantiatedBlock.tag = "Board";

            if (block.type == BlockType.DISAPPEARING) {
                instantiatedBlock.AddComponent<Rigidbody>().useGravity = false;
            }
        }

        levelText.text = "LEVEL " + gameProgress.GetLevel().ToString();

        Vector3 cameraOverride = Vector3.zero;
        if (allLevels.cameraOverrides.ContainsKey(gameProgress.GetLevel())) {
            cameraOverride = allLevels.cameraOverrides[gameProgress.GetLevel()];
        }

        cameraPosition.UpdateCameraPosition(level, cameraOverride);

        Destroy(newLevelEffect);

        newLevelEffect = Instantiate(newLevelEffect, startBlockPosition, Quaternion.identity);
    }

    void resetCubePosition() {
        movement.resetCube();
        cube.GetComponent<Rigidbody>().useGravity = false;
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cube.transform.SetPositionAndRotation(new Vector3(0.5f, 1, 0.5f), Quaternion.identity);
    }

    void resetRemovedCubes(Block[] level) {
        int j = 0;

        for (int i = 0; i < level.Length; i++) {
            if (level[i] == null) {
                Block block = disappearedBlocks[j++];
                level[i] = block;

                GameObject blockGameObject = GameObject.Find(block.getName());
                blockGameObject.GetComponent<Rigidbody>().useGravity = false;
                blockGameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                blockGameObject.transform.SetPositionAndRotation(new Vector3(block.x + 0.5f, -0.05f, block.y + 0.5f), Quaternion.identity);
            }
        }

        prevFirstPointBlockIndex = null;
        prevSecondPointBlockIndex = null;
        disappearedBlocks.Clear();
    }

    public void resetLevel() {
        if (!movement.isMoving) {
            resetLevel(level);
        }
    }

    public void resetLevel(Block[] level) {
        if (removedColliderBlock != null) {
            removedColliderBlock.GetComponent<BoxCollider>().enabled = true;
            removedColliderBlock = null;
        }

        resetCubePosition();
        resetRemovedCubes(level);
    }

    public void checkCubePositionOnBoard() {
        bool isOnBoard = false;
        bool firstPointOnBoard = false;
        bool secondPointOnBoard = false;
        Block firstPointBlock = null;
        Block secondPointBlock = null;

        if (prevFirstPointBlockIndex != null && level[(int)prevFirstPointBlockIndex].type == BlockType.DISAPPEARING) {
            GameObject.Find(level[(int)prevFirstPointBlockIndex].getName()).GetComponent<Rigidbody>().useGravity = true;
            disappearedBlocks.Add(level[(int)prevFirstPointBlockIndex]);
            level[(int)prevFirstPointBlockIndex] = null;
        }

        if (prevSecondPointBlockIndex != null && prevFirstPointBlockIndex != prevSecondPointBlockIndex && level[(int)prevSecondPointBlockIndex].type == BlockType.DISAPPEARING) {
            GameObject.Find(level[(int)prevSecondPointBlockIndex].getName()).GetComponent<Rigidbody>().useGravity = true;
            disappearedBlocks.Add(level[(int)prevSecondPointBlockIndex]);
            level[(int)prevSecondPointBlockIndex] = null;
        }

        for (int i = 0; i < level.Length; i++) {
            Block block = level[i];

            if (block == null) {
                continue;
            }

            if (block.x == movement.cube.point1.x && block.y == movement.cube.point1.y) {
                firstPointOnBoard = true;
                firstPointBlock = block;
                prevFirstPointBlockIndex = i;
            }

            if (block.x == movement.cube.point2.x && block.y == movement.cube.point2.y) {
                secondPointOnBoard = true;
                secondPointBlock = block;
                prevSecondPointBlockIndex = i;
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
                playMovementSound();
                break;
            }
        }

        if (firstPointOnBoard && !secondPointOnBoard || !firstPointOnBoard && secondPointOnBoard) {
            playMovementSound();
        }

        if (!isOnBoard) {
            Vector3 push = determinePushDirection(firstPointOnBoard, secondPointOnBoard, firstPointBlock, secondPointBlock);
            StartCoroutine(onLevelLose(push));
        }
    }

    private void playMovementSound() {
        if (movement.cube.isHorizontal()) {
            moveHorizontalSound.Play();
        } else if (movement.cube.isVertical()) {
            moveVerticalSound.Play();
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
                float rotationAngle = Mathf.Min(2.4f, remainingAngle);
                cube.transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
                remainingAngle -= rotationAngle;
                yield return new WaitForSeconds(0.01f);
            }
        }

        cube.GetComponent<Rigidbody>().useGravity = true;
        loseSound.Play();
        yield return new WaitForSeconds(3f);

        resetLevel(level);
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

        if (gameProgress.GetLevel() == 15) {
            cameraPosition.LockCamera();
            congratulationsText.SetActive(true);
            stopwatch.StopStopwatch();
            backgroundMusic.Stop();
            gameWinSound.Play();
            stopwatch.SetElapsedText("COMPLETED");
            StartCoroutine(MoveCamera());
            endGameEffect.SetActive(true);
        }

        cube.GetComponent<Rigidbody>().useGravity = true;

        if (gameProgress.GetLevel() != 15) {
            winSound.Play();
            yield return new WaitForSeconds(3f);
            gameProgress.AdvanceLevel();
            renderLevel();
            movement.isMoving = false;
        }
    }

    public void exitClick() {
        Menu.fromLevel = true;
        SceneManager.LoadScene("Menu");
    }

    IEnumerator MoveCamera() {
        while (Camera.main.transform.position.y <= 30) {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + Time.deltaTime * 1.5f, Camera.main.transform.position.z);
            yield return null;
        }
    }
}
