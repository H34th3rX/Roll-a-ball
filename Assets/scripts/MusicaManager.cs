using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicaManager : MonoBehaviour {
	private static MusicaManager instance;

	void Awake() {
		// Si ya existe una instancia, destruir este objeto
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		}

		// Esta es la única instancia
		instance = this;

		// No destruir al cambiar de escena
		DontDestroyOnLoad(this.gameObject);
	}
}