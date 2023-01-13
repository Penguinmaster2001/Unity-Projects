using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameLists : MonoBehaviour
{
    public static string[] alienNames   = new string[4]     {"Nortblac", "voschk", "rathms", "turgh"};

    public static string[] tPlanetNames = new string[15]    { "Zentar", "Huner", "Endaenia", "Chelneturn", "Uchomia", "Itrippe", "Liupra", "Rania", "Bangutune", "Tundoria", "Udrora", "Getania", "Ocury", "Cevinus", "Goyuhiri"};

    public static string[] gPlanetNames = new string[15]    { "Zentar", "Huner", "Endaenia", "Chelneturn", "Uchomia", "Itrippe", "Liupra", "Rania", "Bangutune", "Tundoria", "Udrora", "Getania", "Ocury", "Cevinus", "Goyuhiri" };

    public Texture[] portraits;
    public static Texture[] alienPortraits;

    void Start()
    {
        alienPortraits = portraits;
    }
}