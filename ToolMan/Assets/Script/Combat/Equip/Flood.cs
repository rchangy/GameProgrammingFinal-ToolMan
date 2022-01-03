using UnityEngine;
using System.Collections;
using Assets.Scripts.Water;
using ToolMan.Combat.Stats;

namespace ToolMan.Combat.Equip
{
    public class Flood : MonoBehaviour
    {
        [SerializeField] private WaterPropertyBlockSetter waterProperty;
        [SerializeField] private Transform _waterArea;
        [SerializeField] private float HeightSettingSpeed = 0.1f;
        [SerializeField] private float LowestHeight;
        [SerializeField] private float HighestHeight;
        [SerializeField] private float SpeedDownPercent = 0.5f;

        [SerializeField] private Plug[] plugs;
        private Plug _currentPlug;

        private StatModifier _statMod;

        private bool _HeightSetting;

        private void Start()
        {
            waterProperty.WaterHeight = LowestHeight;
            _waterArea.position = new Vector3(_waterArea.position.x, waterProperty.WaterHeight, _waterArea.position.z);
            _statMod = new StatModifier(-SpeedDownPercent, StatModType.PercentAdd);
            if(plugs != null && plugs.Length > 0)
            {
                foreach(Plug p in plugs)
                {
                    p.SetFlood(this);
                }
            }
        }

        public void StartFloodingForSeconds(float duration)
        {
            StartCoroutine(FloodingForSeconds(duration));
        }

        public IEnumerator FloodingForSeconds(float duration)
        {
            yield return SetWaterHeight(HighestHeight);
            yield return new WaitForSeconds(duration);
            yield return SetWaterHeight(LowestHeight);
        }

        public void StartFlooding()
        {
            if (_HeightSetting) return;
            if (plugs != null && plugs.Length > 0)
            {
                int plugIdx = Random.Range(0, plugs.Length);
                _currentPlug = plugs[plugIdx];
                _currentPlug.StartFlooding();
            }
            StartCoroutine(SetWaterHeight(HighestHeight));
        }

        public void StopFlooding()
        {
            if (_HeightSetting) return;
            if(_currentPlug != null)
            {
                _currentPlug.StopFlooding();
            }
            StartCoroutine(SetWaterHeight(LowestHeight));
        }

        private IEnumerator SetWaterHeight(float targetHeight)
        {
            _HeightSetting = true;
            float dist = targetHeight - waterProperty.WaterHeight;

            while (waterProperty.WaterHeight != targetHeight)
            {
                float newHeight = waterProperty.WaterHeight + dist * HeightSettingSpeed * Time.deltaTime;
                if ((newHeight > waterProperty.WaterHeight && newHeight > targetHeight) || (newHeight < waterProperty.WaterHeight && newHeight < targetHeight))
                {
                    newHeight = targetHeight;
                }
                waterProperty.WaterHeight = newHeight;
                _waterArea.position = new Vector3(_waterArea.position.x, newHeight, _waterArea.position.z);
                yield return null;
            }
            _HeightSetting = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerCombat playerCombat;
            if ((playerCombat = other.GetComponent<PlayerCombat>()) != null)
            {
                playerCombat.AddStatMod("SPD", _statMod);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            PlayerCombat playerCombat;
            if ((playerCombat = other.GetComponent<PlayerCombat>()) != null)
            {
                playerCombat.RemoveStatMod("SPD", _statMod);
            }
        }
    }
}