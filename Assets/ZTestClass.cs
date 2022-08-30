using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZTestClass : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        public string name;
        public bool use;
        public int value01;
        public int value02;
        public float value03;
        public float value04;
    }

    public List<Data> allData;
}
