using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonController : MonoBehaviour {

    public float movementSpeed;

    public float moveSpeedX;
    public float moveSpeedZ;

    public float value; // the value that represents the person, will be the same value as their scale

    private SpriteRenderer sprite;

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        moveSpeedX = Random.Range(-4f, 4f);
        moveSpeedZ = Random.Range(-4f, 4f);
    }
}
