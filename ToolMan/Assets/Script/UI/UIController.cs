using UnityEngine;
using ToolMan.UI;

public class UIController : MonoBehaviour
{
    public bool showingBattleUI;
    public bool showingTutorialUI;
    public bool showingNotificationUI;

    public HintNotRead hintNotRead;

    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject battleUI;
    [SerializeField]
    private GameObject hintPanelObj;
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
    public bool controlEnalbe { get => _controlEnable; }

    private float originalTimeScale;

    private void Awake()
    {
        originalTimeScale = Time.timeScale;
    }
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
        hintPanelObj.SetActive(val);
        showingTutorialUI = val;
        Objective o = gameManager.currentObjective;
        if (o) {
            Cutscene c = o.GetComponent<Cutscene>();
            if (c) c._skipEnable = !val;
        }
        
        if (val)
        {
            //Debug.Log("true, time = " + originalTimeScale);
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = originalTimeScale;
        }
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
