using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public Vector3 destination;
    public Vector3 startingLocation;

    private void Start() {
        this.transform.position = startingLocation;
    }

    public void StartMovement() {
        StartCoroutine("Fall");
    }

    IEnumerator Fall() {
        float t = 0;
        float percent = 0;

        while ( t < 1 ) {
            t += Time.deltaTime;
            percent = t * t;
            this.transform.position = Vector3.Lerp(startingLocation, destination, percent);
            yield return null;
        }
    }
}
