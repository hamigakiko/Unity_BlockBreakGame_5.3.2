using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UniRx;

public class GameController : MonoBehaviour {

  [Header("component")]
  public ParticleSystem particleScoreTitle;
  public Text _score;
  public Text _life;
  public BlockController blockController;
  public BallController ballController;
  public GameObject scoreField;

  [Header("property")]
  public int _lifeValue = 1;
  public int _bonusScoreValue;
  private int _scoreValue = 0;
  private bool _isLose;
  private bool _isWin;
  private bool _isStart;
  private Animator bonusAnim;
  private Animator criticalAnim;

  CompositeDisposable c = new CompositeDisposable();

  private GameObject gameStartImage;
  private GameObject gameClearImage;
  private GameObject gameOverImage;

  void Awake(){
    _isStart = false;
    _isLose  = false;
    _isWin   = false;

    UpdateLife();

    bonusAnim    = ((GameObject)Instantiate(Resources.Load("Prefabs/Animations/BonusAnim"))).GetComponent<Animator>();
    criticalAnim = ((GameObject)Instantiate(Resources.Load("Prefabs/Animations/CriticalAnim"))).GetComponent<Animator>();
    gameStartImage = (GameObject)Instantiate(Resources.Load("Prefabs/Images/GameStart"));
    gameClearImage = (GameObject)Instantiate(Resources.Load("Prefabs/Images/GameClear"));
    gameOverImage  = (GameObject)Instantiate(Resources.Load("Prefabs/Images/GameOver"));

    particleScoreTitle.Stop();
  }

  void Start(){
    // ボールオブジェクトの準備
    ballController.InitBall();

    var startStream = Observable.EveryUpdate()
      .Where(_ => _isStart)
      .Where(_ => Input.GetMouseButtonDown(0));

    var s = startStream
      .Subscribe(_ => {
        ballController.ChangeBallsStatus(BallStatus.MOVING);
        // HideGameStatusText();
        ShowMessage("game_start", true);
      });
    c.Add(s);

    // ゲーム開始の準備
    Observable.EveryUpdate()
      .TakeUntil(startStream)
      .Where(_ => blockController.isCompleted())
      .Where(_ => ballController.HasBall())
      .Subscribe(_ => {

        ShowMessage("game_start");
        // ShowGameStatusText("TOUCH START", Color.white);
        _isStart = true;
      }).AddTo(gameObject);
  }

  void OnDestroy() {
    c.Dispose();
  }

  void Update(){
    // 失敗orクリア時にシーンを変える
    if(_isLose || _isWin){
      if(Input.GetMouseButtonDown(0)){
        SceneManager.LoadScene("title", LoadSceneMode.Single);
      }
    }

    if(ballController.isBallMax()){
      particleScoreTitle.Play();
    }else{
      particleScoreTitle.Stop();
    }
  }

  public void UpdateBlock(Block block, GameObject ballObj){
    blockController.UpdateDestroyBlockCount(1);

    int tmpScore = block.data.score;
    if(ballController.isBallMax()){
      tmpScore *= _bonusScoreValue;
    }
    AddScore(tmpScore);

    if(blockController.isAllDestroyBlock()){
      Win();
    }else{
      LotBonusBallGain((Vector2)ballObj.transform.position);
    }
  }

  public void Restart(){
    ballController.AllDeleteBall();
    ballController.RestartBall();
  }

  public void Lose(GameObject ballObj){
    ballController.DeleteBall(ballObj);

    if(ballController.HasBall() == false){
      UpdateLife(-1);
      if(_lifeValue <= 0){
        // ShowGameStatusText("Game Over...", Color.white);
        ShowMessage("game_over");
        _isLose = true;
      }else{
        ballController.RestartBall();
      }
    }
  }

  public void Win(){
    // ShowGameStatusText("Game Clear!!", Color.yellow);
    ShowMessage("game_clear");
    _isWin = true;
    ballController.ChangeBallsStatus(BallStatus.STOP);
  }

  public bool LotBonusHitBreak(){
    bool res = blockController.LotBonusHitBreak();
    if (res){
      ShowMessage("critical");
    }
    return res;
  }

  public GameObject ScoreField(){
    return scoreField;
  }

  private void UpdateLife(int val = 0){
    _lifeValue += val;
    _life.text = _lifeValue.ToString();
  }

  private void AddScore(int addValue){
    _scoreValue += addValue;
    _score.text = _scoreValue.ToString();
  }

  private void LotBonusBallGain(Vector2 pos){
    if(blockController.LotBonusBallGain()){
      if(ballController.isBallMax() == false){
        ShowMessage("bonus");
        ballController.ReplicateBall(pos);
      }
    }
  }

  private void ShowMessage(string val, bool isHide = false){
    switch(val)
    {
      case "bonus":
        bonusAnim.Play("show", 0, 0.0f);
        break;
      case "critical":
        criticalAnim.Play("show", 0, 0.0f);
        break;
      case "game_start":
        if(isHide){
          gameStartImage.SetActive(false);
          iTween.Stop(gameStartImage);
        }else{
          gameStartImage.SetActive(true);
          iTween.ScaleTo(gameStartImage, iTween.Hash(
            "x",        1.2,
            "y",        1.2,
            "looptype", "pingPong"
          ));
        }
        break;
      case "game_clear":
        if(isHide){
          gameClearImage.SetActive(false);
          iTween.Stop(gameClearImage);
        }else{
          gameClearImage.SetActive(true);
          iTween.ScaleTo(gameClearImage, iTween.Hash(
            "x",        1.2,
            "y",        1.2,
            "looptype", "pingPong"
          ));
        }
        break;
      case "game_over":
        if(isHide){
          gameOverImage.SetActive(false);
          iTween.Stop(gameOverImage);
        }else{
          gameOverImage.SetActive(true);
          iTween.ScaleTo(gameOverImage, iTween.Hash(
            "x",        1.2,
            "y",        1.2,
            "looptype", "pingPong"
          ));
        }
        break;
    }
  }
}
