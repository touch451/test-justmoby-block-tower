using UnityEngine;
using UnityEngine.UI;

public class TabletCanvasMatcher : MonoBehaviour
{
    [SerializeField] private float tabletMatchValue = 0;

    private void Start()
    {
        if (!DeviceUtils.IsTablet(out TabletType tabletType))
            return;

        if (!TryGetComponent(out CanvasScaler scaler))
        {
            Debug.LogError("Couldn't get Canvas Scaler component! Obj name - " + name);
            return;
        }

        scaler.matchWidthOrHeight = tabletMatchValue;
    }
}
