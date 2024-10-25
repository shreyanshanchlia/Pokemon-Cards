using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 

public class PokemonList : MonoBehaviour
{
    [SerializeField] PokemonListing pokemonPrefab;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Transform content; 

    private bool isLoading = false;
    private string nextUrl = "https://pokeapi.co/api/v2/pokemon?limit=10&offset=0";

    private void Start()
    {
        LoadNextPage();
        scrollRect.onValueChanged.AddListener(OnScroll);
    }
    
    private void OnScroll(Vector2 position)
    {
        if (scrollRect.content.anchoredPosition.y >= scrollRect.content.sizeDelta.y - scrollRect.viewport.rect.height - Screen.height * 0.8f && !isLoading)
        {
            LoadNextPage();
        }
    }

    private void LoadNextPage()
    {
        if (!string.IsNullOrEmpty(nextUrl))
        {
            isLoading = true;
            StartCoroutine(GetPokemonList(nextUrl));
        }
    }
    
    IEnumerator GetPokemonList(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            PokemonListResponse response = JsonUtility.FromJson<PokemonListResponse>(request.downloadHandler.text);

            nextUrl = response.next;

            foreach (var pokemon in response.results)
            {
                PokemonListing pokemonItem = Instantiate(pokemonPrefab, content);
                pokemonItem.SetPokemon(pokemon);
            }
        }
        else
        {
            Debug.LogError(request.error);
        }
        isLoading = false;
    }
}

[System.Serializable]
public class PokemonListResponse
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
public class PokemonSpritesResponse
{
    public PokemonSprites sprites;
}

[System.Serializable]
public class PokemonSprites
{
    public string front_default;
}
