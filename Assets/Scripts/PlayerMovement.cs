using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float mainThrust = 1f;
    [SerializeField] float mainRotate = 1f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainJet;
    [SerializeField] ParticleSystem sideLeft;
    [SerializeField] ParticleSystem sideRight;


    Rigidbody rb;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }

        else
        {
            StopThrusting();
        }
    }


    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }

        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }

        else
        {
            StopRotating();
        }
    }

    void StopRotating()
    {
        sideLeft.Stop();
        sideRight.Stop();
    }

    void RotateRight()
    {
        ApplyRotation(-mainRotate);

        if (!sideLeft.isPlaying)
        {
            sideLeft.Play();
        }
    }

    void RotateLeft()
    {
        ApplyRotation(mainRotate);

        if (!sideRight.isPlaying)
        {
            sideRight.Play();
        }
    }

    void ApplyRotation(float rotation)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotation * Time.deltaTime);
        rb.freezeRotation = false;
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }

        if (!mainJet.isPlaying)
        {
            mainJet.Play();
        }
    }
    void StopThrusting()
    {
        audioSource.Stop();
        mainJet.Stop();
    }
}
