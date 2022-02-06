using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class csShowAllEffect : MonoBehaviour
{
    public string[] EffectName;
    public Transform[] Effect;
    public Text Text1;
    public int i = 0;

    void Start()
    {
        Instantiate(Effect[i], new Vector3(0, 0, 0), Quaternion.identity);
    }

    void Update ()
    {
        Text1.text = i + 1 + ":" + EffectName[i];
    }
}
