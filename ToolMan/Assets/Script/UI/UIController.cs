using UnityEngine;
using ToolMan.UI;

public class UIController : MonoBehaviour
{
    public bool showingBattleUI;
    public bool showingTutorialUI;
    public bool showingNotificationUI;

    [SerializeField]
    private GameObject battleUI;
    [SerializeField]
    private GameObject hintUI;
    [SerializeField]
    private HintPanel _hintPanel;
    public HintPanel hintPanel { get => _hintPanel; }
    [SerializeField]
    private GameObject notificationUI;

    [SerializeField]
    private PlayerController p1;
    [SerializeField]
    private PlayerController p2;
    private bool _controlEnable;

    private void Start()
    {
        SetBattleUI(true);
        SetHintUI(false);
        SetNotificationUI(true);
        _controlEnable = false;
    }

    public void SetBattleUI(bool val) {
        battleUI.SetActive(val);
        showingBattleUI = val;
    }

    public void SetHintUI(bool val)
    {
        hintUI.SetActive(val);
        //p1.controlEnable = !val;
        //p2.controlEnable = !val;
        showingTutorialUI = val;
    }

    public void SetNotificationUI(bool val)
    {
        notificationUI.SetActive(val);
        showingNotificationUI = val;
    }

    private void Update()
    {
        if (_controlEnable && Input.GetButtonDown("Hint"))
        {
            SetHintUI(!showingTutorialUI);
        }
    }

    public void SetControlEnable(bool val) { _controlEnable = val; }
}
