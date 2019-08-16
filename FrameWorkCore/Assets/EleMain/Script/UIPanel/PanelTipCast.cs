using UnityEngine;
using System.Collections;

public class PanelTipCast : MonoBehaviour {

    private float _timer = 1.5f;

    private void OnEnable()
    {
        _timer = 1.5f;
    }


    // Update is called once per frame
    void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                UIManager.Instance().HidePanel(UIManager.PanelType.PanelTipCast);
            }
        }


    }
}
