using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera hoodCamera;
    public KeyCode switchKey;

    [SerializeField] private float turnSpeed = 25;
    private float horizontalInput;

    public string inputID;

    private Rigidbody playerRb;
    [SerializeField] GameObject centerOfMass;
    [SerializeField] float speed = 5;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.centerOfMass = centerOfMass.transform.position;
    }

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal" + inputID);
        Vector3 rotation = transform.rotation.eulerAngles;
        transform.Translate(Vector3.right * Mathf.Cos(Mathf.PI / 2f - rotation.y / 180f * Mathf.PI) * Time.deltaTime * speed);
        transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime * horizontalInput);
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            mainCamera.enabled = !mainCamera.enabled;
            hoodCamera.enabled = !hoodCamera.enabled;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject road = other.gameObject.transform.parent.gameObject;
        Vector3 newPosition = new Vector3(road.transform.position.x, road.transform.position.y, road.transform.position.z + 340);
        Instantiate(road, newPosition, road.transform.rotation);
        Destroy(other.gameObject);
    }
}
