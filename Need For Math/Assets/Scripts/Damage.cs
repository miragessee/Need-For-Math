using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Damage : MonoBehaviourPun {

	void Start() {
	}

	void Update() {
	}

	void OnTriggerEnter(Collider other) {
		// only process bullets in the car's original local computer (where physics are computed)
		if (photonView.Owner != PhotonNetwork.LocalPlayer)
			return;

		if (other.tag == "Bullet") {
			Bullet bullet = other.GetComponent<Bullet> ();

			// do not hit the car that created the bullet
			if (photonView.Owner != bullet.photonView.Owner) {
				bullet.EventHit (gameObject);
			}
		} else if (other.tag == "Collectable") {
			CollectableEffect collectableEffect = other.GetComponent<CollectableEffect> ();
			collectableEffect.EventHit (gameObject);
		}
	}

}