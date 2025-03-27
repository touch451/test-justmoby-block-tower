using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private ScrollPanel scrollPanel;

    public static GameManager Instance { get; private set; }

    public EventsSystem events = new EventsSystem();
    public LevelConfig LevelConfig => levelConfig;

    private void Awake()
    {
        SetInstance();
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
}
