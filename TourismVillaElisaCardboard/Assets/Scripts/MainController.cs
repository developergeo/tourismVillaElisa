using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.VR;

public class MainController : MainElement
{
    public GameObject parentCamera;

    public Image backoutImage;
    public float backoutOpacityChangeStep;
    private float blackTargetOpacity;

    public GameObject firstPoint;
    private Vector3 rotateOld = new Vector3 (0, 90, 0);

    [HideInInspector]
    public GameObject activeSelected;

    // Start is called before the first frame update
    void Start()
    {
        //RenderTexture backupRenderTexture = RenderTexture.active;
        //backupRenderTexture.DiscardContents();
        ChangeTexture(firstPoint, null);
        mainApp.mainController.parentCamera.transform.position = new Vector3(firstPoint.transform.position.x, 0, firstPoint.transform.position.z);
    }

    void FixedUpdate()
    {
        Blackout();
    }

    public void ChangeTexture(GameObject point, GameObject oldpoint)
    {
        StartCoroutine(AnimationTexture(point, oldpoint));
    }

    public IEnumerator AnimationTexture(GameObject point, GameObject oldpoint)
    {
        SetBlackoutOpacity(1);
        yield return new WaitUntil(() => backoutImage.color.a == 1);
        point.SetActive(true);
        UnityWebRequest wr = new UnityWebRequest("file://" + Application.persistentDataPath + "/" + point.gameObject.name + ".jpg");
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;
        yield return wr.SendWebRequest();
        if (!(wr.isNetworkError || wr.isHttpError))
        {
            Texture2D t = texDl.texture;
            mainApp.mainView.photoApartment.GetComponent<Renderer>().material.mainTexture = t;
        }

        //var fileName = Application.persistentDataPath + "/" + point.gameObject.name + ".jpg";
        //var bytes = File.ReadAllBytes(fileName);
        //var texture = new Texture2D(6080, 3040, TextureFormat.RGBA32, false);
        //texture.LoadImage(bytes);

        //mainApp.mainView.photoApartment.GetComponent<Renderer>().material.mainTexture = texture;
        //mainApp.mainView.photoApartment.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture2D>("Textures/" + point.gameObject.name); ;
        mainApp.mainView.photoApartment.transform.position =
            new Vector3(point.transform.position.x, mainApp.mainView.photoApartment.transform.position.y, point.transform.position.z);
        mainApp.mainController.parentCamera.transform.position = new Vector3(point.transform.position.x, 0, point.transform.position.z);
        //Rotate
        mainApp.mainView.photoApartment.transform.Rotate(-rotateOld);
        mainApp.mainView.photoApartment.transform.Rotate(point.GetComponent<ObjectJumping>().rotate);
        rotateOld = point.GetComponent<ObjectJumping>().rotate;

        if (oldpoint != null)
        {
            oldpoint.gameObject.SetActive(false);
            oldpoint.GetComponent<ObjectJumping>().activePoint = true;
        }

        SetBlackoutOpacity(0);

        //Get points
        //for (int i = 0; i < mainApp.mainModel.points.Count; i++)
        //{
        //    mainApp.mainModel.points[i].gameObject.SetActive(false);
        //}
        //for (int i = 0; i < point.GetComponent<ObjectJumping>().neighbors.Count; i++)
        //{
        //    point.GetComponent<ObjectJumping>().neighbors[i].gameObject.SetActive(true);
        //}
        //point.transform.gameObject.SetActive(false);
    }

    private RenderTexture CreateNewRenderTexture(int width, int height)
    {
        RenderTexture renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        renderTexture.filterMode = FilterMode.Point;
        renderTexture.DiscardContents();

        return renderTexture;
    }

    private void Blackout()
    {
        float currentOpacity = backoutImage.color.a;

        if (currentOpacity < blackTargetOpacity)
        {
            backoutImage.color = 
                new Color(backoutImage.color.r, backoutImage.color.g, backoutImage.color.b, backoutImage.color.a + backoutOpacityChangeStep);
            if (backoutImage.color.a > blackTargetOpacity)
            {
                backoutImage.color = new Color(backoutImage.color.r, backoutImage.color.g, backoutImage.color.b, blackTargetOpacity);
            }
        } else if (currentOpacity > blackTargetOpacity)
        {
            backoutImage.color =
                new Color(backoutImage.color.r, backoutImage.color.g, backoutImage.color.b, backoutImage.color.a - backoutOpacityChangeStep);
            if (backoutImage.color.a < blackTargetOpacity)
            {
                backoutImage.color = new Color(backoutImage.color.r, backoutImage.color.g, backoutImage.color.b, blackTargetOpacity);
            }
        }
    }

    public void SetBlackoutOpacity(float o)
    {
        blackTargetOpacity = o;
    }

}