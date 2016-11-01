using UnityEngine;
using System.Collections;

public class Casing_Destory : MonoBehaviour {


    float delay = 20f; //This implies a delay of 2 seconds.
    void Start () {
        Destroy(this.gameObject, delay);
    }

}
