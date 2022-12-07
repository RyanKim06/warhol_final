using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace n_O2Gauge
{
    public class O2 : MonoBehaviour
    {
        [SerializeField]
        public static Slider O2Gaugebar;

        private static float maxO2Gauge = 100;
        public static float curO2Gauge = 100;
        private static float DownValue = 0.03f;

        public static bool hasShield = false;

        private void Awake()
        {
            O2Gaugebar = GameObject.Find("O2Slider").GetComponent<Slider>();
            O2Gaugebar.value = (float)curO2Gauge / (float)maxO2Gauge;
        }

        public static void MoveO2GaugeDown()
        {
            if(curO2Gauge > 0)
            {
                curO2Gauge -= DownValue;
            }
            else
            {
                curO2Gauge = 0;
            }
            O2Gaugebar.value = curO2Gauge / maxO2Gauge;
        }

        public static void DamageO2(float damagevalue)
        {
            if(hasShield) //방패 true
            {
                hasShield = false;
            }
            else //방패 false
            {
                curO2Gauge -= damagevalue;
                O2Gaugebar.value = curO2Gauge / maxO2Gauge;
            }
        }

        public static void IncreseO2(float increseValue)
        {
            curO2Gauge += increseValue;
            if(curO2Gauge > maxO2Gauge)
            {
                curO2Gauge = maxO2Gauge;
            }
            O2Gaugebar.value = curO2Gauge / maxO2Gauge;
        }
    }

}
