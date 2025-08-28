using System;
using UnityEngine;
public class Spawner : MonoBehaviour
{
    public GameObject letters3DModels;
    public GameObject letterCubeModel;
    public Cinemachine.CinemachineFreeLook cineFreeCam;
    public float letterCubeScale = 97f;
    public PlaygroundType playgroundType = PlaygroundType.Alphabet;
    public SlotSensorsHandler slotSensorsHandler;
    public GameDataSave gameDataSave;

    //
    protected GameObject single3DLetterModel;
    protected GameObject activeLetterCube;
    protected string letterChoosen;
    protected LetterCubeEventHandler activeLetterCubeEventHandler;
    protected LetterCubeMovement letterCubeMovement;
    public virtual void OnPlacedInSlot(string letterOfSlotSensor) { }

}
