using UnityEngine;
using System;
using System.Collections;

using UniRx;

public class Block : MonoBehaviour {
  [Header("property")]
  public Color _beforeBreakColor;
  [System.Serializable]
  public class Data{
    public int hp;
    public int score;
    public string type;
  }
  public Data data;

  private GameController gameController;
  private BlockController blockController;

  private ParticleSystem partcileDamage;
  private ParticleSystem partcileDamageCritical;
  private ParticleSystem partcileFire;
  private ParticleSystem partcileFireCritical;

  private ParticleSystem partcileStar;
  ParticleSystem.Particle[] particles;

  void OnDestroy(){
    gameController = null;
  }

  void Awake(){
    var gameObj = GameObject.Find("Game");
    gameController  = gameObj.GetComponent<GameController>();
    blockController = gameObj.GetComponent<BlockController>();

    partcileDamage         = transform.FindChild("ParticleDamage").GetComponent<ParticleSystem>();
    partcileDamageCritical = transform.FindChild("par1").GetComponent<ParticleSystem>();
    // partcileDamageCritical = transform.FindChild("ParticleDamageCritical").GetComponent<ParticleSystem>();
    partcileFire           = transform.FindChild("ParticleFire").GetComponent<ParticleSystem>();
    partcileFireCritical   = transform.FindChild("ParticleFireCritical").GetComponent<ParticleSystem>();

    partcileStar           = transform.FindChild("ParticleStar").GetComponent<ParticleSystem>();



    particles = new ParticleSystem.Particle[partcileStar.maxParticles];
  }





  void OnCollisionEnter(Collision col){
    if(col.gameObject.tag == "Ball"){

      bool is_critical = damage();

      if(is_critical){
        partcileDamageCritical.Play();
      }else{
        partcileDamage.Play();
      }

      if(isBroken()){
        transform.localScale = Vector3.zero;

        partcileStar.gameObject.transform.LookAt(gameController.ScoreField().transform);

        partcileStar.Play();

        if(is_critical){
          partcileFireCritical.gameObject.SetActive(true);
          partcileFireCritical.playbackSpeed = 2.0f;
          partcileFireCritical.Play();
        }else{
          partcileFire.gameObject.SetActive(true);
          partcileFire.playbackSpeed = 2.0f;
          partcileFire.Play();
        }

        gameObject.layer = LayerMask.NameToLayer("Effect");
        Destroy(gameObject, 1.5f);

        gameController.UpdateBlock(this, col.gameObject);
      }else if(isBeforeBroke()){
        gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", _beforeBreakColor);
      }
    }
  }

  public bool isBroken(){
    bool result = false;
    if(data.hp == 0){
      result = true;
    }
    return result;
  }

  public bool isBeforeBroke(){
    bool result = false;
    if(data.hp == 1){
      result = true;
    }
    return result;
  }

  private bool damage(){
    bool result = false;
    data.hp -= 1;
    if(gameController.LotBonusHitBreak()){
      data.hp = 0;
      result = true;
    }
    return result;
  }
}
