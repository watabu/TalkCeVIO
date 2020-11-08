using UnityEngine;
using System.Collections;

/// <summary>
/// 立ち絵の変更を行う
/// </summary>
public class Sasara : MonoBehaviour
{
    private SpriteRenderer m_Sprite;
    
    [SerializeField]
    private Sprite[] m_BaseSprite;
    // Use this for initialization
    private void Start()
    {
        m_Sprite = GetComponent<SpriteRenderer>();
        m_BaseSprite = Resources.LoadAll<Sprite>("Sasara");
    }
    public void ChangeSasara()
    {
        int i = Random.Range(0, 256) % m_BaseSprite.Length;
        m_Sprite.sprite = m_BaseSprite[i];
    }

}
