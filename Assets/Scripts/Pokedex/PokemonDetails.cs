using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PokemonDetails
{
	public string name;
	public int id;
	public int height;
	public int weight;
	public int base_experience;
	public List<Stat> stats;
	public Sprite sprite;
	public List<TypeEntry> types;
}

[System.Serializable]
public class TypeEntry
{
	public TypeInfo type;
}
[System.Serializable] public class TypeInfo
{
	public string name;
}

[System.Serializable]
public class Stat
{
	public StatInfo stat;
	public int base_stat;
}
[System.Serializable] public class StatInfo
{
	public string name;
}
