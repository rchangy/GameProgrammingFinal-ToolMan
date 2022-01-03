namespace Assets.Scripts.Water
{
    using UnityEngine;

    /// <summary>
    /// This class switchs material of dynamic objects if they enter or exit any water area
    /// After switching to water material it pushes water area properties to dynamic object material
    /// It allows objects to be under the lake or be in the different water
    /// Also switching material to diffuse after exiting the water gives a bit performance
    /// </summary>
    public class WaterMaterialSwitcher : MonoBehaviour
    {
        [SerializeField] protected Renderer renderer;
        [SerializeField] protected Material waterMaterial;
        [SerializeField] protected Material diffuseMaterial;

        protected MaterialPropertyBlock defaulPropertyBlock;

        public void Awake()
        {
            defaulPropertyBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(defaulPropertyBlock);
        }

        public virtual void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Water")
            {
                var waterPropertyBlock = collider.GetComponent<WaterArea>().WaterPropertyBlock;

                renderer.sharedMaterial = waterMaterial;
                renderer.SetPropertyBlock(waterPropertyBlock);
            }
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            if (collider.tag == "Water")
            {
                renderer.sharedMaterial = diffuseMaterial;
                renderer.SetPropertyBlock(defaulPropertyBlock);
            }
        }
    }
}
