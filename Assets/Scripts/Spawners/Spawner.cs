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

    [Header("Do not assign anything to these")]
    public GameObject single3DLetterModel;
    public GameObject activeLetterCube;
    public string letterChoosen;

    public LetterCubeEventHandler activeLetterCubeEventHandler;
    public LetterCubeMovement letterCubeMovement;

    public virtual void OnPlacedInSlot(string letterOfSlotSensor) { }

}
