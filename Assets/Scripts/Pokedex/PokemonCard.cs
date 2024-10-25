using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PokemonCard : MonoBehaviour
{
    public static PokemonCard instance;
    
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image spriteImage;
    [SerializeField] TextMeshProUGUI heightText;
    [SerializeField] TextMeshProUGUI weightText;
    [SerializeField] TextMeshProUGUI baseExperienceText;
    [SerializeField] TextMeshProUGUI typeText;
    [SerializeField] TextMeshProUGUI statsText;

    public UnityEvent OnPokemonCardOpen;
    
    private void Awake()
    {
        instance = this;
    }

    public void SetPokemon(string pokemonApiUrl, PokemonListing pokemonListing)
    {
        OnPokemonCardOpen.Invoke();
        nameText.text = pokemonListing.name;
        spriteImage.sprite = pokemonListing.spriteImage.sprite;
        StartCoroutine(FetchPokemonDetails(pokemonApiUrl));
    }

    IEnumerator FetchPokemonDetails(string pokemonApiUrl)
    {
        UnityWebRequest request = UnityWebRequest.Get(pokemonApiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            PokemonDetails details = JsonUtility.FromJson<PokemonDetails>(request.downloadHandler.text);
            
            nameText.text = details.id + ". " + details.name;
            heightText.text = "Height: " + details.height;
            weightText.text = "Weight: " + details.weight;
            baseExperienceText.text = "Experience: " + details.base_experience;

            typeText.text = string.Join(", ", details.types.ConvertAll(t => t.type.name));

            statsText.text = "";
            foreach (var stat in details.stats)
            {
                statsText.text += stat.stat.name + ": " + stat.base_stat + "\n";
            }
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}
