using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectJumping : MainElement
{
    public Vector3 rotate;
    private bool activePoint = true;
    private Renderer myRenderer;

    public Material inactiveMaterial;
    public Material gazedAtMaterial;

    public void OnEnter(GameObject thisObject)
    {
        myRenderer = thisObject.GetComponent<Renderer>();
        myRenderer.material = gazedAtMaterial;
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/Touch"), mainApp.mainController.parentCamera.transform.position, 0.5f);
    }

    public void OnExit(GameObject thisObject)
    {
        myRenderer = thisObject.GetComponent<Renderer>();
        myRenderer.material = inactiveMaterial;
    }

    public void TransportCamera(GameObject point)
    {
        if (activePoint)
        {
            activePoint = false;
            point.SetActive(true);
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/TouchDown"), mainApp.mainController.parentCamera.transform.position);
            StartCoroutine(ChangePoint(point));
        }
    }

    IEnumerator ChangePoint(GameObject point)
    {
        mainApp.mainController.parentCamera.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        mainApp.mainController.ChangeTexture(point, gameObject);
        activePoint = true;

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
