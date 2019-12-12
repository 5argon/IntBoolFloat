using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Entities.Hybrid;

public class WorkObjectWithoutUpdate : MonoBehaviour, IConvertGameObjectToEntity 
{
    //`public` for hybrid system to modify.
    public int integer;
    public bool boolean;
    public float floating;
    public int timesUpdated;

    /// <summary>
    /// For the manager to call.
    /// </summary>
    public void ManualUpdate()
    {
        integer += Mathf.RoundToInt((Random.value * 100));
        boolean = !boolean;
        floating += Random.value * 100;
        timesUpdated++;
    }

    /// <summary>
    /// For the smarter system.
    /// </summary>
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent<WorkObjectData>(entity);
    }
}