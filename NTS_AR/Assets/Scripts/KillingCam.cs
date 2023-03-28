using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingCam : MonoBehaviour
{

    //particleEffect to instantiate when a GameObject is destroyed
    public GameObject particleEffect;
    
    //the last position where we touch our screen
    private Vector2 touchpos;
    
    //last raycast from the camera to it's environment
    private RaycastHit hit;
    private Camera cam;
    
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount <= 0)
        {
            return;
        }

        //saving the touch position
        Touch touch = Input.GetTouch(0);
        touchpos = touch.position;
        if (touch.phase != TouchPhase.Began)
        {
            
            return;
                
        }
        
        //convert the touch position on the screen to a ray from the camera
        Ray ray = cam.ScreenPointToRay(touchpos);
        
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObj = hit.collider.gameObject;
            if (hitObj.tag == "Enemy")
            {
                var clone = Instantiate(particleEffect, hitObj.transform.position, Quaternion.identity);
                clone.transform.localScale = hitObj.transform.localScale;
                Destroy(hitObj);
            }
        }
    }
}
