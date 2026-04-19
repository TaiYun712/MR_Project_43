using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRUKSingleton : MonoBehaviour
{
    private static MRUKSingleton instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("刪除重複的 MRUK：" + gameObject.name);
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("保留 MRUK：" + gameObject.name);
    }
}
