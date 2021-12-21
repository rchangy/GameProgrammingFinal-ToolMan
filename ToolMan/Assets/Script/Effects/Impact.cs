using UnityEngine;
using System.Collections;

public class Impact : Effect
{
    //public Transform contactPoint;
    public Transform PickaxeContactPoint;
    public Transform BoomerangContactPoint;
    public Transform LightSaberContactPoint;
    private string toolName;

    [SerializeField]
    private GameObject effectPrefab;

    public void setToolName(string name) { toolName = name; }

    public override void PlayEffect() {
        Vector3 pos = Vector3.zero;
        switch (toolName) {
            case "Pickaxe":
                pos = PickaxeContactPoint.position;
                break;
            case "Boomerang":
                pos = BoomerangContactPoint.position;
                break;
            case "LightSaber":
                pos = LightSaberContactPoint.position;
                break;
        }
        //transform.position = pos;
        Effect e = Instantiate(effectPrefab, pos, Quaternion.identity).GetComponent<Effect>();
        e.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DestroyEffect(e));
    }

    IEnumerator DestroyEffect(Effect e) {
        yield return new WaitForSeconds(2);
        Destroy(e.gameObject);
    }
}
