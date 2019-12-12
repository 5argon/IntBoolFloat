using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

[DisableAutoCreation]
public class WorkObjectSmarterNoCopySystem : JobComponentSystem
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
        
        return default;
    }
}