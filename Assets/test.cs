using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    
    Renderer rend;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log ("hello world");
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3 (0, 0.01f, 0);
        rend.material.color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));
    }
}
