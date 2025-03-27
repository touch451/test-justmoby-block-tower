using UnityEngine;

public class SplashSpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem splashPrefab;

    public static SplashSpawner Instance;

    private void Awake()
    {
        SetInstance();
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

    public void Spawn(BlockColor blockColor, Vector3 blockPosition)
    {
        Color32 color32 = ScriptTools.ConvertBlockColorToColor32(blockColor);
        Vector3 position = blockPosition += Vector3.back;

        var splash = Instantiate(splashPrefab, position, Quaternion.identity);

        var main = splash.main;
        main.startColor = (Color)color32;

        splash.Play();
    }
}
