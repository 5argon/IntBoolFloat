using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

[DisableAutoCreation]
public class WorkObjectSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities.ForEach((WorkObjectWithoutUpdate wo) =>
        {
            wo.integer += Mathf.RoundToInt((Random.value * 100));
            wo.boolean = !wo.boolean;
            wo.floating += Random.value * 100;
            wo.timesUpdated++;
        }).WithoutBurst().Run();
        return default;
    }
}