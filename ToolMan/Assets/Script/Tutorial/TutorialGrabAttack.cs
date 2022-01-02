using UnityEngine;
using System.Collections;
using ToolMan.Player;

public class TutorialGrabAttack : TutorialController
{
    [SerializeField] private PlayerController _player1;
    [SerializeField] private PlayerController _player2;
    [SerializeField] private GameObject HPCrystal;

    private bool crystalDie = false;

    private void Update()
    {
        if (!crystalDie && HPCrystal == null)
        {
            crystalDie = true;
        }
    }
    
    protected override IEnumerator TutorialProcess()
    {
        uIController.SetBattleUI(true);
        HPCrystal.SetActive(true);
        _player1.controlEnable = true;
        _player2.controlEnable = true;
        while (!crystalDie)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        _player1.controlEnable = false;
        _player2.controlEnable = false;
        uIController.SetBattleUI(false);
    }
}
