using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartApp : MonoBehaviour
{
    public Image processDownload;
    public Text descriptionDownload;

    [HideInInspector]
    public List<string> links;
    [HideInInspector]
    public List<string> linksName;

    void Start()
    {
        Button startMain = transform.Find("Panel/Button").GetComponent<Button>();

        //Up date
        UploadInfo();

        //Downloads
        CheckDownloads();
    }

    IEnumerator StartVR(string newDevice)
    {
        UnityEngine.XR.XRSettings.LoadDeviceByName(newDevice);
        yield return null;
        UnityEngine.XR.XRSettings.enabled = true;
        SceneManager.LoadScene("Main");
    }

    void StartVRScene()
    {
        StartCoroutine(StartVR("Cardboard"));
    }

    void CheckDownloads()
    {
        List<string> linksTemp = new List<string>();
        List<string> linksNameTemp = new List<string>();

        for (int i = 0; i < linksName.Count; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/" + linksName[i]))
            {
                linksTemp.Add(links[i]);
                linksNameTemp.Add(linksName[i]);
            }
        }
        for (int i = 0; i < linksTemp.Count; i++)
        {
            links.Remove(linksTemp[i]);
            linksName.Remove(linksNameTemp[i]);
        }

        if (links.Count == 0)
        {
            StartVRScene();
        }
        else
        {
            StartCoroutine(DownloadFiles());
        }
    }

    IEnumerator DownloadFiles()
    {
        UnityWebRequest unityWebRequest;

        for (int i = 0; i < links.Count; i++)
        {
            unityWebRequest = UnityWebRequest.Get(links[i]);
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            StartCoroutine(ShowDownloadProgress(unityWebRequest, i + 1, links.Count));
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                Debug.Log(unityWebRequest.error);
                yield return null;
            }
            else
            {
                // Show results as text
                Debug.Log(unityWebRequest.downloadHandler.text);

                // Or retrieve results as binary data
                byte[] results = unityWebRequest.downloadHandler.data;

                string fullPath = Application.persistentDataPath + "/" + linksName[i];
                File.WriteAllBytes(fullPath, results);
            }
        }
        StartVRScene();
    }

    IEnumerator ShowDownloadProgress(UnityWebRequest unityWebRequest, int currentList, int sizeList)
    {
        while (!unityWebRequest.isDone)
        {
            processDownload.fillAmount = unityWebRequest.downloadProgress;
            descriptionDownload.text = "Descangando " + currentList + " de " + sizeList
                + "\n" + string.Format("{0:P1}", unityWebRequest.downloadProgress);
            yield return new WaitForSeconds(.1f);
        }
        Debug.Log("Done");
    }

    public void UploadInfo()
    {
        links = new List<string>();
        linksName = new List<string>();

        links.Add("https://villaelisahb.com/vr/images/fachadaVillaElisa.jpg");
        links.Add("https://villaelisahb.com/vr/images/garaje.jpg");
        links.Add("https://villaelisahb.com/vr/images/entradaRecepcion.jpg");

        linksName.Add("fachadaVillaElisa.jpg");
        linksName.Add("garaje.jpg");
        linksName.Add("entradaRecepcion.jpg");
    }
}