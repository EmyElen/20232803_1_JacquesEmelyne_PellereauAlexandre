using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;


public class Scene2Manager : MonoBehaviour
{

    public ARRaycastManager raycastManager;
    public TrackableType typeToTrack = TrackableType.PlaneWithinBounds;
    public GameObject prefabToInstantiate;
    private List<GameObject> _instantiatedCubes = new List<GameObject>();
    public List<Material> materials = new List<Material>();
    [SerializeField] private Text countText;
    private int _cubeCount;

    private void OnTouch()
    {
        Touch touch = Input.GetTouch(0);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(touch.position, hits, typeToTrack);
        
        if (hits.Count > 0)
        {
            ARRaycastHit firstHit = hits[0];
            Vector3 pos = firstHit.pose.position;
            pos.y += 0.1f;
            InstantiateObject(pos, firstHit.pose.rotation);
        }
    }

    
    
    void InstantiateObject(Vector3 position, Quaternion rotation)
    {
        GameObject cube = Instantiate(prefabToInstantiate, position, rotation);
        _instantiatedCubes.Add(cube);
        _cubeCount++;
        countText.text = "Cubes : " + _cubeCount;
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began)
            {
                return;
                
            }
            OnTouch();
        }
            
    }


    public void ChangeColor()
    {
        foreach (GameObject cube in _instantiatedCubes)
        {
            int randomIndex = Random.Range(0, materials.Count);
            Material randomMaterial = materials[randomIndex];
            cube.GetComponent<MeshRenderer>().material = randomMaterial;
        }
    }
    
}
