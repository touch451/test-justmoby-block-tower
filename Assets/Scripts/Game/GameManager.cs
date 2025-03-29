using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private ScrollPanel scrollPanel;
    [SerializeField] private GameManager finishScreen;
    
    public static GameManager Instance { get; private set; }
    public EventsSystem events = new EventsSystem();
    public BlocksOrderSystem blocksOrder = new BlocksOrderSystem();
    public LevelConfig config => levelConfig;
    public ScrollPanel ScrollPanel => scrollPanel;

    private float finishTowerHeight;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        SetInstance();
        SetListeners();
    }

    private void Start()
    {
        CalculateFinishHeight();
        scrollPanel.SetBlocks(config.BlocksCount, config.Colors);
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
        events.onBlockDestroy.AddListener(OnBlockDestroy);
        events.onBlockBeginDrag.AddListener(OnBlockBeginDrag);
    }

    private void OnBlockBeginDrag(Block draggedBlock, PointerEventData eventData)
    {
        if (!blocksOrder.ContainsBlock(draggedBlock))
            return;

        var upperBlocks = blocksOrder.GetUpperBlocks(draggedBlock);
        blocksOrder.RemoveBlock(draggedBlock);
        if (upperBlocks.Count == 0)
        {
            //blocksOrder.RemoveBlock(draggedBlock);
            return;
        }

        bool hasBlockBelow =
            upperBlocks[0].HasBlockBelow(out float distanceToBlock, 1);

        for (int i = 0; i < upperBlocks.Count; i++)
        {
            var block = upperBlocks[i];

            float fallDistance = hasBlockBelow ? distanceToBlock : 100f;
            float delay = (i + 1) * 0.1f;

            block.DoFall(fallDistance, delay);
            blocksOrder.RemoveBlock(block);
        }
    }

    private void OnBlockDestroy(Block destroyededBlock)
    {
        blocksOrder.RemoveBlock(destroyededBlock);
    }

    private void OnBlockInstall(Block installedBlock)
    {
        blocksOrder.AddBlock(installedBlock);

        if (IsFinish())
        {
            ShowFinishScreen();
        }
    }

    private bool IsFinish()
    {
        return GetTowerHeight() >= finishTowerHeight;
    }

    private void ShowFinishScreen()
    {
        // Show finish screen
    }

    private void CalculateFinishHeight()
    {
        var cam = Camera.main;

        finishTowerHeight =
            cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane)).y;
    }

    public float GetTowerHeight()
    {
        float height = -100f;

        if (blocksOrder.hasInstalledBlocks)
        {
            var upperBlock = blocksOrder.GetUpperBlock();
            height = upperBlock.bounds.max.y;
        }
            
        return height;
    }
}
