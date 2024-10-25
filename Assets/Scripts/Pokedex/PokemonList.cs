using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI; // For handling text display

public class PokemonList : MonoBehaviour
{
    public GameObject pokemonPrefab; // Prefab for displaying Pokémon in the list
    public Transform content; // ScrollView content holder

    private string nextUrl = "https://pokeapi.co/api/v2/pokemon?limit=10&offset=0";

    private void Start()
    {
        FetchPokemonList();
    }

    // Method to fetch the list of Pokémon
    public void FetchPokemonList()
    {
        if (!string.IsNullOrEmpty(nextUrl))
        {
            StartCoroutine(GetPokemonList(nextUrl));
        }
    }
    
    IEnumerator GetPokemonList(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            PokemonResponse response = JsonUtility.FromJson<PokemonResponse>(request.downloadHandler.text);

            nextUrl = response.next;

            foreach (var pokemon in response.results)
            {
                GameObject pokemonItem = Instantiate(pokemonPrefab, content);
                pokemonItem.GetComponentInChildren<TMP_Text>().text = pokemon.name;

                StartCoroutine(FetchPokemonDetails(pokemon.url, pokemonItem));
            }
        }
        else
        {
            Debug.LogError(request.error);
        }
    }

    // Fetch detailed info about a Pokémon, including its image
    IEnumerator FetchPokemonDetails(string pokemonUrl, GameObject pokemonItem)
    {
        UnityWebRequest request = UnityWebRequest.Get(pokemonUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            PokemonDetails details = JsonUtility.FromJson<PokemonDetails>(request.downloadHandler.text);
            StartCoroutine(GetSpriteFromUrl(details.sprites.front_default, sprite => pokemonItem.GetComponentInChildren<Image>().sprite = sprite));

        }
        else
        {
            Debug.LogError(request.error);
        }
    }
    
    IEnumerator GetSpriteFromUrl(string url, System.Action<Sprite> callback)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            callback(sprite);
        }
        else
        {
            Debug.LogError(request.error);
            callback(null);
        }
    }
}

[System.Serializable]
public class PokemonResponse
{
    public int count;
    public string next;
    public string previous;
    public List<Pokemon> results;
}

[System.Serializable]
public class Pokemon
{
    public string name;
    public string url;
}

[System.Serializable]
public class PokemonDetails
{
    public PokemonSprites sprites;
}

[System.Serializable]
public class PokemonSprites
{
    public string front_default;
}
