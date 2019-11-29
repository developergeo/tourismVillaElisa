using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainElement : MonoBehaviour
{
    public MainApplication mainApp { get { return GameObject.FindObjectOfType<MainApplication>(); } }
}

public class MainApplication : MonoBehaviour
{
    public MainModel mainModel;
    public MainView mainView;
    public MainController mainController;
}