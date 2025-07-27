using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectDatabaseSO", menuName = "Scriptable Objects/ObjectDB")]
public class ObjectDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectData;
}

[Serializable]
public class ObjectData
{
    // [field: SerializeField]
    // public int MyProperty { get; set; }
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public Vector2Int Size { get; set; } = Vector2Int.one;
    [field: SerializeField]
    public int Amound { get; private set; } = 1;
    [field: SerializeField]
    public GameObject Prefab { get; private set; }

}