using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ScriptableVariables/ScriptableFloat")]
public class ScriptableFloat : ScriptableObject, ISerializationCallbackReceiver
{
    [HideInInspector]
    public float RuntimeValue;

    [SerializeField]
    private float initialValue;

    public float DefaultValue { get => initialValue; }

    public void OnAfterDeserialize()
    {
        RuntimeValue = initialValue;
    }

    public void OnBeforeSerialize()
    {

    }
}
