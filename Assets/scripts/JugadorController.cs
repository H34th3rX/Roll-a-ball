using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JugadorController : MonoBehaviour {
	private Rigidbody rb;
	private int contador;
	public float tiempoRestante = 120f;
	private bool juegoTerminado = false;
	private bool invulnerable = false;

	public float velocidad = 10f;
	public int totalColeccionables = 12;
	public string siguienteNivel = "Nivel2";
	public float penalizacionTiempo = 5f;
	public float duracionInvulnerabilidad = 1f;
	public Text textoContador, textoGanar, textoTimer;

	// Sonidos
	public AudioClip sonidoColeccion;
	public AudioClip sonidoGolpe;
	private AudioSource audioSourceColeccion;
	private AudioSource audioSourceGolpe;

	void Start () {
		rb = GetComponent<Rigidbody>();
		contador = 0;
		setTextoContador();
		textoGanar.text = "";

		// Obtener los dos AudioSource
		AudioSource[] audioSources = GetComponents<AudioSource>();
		if (audioSources.Length >= 2) {
			audioSourceColeccion = audioSources[0];
			audioSourceGolpe = audioSources[1];
		}
	}

	void Update() {
		if (!juegoTerminado) {
			tiempoRestante -= Time.deltaTime;
			ActualizarTimer();

			if (tiempoRestante <= 0) {
				PerderJuego();
			}
		}
	}

	void FixedUpdate () {
		if (!juegoTerminado) {
			float movimientoH = Input.GetAxis("Horizontal");
			float movimientoV = Input.GetAxis("Vertical");
			Vector3 movimiento = new Vector3(movimientoH, 0.0f, movimientoV);
			rb.AddForce(movimiento * velocidad);
		}
	}

	void OnTriggerEnter(Collider other) {
		// Coleccionables
		if (other.gameObject.CompareTag("Coleccionable")) {
			other.gameObject.SetActive(false);
			contador++;
			setTextoContador();

			// Reproducir sonido de colección
			ReproducirSonidoColeccion();
		}

		// Obstáculos
		if (other.gameObject.CompareTag("Obstaculo") && !invulnerable) {
			RestarTiempo();

			// Reproducir sonido de golpe
			ReproducirSonidoGolpe();
		}
	}

	void setTextoContador() {
		textoContador.text = "Contador: " + contador.ToString() + "/" + totalColeccionables;
		if (contador >= totalColeccionables) {
			GanarJuego();
		}
	}

	void ActualizarTimer() {
		int minutos = Mathf.FloorToInt(tiempoRestante / 60);
		int segundos = Mathf.FloorToInt(tiempoRestante % 60);
		textoTimer.text = string.Format("Tiempo: {0:00}:{1:00}", minutos, segundos);

		if (tiempoRestante < 30) {
			textoTimer.color = Color.red;
		}
	}

	void RestarTiempo() {
		tiempoRestante -= penalizacionTiempo;
		StartCoroutine(ParpadeoTimer());
		StartCoroutine(Invulnerabilidad());
		Debug.Log("¡Golpe! -" + penalizacionTiempo + " segundos");
	}

	// Reproducir sonido de colección
	void ReproducirSonidoColeccion() {
		if (audioSourceColeccion != null && sonidoColeccion != null) {
			audioSourceColeccion.PlayOneShot(sonidoColeccion);
		}
	}

	// Reproducir sonido de golpe
	void ReproducirSonidoGolpe() {
		if (audioSourceGolpe != null && sonidoGolpe != null) {
			audioSourceGolpe.PlayOneShot(sonidoGolpe);
		}
	}

	IEnumerator ParpadeoTimer() {
		Color colorOriginal = textoTimer.color;
		for (int i = 0; i < 3; i++) {
			textoTimer.color = Color.yellow;
			yield return new WaitForSeconds(0.1f);
			textoTimer.color = colorOriginal;
			yield return new WaitForSeconds(0.1f);
		}
	}

	IEnumerator Invulnerabilidad() {
		invulnerable = true;

		Renderer renderer = GetComponent<Renderer>();
		Color colorOriginal = renderer.material.color;

		for (int i = 0; i < 3; i++) {
			renderer.material.color = new Color(1f, 1f, 1f, 0.5f);
			yield return new WaitForSeconds(0.15f);
			renderer.material.color = colorOriginal;
			yield return new WaitForSeconds(0.15f);
		}

		invulnerable = false;
	}

	void GanarJuego() {
		juegoTerminado = true;
		textoGanar.text = "¡NIVEL COMPLETADO!";
		textoGanar.color = Color.yellow;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		StartCoroutine(SiguienteNivel(3f));
	}

	void PerderJuego() {
		juegoTerminado = true;
		textoGanar.text = "¡PERDISTE!\nSe acabó el tiempo";
		textoGanar.color = Color.red;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		StartCoroutine(VolverAlMenu(3f));
	}

	IEnumerator SiguienteNivel(float segundos) {
		yield return new WaitForSeconds(segundos);
		SceneManager.LoadScene(siguienteNivel);
	}

	IEnumerator VolverAlMenu(float segundos) {
		yield return new WaitForSeconds(segundos);
		SceneManager.LoadScene("MenuPrincipal");
	}
}