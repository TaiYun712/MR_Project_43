using UnityEngine;
using Meta.XR.MRUtilityKit;

public class TestFindMRUK : MonoBehaviour
{
    private void Start()
    {
        MRUK[] allMruks = FindObjectsOfType<MRUK>();

        Debug.Log("目前場上 MRUK 數量：" + allMruks.Length);

        for (int i = 0; i < allMruks.Length; i++)
        {
            Debug.Log("MRUK " + i + " 名稱：" + allMruks[i].gameObject.name);
        }
    }
}