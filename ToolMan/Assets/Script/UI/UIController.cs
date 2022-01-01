using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject battleUI;
    [SerializeField]
    GameObject tutorialUI;

    public void SetBattleUI(bool val) {
        battleUI.SetActive(val);
    }

    public void SetTutorialUI(bool val)
    {
        tutorialUI.SetActive(val);
    }

}
