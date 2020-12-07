using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class SortingSystem : ComponentSystem {
    int index = 0;
    protected override void OnUpdate() {
        float dt = Time.DeltaTime;
        Entities.ForEach((ref SortableComponent sortableComponent, ref MoveSpeedComponent moveSpeedComponent) => {
            //check if at correct index and has not already been sorted
            if ((float)index == sortableComponent.value && !sortableComponent.sorted) {
                sortableComponent.sorted = true;
                Debug.Log("person at index " + index + " is being sorted");
            }
            //move on when in position
            else if ((float)index == sortableComponent.value && sortableComponent.inPosition) {
                index++;
            }

        });

    }
}
