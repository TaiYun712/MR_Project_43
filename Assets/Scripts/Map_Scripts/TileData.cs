using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class TileData
{
    public TileKind kind;
    public HabitaKind habitat = HabitaKind.None;
}

public enum SetTownType
{
    None,
    City,
    Village,
    Industry
}

public enum TileKind
{   Empty, 
    Land,
    Water,
    TownCity,
    TownVillage,
    TownIndustry
}

public enum HabitaKind
{
    None,
    LandBasic,
    LandAdvanced, 
    WetBasic,
    WetAdvanced
}




