using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SetBlockSprite(Sprite blockSprite)
    {
        spriteRenderer.sprite = blockSprite;
    }
}
