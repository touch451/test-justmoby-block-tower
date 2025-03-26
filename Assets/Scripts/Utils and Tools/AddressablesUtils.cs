using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AddressablesUtils
{
    public static IEnumerator CheckAddressableAssetDownloaded_Co(string addressableKey, Action<bool> onComplete)
    {
        AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(addressableKey);
        yield return getDownloadSize;

        if (getDownloadSize.Status == AsyncOperationStatus.Succeeded)
            onComplete?.Invoke(getDownloadSize.Result > 0);
    }

    public static IEnumerator LoadAddressableAsset_Co<T>(string key, Action<AsyncOperationHandle<T>> onLoaded = null, Action onFailed = null, Action<float> onUpdate = null)
    {
        AsyncOperationHandle<T> opHandle;
        opHandle = Addressables.LoadAssetAsync<T>(key);

        while (!opHandle.IsDone)
        {
            float percent = opHandle.GetDownloadStatus().Percent;
            onUpdate?.Invoke(percent);
            yield return null;
        }

        if (opHandle.Status == AsyncOperationStatus.Succeeded)
            onLoaded?.Invoke(opHandle);
        else
            onFailed?.Invoke();
    }

    public static void ReleaseAsset(AsyncOperationHandle handle)
    {
        if (handle.IsValid())
            Addressables.Release(handle);
    }

    public static void ReleaseAssets(List<AsyncOperationHandle> handles)
    {
        foreach (var handle in handles)
        {
            if (handle.IsValid())
                Addressables.Release(handle);
        }
    }
}