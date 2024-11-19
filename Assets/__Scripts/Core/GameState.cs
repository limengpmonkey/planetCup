using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State
{
    Login,
    LevelOne,
    LevelTwo,
    LevelThree,
}
public class GameState : MonoBehaviour
{
    private static GameState instance;
    [SerializeField] private State _state;
    
    public static GameState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameState>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameState");
                    instance = obj.AddComponent<GameState>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGameState(State state)
    {
        _state = state;
    }
}
