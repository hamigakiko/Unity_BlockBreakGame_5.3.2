using UnityEngine;

public class Player : MonoBehaviour {
  private Vector3 pos;
  private Vector3 worldPointPos;

  private bool onPlayer = false;

  RaycastHit hit = new RaycastHit();

  void Update(){

    if(Input.GetMouseButtonDown(0)){
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out hit)) {
         if(hit.collider.gameObject.name == "Player"){
          onPlayer = true;
         }
      }
    }else if(Input.GetMouseButtonUp(0)){
      onPlayer = false;
    }

    if(onPlayer){
      pos = Input.mousePosition;
      worldPointPos = Camera.main.ScreenToWorldPoint(pos);

      if(worldPointPos.x <= -2.0f){
        worldPointPos.x = -2.0f;
      }else if(worldPointPos.x >= 2.0f){
        worldPointPos.x = 2.0f;
      }
      worldPointPos.y = -3.0f;
      worldPointPos.z = 0.0f;

      gameObject.transform.position = worldPointPos;

    }
  }
}
