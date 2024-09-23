using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;



public class ARWallFloorManager : MonoBehaviour
{
    [SerializeField] private ARPlaneManager m_ARPlaneManager;

    // Transparent material to apply to the AR planes (walls and floors)
    [SerializeField] private Material transparentMaterial;

    // Portal material to create the hole effect
    [SerializeField] private Material portalMaterial;

    // Original material when the plane is reset after a bullet hit
    // [SerializeField] private Material defaultMaterial;

    void Start()
    {
        // Enable ARPlaneManager to detect planes
        m_ARPlaneManager.enabled = true;
        
        // Subscribe to AR plane detection changes
        m_ARPlaneManager.planesChanged += OnPlanesChanged;
    }

    // Detect new planes and apply the transparent material and add colliders
    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
    	foreach (var plane in m_ARPlaneManager.trackables)
        {
        	Debug.Log($"AR plane (ID: {plane.trackableId}).");

                        // Access the mesh renderer of the plane visualizer and set its material
            //MeshRenderer meshRenderer = meshVisualizer.GetComponent<MeshRenderer>();
            MeshRenderer meshRenderer = plane.GetComponent<MeshRenderer>();

            if (meshRenderer != null)
            {
                meshRenderer.material = transparentMaterial;
            }
        }

        foreach (var plane in args.added)
        {
        	 // Debug.Log($"AR plane (ID: {plane.trackableId}).");

            // Apply the transparent material to both walls (vertical) and floors (horizontal)
            // ARPlaneMeshVisualizer meshVisualizer = plane.GetComponent<ARPlaneMeshVisualizer>();
            // if (meshVisualizer != null)
            // {
            //     ApplyTransparentMaterial(meshVisualizer);
            // }

            // Add a BoxCollider to the plane so that it can interact with bullets
            // AddColliderToPlane(plane);
        }
    }

    // Apply the transparent material to AR planes
    void ApplyTransparentMaterial(ARPlaneMeshVisualizer meshVisualizer)
    {
        if (transparentMaterial != null)
        {
            // Access the mesh renderer of the plane visualizer and set its material
            MeshRenderer meshRenderer = meshVisualizer.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = transparentMaterial;
            }
        }
    }

    // Add a BoxCollider to the ARPlane so it can interact with bullets
    void AddColliderToPlane(ARPlane plane)
    {
	    BoxCollider boxCollider = plane.gameObject.GetComponent<BoxCollider>();
	    if (boxCollider == null)
	    {
	        boxCollider = plane.gameObject.AddComponent<BoxCollider>();
	        Debug.Log("Collider added to plane: " + plane.trackableId);
	    }
	    else
	    {
	        Debug.Log("Plane already has a collider: " + plane.trackableId);
	    }

        // Adjust the BoxCollider size and position based on the AR plane's size
        boxCollider.center = Vector3.zero;
        boxCollider.size = new Vector3(plane.size.x, 1.0f, plane.size.y); // Very thin in the y-axis to represent the plane's surface
        boxCollider.isTrigger = true; // Set to trigger mode for OnTriggerEnter
    }
// void OnDrawGizmos()
// {
//     if (m_ARPlaneManager != null)
//     {
//         foreach (var plane in m_ARPlaneManager.trackables)
//         {
//             var collider = plane.gameObject.GetComponent<BoxCollider>();
//             if (collider != null)
//             {
//                 Gizmos.color = Color.green;
//                 Gizmos.DrawWireCube(collider.center, collider.size);
//             }
//         }
//     }
// }

// Handle collision detection with bullets to create the portal (hole) in the AR plane
private void OnTriggerEnter(Collider other)
{
    // Debug.Log("Collision detected with: " + other.gameObject.name + ", Tag: " + other.tag);

    // Assuming the bullet has the "Bullet" tag
    if (other.CompareTag("Bullet"))
    {
        // Get the bullet's position
        Vector3 bulletPosition = other.transform.position;

        // Check through all tracked planes in the ARPlaneManager
        foreach (var plane in m_ARPlaneManager.trackables)
        {
            // Get the ARPlaneMeshVisualizer
            //ARPlaneMeshVisualizer meshVisualizer = plane.GetComponent<ARPlaneMeshVisualizer>();
            Debug.Log($"Bullet hit the AR plane (ID: {plane.trackableId}).");

            // if (meshVisualizer != null)
            // {
        	   //                  // Create the portal (hole) where the bullet hits
            //     CreatePortalEffect(meshVisualizer);

            //     // Display debug message when a bullet hits the AR plane
            //     Debug.Log($"Bullet hit the AR plane (ID: {plane.trackableId}).");
            //     break;  // Break after finding the plane that was hit
            //     // Check if the bullet is within the bounds of the AR plane
            //     // if (plane.boundary.Contains(new Vector2(bulletPosition.x, bulletPosition.z)))
            //     // {

            //     // }
            // }
        }
    }
}


    // Create the portal effect (hole) on the AR plane where the bullet hit
    void CreatePortalEffect(ARPlaneMeshVisualizer meshVisualizer)
    {
        if (portalMaterial != null)
        {
            // Access the mesh renderer of the plane visualizer and set the portal material to create the hole effect
            MeshRenderer meshRenderer = meshVisualizer.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = portalMaterial; // Set the portal material
            }
        }
    }

    // Remove the transparent material (reset to default or transparent)
    // void RemoveTransparentMaterial(ARPlaneMeshVisualizer meshVisualizer)
    // {
    //     if (defaultMaterial != null)
    //     {
    //         // Access the mesh renderer of the plane visualizer and reset its material
    //         MeshRenderer meshRenderer = meshVisualizer.GetComponent<MeshRenderer>();
    //         if (meshRenderer != null)
    //         {
    //             meshRenderer.material = defaultMaterial;
    //         }
    //     }
    // }
}
