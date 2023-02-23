using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowHighLight : MonoBehaviour
{
    Dictionary<Renderer, Material[]> glowMaterialDict = new Dictionary<Renderer, Material[]>();
    Dictionary<Renderer, Material[]> originalMaterialDict = new Dictionary<Renderer, Material[]>();

    Dictionary<Color, Material> cachedGlowMAterials = new Dictionary<Color, Material>();

    public Material glowMaterial;

    private bool isGlowing = false;

    Color validSpaceColor = Color.green;
    Color originalGlowColor;

    private void Awake()
    {
        PrepareMaterialDictionaries();
        originalGlowColor = glowMaterial.GetColor("_GlowColor");
    }
    void PrepareMaterialDictionaries()
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            Material[] originalMaterials = renderer.materials;
            originalMaterialDict.Add(renderer, originalMaterials);
            Material[] newMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < originalMaterials.Length; i++)
            {
                Material mat = null;
                if (cachedGlowMAterials.TryGetValue(originalMaterials[i].color,out mat) == false)
                {
                    mat = new Material(glowMaterial);
                    mat.color = originalMaterials[i].color;
                }
                newMaterials[i] = mat;
            }
            glowMaterialDict.Add(renderer, newMaterials);
        }
    }

    internal void HighlightValidPath()
    {
        if (isGlowing == false)
            return;
        foreach (Renderer renderer in glowMaterialDict.Keys)
        {
            foreach (var item in glowMaterialDict[renderer])
            {
                item.SetColor("_GlowColor", validSpaceColor);
            }
        }
    }

    internal void ResetGlowHighlight()
    {
        foreach (Renderer renderer in glowMaterialDict.Keys)
        {
            foreach(var item in glowMaterialDict[renderer])
            {
                item.SetColor("_GlowColor", originalGlowColor);
            }
        }
    }

    public void ToggleGlow()
    {
        if (isGlowing == false)
        {
            ResetGlowHighlight();
            foreach (Renderer renderer in originalMaterialDict.Keys)
            {
                renderer.materials = glowMaterialDict[renderer];
            }
        }
        else
        {
            foreach (Renderer renderer in originalMaterialDict.Keys)
            {
                renderer.materials = originalMaterialDict[renderer];
            }
        }
        isGlowing = !isGlowing;
    }
    public void ToggleGlow(bool state)
    {
        if (isGlowing == state)
            return;
        isGlowing = !state;
        ToggleGlow();
    }
}
