using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObjectListUI : MonoBehaviour
{
    // image will be shown, and the sprites in images can be changed
    private int MaxImageNum;
    public int CurrentActiveImageNum;
    public Image[] Images;
    public string[] Keys;
    public Sprite[] Sprites;

    public int currentIdx;
    private bool[] chosenImages;
    public string NextButton = "Next1";
    public string PreviousButton = "Prev1";
    public string ChooseButton = "Choose1";
    
    

    public Dictionary<string, Sprite> spriteMap;

    private void Awake()
    {
        // check current active images
        MaxImageNum = Images.Length;
        //Debug.Log("MaxImageNum = " + MaxImageNum);
        foreach(Image image in Images)
        {
            if (image.enabled) CurrentActiveImageNum++;
            else break;
        }
        // Image can be enabled only if its previous one is enabled
        for (int i = CurrentActiveImageNum+1; i < MaxImageNum; i++)
        {
            Images[i].enabled = false;
        }

        // init sprite map
        int spriteMapSize = Mathf.Min(Keys.Length, Sprites.Length);
        //Debug.Log("spriteMapSize = " + spriteMapSize);
        spriteMap = new Dictionary<string, Sprite>(spriteMapSize);
        for(int i = 0; i < spriteMapSize; i++)
        {
            spriteMap.Add(Keys[i], Sprites[i]);
        }

        // unchoose every image and point to the first image
        chosenImages = new bool[MaxImageNum];
        ResetChoices();
        SetSprites(new List<string>(Keys));
    }

    private void Update()
    {
        if (Input.GetButtonDown(NextButton))
        {
            Next();
        }
        else if (Input.GetButtonDown(PreviousButton))
        {
            Previous();
        }
        else if (Input.GetButtonDown(ChooseButton))
        {
            if (chosenImages[currentIdx]) Unchoose();
            else Choose();
        }
    }


    public void SetSprites(List<string> keys)
    {
        if (keys.Count > MaxImageNum) Debug.Log("Too much sprites, only " + MaxImageNum + "will be shown.");
        CurrentActiveImageNum = Mathf.Min(keys.Count, MaxImageNum);
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
        ResetChoices();
    }

    // unchoose every image and point to the first image
    public virtual void ResetChoices()
    {
        for (int i = 0; i < chosenImages.Length; i++)
        {
            chosenImages[i] = false;
        }
        for(currentIdx = 0; currentIdx < CurrentActiveImageNum; currentIdx++)
        {
            Unpoint();
        }
        currentIdx = 0;
        Pointing();
    }

    // set restrictions on choosing, default: at most one object can be chosen at the same time
    public virtual bool canChoose()
    {
        for(int i = 0; i < chosenImages.Length; i++)
        {
            if (chosenImages[i])return false;
        }
        return true;
    }

    public virtual void Next()
    {
        Unpoint();
        currentIdx++;
        if (currentIdx >= CurrentActiveImageNum) currentIdx = 0;
        Pointing();
    }

    public virtual void Previous()
    {
        Unpoint();
        currentIdx--;
        if (currentIdx < 0) currentIdx = CurrentActiveImageNum - 1;
        Pointing();
    }

    public virtual void Choose()
    {
        if (canChoose())
        {
            chosenImages[currentIdx] = true;
            Images[currentIdx].color = new Color(255, 255, 255, 0f);
        }
    }

    public virtual void Unchoose()
    {
        chosenImages[currentIdx] = false;
        Pointing();
    }

    public virtual void Pointing()
    {
        if (!chosenImages[currentIdx]) Images[currentIdx].color = new Color(255, 255, 255, 1f);
    }
    public virtual void Unpoint()
    {
        if(!chosenImages[currentIdx]) Images[currentIdx].color = new Color(255, 255, 255, .5f);
    }
}
