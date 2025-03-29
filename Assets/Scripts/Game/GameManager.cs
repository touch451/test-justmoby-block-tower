using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private ScrollPanel scrollPanel;

    private List<Cube> orderInstalledBlocks = new List<Cube>();

    public static GameManager Instance { get; private set; }
    public EventsSystem events = new EventsSystem();
    public LevelConfig config => levelConfig;
    public bool hasInstalledBlocks => orderInstalledBlocks.Count > 0;
    
    private void Awake()
    {
        SetInstance();
        SetListeners();
    }

    private void Start()
    {
        scrollPanel.SetBlocks(levelConfig.Colors);
    }

    private void OnDestroy()
    {
        events.RemoveAllListeners();
    }

    private void SetInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void SetListeners()
    {
        events.onBlockInstall.AddListener(OnBlockInstall);
        events.onBlockBeginDrag.AddListener(OnBlockBeginDrag);
    }

    private void OnBlockBeginDrag(Cube draggedBlock, PointerEventData eventData)
    {
        orderInstalledBlocks.Remove(draggedBlock);
    }

    private void OnBlockInstall(Cube installedBlock)
    {
        orderInstalledBlocks.Add(installedBlock);
    }

    public float GetTowerHeight()
    {
        float height = -100;

        if (hasInstalledBlocks)
        {
            var lastBlock = orderInstalledBlocks[orderInstalledBlocks.Count - 1];
            height = lastBlock.bounds.max.y;
        }
            
        return height;
    }
}
