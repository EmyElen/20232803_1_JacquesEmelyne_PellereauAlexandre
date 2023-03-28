using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class  Game : MonoBehaviour
{

    public GameObject EnemyPrefab;
    private bool isTouched = false;
    
    
    //particleEffect to instantiate when a GameObject is destroyed
    public GameObject particleEffect;

    [SerializeField] private Text chrono;
    private float timer = 60f;
    
    private RaycastHit hit;
    private Camera cam;
    
    public Text killed;
    public GameObject endWindow;
    
    
    public ARRaycastManager raycastManager;
    public TrackableType typeToTrack = TrackableType.PlaneWithinBounds;
    public List<Material> materials = new List<Material>();
    public Text countText;
    public int _cubeCount;

    private Touch generateTouch;


    

    private void OnTouch()
    {
        isTouched = true;
        generateTouch = Input.GetTouch(0);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(generateTouch.position, hits, typeToTrack);
        
        if (hits.Count > 0)
        {
            Spawn();
        }
        
        
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
            EndGame();
        }

        Touch touch = generateTouch;
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
        Invoke("Spawn",1);

        
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
            Material material = hitObj.GetComponent<Material>();
            particleEffect.GetComponent<MeshRenderer>().material = material;
            if (hitObj.tag == "Enemy")
            {
                var clone = Instantiate(particleEffect, hitObj.transform.position, Quaternion.identity);
                clone.transform.localScale = hitObj.transform.localScale;
                _cubeCount++;
                countText.text = "Enemy killed " + _cubeCount;
                Destroy(hitObj);
                
            }
        }
    }
    
    public void PlayAgain()
    {
        //endWindow.SetActive(false);
        SceneManager.LoadScene("Scene1");
    }

    void EndGame()
    {
        endWindow.SetActive(true);
        killed.text += " " + _cubeCount;
    }




}
