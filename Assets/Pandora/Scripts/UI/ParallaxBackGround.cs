using UnityEngine;
using UnityEngine.InputSystem;

namespace Pandora.Scripts.UI
{
    public class ParallaxBackGround : MonoBehaviour
    {
        private Vector3 pz;
        private Vector3 StartPos;
        
        public float moveModifier;
        
	    // Use this for initialization
	    void Start ()
        {
            StartPos = transform.position;
        }
	    
	    // Update is called once per frame
	    void Update ()
        {
            pz = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            gameObject.transform.position = pz;
            //Debug.Log("Mouse Position: " + pz);

            transform.position = new Vector3(StartPos.x + (pz.x * moveModifier), StartPos.y + (pz.y * moveModifier), 0);
            //move based on the starting position and its modified value.
        }
        
    }
}