using UnityEngine;

public static class Pathes
{
    public static string GetAddressablesKeyToAsset<T>(T assetType)
    {
        switch (assetType)
        {
            case BlockColor:
                return $"Block/{assetType}";

            default:
                Debug.LogWarning("None addressables key for asset type: " + assetType.ToString());
                return "none-key";
        } 
    }
}
