using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObjectListUI : MonoBehaviour
{
    // image will be shown, and the sprites in images can be changed
    private int MaxImageNum;
    public Image[] Images;
    public string[] Keys;
    public Sprite[] Sprites;


    public Dictionary<string, Sprite> spriteMap;

    private void Awake()
    {
        MaxImageNum = Images.Length;
        int spriteMapSize = Mathf.Min(Keys.Length, Sprites.Length);
        spriteMap = new Dictionary<string, Sprite>(spriteMapSize);
        for(int i = 0; i < spriteMapSize; i++)
        {
            spriteMap.Add(Keys[i], Sprites[i]);
        }
    }

    public void SetSprites(List<string> keys)
    {
        if (keys.Count > MaxImageNum) Debug.Log("Too much sprites, only " + MaxImageNum + "will be shown.");
        int i = 0, j = 0;
        for(i = 0, j = 0; i < MaxImageNum && j < keys.Count; i++, j++)
        {
            if (spriteMap.ContainsKey(keys[j]))
            {
                Sprite sprite = spriteMap[keys[j]];
                Images[i].enabled = true;
                Images[i].sprite = sprite;
            }
            else
            {
                Debug.Log("Cannot find sprite " + keys[j]);
                i--;
            }
        }
        for(; i < MaxImageNum; i++)
        {
            Images[i].enabled = false;
        }
    }


}
