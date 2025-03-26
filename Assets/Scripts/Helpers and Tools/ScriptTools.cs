using UnityEngine;

public static class ScriptTools
{
    public static Vector3[] GetRectWorldCorners(RectTransform rect)
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        return corners;
    }

    public static Vector3 GetRectWorldOffsetMax(RectTransform rect)
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        return corners[2];
    }

    public static Vector3 GetRectWorldOffsetMin(RectTransform rect)
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        return corners[0];
    }
}
