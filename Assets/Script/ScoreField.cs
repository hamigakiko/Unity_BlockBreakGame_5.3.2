using UnityEngine;
using System.Collections;

public class ScoreField : MonoBehaviour {

  ParticleSystem ps;

  void Awake(){
    ps = GetComponent<ParticleSystem>();
  }

  void OnParticleCollision(GameObject obj){
    ps.Play();
  }
}
