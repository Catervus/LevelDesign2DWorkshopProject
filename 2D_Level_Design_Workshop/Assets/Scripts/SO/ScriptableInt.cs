using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ScriptableVariables/ScriptableInt")]
public class ScriptableInt : ScriptableObject, ISerializationCallbackReceiver
{
    [HideInInspector]
    public int RuntimeValue;

    [SerializeField]
    private int initialValue;

    public int DefaultValue { get => initialValue; }

    public void OnAfterDeserialize()
    {
        RuntimeValue = initialValue;
    }

    public void OnBeforeSerialize()
    {
        
    }
}
