using UnityEngine;
using System.Collections;
using System.Collections.Generic;


using UniRx;
using System.Linq;


public class BlockController : MonoBehaviour {

  public int colBlockCount;
  private int rowBlockCount = 5;
  private int totalBlockCount;
  private int destroyBlockCount;

  [System.Serializable]
  public class PosData{
    public float defX   = -2.0f;
    public float defY   =  3.5f;
    public float defZ   =  0;
    public float rangeX =  1.0f;
    public float rangeY =  0.5f;
  }
  public PosData posData;

  [System.Serializable]
  public class HpData{
    public int col1;
    public int col2;
    public int col3;
    public int col4;
    public int col5;
  }
  public HpData hpData;
  private int[] lHpData;

  [System.Serializable]
  public class ScoreData{
    public int col1;
    public int col2;
    public int col3;
    public int col4;
    public int col5;
  }
  public ScoreData scoreData;
  private int[] lScoreData;

  [System.Serializable]
  public class MaterialData{
    public Material col1;
    public Material col2;
    public Material col3;
    public Material col4;
    public Material col5;
  }
  public MaterialData materialData;
  private Material[] lMaterialData;

  [System.Serializable]
  public class BonusData{
    public int ballGain;
    public int hitBreak;
  }
  public BonusData bonusData;

  private GameObject blockPrefab;
  private GameObject animBreakBlockPrefab;

  private bool isGenerated = false;

  void Awake(){

    animBreakBlockPrefab = (GameObject)Resources.Load("prefabs/animations/BreakBlock");

    // // UniRx
    // Observable.EveryUpdate()
    //   .Skip(5)
    //   // .Where(_ => isGenerated)
    //   .Select(_ => Input.mousePosition)
    //   .Subscribe(
    //     _  => {},
    //     ex => {}
    //   );
  }

  void Start(){
    totalBlockCount = rowBlockCount * colBlockCount;
    destroyBlockCount = 0;

    setLHpData();
    setLScoreData();
    setLMaterialData();
    StartCoroutine(createBlocks(() => {
      isGenerated = true;
    }));

  }

  // public AnimationCurve curve;

  IEnumerator createBlocks(System.Action callBack){
    blockPrefab = (GameObject)Resources.Load("prefabs/Block");
    float tmpX;
    float tmpY;
    int colY = 0;
    int tmpHp;
    int tmpScore;
    Material tmpMaterial;
    GameObject tmpObj;
    for(int i=0; i<totalBlockCount; i++){
      if(i > 0 && i%rowBlockCount == 0){
        colY += 1;
      }
      tmpY  = posData.defY - (posData.rangeY * colY);
      tmpX  = posData.defX + (posData.rangeX * (i%rowBlockCount));
      tmpHp    = lHpData[colY];
      tmpScore = lScoreData[colY];
      tmpMaterial = lMaterialData[colY];

      tmpObj = createBlock(new Vector2(tmpX, tmpY));
      tmpObj.GetComponent<Block>().data.hp    = tmpHp;
      tmpObj.GetComponent<Block>().data.score = tmpScore;
      tmpObj.GetComponent<Renderer>().material = tmpMaterial;

      yield return new WaitForSeconds(0.05f);
    }
    callBack();
  }

  public bool isCompleted(){
    bool result = false;
    result = isGenerated;
    return result;
  }

  public bool LotBonusBallGain(){
    bool res = false;
    int randVal = new System.Random().Next(100);
    if(randVal < bonusData.ballGain){
      res = true;
    }
    return res;
  }

  public bool LotBonusHitBreak(){
    bool res = false;
    int randVal = new System.Random().Next(100);
    if(randVal < bonusData.hitBreak){
      res = true;
    }
    return res;
  }

  public void UpdateDestroyBlockCount(int cnt){
    destroyBlockCount += cnt;
  }

  public bool isAllDestroyBlock(){
    bool res = false;

    if(totalBlockCount == destroyBlockCount){
      res = true;
    }
    return res;
  }

  public void ShowAnimBreakBlock(Vector2 pos){
    var animBreakBlock = (GameObject)Instantiate(animBreakBlockPrefab);
    animBreakBlock.transform.position = pos;
    animBreakBlock.GetComponent<Animator>().Play("breakBlock", 0, 0.1f);
  }

  private GameObject createBlock(Vector2 pos){
    GameObject blockObj = (GameObject)Instantiate(blockPrefab);
    blockObj.transform.position = pos;

    blockObj.transform.SetParent(transform);

    return blockObj;
  }

  private void setLHpData(){
    var res = new int[]{
      hpData.col1,
      hpData.col2,
      hpData.col3,
      hpData.col4,
      hpData.col5
    };
    lHpData = res;
  }
  private void setLScoreData(){
    var res = new int[]{
      scoreData.col1,
      scoreData.col2,
      scoreData.col3,
      scoreData.col4,
      scoreData.col5
    };
    lScoreData = res;
  }
  private void setLMaterialData(){
    var res = new Material[]{
      materialData.col1,
      materialData.col2,
      materialData.col3,
      materialData.col4,
      materialData.col5
    };
    lMaterialData = res;
  }
}
