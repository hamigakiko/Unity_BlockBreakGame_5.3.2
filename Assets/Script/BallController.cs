using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BallController : MonoBehaviour {

  public Vector2 defaultBallPos = new Vector2(0, -2.0f);

  public int maxBoolsCount;

  private GameObject ballPrefab;
  private int boolsCount;
  private List<GameObject> ballObjs = new List<GameObject>();

  void Awake(){
    boolsCount = 0;
    ballPrefab = (GameObject)Resources.Load("Prefabs/Ball");
  }

  void Start(){
  }

  public void InitBall(){
    UpdateBoolsCount(0);
    ballObjs.Clear();
    GameObject ballObj = CreateBall(defaultBallPos);
  }

  public void RestartBall(){
    GameObject ballObj = CreateBall(defaultBallPos);
  }

  public void ReplicateBall(Vector2 pos){
    GameObject ballObj = CreateBall(pos);
    ballObj.GetComponent<Ball>().Moving();
  }

  public bool HasBall(){
    bool res = false;
    if(boolsCount > 0){
      res = true;
    }
    return res;
  }

  public bool isBallMax(){
    bool res = false;
    if(boolsCount >= maxBoolsCount){
      res = true;
    }
    return res;
  }

  public void ChangeBallsStatus(BallStatus status){
    switch(status)
    {
      case BallStatus.STOP:
        StatusStop();
        break;
      case BallStatus.MOVING:
        StatusMoving();
        break;
    }
  }

  public void DeleteBall(GameObject ballObj){
    UpdateBoolsCount(-1);
    Destroy(ballObj);

    foreach(var obj in ballObjs){
      if(obj.GetInstanceID() == ballObj.GetInstanceID()){
        ballObjs.Remove(obj);
        break;
      }
    }
  }

  public void AllDeleteBall(){
    foreach(var obj in ballObjs){
      Destroy(obj);
    }
    UpdateBoolsCount(0);
    ballObjs.Clear();
  }

  private GameObject CreateBall(Vector2 pos){
    UpdateBoolsCount(1);
    var ballObj = (GameObject)Instantiate(ballPrefab);
    ballObj.transform.position = pos;
    ballObjs.Add(ballObj);
    return ballObj;
  }

  private void UpdateBoolsCount(int cnt){
    if(cnt == 0){
      boolsCount = 0;
    }else{
      boolsCount += cnt;
      if(boolsCount < 0){
        boolsCount = 0;
      }
    }
  }

  private void StatusMoving(){
    foreach(GameObject obj in ballObjs){
      var ball = obj.GetComponent<Ball>();
      if(ball.NowBallStatus() == BallStatus.STOP){
        ball.Moving();
      }
    }
  }

  private void StatusStop(){
    foreach(GameObject obj in ballObjs){
      var ball = obj.GetComponent<Ball>();
      if(ball.NowBallStatus() == BallStatus.MOVING){
        ball.Stop();
      }
    }
  }
}
