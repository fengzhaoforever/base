using UnityEngine;
using UnityEngine.UI;

public class PanelShowDes : MonoBehaviour
{

    public Button BtnClose;
    public Button BtnNext;
    public Button BtnLast;
    public Image TargetIma;
    private int mIndex;
    private Sprite curSp;
    private const int maxIndex = 9;

    private Sprite[] spArr = new Sprite[10];
    private void Start()
    {
        BtnNext.onClick.AddListener(OnClickNext);
        BtnLast.onClick.AddListener(OnClickLast);
        BtnClose.onClick.AddListener(() =>
        {
            UIManager.Instance().HidePanel(UIManager.PanelType.PanelShowDes);
            if (StepManager.Instance().GameState == StepManager.StepState.None)
            {
                EventBase.Instance().DoEleIntroduceMove();
            }
        });
        mIndex = 0;
        ShowTargetImage();
    }
    
    private void OnClickNext()
    {
        mIndex++;
        if (mIndex > 9)
            mIndex = 9;
        ShowTargetImage();
    }
    private void OnClickLast()
    {
        mIndex--;
        if (mIndex < 0)
            mIndex = 0;
        ShowTargetImage();
    }
    private void ShowTargetImage()
    {
        if (spArr[mIndex] == null)
        {
            //根据mIndex加载对应的图片
            Object tar = Resources.Load("UIsprite/UIIntroduce/"+mIndex.ToString(),typeof(Sprite));
            if (tar != null)
            {
                curSp = tar as Sprite;
                spArr[mIndex] = curSp;
                //Resources.UnloadAsset(tar);
            }
        }
        if (spArr[mIndex] != null)
            TargetIma.sprite = spArr[mIndex];
    }

  
   
}
