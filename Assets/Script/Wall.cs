using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

  [SerializeField]
  private GameController gameController;

  void OnCollisionEnter(Collision col){
    gameController.Lose(col.gameObject);
  }
}
