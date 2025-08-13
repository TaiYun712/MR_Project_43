using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class DestructibleEnvironment_Manager : MonoBehaviour
{
    public DestructibleGlobalMeshSpawner meshSpawner;
    
    private List<GameObject> segments = new List<GameObject>();
    private DestructibleMeshComponent currentComponent;
    void Start()
    {
        meshSpawner.OnDestructibleMeshCreated.AddListener(SetUpDestructibleComponents);
    }

    public void SetUpDestructibleComponents(DestructibleMeshComponent component)
    {
        currentComponent = component;
        
        component.GetDestructibleMeshSegments(segments);

        foreach (var item in segments)
        {
            item.AddComponent<MeshCollider>();
            item.tag = "DestructibleWalls";
        }
    }

    public void DestroyMeshSegment(GameObject segment)
    {
        if (segments.Contains(segment) && currentComponent.ReservedSegment != segment)
        {
           segment.SetActive(false);
        }
    }
}
