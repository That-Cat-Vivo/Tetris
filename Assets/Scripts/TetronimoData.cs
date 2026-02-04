using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


    // Added Pieces are at end of enum.
    public enum Tetronimo { I, O, T, J, L, S, Z, lowt, BIGJ,}

    [Serializable]
    public struct TetronimoData
    {
        public Tetronimo tetronimo;
        public Vector2Int[] cells;
        public Tile tile;
    }

