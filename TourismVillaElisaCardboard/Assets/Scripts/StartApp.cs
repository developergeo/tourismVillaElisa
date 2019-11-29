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

    void Awake()
    {
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

        links.Add("https://villaelisahb.com/vr/images/balconHabitacionVip2p.jpg");
        links.Add("https://villaelisahb.com/vr/images/banoHabitacionVip.jpg");
        links.Add("https://villaelisahb.com/vr/images/banoHabitacionVip2p.jpg");
        links.Add("https://villaelisahb.com/vr/images/entradaEdificio.jpg");
        links.Add("https://villaelisahb.com/vr/images/entradaRecepcion.jpg");
        links.Add("https://villaelisahb.com/vr/images/fachadaVillaElisa.jpg");
        links.Add("https://villaelisahb.com/vr/images/garaje.jpg");
        links.Add("https://villaelisahb.com/vr/images/habitacionDoble.jpg");
        links.Add("https://villaelisahb.com/vr/images/habitacionMatrimonialVip.jpg");
        links.Add("https://villaelisahb.com/vr/images/habitacionSimple.jpg");
        links.Add("https://villaelisahb.com/vr/images/habitacionVip2p.jpg");
        links.Add("https://villaelisahb.com/vr/images/jardin_1.jpg");
        links.Add("https://villaelisahb.com/vr/images/jardin_2.jpg");
        links.Add("https://villaelisahb.com/vr/images/jardin_3.jpg");
        links.Add("https://villaelisahb.com/vr/images/jardin_4.jpg");
        links.Add("https://villaelisahb.com/vr/images/pasillo.jpg");
        links.Add("https://villaelisahb.com/vr/images/pasillopiso2.jpg");
        links.Add("https://villaelisahb.com/vr/images/recepcion.jpg");
        links.Add("https://villaelisahb.com/vr/images/salaConferencia.jpg");
        links.Add("https://villaelisahb.com/vr/images/restaurante_1.jpg");
        links.Add("https://villaelisahb.com/vr/images/restaurante_2.jpg");
        links.Add("https://villaelisahb.com/vr/images/salaGrande.jpg");
        links.Add("https://villaelisahb.com/vr/images/salaPequena.jpg");
        links.Add("https://villaelisahb.com/vr/images/salidaJardin.jpg");
        links.Add("https://villaelisahb.com/vr/images/pisina_1.jpg");
        links.Add("https://villaelisahb.com/vr/images/vistageneral.jpg");



        linksName.Add("balconHabitacionVip2p.jpg");
        linksName.Add("banoHabitacionVip.jpg");
        linksName.Add("banoHabitacionVip2p.jpg");
        linksName.Add("entradaEdificio.jpg");
        linksName.Add("entradaRecepcion.jpg");
        linksName.Add("fachadaVillaElisa.jpg");
        linksName.Add("garaje.jpg");
        linksName.Add("habitacionDoble.jpg");
        linksName.Add("habitacionMatrimonialVip.jpg");
        linksName.Add("habitacionSimple.jpg");
        linksName.Add("habitacionVip2p.jpg");
        linksName.Add("jardin_1.jpg");
        linksName.Add("jardin_2.jpg");
        linksName.Add("jardin_3.jpg");
        linksName.Add("jardin_4.jpg");
        linksName.Add("pasillo.jpg");
        linksName.Add("pasillopiso2.jpg");
        linksName.Add("recepcion.jpg");
        linksName.Add("salaConferencia.jpg");
        linksName.Add("restaurante_1.jpg");
        linksName.Add("restaurante_2.jpg");
        linksName.Add("salaGrande.jpg");
        linksName.Add("salaPequena.jpg");
        linksName.Add("salidaJardin.jpg");
        linksName.Add("pisina_1.jpg");
        linksName.Add("vistageneral.jpg");
    }
}