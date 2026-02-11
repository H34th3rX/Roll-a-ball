using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliceRotatoria : MonoBehaviour {
	public float velocidadRotacion = 100f;
	public Vector3 ejeRotacion = Vector3.up; // Rotar en Y por defecto

	void Update() {
		// Rotar continuamente
		transform.Rotate(ejeRotacion * velocidadRotacion * Time.deltaTime);
	}
}