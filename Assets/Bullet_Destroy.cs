using UnityEngine;
using System.Collections;

public class Bullet_Destroy : MonoBehaviour {

	void OnCollisionEnter(Collision collision) {
		Debug.Log ("COLLISION");
		StartCoroutine(Example());
	}

	IEnumerator Example() {
			yield return new WaitForSeconds(10);
			Debug.Log ("DESTROYED");
			Destroy (this.gameObject);
			
	}
}
