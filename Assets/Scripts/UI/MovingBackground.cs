using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousScrollingBackground : MonoBehaviour
{
    [SerializeField] private List<Sprite> backgroundImages;
    [SerializeField] private float scrollSpeed = 0.5f;
    [SerializeField] private Material blurMaterial;
    [SerializeField] private float blurSize = 1.0f;

    private List<GameObject> backgroundObjects = new List<GameObject>();
    private float spriteHeight;

    private void Start()
    {
        for (int i = 0; i < backgroundImages.Count; i++)
        {
            GameObject bg = new GameObject("Background_" + i);
            SpriteRenderer sr = bg.AddComponent<SpriteRenderer>();
            sr.sprite = backgroundImages[i];

            sr.material = blurMaterial;
            sr.material.SetFloat("_BlurSize", blurSize);

            spriteHeight = sr.bounds.size.y;
            bg.transform.position = new Vector3(0, i * spriteHeight, 0);
            bg.transform.parent = transform;
            backgroundObjects.Add(bg);
        }
    }

    private void Update()
    {
        foreach (var bg in backgroundObjects)
        {
            bg.transform.position += Vector3.down * scrollSpeed * Time.deltaTime;

            if (bg.transform.position.y < -spriteHeight)
            {
                float highestY = GetHighestBackgroundY();
                bg.transform.position = new Vector3(0, highestY + spriteHeight, 0);
            }
        }
    }

    private float GetHighestBackgroundY()
    {
        float highestY = float.MinValue;
        foreach (var bg in backgroundObjects)
        {
            if (bg.transform.position.y > highestY)
                highestY = bg.transform.position.y;
        }
        return highestY;
    }
}
