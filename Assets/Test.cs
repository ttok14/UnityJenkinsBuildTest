using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Networking;

public struct Data
{
    public string Name;
    public string ImageURL;
}

public class Test : MonoBehaviour
{
    [SerializeField]
    Text txtName;
    [SerializeField]
    RawImage img;
    [SerializeField]
    Slider proressSlider;

    string jsonURL = "https://drive.google.com/uc?export=download&id=1KHd45B4urUS1HzPlS8PWUHUC9t0egz1l";

    // Start is called before the first frame update
    IEnumerator Start()
    {
        UnityWebRequest req = UnityWebRequest.Get(jsonURL);

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("Error : " + req.error);
        }
        else
        {
            Data data = JsonUtility.FromJson<Data>(req.downloadHandler.text);

            txtName.text = data.Name;

            req.Dispose();

            req = UnityWebRequestTexture.GetTexture(data.ImageURL);
            var asyncOp = req.SendWebRequest();

            while (asyncOp.isDone == false)
            {
                Debug.LogError("Progress : " + asyncOp.progress);
                proressSlider.value = asyncOp.progress;
                yield return null;
            }

            if (req.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(req.error);
            }
            else
            {
                img.texture = ((DownloadHandlerTexture)req.downloadHandler).texture;
                req.Dispose();
            }
        }

        yield break;

        var handle = Addressables.InitializeAsync();
        yield return handle;

        var locators = Addressables.ResourceLocators;

        foreach (var item in locators)
        {
            Debug.LogError("Locator ID : " + item.LocatorId);

            foreach (var item2 in item.Keys)
            {
                Debug.LogError("key : " + item2);
            }
        }

        Addressables.GetDownloadSizeAsync("Main").Completed +=
            (AsyncOperationHandle<long> sizeHandle) =>
            {
                Debug.LogError("Download Size : " + sizeHandle.Result);
                Addressables.Release(sizeHandle);
            };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
