using UnityEngine;

public static class DeviceUtils
{
    public static bool IsTablet(out TabletType tabletType)
    {
        tabletType = TabletType.None;
        bool isLandscape = Screen.width > Screen.height;

        float aspectRatio = isLandscape
            ? Screen.height / (1f * Screen.width)
            : Screen.width / (1f * Screen.height);

        if (aspectRatio > 0.58f && aspectRatio < 0.65f)
        {
            tabletType = TabletType.Slim;
            return true;
        }

        if (Screen.width / (1f * Screen.height) >= 0.65f)
        {
            tabletType = TabletType.Wide;
            return true;
        }

        return false;
    }
}

public enum DeviceType
{
    Phone,
    Tablet
}

public enum TabletType
{
    None,
    Slim,
    Wide
}
