using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

public class PanelQuit : MonoBehaviour {
    public Button BtnQuit;
    public Button BtnCancel;
    private void Awake()
    {
        BtnQuit.onClick.AddListener(Quit);
        BtnCancel.onClick.AddListener(() => UIManager.Instance().HidePanel(UIManager.PanelType.PanelQuit));
    }

    private void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; 
#else
        Application.Quit();
#endif

    }
}
