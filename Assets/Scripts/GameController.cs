using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int level = 0;

    private GameObject winSpot;

    private BoxCollider2D winCollider;

    private GameObject player;

    private BoxCollider2D playerCollider;

    private SaveData saveData;

    public GameObject[] checkPoints;

    public int finalLevel;


    private void  Awake() {
        GameObject gameController=GameObject.Find("GameController");
        DontDestroyOnLoad(gameController);
        if(GameObject.FindGameObjectsWithTag(gameObject.tag).Length>1){
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        winSpot=GameObject.Find("Win");
        winCollider = winSpot.GetComponent<BoxCollider2D>();
        player=GameObject.Find("Player");
        playerCollider = player.GetComponent<BoxCollider2D>();
        if(saveData!=null){
            Debug.Log("Lo que sea");
            level = saveData.level;
            player.transform.position = new Vector3(saveData.position[0],saveData.position[1],saveData.position[2]);
            saveData=null;
        }
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (winCollider.IsTouching(playerCollider))
        {
            if (finalLevel == level)
            {
                SceneManager.LoadScene("Win");
            }
            else
            {
                level+=1;
                SceneManager.LoadScene("Level" + (level));
            
            }
        }

        /*foreach (var checkpoint in checkPoints)
        {
            BoxCollider2D checkPointCollider =
                checkpoint.GetComponent<BoxCollider2D>();
            if (checkPointCollider.IsTouching(playerCollider))
            {
                player.GetComponent<Player>().respawnpoint =
                    player.transform.position;
            }
        }*/
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if(Input.GetKeyDown(KeyCode.G)){
           SaveManager.SaveData(player.GetComponent<Player>(), this);

        }

        if(Input.GetKeyDown(KeyCode.L)){
          LoadData();
        }
    }

    public void Pause()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void LoadData(){
        SaveData loadedData = SaveManager.LoadData();
        if(loadedData!=null){
            saveData=loadedData;
            //Debug.Log(level);
            //Debug.Log(loadedData.level);
            if(level!=loadedData.level){
                SceneManager.LoadScene("Level"+loadedData.level);

            }else{
                level = saveData.level;
                player.transform.position=new Vector3(saveData.position[0],saveData.position[1],saveData.position[2]);
                saveData = null;
            }
        }
    }

}
