# Hybrid ECS performance test

Run the PlayMode Test to get result in the test log. You must run one by one as I didn't write a proper cleanup between tests. Inspired by [this thread](https://forum.unity.com/threads/my-hybrid-ecs-performance-test.792585/).

- A : 250000 game objects with `Update()`.
- B : Manager game object looping 250000 times in one frame to call `Update()` equivalent.
- C : One ECS system iterate on 250000 entities that was generated from `ConvertToEntity` with inject mode, and modify values directly to the `MonoBehaviour` reference like in `Update()` with `Entities.ForEach`. The modified values reflected on the `GameObject`'s inspector on the scene. No Burst, no leak detection.
- D : Like C but with a little more work. Added a dedicated `IComponentData` data storage on conversion for parallel calculation. Apply the calculated value by a copy. Burst on, no safety checks, in editor. No leak detection.
- E : Removed the copy back step from D. 

## Results

All tests run on MacBook Pro Early 2015 in editor. All units in milliseconds.

- A : Median:88.87 Min:83.32 Max:657.38 Avg:113.88 Std:77.03 Zeroes:0 SampleCount: 100 Sum: 11388.47
- B : Median:21.48 Min:18.86 Max:48.13 Avg:23.81 Std:6.50 Zeroes:0 SampleCount: 100 Sum: 2380.65
- C : Median:27.93 Min:23.71 Max:29.69 Avg:27.38 Std:1.46 Zeroes:0 SampleCount: 100 Sum: 2738.49
- D : Median:24.09 Min:23.31 Max:40.47 Avg:25.19 Std:2.54 Zeroes:0 SampleCount: 100 Sum: 2518.88
- E : Median:7.95 Min:6.32 Max:11.36 Avg:8.04 Std:0.57 Zeroes:0 SampleCount: 100 Sum: 804.17

## Evaluation

- `Update()` is expensive.
- There is a cost on using the query to assemble native array for `ForEach` from large amount of entities. Otherwise C must be around the same as B, and D should be a bit faster than B.
- You better design for a one-way hybrid approach. Let `GameObject` use the calculated result stored in ECS via `EntityManager`, instead of expensive setting back value to `MonoBehaviour` that is not as cache friendly.