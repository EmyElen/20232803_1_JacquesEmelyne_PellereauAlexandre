using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public GameObject EnemyPrefab;
    private bool isTouched = false;
    
    
    //particleEffect to instantiate when a GameObject is destroyed
    public GameObject particleEffect;

    [SerializeField] private Text chrono;
    private float timer = 30f;
    
    private RaycastHit hit;
    private Camera cam;
    
    
    public ARRaycastManager raycastManager;
    public TrackableType typeToTrack = TrackableType.PlaneWithinBounds;
    private List<GameObject> _instantiatedCubes = new List<GameObject>();
    public List<Material> materials = new List<Material>();
    [SerializeField] private Text countText;
    private int _cubeCount;
    
    
   

    private void OnTouch()
    {
        isTouched = true;
        Touch touch = Input.GetTouch(0);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(touch.position, hits, typeToTrack);
        
        if (hits.Count > 0)
        {
            Spawn();
        }
        
        
    }

    
    
    void InstantiateObject(Vector3 position, Quaternion rotation)
    {
        GameObject cube = Instantiate(EnemyPrefab, position, rotation);
        _instantiatedCubes.Add(cube);
        
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    
    
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        chrono.text = "Timer :" + timer;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began)
            {
                return;
                
            }

            if (isTouched == false)
            {
                OnTouch();
            }
            else
            {
                KillingEnemy();
            }
            
        }
            
    }
    
    
    
    void Spawn()
    {
        if (timer < 0)
        {
            return;
        }
        Touch touch = Input.GetTouch(0);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(touch.position, hits, typeToTrack);
        
        if (hits.Count > 0)
        {
            ARRaycastHit firstHit = hits[0];
            Vector3 pos = firstHit.pose.position;

            pos.x += Random.Range(-0.2f, 0.2f);
            pos.y += 0.1f;
            pos.z += Random.Range(-0.2f, 0.2f);
            
            GameObject cube = Instantiate(EnemyPrefab, pos, firstHit.pose.rotation);
            int randomIndex = Random.Range(0, materials.Count);
            Material randomMaterial = materials[randomIndex];
            cube.GetComponent<MeshRenderer>().material = randomMaterial;
        }
        Invoke("Spawn",3);

        
    }


    void KillingEnemy()
    {
        Touch touch = Input.GetTouch(0);
        Vector2 touchpos = touch.position;
        //convert the touch position on the screen to a ray from the camera
        Ray ray = cam.ScreenPointToRay(touchpos);
        
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObj = hit.collider.gameObject;
            Material material = hitObj.GetComponent<MeshRenderer>().material;
            particleEffect.GetComponent<MeshRenderer>().material = material;
            if (hitObj.tag == "Enemy")
            {
                var clone = Instantiate(particleEffect, hitObj.transform.position, Quaternion.identity);
                clone.transform.localScale = hitObj.transform.localScale;
                Destroy(hitObj);
                _cubeCount++;
                countText.text = "Enemy killed " + _cubeCount;
            }
        }
    }
    
   

    
}
