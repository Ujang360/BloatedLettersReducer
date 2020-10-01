﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NullUntracker))]
public class LettersAligner : MonoBehaviour
{
    [SerializeField]
    private float PositionXOffset = 0.9f;

    private bool repositionedBefore = false;
    private List<GameObject> letters;
    private NullUntracker untracker;

    private float GetWordWidth()
    {
        var widthTotalLetters = 0f;
        letters.ForEach(x => widthTotalLetters += ((RectTransform)x.transform).rect.width);
        return widthTotalLetters;
    }

    private float GetLeftPosition(float totalWidth) => PositionXOffset + (-1f * (totalWidth / 2f));

    private void RepositionLetters(float leftMostPosition)
    {
        var previousWidth = 0f;

        for (var i = 0; i < letters.Count; ++i)
        {
            var positionX = leftMostPosition + previousWidth;
            previousWidth += ((RectTransform)letters[i].transform).rect.width;
            iTween.MoveTo(letters[i], new Vector3(positionX, 0, 0), 1);
        }
    }

    public void InitLetters(ref List<GameObject> letters)
    {
        this.letters = letters;
        untracker = GetComponent<NullUntracker>();
        untracker.InitTrackable(ref letters);
    }

    public bool TryRepositionLetters()
    {
        if (!untracker.TryUntrack() && repositionedBefore)
        {
            return false;
        }

        var wordWidth = GetWordWidth();
        var leftMostPosition = GetLeftPosition(wordWidth);
        RepositionLetters(leftMostPosition);
        repositionedBefore = true;

        return true;
    }
}
