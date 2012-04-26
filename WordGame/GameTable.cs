using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace WordGame {
    class GameTable {
        Dictionary<GameObject, GameCellPos> objectDict = new Dictionary<GameObject, GameCellPos>();
        int width;
        int height;
        GameCell[][] cells;
        public int Width {
            get { return width; }
        }
        public int Height {
            get { return height; }
        }
        public GameTable(int width, int height) {
            this.width = width;
            this.height = height;
            cells = new GameCell[height][];
            for (int i = 0; i < height; i++)
                cells[i] = new GameCell[width];
        }
        public GameCell this[int row, int column] {
            get { return cells[row][column]; }
        }
        public GameCell this[GameCellPos position] {
            get { return cells[position.Row][position.Column]; }
        }

        public void AddObject(GameObject obj, int startRow, int startColumn) {
            if (TryMoveObjectHere(obj, startRow, startColumn) != CanMoveResult.Success) throw new InvalidOperationException(string.Format("Can't place object here {0}:{1}.", startRow, startColumn));
        }

        public bool RemoveObject(GameObject obj) {
            GameCellPos oldStart;
            if (!objectDict.TryGetValue(obj, out oldStart)) return false;
            ClearObjectCells(obj, oldStart.Row, oldStart.Column);
            objectDict.Remove(obj);
            return true;
        }

        void FillObjectCells(GameObject obj, int startRow, int startColumn) {
            for (int i = 0; i < obj.Height; i++) {
                for (int j = 0; j < obj.Width; j++) {
                    if (!obj.Bitmap[i, j]) continue;
                    cells[i + startRow][j + startColumn] = new GameCell(obj);
                }
            }
        }

        void ClearObjectCells(GameObject obj, int startRow, int startColumn) {
            for (int i = 0; i < obj.Height; i++) {
                for (int j = 0; j < obj.Width; j++) {
                    if (!obj.Bitmap[i, j]) continue;
                    cells[i + startRow][j + startColumn] = new GameCell(null);
                }
            }
        }

        public CanMoveResult CanMoveObjectHere(GameObject obj, int startRow, int startColumn) {
            for (int i = 0; i < obj.Height; i++) {
                for (int j = 0; j < obj.Width; j++) {
                    if (!obj.Bitmap[i, j]) continue;
                    int iTable = i + startRow;
                    int jTable = j + startColumn;
                    if (iTable < 0 || iTable >= Height || jTable < 0 || jTable >= Width) return CanMoveResult.TableBoundsCollision;
                }
            }
            for (int i = 0; i < obj.Height; i++) {
                for (int j = 0; j < obj.Width; j++) {
                    if (!obj.Bitmap[i, j]) continue;
                    int iTable = i + startRow;
                    int jTable = j + startColumn;
                    if (cells[iTable][jTable].GameObject != null && cells[iTable][jTable].GameObject != obj) return CanMoveResult.ObjectCollision;
                }
            }
            return CanMoveResult.Success;
        }

        public CanMoveResult TryMoveObjectHere(GameObject obj, int startRow, int startColumn) {
            CanMoveResult result = CanMoveObjectHere(obj, startRow, startColumn);
            if (result != CanMoveResult.Success) return result;
            RemoveObject(obj);
            FillObjectCells(obj, startRow, startColumn);
            objectDict[obj] = new GameCellPos(startRow, startColumn);
            return CanMoveResult.Success;
        }

        public CanMoveResult TryMoveObject(GameObject obj, int deltaRow, int deltaColumn) {
            GameCellPos start;
            if (!TryGetObjectPosition(obj, out start)) return CanMoveResult.ObjectNotFound;
            return TryMoveObjectHere(obj, start.Row + deltaRow, start.Column + deltaColumn);
        }

        public bool TryGetObjectPosition(GameObject obj, out GameCellPos position) {
            return objectDict.TryGetValue(obj, out position);
        }
    }

    public enum CanMoveResult {
        Success,
        ObjectCollision,
        TableBoundsCollision,
        ObjectNotFound
    }

    struct GameCell {
        GameObject gameObject;
        public GameObject GameObject {
            get { return gameObject; }
        }
        public GameCell(GameObject gameObject) {
            this.gameObject = gameObject;                 
        }
    }

    struct GameCellPos {
        public int Row;
        public int Column;
        public GameCellPos(int row, int column) {
            Row = row;
            Column = column;
        }
        public void Offset(int dRow, int dColumn) {
            Row += dRow;
            Column += dColumn;
        }
        public void Offset(GameCellPos dPos) {
            Row += dPos.Row;
            Column += dPos.Column;
        }
        public void MoveLeft() {
            Column -= 1;
        }
        public void MoveRight() {
            Column += 1;
        }
        public void MoveUp() {
            Row -= 1;
        }
        public void MoveDown() {
            Row += 1;
        }
        public override string ToString() {
            return string.Format("(R = {0}: C = {1})", Row, Column);
        }
    }
}