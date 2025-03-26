using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Config", menuName = "Configs/Level Config")]
public class LevelConfig : ScriptableObject
{
    [Header("Used Blocks:")]
    [SerializeField] private List<BlockColor> blocks = new List<BlockColor>();

    public List<BlockColor> Blocks => blocks;
}
