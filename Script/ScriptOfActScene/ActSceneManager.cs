using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ActSceneManager : MonoBehaviour
{
    public enum PHASE { EDITING, PLAYING }
    PHASE phase;

    [SerializeField] GameObject playModeCameraObj;
    [SerializeField] GameObject editModeCameraObj;
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject tilemapObj;
    [SerializeField] GameObject bgmObj;
    [SerializeField] GameObject timerObj;

    EditViewManager editViewManager;
    PlayerManager playerManager;
    TilemapManager tilemapManager;
    BgmManager bgmManager;
    TextTimerManager textTimerManager;

    string mode;

    void Start()
    {
        editViewManager = editModeCameraObj.GetComponent<EditViewManager>();
        playerManager = playerObj.GetComponent<PlayerManager>();
        tilemapManager = tilemapObj.GetComponent<TilemapManager>();
        bgmManager = bgmObj.GetComponent<BgmManager>();
        textTimerManager = timerObj.GetComponent<TextTimerManager>();

        GameObject dontDestoyObj = GameObject.Find("DontDestroy");
        mode = dontDestoyObj.GetComponent<DontDestroyManager>().Mode;

        if (mode == "Edit")
        {
            phase = PHASE.EDITING;
        }
        else if (mode == "Play")
        {
            ChangePhaseToPlaying();
        }
    }

    void Update()
    {
        if (phase == PHASE.EDITING)
        {
            if(mode == "Play") return;

            playerManager.PlayerChangePlace();
            tilemapManager.UpdateTile();
        }
        else if (phase == PHASE.PLAYING)
        {
            playerManager.PlayerJump();
            playerManager.PlayerAttack();
            playerManager.PlayerAnimate();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(mode == "Play") return;
            
            if (phase == PHASE.EDITING)
            {
                ChangePhaseToPlaying();
            }
            else if (phase == PHASE.PLAYING)
            {
                ChangePhaseToEditing();
            }
        }
    }
    void ChangePhaseToPlaying()
    {            
        phase = PHASE.PLAYING;

        bgmManager.PlayPlayingBgm();

        editModeCameraObj.SetActive(false);
        playModeCameraObj.SetActive(true);

        tilemapManager.SpawnEnemy();

        playerManager.Activate();

        textTimerManager.StartTimer();
    }
    void ChangePhaseToEditing()
    {            
        phase = PHASE.EDITING;

        bgmManager.PlayEditingBgm();

        playModeCameraObj.SetActive(false);
        editModeCameraObj.SetActive(true);

        tilemapManager.ResetEnemy();
        tilemapManager.ResetTile();
        tilemapManager.StopAllDelaySpreadFireCoroutines();

        playerManager.ResetStatus();
        playerManager.AttackReset();
        playerManager.ItemReset();

        textTimerManager.EndTimer();
    }

    void FixedUpdate()
    {
        if (phase == PHASE.EDITING)
        {
            if(mode == "Play") return;
            
            editViewManager.MoveEditView();
        }
        else if (phase == PHASE.PLAYING)
        {
            playerManager.PlayerMove();
        }
    }
}
