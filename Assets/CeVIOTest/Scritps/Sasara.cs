using UnityEngine;
using System.Collections;

public class Sasara : MonoBehaviour
{
    public SpriteRenderer sprite;
    const int size = 16;
    public  Sprite[] sprites = new Sprite[size+2];
    // Use this for initialization
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("Sprite/Sasara");
    }
    public void ChangeSasara()
    {
        int i = Random.Range(0, 256) % 16;
        sprite.sprite = sprites[i];
    }

}
