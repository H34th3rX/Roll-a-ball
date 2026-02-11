using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour {

	public void Jugar() {
		SceneManager.LoadScene("Juego");
	}

	public void Opciones() {
		SceneManager.LoadScene("Controles");
	}

	public void VolverAlMenu() {
		SceneManager.LoadScene("MenuPrincipal");
	}

	public void Salir() {
		Debug.Log("Saliendo del juego...");
		Application.Quit();
	}
}