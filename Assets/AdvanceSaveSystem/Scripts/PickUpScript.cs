using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public Color[] colorList; //lsit of colors 

    [SerializeField] private MeshRenderer cubeRenderer;//ref to cube renderer

    void OnEnable()
    {
        //on enalbe we set the color of renederer
        cubeRenderer.material.color = colorList[Random.Range(0, colorList.Length)];
    }
}
