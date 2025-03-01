using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlashMaterialOnHit : MonoBehaviour
{
    [SerializeField] Material FlashMaterial;
    List<SpriteRenderer> AllSprites = new();
    List<Material> CopyOfAllMaterials = new();

    Coroutine FlashCoroutine;

    private void OnDisable()
    {
        for (int i = 0; i < AllSprites.Count; i++)
        {
            AllSprites[i].material = CopyOfAllMaterials[i];
        }
    }

    private void Start()
    {
        AllSprites = GetComponentsInChildren<SpriteRenderer>().ToList();
        for (int i = 0; i < AllSprites.Count; i++)
        {
            CopyOfAllMaterials.Add(AllSprites[i].material);
        }
    }

    public void Flash(float duration = 0.1f)
    {
        if (FlashCoroutine != null) return;
        Debug.Log("Flash");
        FlashCoroutine = StartCoroutine(FlashMaterials(duration));
    }

    private IEnumerator FlashMaterials(float duration)
    {
        foreach (var sprite in AllSprites)
        {
            sprite.material = FlashMaterial;
        }
        yield return new WaitForSeconds(duration);
        for (int i = 0; i < AllSprites.Count; i++)
        {
            AllSprites[i].material = CopyOfAllMaterials[i];
        }
        FlashCoroutine = null;
    }
}
