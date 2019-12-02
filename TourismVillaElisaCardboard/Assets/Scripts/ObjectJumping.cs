using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ObjectJumping : MainElement
{
    public Vector3 rotate;
    public bool activePoint = true;
    private Renderer myRenderer;

    private Image pointerLoading;

    private Coroutine transport;
    private GameObject point;

    public Material inactiveMaterial;
    public Material gazedAtMaterial;

    public void OnEnter(GameObject point)
    {
        this.point = point;
        GameObject thisObject = null;
        for (int i = 0; i < this.gameObject.transform.childCount; i ++)
        {
            if (this.gameObject.transform.GetChild(i).GetChild(0).name == point.name)
            {
                thisObject = this.gameObject.transform.GetChild(i).GetChild(0).gameObject;
            }
        }

        Debug.Log(thisObject.name);
        pointerLoading = thisObject.transform.Find("canvasLoading").GetChild(0).GetComponent<Image>();

        if (thisObject.transform.childCount > 0)
            thisObject.transform.GetChild(0).gameObject.SetActive(true);

        myRenderer = thisObject.GetComponent<Renderer>();
        myRenderer.material = gazedAtMaterial;
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/Touch"), mainApp.mainController.parentCamera.transform.position, 0.75f);

        transport = StartCoroutine(TransportCameraTime(point));
    }

    public void OnExit()
    {
        if (transport != null)
            StopCoroutine(transport);
        pointerLoading.fillAmount = 0;

        GameObject thisObject = null;
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if (this.gameObject.transform.GetChild(i).GetChild(0).name == point.name)
                thisObject = this.gameObject.transform.GetChild(i).GetChild(0).gameObject;
        }

        myRenderer = thisObject.GetComponent<Renderer>();
        myRenderer.material = inactiveMaterial;

        if (thisObject.transform.childCount > 0)
            thisObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    private IEnumerator TransportCameraTime(GameObject point)
    {
        float timeLoading = 1.5f;
        float timeCurrent = 0;
        while (timeCurrent <= timeLoading)
        {
            pointerLoading.fillAmount = timeCurrent / timeLoading;
            timeCurrent += Time.deltaTime;
            yield return null;
        }
        TransportCamera(point);
    }

    public void TransportCamera(GameObject point)
    {
        if (activePoint)
        {
            activePoint = false;
            Debug.Log("Algo pasa");
            StopCoroutine(transport);
            pointerLoading.fillAmount = 0;
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/TouchDown"), mainApp.mainController.parentCamera.transform.position);
            StartCoroutine(ChangePoint(point));
        }
    }

    IEnumerator ChangePoint(GameObject point)
    {
        mainApp.mainController.parentCamera.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        mainApp.mainController.ChangeTexture(point, gameObject);

        yield return null;

        //while (true)
        //{
        //    mainApp.mainController.parentCamera.transform.position = Vector3.Lerp(
        //        mainApp.mainController.parentCamera.transform.position,
        //        new Vector3(transform.position.x, 0, transform.position.z
        //        ), Time.deltaTime * 2);
        //    if (Mathf.RoundToInt(mainApp.mainController.parentCamera.transform.position.x) == transform.position.x &&
        //        Mathf.RoundToInt(mainApp.mainController.parentCamera.transform.position.z) == transform.position.z)
        //    {
        //        mainApp.mainController.ChangeTexture(this.gameObject);
        //        break;
        //    }
        //    yield return new WaitForSeconds(Time.deltaTime);
        //}
    }
}
