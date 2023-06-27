using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    public struct TileAccess
    {
        int endX, endY;
        int amountOfSteps;
        bool hitsEnemy;
        public TileAccess(int _endX, int _endY, int _amountOfSteps, bool _hitsEnemy)
        {
            this.endX = _endX;
            this.endY = _endY;
            this.amountOfSteps = _amountOfSteps;
            this.hitsEnemy = _hitsEnemy;
        }

        public int GetEndX() { return this.endX; }
        public int GetEndY() { return this.endY; }  
        public int GetAmountOfSteps() { return this.amountOfSteps; }
        public bool GetHitsEnemy() { return this.hitsEnemy; }
    }

    public List<TileAccess> Run(int startX, int startY, int stepX, int stepY, int maxSteps, bool isWhite, string[,] board)
    {
        List<TileAccess> answer = new List<TileAccess>();
        (int x, int y) coords = (startX, startY);
        for (int i = 0; i < maxSteps; i++)
        {
            coords.x += stepX;
            coords.y += stepY;
            if (coords.y < 0 || coords.y > 15) return answer;
            if (coords.x < 0) coords.x += 4;
            if (coords.x > 3) coords.x -= 4;

            if (board[coords.x, coords.y] == null)
            {
                TileAccess accessibleTile = new TileAccess(coords.x, coords.y, i, false);
                answer.Add(accessibleTile);
            }
            else if ((board[coords.x, coords.y][0] == 'w' && isWhite) || (board[coords.x, coords.y][0] == 'b' && !isWhite))
            {
                return answer;
            }
            else
            {
                TileAccess enemyTile = new TileAccess(coords.x, coords.y, i, true);
                answer.Add(enemyTile);
                return answer;
            }
        }
        return answer;
    }
}
