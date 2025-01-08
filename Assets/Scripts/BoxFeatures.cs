using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BoxFeatures : MonoBehaviour
{
     [SerializeField] private Sprite blueA;
     [SerializeField] private Sprite blueB;
     [SerializeField] private Sprite blueC;
     [SerializeField] private Sprite blueD;
                              
     [SerializeField] private Sprite greenA;
     [SerializeField] private Sprite greenB;
     [SerializeField] private Sprite greenC;
     [SerializeField] private Sprite greenD;
                              
     [SerializeField] private Sprite pinkA;
     [SerializeField] private Sprite pinkB;
     [SerializeField] private Sprite pinkC;
     [SerializeField] private Sprite pinkD;
                              
     [SerializeField] private Sprite purpleA;
     [SerializeField] private Sprite purpleB;
     [SerializeField] private Sprite purpleC;
     [SerializeField] private Sprite purpleD;
                              
     [SerializeField] private Sprite redA;
     [SerializeField] private Sprite redB;
     [SerializeField] private Sprite redC;
     [SerializeField] private Sprite redD;
                              
     [SerializeField] private Sprite yellowA;
     [SerializeField] private Sprite yellowB;
     [SerializeField] private Sprite yellowC;
     [SerializeField] private Sprite yellowD;
     
     private Sprite[] aTeam = new Sprite[6];
     private Sprite[] bTeam = new Sprite[6];
     private Sprite[] cTeam = new Sprite[6];
     private Sprite[] dTeam = new Sprite[6];

     [SerializeField] private GridManager _gridManager;
     [SerializeField] private int speed;
     
     
     
     public Sprite[] Ateam
     {
          get { return aTeam; }
     }
     
     public Sprite[] Bteam
     {
          get { return bTeam; }
     }
     public Sprite[] Cteam
     {
          get { return cTeam; }
     }
     public Sprite[] Dteam
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
     

     private Sprite[] AddGroup(Sprite b,Sprite g, Sprite p, Sprite pr,Sprite r, Sprite y, Sprite[] team)
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
