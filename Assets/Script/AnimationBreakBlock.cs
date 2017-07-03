using UnityEngine;

public class AnimationBreakBlock : MonoBehaviour {
  public void OnAnimationFinish(){
    Destroy(gameObject);
  }
}
