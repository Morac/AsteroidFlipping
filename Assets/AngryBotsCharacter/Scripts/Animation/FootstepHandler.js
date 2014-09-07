#pragma strict
//
//enum FootType {
//	Player,
//	Mech,
//	Spider
//}

var audioSource : AudioSource;
var sounds : AudioClip[];

//var footType : FootType;

//private var physicMaterial : PhysicMaterial;
//
//function OnCollisionEnter (collisionInfo : Collision) {
//	physicMaterial = collisionInfo.collider.sharedMaterial;
//}

function OnFootstep () {
	if (!audioSource.enabled)
	{
		return;
	}
	
	var sound : AudioClip;
	sound = sounds[Random.Range(0, sounds.Length)];
	audioSource.pitch = Random.Range (0.98, 1.02);
	audioSource.PlayOneShot (sound, Random.Range (0.8, 1.2));
}
