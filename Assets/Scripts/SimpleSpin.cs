using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpin : MonoBehaviour {
    
    public bool spinning = false;
    public float spinSpeed = 1;
    private float t = 0;

    private void Update() {
        if ( spinning ) {
            t += Time.deltaTime;
            this.transform.rotation = Quaternion.AngleAxis(t * spinSpeed, Vector3.back);
        }
    }

}
