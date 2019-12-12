using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Collections;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.Entities;

namespace Tests
{
    public class HybridPerformanceTest
    {
        const int amount = 250000;

        [UnityTest,Performance]
        public IEnumerator A()
        {
            GameObject go = null;
            for (int i = 0; i < amount; i++)
            {
                go = new GameObject("test", typeof(WorkObject));
            }
            yield return Measure.Frames().WarmupCount(10).MeasurementCount(100).Run();
            Assert.That(go.GetComponent<WorkObject>().timesUpdated, Is.EqualTo(110));
        }
        
        [UnityTest,Performance]
        public IEnumerator B()
        {
            var wos = new WorkObjectWithoutUpdate[amount]; 
            for (int i = 0; i < amount; i++)
            {
                var go = new GameObject("test");
                var component = go.AddComponent<WorkObjectWithoutUpdate>();
                wos[i] = component;
            }

            var manager = new GameObject("manager", typeof(WorkObjectManager));
            manager.GetComponent<WorkObjectManager>().workObjects = wos;
            
            yield return Measure.Frames().WarmupCount(10).MeasurementCount(100).Run();
            Assert.That(wos[amount - 1].GetComponent<WorkObjectWithoutUpdate>().timesUpdated, Is.EqualTo(110));
        }
        
        [UnityTest,Performance]
        public IEnumerator C()
        {
            GameObject go = null;
            for (int i = 0; i < amount; i++)
            {
                go = new GameObject("test", typeof(WorkObjectWithoutUpdate));
                var cte = go.AddComponent<ConvertToEntity>();
                cte.ConversionMode = ConvertToEntity.Mode.ConvertAndInjectGameObject;
            }

            DefaultWorldInitialization.AddSystemsToRootLevelSystemGroups(
                World.DefaultGameObjectInjectionWorld,
                new List<Type> {typeof(WorkObjectSystem)});
            
            yield return Measure.Frames().WarmupCount(10).MeasurementCount(100).Run();
            Assert.That(go.GetComponent<WorkObjectWithoutUpdate>().timesUpdated, Is.EqualTo(110-1));
        }
        
        [UnityTest,Performance]
        public IEnumerator D()
        {
            GameObject go = null;
            for (int i = 0; i < amount; i++)
            {
                go = new GameObject("test", typeof(WorkObjectWithoutUpdate));
                var cte = go.AddComponent<ConvertToEntity>();
                cte.ConversionMode = ConvertToEntity.Mode.ConvertAndInjectGameObject;
            }

            DefaultWorldInitialization.AddSystemsToRootLevelSystemGroups(
                World.DefaultGameObjectInjectionWorld,
                new List<Type> {typeof(WorkObjectSmarterSystem)}); // <-------
            
            yield return Measure.Frames().WarmupCount(10).MeasurementCount(100).Run();
            Assert.That(go.GetComponent<WorkObjectWithoutUpdate>().timesUpdated, Is.EqualTo(110-1));
        }
        
        [UnityTest,Performance]
        public IEnumerator E()
        {
            GameObject go = null;
            for (int i = 0; i < amount; i++)
            {
                go = new GameObject("test", typeof(WorkObjectWithoutUpdate));
                var cte = go.AddComponent<ConvertToEntity>();
                cte.ConversionMode = ConvertToEntity.Mode.ConvertAndInjectGameObject;
            }

            DefaultWorldInitialization.AddSystemsToRootLevelSystemGroups(
                World.DefaultGameObjectInjectionWorld,
                new List<Type> {typeof(WorkObjectSmarterNoCopySystem)}); // <-------
            
            yield return Measure.Frames().WarmupCount(10).MeasurementCount(100).Run();
            
            var cda = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType
                .ReadOnly<WorkObjectData>()).ToComponentDataArray<WorkObjectData>(Allocator.TempJob);
            Assert.That( cda[0].timesUpdated, Is.EqualTo(110-1));
            
            cda.Dispose();
        }
    }
}
