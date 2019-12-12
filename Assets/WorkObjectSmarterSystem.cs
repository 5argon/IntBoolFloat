using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[DisableAutoCreation]
public class WorkObjectSmarterSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var random = new Unity.Mathematics.Random(1234);
        Entities.ForEach((ref WorkObjectData wod) =>
        {
            wod.integer += (int) (math.round((random.NextFloat() * 100)));
            wod.boolean = !wod.boolean;
            wod.floating += random.NextFloat() * 100;
            wod.timesUpdated++;
        }).Schedule(inputDeps).Complete();
        
        Entities.ForEach((WorkObjectWithoutUpdate wo, in WorkObjectData wod) =>
        {
            wo.integer = wod.integer;
            wo.boolean = wod.boolean;
            wo.floating = wod.floating;
            wo.timesUpdated = wod.timesUpdated;
        }).WithoutBurst().Run();
        
        return default;
    }
}