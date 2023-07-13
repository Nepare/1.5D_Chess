using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    public class TileAccess
    {
        int endX, endY;
        int horizontalDistance;
        bool hitsEnemy;
        public TileAccess(int _endX, int _endY, int _horizontalDistance, bool _hitsEnemy)
        {
            this.endX = _endX;
            this.endY = _endY;
            this.horizontalDistance = _horizontalDistance;
            this.hitsEnemy = _hitsEnemy;
        }

        public int GetEndX() { return this.endX; }
        public int GetEndY() { return this.endY; }  
        public int GetHorizontalDistance() { return this.horizontalDistance; }
        public bool GetHitsEnemy() { return this.hitsEnemy; }
    }

    public List<TileAccess> Run(int startX, int startY, int stepX, int stepY, int maxSteps, bool isWhite, string[,] board)
    {
        List<TileAccess> answer = new List<TileAccess>();
        (int x, int y) coords = (startX, startY);
        int horizontalDistance = 0;
        for (int i = 0; i < maxSteps; i++)
        {
            coords.x += stepX;
            horizontalDistance += stepX;
            coords.y += stepY;
            if (coords.y < 0 || coords.y > 15) return answer;
            if (coords.x < 0) coords.x += 4;
            if (coords.x > 3) coords.x -= 4;

            if (board[coords.x, coords.y] == null)
            {
                TileAccess accessibleTile = new TileAccess(coords.x, coords.y, horizontalDistance, false);
                answer.Add(accessibleTile);
            }
            else if ((board[coords.x, coords.y][0] == 'w' && isWhite) || (board[coords.x, coords.y][0] == 'b' && !isWhite))
            {
                return answer;
            }
            else
            {
                TileAccess enemyTile = new TileAccess(coords.x, coords.y, horizontalDistance, true);
                answer.Add(enemyTile);
                return answer;
            }
        }
        return answer;
    }
}
