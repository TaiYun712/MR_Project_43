using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;
    public GameObject bulletPf;
    public int initialSize = 5; //子彈預置數量

    private Queue<GameObject> bulletPool = new Queue<GameObject>();

    private void Awake()
    {
        instance = this;

        //預先生成子彈
        for (int i = 0; i < initialSize; i++)
        {
            GameObject bullet = CreateNewBullet();
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
        
    }

    //生成子彈
    GameObject CreateNewBullet()
    {
        GameObject bullet = Instantiate(bulletPf);
        bullet.transform.SetParent(this.transform);
        return bullet;
    }

    //取出子彈
    public GameObject GetBullet(Vector3 pos, Quaternion rota)
    {
        GameObject bullet;
        if (bulletPool.Count > 0)
        {
            bullet = bulletPool.Dequeue();
        }
        else
        {
            bullet = CreateNewBullet();
        }

        bullet.transform.position = pos;
        bullet.transform.rotation = rota;
        bullet.SetActive(true);
        return bullet;
    }
    
    //收回子彈
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
