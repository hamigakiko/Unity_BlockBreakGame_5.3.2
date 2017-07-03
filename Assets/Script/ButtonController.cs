using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UniRx;

public class ButtonController : MonoBehaviour {

  // RaycastHit hit = new RaycastHit();

  // void Update(){
  //   if (Input.GetMouseButtonDown(0)){
  //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
  //     if (Physics.Raycast(ray, out hit)) {
  //        var obj = hit.collider.gameObject;
  //        if(obj.name == "pengin"){
  //         var obj_anim = obj.GetComponent<Animator>();
  //         obj_anim.Play("hide");
  //       }
  //     }
  //   }
  // }

  public void toMain(){
    SceneManager.LoadScene("main");
  }

  public void Quit(){
    Application.Quit();
  }
}
