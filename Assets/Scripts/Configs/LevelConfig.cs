using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Config", menuName = "Configs/Level Config")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] [Range(1.0f, 20.0f)] private float fallSpeed = 10f;
    [SerializeField] private int blocksCount = 20;

    [Header("Used colors:")]
    [SerializeField] private List<BlockColor> colors = new List<BlockColor>
    {
        BlockColor.Black,
        BlockColor.Blue_Light,
        BlockColor.Blue,
        BlockColor.Brown,
        BlockColor.Cyan_Light,
        BlockColor.Cyan,
        BlockColor.Gray,
        BlockColor.Green_Light,
        BlockColor.Green,
        BlockColor.Mauve,
        BlockColor.Orange,
        BlockColor.Pink,
        BlockColor.Plum,
        BlockColor.Purple_Light,
        BlockColor.Purple,
        BlockColor.Red_Dark,
        BlockColor.Red,
        BlockColor.Rosybrown,
        BlockColor.Yellow_Light,
        BlockColor.Yellow
    };

    public List<BlockColor> Colors => colors;
    public float FallSpeed => fallSpeed;
    public int BlocksCount => blocksCount;
}
