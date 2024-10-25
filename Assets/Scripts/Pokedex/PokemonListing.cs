using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PokemonListing : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image spriteImage;
    string pokemonUrl;
    public void OpenPokemonDetails()
    {
        PokemonCard.instance.SetPokemon(pokemonUrl, this);
    }
    
    public void SetPokemon(Pokemon pokemon)
    {
        nameText.text = pokemon.name;
        pokemonUrl = pokemon.url;
        StartCoroutine(FetchPokemonDetails());
    }
    IEnumerator FetchPokemonDetails()
    {
        UnityWebRequest request = UnityWebRequest.Get(pokemonUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            PokemonSpritesResponse imageUrl = JsonUtility.FromJson<PokemonSpritesResponse>(request.downloadHandler.text);
            StartCoroutine(GetSpriteFromUrl(imageUrl.sprites.front_default, sprite => spriteImage.sprite = sprite));
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
