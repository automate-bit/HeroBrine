using UnityEngine;
using GamerWolf.Utils;
using System.Collections;
using System.Collections.Generic;
namespace HeroBrine {
    public class LevelManager : MonoBehaviour{

        [SerializeField] private PlayerController playerController;
        [SerializeField] private Transform startingPoint;
        [SerializeField] private ObjectPoolingManager poolingManager;
        [SerializeField] private PoolObjectTag[] straingRoadTagArray,rightTurnOnlyTagArray,leftTurnOnlyTagArray;
        private LevelVariations newVariations;
        private bool canSpawn;

        public int levelSegmentCrossed;
        public static LevelManager current{get;private set;}
        
        private List<LevelVariations> previouslySpawnedVariationsList;
        private void Awake(){
            current = this;
        }
        private void Start(){
            previouslySpawnedVariationsList = new List<LevelVariations>();
            playerController.OnPlayerTurn += (object sender,System.EventArgs e) => {
                startingPoint.gameObject.SetActive(false);
                // Invoke(nameof(DesableLastTurn),2f);
                // for (int i = 0; i < previouslySpawnedVariationsList.Count - 1; i++){
                //     previouslySpawnedVariationsList[i].DestroyWithOutDelay();
                // }
                // previouslySpawnedVariationsList = new List<LevelVariations>();
                int randAmountOfLevel = UnityEngine.Random.Range(5,30);
                SpawnNumberOfStraightRoad(randAmountOfLevel);
            };

            Invoke(nameof(SpawnInitialSegment),0.1f);
        }
        public void SpawnInitialSegment(){
            GameObject levelObject = poolingManager.SpawnFromPool(PoolObjectTag.LevelStraightRoad_1,startingPoint.position,Quaternion.identity);
            LevelVariations Obstaclelevel = levelObject.GetComponent<LevelVariations>();
            if(Obstaclelevel != null){
                newVariations = Obstaclelevel;
            }
            StartCoroutine(AddLevel(newVariations));
            
            SpawnNumberOfStraightRoad(4);
        }
        public void SpawnNumberOfStraightRoad(int totalAmount){
            if(canSpawn){
                for (int i = 0; i < totalAmount; i++){
                    if(i == totalAmount - 1){
                        int rand = Random.Range(0,10);
                        if(rand >= 2){
                            SpawnLeftTurnRoad();
                        }else if(rand == 5){
                            SpawnT_Section();
                        }else{
                            SpawnRightTurnRoad();
                        }
                    }else{
                        SpawnStraightRoad();
                    }

                }
            }
        }
        private void SpawnStraightRoad(){
            if(newVariations.GetLevelTurn().GetTurnType() == TurnType.T_Section){
                for (int i = 0; i < newVariations.GetObstacleSpawnPointArray().Length; i++){
                    GameObject levelObject = poolingManager.SpawnFromPool(GetRandomStraightTag(),newVariations.GetObstacleSpawnPointArray()[i].position,newVariations.GetNewObstacleSpawnPoint().rotation);
                    LevelVariations Obstaclelevel = levelObject.GetComponent<LevelVariations>();
                    newVariations = Obstaclelevel;        
                }   
            }else{
                GameObject levelObject = poolingManager.SpawnFromPool(GetRandomStraightTag(),newVariations.GetNewObstacleSpawnPoint().position,newVariations.GetNewObstacleSpawnPoint().rotation);
                LevelVariations Obstaclelevel = levelObject.GetComponent<LevelVariations>();
                newVariations = Obstaclelevel;

            }
            StartCoroutine(AddLevel(newVariations));
        }
        private IEnumerator AddLevel(LevelVariations level){
            yield return new WaitForSeconds(0.1f);
            if(newVariations != null){
                if(!previouslySpawnedVariationsList.Contains(level)){
                    previouslySpawnedVariationsList.Add(level);
                }
            }
        }
        private void SpawnRightTurnRoad(){
            if(newVariations.GetLevelTurn().GetTurnType() == TurnType.T_Section){
                for (int i = 0; i < newVariations.GetObstacleSpawnPointArray().Length; i++){
                    GameObject levelObject = poolingManager.SpawnFromPool(GetRandomRightTurnOnlyTag(),newVariations.GetObstacleSpawnPointArray()[i].position,newVariations.GetNewObstacleSpawnPoint().rotation);
                    LevelVariations Obstaclelevel = levelObject.GetComponent<LevelVariations>();
                    newVariations = Obstaclelevel;
                }
            }else{

                GameObject levelObject = poolingManager.SpawnFromPool(GetRandomRightTurnOnlyTag(),newVariations.GetNewObstacleSpawnPoint().position,newVariations.GetNewObstacleSpawnPoint().rotation);
                LevelVariations Obstaclelevel = levelObject.GetComponent<LevelVariations>();
                newVariations = Obstaclelevel;
            }
            StartCoroutine(AddLevel(newVariations));
        }
        private void SpawnLeftTurnRoad(){
            if(newVariations.GetLevelTurn().GetTurnType() == TurnType.T_Section){
                for (int i = 0; i < newVariations.GetObstacleSpawnPointArray().Length; i++){
                    GameObject levelObject = poolingManager.SpawnFromPool(GetRandomLeftTurnOnlyTag(),newVariations.GetObstacleSpawnPointArray()[i].position,newVariations.GetNewObstacleSpawnPoint().rotation);
                    LevelVariations Obstaclelevel = levelObject.GetComponent<LevelVariations>();
                    newVariations = Obstaclelevel;

                }
            }else{
                GameObject levelObject = poolingManager.SpawnFromPool(GetRandomLeftTurnOnlyTag(),newVariations.GetNewObstacleSpawnPoint().position,newVariations.GetNewObstacleSpawnPoint().rotation);
                LevelVariations Obstaclelevel = levelObject.GetComponent<LevelVariations>();
                newVariations = Obstaclelevel;
            }
            StartCoroutine(AddLevel(newVariations));

        }
        private void SpawnT_Section(){
            GameObject levelObject = poolingManager.SpawnFromPool(PoolObjectTag.T_Section,newVariations.GetNewObstacleSpawnPoint().position,newVariations.GetNewObstacleSpawnPoint().rotation);
            LevelVariations Obstaclelevel = levelObject.GetComponent<LevelVariations>();
            newVariations = Obstaclelevel;
            StartCoroutine(AddLevel(newVariations));
        }
        

        public void ToggleCanSpawn(bool value){
            canSpawn = value;
        }

        public void OnSegmentCrossed(){
            levelSegmentCrossed++;
        }
        private PoolObjectTag GetRandomStraightTag(){
            int rand = Random.Range(0,straingRoadTagArray.Length);
            return straingRoadTagArray[rand];
        }
        private PoolObjectTag GetRandomRightTurnOnlyTag(){
            int rand = Random.Range(0,leftTurnOnlyTagArray.Length);
            return leftTurnOnlyTagArray[rand];
        }
        private PoolObjectTag GetRandomLeftTurnOnlyTag(){
            int rand = Random.Range(0,rightTurnOnlyTagArray.Length);
            return rightTurnOnlyTagArray[rand];
        }
        

    }
}
