using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{

    [SerializeField]
    private string name;
    [SerializeField]
    private int posX;
    [SerializeField]
    private int posY;
    [SerializeField]
    private int woodStored;
    [SerializeField]
    private int stoneStored;
    [SerializeField]
    private int chestStored;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//Inspect Object
		if (Input.GetMouseButtonDown(1))
		{
			Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (hitCollider != null && hitCollider.transform == gameObject.transform)
			{
				UIManager.SetVisibleStorage(this.name, this.posX, this.posY, this.woodStored, this.stoneStored, this.chestStored, gameObject.GetComponent<SpriteRenderer>().sprite);
			}
		}
	}

    public void SetName(string name)
    {
        this.name = name;
    }

    public string GetName()
    {
        return this.name;
    }

    public void SetAttribute(string attribute, string value)
    {
        switch (attribute)
        {

            case "posX":
                this.posX = int.Parse(value);
                break;
            case "posY":
                this.posY = int.Parse(value);
                break;
            case "wood-stored":
                this.woodStored = int.Parse(value);
                break;
            case "stone-stored":
                this.stoneStored = int.Parse(value);
                break;
            case "chest-stored":
                this.chestStored = int.Parse(value);
                break;
            default:
                break;
        }
    }

    public void MoveToDestination(float tileSize, Vector2 position)
    {
        gameObject.transform.position = new Vector2(position.x + (posX * tileSize) - (tileSize / 2), position.y + (posY * tileSize) + (tileSize / 2));
    }

    public IEnumerator StoreWood()
    {
        int actionTime = 120;

        for (int i = 0; i < actionTime; i++)
        {
            yield return null;
        }

        //Actual effect
        this.woodStored++;

        //TODO - UpdateBeliefs
    }

    public IEnumerator StoreStone()
    {
        int actionTime = 120;

        for (int i = 0; i < actionTime; i++)
        {
            yield return null;
        }

        //Actual effect
        this.stoneStored++;

        //TODO - UpdateBeliefs
    }

    public IEnumerator StoreChest()
    {
        int actionTime = 120;

        for (int i = 0; i < actionTime; i++)
        {
            yield return null;
        }

        //Actual effect
        this.chestStored++;

        //TODO - UpdateBeliefs
    }

}
