using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandPosTracking : MonoBehaviour
{
    public GameObject firePf;

    public GameObject ballPf;
    public float spawnSpeed = 5;

    public void FireOn()
    {
        firePf.SetActive(true);
    }

    public void FireOff()
    {
        firePf.SetActive(false);
    }

    public void ShootBall()
    {
        GameObject shootBall = Instantiate(ballPf, transform.position, Quaternion.identity);
        Rigidbody shootBallRB = shootBall.GetComponent<Rigidbody>();
        shootBallRB.velocity = transform.forward * spawnSpeed;
    }

  
}
