using UnityEngine;
using System.Collections;
using Assets.Scripts.Water;

public class BearManWaterMatSwitcher : WaterMaterialSwitcher
{
    private bool _inWater;
    public bool InWater
    {
        get => _inWater;
    }

    private MaterialPropertyBlock _waterPropertyBlock;

    public override void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Water")
        {
            _inWater = true;
            if (renderer.sharedMaterial != diffuseMaterial) return;
            _waterPropertyBlock = collider.GetComponent<WaterArea>().WaterPropertyBlock;

            renderer.sharedMaterial = waterMaterial;
            renderer.SetPropertyBlock(_waterPropertyBlock);
        }
    }

    public override void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Water")
        {
            _inWater = false;
            if (renderer.sharedMaterial != waterMaterial) return;
            renderer.sharedMaterial = diffuseMaterial;
            renderer.SetPropertyBlock(defaulPropertyBlock);
        }
    }

    public void SetWaterMat()
    {
        renderer.sharedMaterial = waterMaterial;
        renderer.SetPropertyBlock(_waterPropertyBlock);
    }
}
