using UnityEngine;
using System.Collections;
using System;

public class ProgramStart : MonoBehaviour {

    public Transform UIRoot;
    private float _timer;
	void Start () {
        UIManager.Instance().ShowPanel(UIManager.PanelType.PanelTopBar);
        UIManager.Instance().ShowPanel(UIManager.PanelType.PanelShowDes);
        EventBase.Instance().eventTest += ProgramStart_eventTest;
        EventBase.Instance().DoDelTest = DoDelTest;
        EventBase.Instance().eventTestTime += EventTestTime;

        //ConfigEleObjs tab = ConfigEleObjs.TryGet("1");
        // ConfigEleObjs tab1 = ConfigEleObjs.TryGet("1");
        // ConfigEleObjs tab2 = ConfigEleObjs.TryGet("1");
        ConfigManager.Instance().LoadTxt();

        //ConfigStep config = (ConfigStep)ConfigManager.Instance().GetConfig(ConfigType.ConfigStep);
        //Debug.LogFormat("id:{0} dragName:{1} castName:{2}",config[2].id, config[2].dragName, config[2].castName);
        //ConfigStepDesc configDesc = (ConfigStepDesc)ConfigManager.Instance().GetConfig(ConfigType.ConfigStepDesc);
        //Debug.LogFormat("id:{0} desc:{1}", configDesc[2].id, configDesc[2].Desc);
    }



    private float EventTestTime(string str)
    {
        return Time.time;
    }

    private float DoDelTest()
    {
        return Time.time;
    }

    private void ProgramStart_eventTest(GameObject go, string str)
    {
        Debug.LogWarning(go.name + "/" + str);
        var result = EventBase.Instance().DoDelTest();
        //if (result)
            Debug.Log(result);
    }

    void Update () {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;

        }
        else
        {
            _timer = 5f;
            //EventBase.Instance().DoEvent(gameObject, Time.time.ToString());
            EventBase.Instance().DoEventTestTime(Time.time.ToString());
        }
    }
    private void OnDestroy()
    {
        EventBase.Instance().DoDelTest -= DoDelTest;
        EventBase.Instance().eventTest -= ProgramStart_eventTest;
        EventBase.Instance().eventTestTime -= EventTestTime;
       
    }
}
