using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

public class PanelGameOver : MonoBehaviour {

    public Button BtnQuit;
    public Button BtnCancel;
    private void Awake()
    {
        BtnQuit.onClick.AddListener(Quit);
        BtnCancel.onClick.AddListener(() => UIManager.Instance().HidePanel(UIManager.PanelType.PanelGameOver));
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
