using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    int colorPicker = 0;
    Renderer rend;
    public Material player1Material;
    public Material player2Material;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //When the Primitive collides with the walls, it will reverse direction
    private void OnTriggerEnter(Collider other)
    {
       
    }

    //When the Primitive exits the collision, it will change Color

      private void OnCollisionEnter2D(Collision2D collision)
    {
     //    Debug.Log("hou hou!");
      //  colorPicker = Random.Range(0, 10);
       colorPicker = Random.Range(0, 20);
    }

    //When the Primitive exits the collision, it will change Color
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log(colorPicker);
        switch (colorPicker)
        {
            
            case 0: rend.material = player1Material; break;
            case 1: rend.material = player2Material; break;
        }
    }
}
