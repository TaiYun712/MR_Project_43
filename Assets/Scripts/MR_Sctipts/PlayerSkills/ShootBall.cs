using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBall : MonoBehaviour
{
    public GameObject ballPf;
    public float spawnSpeed = 5;

    void Start()
    {
        
    }

    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Debug.Log("«ö¤UªO¾÷");
            GameObject shootBall = Instantiate(ballPf, transform.position, Quaternion.identity);
            Rigidbody shootBallRB = shootBall.GetComponent<Rigidbody>();
            shootBallRB.velocity = transform.forward * spawnSpeed;
        }
    }
}
