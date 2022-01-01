using UnityEngine;
using ToolMan.UI;

public class UIController : MonoBehaviour
{
    public bool showingBattleUI;
    public bool showingTutorialUI;

    [SerializeField]
    private GameObject battleUI;
    [SerializeField]
    private GameObject tutorialUI;
    [SerializeField]
    private HintPanel _hintPanel;
    public HintPanel hintPanel { get => _hintPanel; }

    [SerializeField]
    private PlayerController p1;
    [SerializeField]
    private PlayerController p2;

    private void Start()
    {
        SetBattleUI(false);
        SetTutorialUI(false);
    }

    public void SetBattleUI(bool val) {
        battleUI.SetActive(val);
        showingBattleUI = val;
    }

    public void SetTutorialUI(bool val)
    {
        tutorialUI.SetActive(val);
        p1.controlEnable = !val;
        p2.controlEnable = !val;
        showingTutorialUI = val;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Hint"))
        {
            SetTutorialUI(!showingTutorialUI);
        }
    }
}
