using UnityEngine;

public class Ball : MonoBehaviour {
  [Header("property")]
  public int _speed = 5;

  [SerializeField]
  private GameController gameController;

  private BallStatus ballStatus;

  private Rigidbody rb;

  void Awake(){
    rb = GetComponent<Rigidbody>();
    ballStatus = BallStatus.STOP;
  }

  void OnDestroy(){
    gameController = null;
  }

  public BallStatus NowBallStatus(){
    return ballStatus;
  }

  public void Moving(){
    ballStatus = BallStatus.MOVING;
    int rand_val = new System.Random().Next(2);

    if(rand_val == 0){
      rb.AddForce((transform.up + transform.right) * _speed, ForceMode.VelocityChange);
    }else{
      rb.AddForce( ((transform.up - transform.right) * _speed), ForceMode.VelocityChange);
    }
  }

  public void Stop(){
    ballStatus = BallStatus.STOP;
    rb.velocity = Vector3.zero;
  }
}
