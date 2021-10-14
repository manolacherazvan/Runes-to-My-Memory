using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    private float tileSizeX=0.7f;
    private float tileSizeY=1.05f;
    private float offsetX;
    private float offsetY;

    public GameObject runePrefab;

    public Sprite[] spriteArray;


    private void calculateOffset(int randuri, int coloane)
    {
        offsetX = (coloane * tileSizeX/2)-(tileSizeX/2);
        offsetY = (randuri * tileSizeY/2)-(tileSizeY/2);
    }

    public void generateGrid(int randuri, int coloane)
    {
        calculateOffset(randuri, coloane);

        for (int i = 0; i < coloane; i++)
        {
            for (int j = 0; j < randuri; j++)
            {
                GameManager.instance.Runes[i,j] = (GameObject)Instantiate(runePrefab, new Vector3(i * tileSizeX - offsetX, j * -tileSizeY + offsetY), Quaternion.identity);
                GameManager.instance.Runes[i, j].name = $"Rune{GameManager.instance.runeIndexes[i, j]}: {i} {j}";
                GameManager.instance.Runes[i, j].GetComponent<Rune>().setIndex(GameManager.instance.runeIndexes[i, j]);
            }
        }
    }


}
