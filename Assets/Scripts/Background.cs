using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour {
    public Image background;
    public Sprite[] sprites;
    public Image starsSmall1;
    public Image starsSmall2;
    public Image starsSmall3;
    public Image starsBig1;
    public Image starsBig2;
    public Image starsBig3;
    public GameProgress gameProgress;
    private readonly float speed = 1;

    void Start() {
        background.sprite = sprites[gameProgress.GetWorldTexture()];
        starsSmall1.transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
        starsSmall2.transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
        starsSmall3.transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
        starsBig1.transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
        starsBig2.transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
        starsBig3.transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
    }

    void Update() {
        starsSmall1.transform.Translate(speed * Time.deltaTime, 0, 0, Space.World);
        starsSmall2.transform.Translate(speed * Time.deltaTime, 0, 0, Space.World);
        starsSmall3.transform.Translate(speed * Time.deltaTime, 0, 0, Space.World);

        starsBig1.transform.Translate(speed * Time.deltaTime, 0, 0, Space.World);
        starsBig2.transform.Translate(speed * Time.deltaTime, 0, 0, Space.World);
        starsBig3.transform.Translate(speed * Time.deltaTime, 0, 0, Space.World);
        
        StartCoroutine(AdjustStarsPosition(starsSmall1));
        StartCoroutine(AdjustStarsPosition(starsSmall2));
        StartCoroutine(AdjustStarsPosition(starsSmall3));
        StartCoroutine(AdjustStarsPosition(starsBig1));
        StartCoroutine(AdjustStarsPosition(starsBig2));
        StartCoroutine(AdjustStarsPosition(starsBig3));
    }

    private IEnumerator AdjustStarsPosition(Image stars) {
        if (stars.transform.localPosition.x >= 2400) {
            stars.transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
            stars.transform.localPosition = new Vector3(-2400, 0, 0);
        }

        yield return null;
    }
}
