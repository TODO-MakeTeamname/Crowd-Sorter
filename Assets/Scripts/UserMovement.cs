using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovement : MonoBehaviour
{
    private bool bUsingMouse = true;

    //Translation
    [SerializeField] float defaultSpeed = 1f;
    [SerializeField] float sprintMultiplier = 5f;
    private float moveSpeed = 1f;
    [SerializeField] float minX = -320;
    [SerializeField] float maxX = 320;
    [SerializeField] float minY = 0;
    [SerializeField] float maxY = 50;
    [SerializeField] float minZ = -320;
    [SerializeField] float maxZ = 320;


    //Rotation
    private Vector3 lastMousePos = Vector3.zero;
    [SerializeField] float pitchSpeed = 1f;
    [SerializeField] float maxPitch = 50f;
    [SerializeField] float minPitch = -50f;
    private float pitchDisplacement = 0f;

    private void Start() {
        lastMousePos = Input.mousePosition;
    }

    private void Update() {
        //Toggle camera/mouse
        if (Input.GetKeyDown(KeyCode.Space))
            bUsingMouse = !bUsingMouse;

        Translate();

        if (!bUsingMouse)
            Rotate();
    }

    private void Translate() {
        //Start sprint
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            moveSpeed = defaultSpeed * sprintMultiplier;
        }
        //End sprint
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            moveSpeed = defaultSpeed;
        }


        //Move forward
        if (Input.GetKey(KeyCode.W)) {
            float deltaZ = moveSpeed * Time.deltaTime;
            transform.position += new Vector3(0f, 0f, deltaZ);
        }
        //Move backward
        if (Input.GetKey(KeyCode.S)) {
            float deltaZ = moveSpeed * Time.deltaTime;
            transform.position += new Vector3(0f, 0f, -deltaZ);
        }
        //Move left
        if (Input.GetKey(KeyCode.A)) {
            float deltaX = moveSpeed * Time.deltaTime;
            transform.position += new Vector3(-deltaX, 0f, 0f);
        }
        //Move right
        if (Input.GetKey(KeyCode.D)) {
            float deltaX = moveSpeed * Time.deltaTime;
            transform.position += new Vector3(deltaX, 0f, 0f);
        }
        //Move up
        if (Input.GetKey(KeyCode.E)) {
            float deltaY = moveSpeed * Time.deltaTime;
            transform.position += new Vector3(0f, deltaY, 0f);
        }
        //Move down
        if (Input.GetKey(KeyCode.Q)) {
            float deltaY = moveSpeed * Time.deltaTime;
            transform.position += new Vector3(0f, -deltaY, 0f);
        }

        //Clamp position
        float x = Mathf.Clamp(transform.position.x, minX, maxX);
        float y = Mathf.Clamp(transform.position.y, minY, maxY);
        float z = Mathf.Clamp(transform.position.z, minZ, maxZ);
        transform.position = new Vector3(x, y, z);

    }
    private void Rotate() {
        Vector3 mouseDelta = Input.mousePosition - lastMousePos;
        lastMousePos = Input.mousePosition;

        //Change pitch
        if (pitchDisplacement - mouseDelta.y < maxPitch && pitchDisplacement - mouseDelta.y> minPitch) {
            transform.Rotate(new Vector3(-mouseDelta.y * pitchSpeed, 0f, 0f));
            pitchDisplacement -= mouseDelta.y;
        }
    }
}
