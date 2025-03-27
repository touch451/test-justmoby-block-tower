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

    public static Color32 ConvertBlockColorToColor32(BlockColor blockColor)
    {
        switch (blockColor)
        {
            case BlockColor.Black: return new Color32(131, 131, 131, 255);
            case BlockColor.Blue_Light: return new Color32(31, 188, 255, 255);
            case BlockColor.Blue: return new Color32(103, 110, 235, 255);
            case BlockColor.Brown: return new Color32(202, 129, 89, 255);
            case BlockColor.Cyan_Light: return new Color32(155, 237, 249, 255);
            case BlockColor.Cyan: return new Color32(62, 220, 231, 255);
            case BlockColor.Gray: return new Color32(210, 210, 210, 255);
            case BlockColor.Green_Light: return new Color32(214, 230, 107, 255);
            case BlockColor.Green: return new Color32(58, 214, 68, 255);
            case BlockColor.Mauve: return new Color32(178, 159, 255, 255);
            case BlockColor.Orange: return new Color32(255, 146, 38, 255);
            case BlockColor.Pink: return new Color32(239, 130, 193, 255);
            case BlockColor.Plum: return new Color32(166, 60, 153, 255);
            case BlockColor.Purple_Light: return new Color32(236, 193, 248, 255);
            case BlockColor.Purple: return new Color32(215, 96, 215, 255);
            case BlockColor.Red_Dark: return new Color32(188, 28, 54, 255);
            case BlockColor.Red: return new Color32(250, 68, 77, 255);
            case BlockColor.Rosybrown: return new Color32(215, 100, 96, 255);
            case BlockColor.Yellow_Light: return new Color32(255, 246, 197, 255);
            case BlockColor.Yellow: return new Color32(255, 229, 83, 255);
            default:
                Debug.LogError($"Block color {blockColor} not converted to Color32.");
                return new Color32(255, 255, 255, 255);
        }
    }
}
