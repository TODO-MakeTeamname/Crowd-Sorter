using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public int populationCount = 1000;
    public float crowdDensity = 1f;

    public Mesh _mesh;
    public Material _material;

    private void Start() {
        Time.timeScale = 100;

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(MoveSpeedComponent),
            typeof(Translation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld),
            typeof(Scale),
            typeof(SortableComponent)
            );

        NativeArray<Entity> entityArray = new NativeArray<Entity>(populationCount, Allocator.Temp);
        entityManager.CreateEntity(entityArchetype, entityArray);

        //Initialize component data
        for (int i = 0; i < entityArray.Length; i++) {
            Entity entity = entityArray[i];

            entityManager.SetComponentData(entity, new SortableComponent
            {
                value = i,
                sorted = false,
                inPosition = false
            });

            entityManager.SetComponentData(entity, new MoveSpeedComponent {
                moveSpeedX = Random.Range(-4f, 4f),
                moveSpeedZ = Random.Range(-4f, 4f)
            });

            entityManager.SetSharedComponentData(entity, new RenderMesh {
                mesh = _mesh,
                material = _material
            });

            entityManager.SetComponentData(entity, new Scale {
                Value = Random.Range(1f, 2f)
            });

            //Initialize positions
            float rowOffset = (Mathf.Sqrt(populationCount) / (2 * crowdDensity));
            float x = ((i % Mathf.Sqrt(populationCount)) / crowdDensity) - rowOffset;
            float z = (i / (Mathf.Sqrt(populationCount) * crowdDensity)) - rowOffset;
            entityManager.SetComponentData(entity, new Translation {
                Value = new Unity.Mathematics.float3(x, 0, z)
            });
        }
        entityArray.Dispose();


        //Set universal data
        UniversalData.populationCount = populationCount;
        UniversalData.crowdDensity = crowdDensity;
    }
}
