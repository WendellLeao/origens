using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedTree : EnemyController, IRevealable
{
    [Header("Sprites")]
    [SerializeField] private Sprite cursedTreeSprite;
    [SerializeField] private Sprite treeSprite;
    private bool wasRevealed = false;
    private bool canBeHitted = true;

    void FixedUpdate()
    {
        SpriteHandler();
    }

    private void SpriteHandler()
    {
        if(!BloquinhoBrancoController.IsBloquinhoBrancoStatic() || wasRevealed)
            GetComponent<SpriteRenderer>().sprite = cursedTreeSprite;
        else
            GetComponent<SpriteRenderer>().sprite = treeSprite;
    }

    public void Reveal()
    {
        wasRevealed = true;
    }
}
