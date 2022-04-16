using UnityEngine;
using GamerWolf.Utils;
namespace HeroBrine{
    public enum VariationsType{
        Normal,Spawner,
    }
    public enum TurnType{
        Strainght,RightTurnOnly,LeftTurnOnly,T_Section,
    }
    public class LevelVariations : MonoBehaviour,IPooledObject{

        
        [SerializeField] private float lifeTime = 30f;
        [SerializeField] private VariationsType variationsType;
        [SerializeField] private LevelTurn levelTurn;
        [SerializeField] private Transform[] newObstacleSpawnPointArray;
        
        [SerializeField] private Transform playerSpawnPointRight,playerSpawnPointLeft;
        [SerializeField] private int rand;
        // [SerializeField] private LevelTurn turn;
        public Transform GetNewObstacleSpawnPoint(){
            return newObstacleSpawnPointArray[rand];
        }
        public Transform GetPlayerSpawnPointRight(){
            return playerSpawnPointRight;
        }
        public Transform GetPlayerSpawnPointLeft(){
            return playerSpawnPointLeft;
        }
        
        public void DestroyMySelfWithDelay(float delay = 0f){
            Invoke(nameof(DestroyWithOutDelay),delay);
        }
        public void DestroyWithOutDelay(){
            gameObject.SetActive(false);
        }

        public void OnObjectReuse(){
            rand = Random.Range(0,newObstacleSpawnPointArray.Length);
            DestroyMySelfWithDelay(lifeTime);
        }
        public Transform[] GetObstacleSpawnPointArray(){
            return newObstacleSpawnPointArray;
        }
        public LevelTurn GetLevelTurn(){
            return levelTurn;
        }
        
        
        

        
    }
    

}