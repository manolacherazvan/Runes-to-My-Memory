using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Rune : MonoBehaviour
{
    [SerializeField] private GameObject highlighter;
    public GameObject runeCover;
    public SpriteRenderer runeSprite;
    int index;
    public TMP_Text indexText;
    public ParticleSystem particle;

    private void Start()
    {
        runeCover.SetActive(true);
        runeSprite.sprite = GameManager.instance.runeSpriteArray[index];
        indexText.gameObject.SetActive(GameManager.instance.getDebugMode());
        indexText.text = index.ToString();
    }
    private void OnMouseEnter()
    {
        highlighter.SetActive(true);
    }
    private void OnMouseExit()
    {
        highlighter.SetActive(false);
    }
    private void OnMouseDown()
    {
        GameManager.instance.selectRune(this.gameObject);
    }
    public void setIndex(int i)
    {
        index = i;
    }
    public int getIndex()
    {
        return index;
    }
    public void showRune()
    {
        runeCover.SetActive(false);
    }
    public void coverRune()
    {
        runeCover.SetActive(true);
    }
    private void OnDestroy()
    {
        ParticleSystem pe = Instantiate(particle, gameObject.transform.position, gameObject.transform.rotation);
        pe.Play();
    }
}