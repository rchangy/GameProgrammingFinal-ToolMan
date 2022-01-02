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
        HPCrystal.SetActive(true);
        while(!crystalDie)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
    }
}
