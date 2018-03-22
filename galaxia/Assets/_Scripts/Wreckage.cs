using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wreckage : MonoBehaviour
{
    public GameObject[] parts;

    IEnumerator breakApart()
    {
        // for each part in parts, set its direction in a random direction
        // give each part a random velocity forward, from a range of speed + or - velRange
        // destroy all pieces of the wreckage 5 seconds later
        yield return null;
    }
}
