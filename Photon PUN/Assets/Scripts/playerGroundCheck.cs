using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerGroundCheck : MonoBehaviour
{
    playerController controller;

    private void Awake()
    {
        controller = GetComponentInParent<playerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == controller.gameObject)
            return;

        controller.setGrounded(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == controller.gameObject)
            return;

        controller.setGrounded(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == controller.gameObject)
            return;

        controller.setGrounded(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == controller.gameObject)
            return;

        controller.setGrounded(true);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == controller.gameObject)
            return;

        controller.setGrounded(false);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == controller.gameObject)
            return;

        controller.setGrounded(true);
    }
}
