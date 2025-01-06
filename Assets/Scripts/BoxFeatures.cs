using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BoxFeatures : MonoBehaviour
{
     [SerializeField] private GameObject blueA;
     [SerializeField] private GameObject blueB;
     [SerializeField] private GameObject blueC;
     [SerializeField] private GameObject blueD;
     
     [SerializeField] private GameObject greenA;
     [SerializeField] private GameObject greenB;
     [SerializeField] private GameObject greenC;
     [SerializeField] private GameObject greenD;
     
     [SerializeField] private GameObject pinkA;
     [SerializeField] private GameObject pinkB;
     [SerializeField] private GameObject pinkC;
     [SerializeField] private GameObject pinkD;
    
     [SerializeField] private GameObject purpleA;
     [SerializeField] private GameObject purpleB;
     [SerializeField] private GameObject purpleC;
     [SerializeField] private GameObject purpleD;
      
     [SerializeField] private GameObject redA;
     [SerializeField] private GameObject redB;
     [SerializeField] private GameObject redC;
     [SerializeField] private GameObject redD;
     
     [SerializeField] private GameObject yellowA;
     [SerializeField] private GameObject yellowB;
     [SerializeField] private GameObject yellowC;
     [SerializeField] private GameObject yellowD;
     
     private GameObject[] aTeam = new GameObject[6];
     private GameObject[] bTeam = new GameObject[6];
     private GameObject[] cTeam = new GameObject[6];
     private GameObject[] dTeam = new GameObject[6];
     
     public GameObject[] Ateam
     {
          get { return aTeam; }
     }
     
     public GameObject[] Bteam
     {
          get { return bTeam; }
     }
     public GameObject[] Cteam
     {
          get { return cTeam; }
     }
     public GameObject[] Dteam
     {
          get { return dTeam; }
     }
     private void Start()
     {
          aTeam = AddGroup(blueA, greenA, pinkA, purpleA, redA, yellowA, aTeam);
          bTeam = AddGroup(blueB, greenB, pinkB, purpleB, redB, yellowB, bTeam);
          cTeam = AddGroup(blueC, greenC, pinkC, purpleC, redC, yellowC, cTeam);
          dTeam = AddGroup(blueD, greenD, pinkD, purpleD, redD, yellowD, dTeam);
     }

     private GameObject[] AddGroup(GameObject b,GameObject g, GameObject p, GameObject pr,GameObject r, GameObject y, GameObject[] team)
     {
          team[0] = b;
          team[1] = g;
          team[2] = p;
          team[3] = pr;
          team[4] = r;
          team[5] = y;

          return team;
     }
}
