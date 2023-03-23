using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private Transform characterTransform;

    public float movementSpeed;
    public float jumpForce;
    public float rotationSpeed;

    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        characterTransform = transform;
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(hor, 0, ver).normalized;

        // Rotar la dirección de movimiento del personaje en base a la rotación de la cámara
        direction = Camera.main.transform.TransformDirection(direction);
        direction.y = 0;

        Vector3 velocity = direction * movementSpeed;
        velocity.y = rigidbody.velocity.y;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isGrounded = false;
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            StartCoroutine(UpdateGroundedStateAfterDelay());
        }

        if (rigidbody != null)
        {
            rigidbody.velocity = velocity;
        }

        // Obtener la rotación actual de la cámara
        Quaternion cameraRotation = Camera.main.transform.rotation;

        // Rotar la cámara en función del movimiento del mouse
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
        cameraRotation *= Quaternion.Euler(-mouseY, mouseX, 0);
        Camera.main.transform.rotation = cameraRotation;

        // Crear un ángulo de rotación para el personaje en base a la rotación de la cámara
        Vector3 characterRotation = new Vector3(0, cameraRotation.eulerAngles.y, 0);

        // Aplicar la rotación al objeto del personaje
        characterTransform.rotation = Quaternion.Euler(characterRotation);
    }

    IEnumerator UpdateGroundedStateAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // Esperar medio segundo
        isGrounded = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}

