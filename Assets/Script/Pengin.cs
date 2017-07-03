using UnityEngine;
using System;
using UniRx;

public class Pengin : MonoBehaviour {

  RaycastHit hit = new RaycastHit();

  void Update(){
    if (Input.GetMouseButtonDown(0)){
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out hit)) {
         var obj = hit.collider.gameObject;
         if(obj.name == "pengin"){
          var obj_anim = obj.GetComponent<Animator>();
          obj_anim.Play("leave");
        }
      }
    }
  }

  public void LeaveEnd(){
    var anim = GetComponent<Animator>();
    Observable.Timer(TimeSpan.FromSeconds(3)).Subscribe(_=> anim.Play("appear", 0, 0.0f)).AddTo(gameObject);
  }

  public void AppearEnd(){
    var anim = GetComponent<Animator>();
    anim.Play("pengin", 0, 0.0f);
  }

}
