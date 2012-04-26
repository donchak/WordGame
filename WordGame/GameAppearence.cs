using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WordGame {
    class GameAppearence {
        bool firstDraw = true;
        float cellSize;
        Size screenSize;
        PointF tableStart;
        Font mainFont;
        byte[] grayCellImageData;
        readonly GameMap<DrawCellInfo> prevDrawMap;
        readonly Game game;
        readonly Random randomize;
        readonly Dictionary<GameObject, Color> objectColorDict = new Dictionary<GameObject, Color>();
        readonly Dictionary<GameObject, Image> objectImageDict = new Dictionary<GameObject, Image>();
        public Font MainFont {
            get {
                return mainFont;
            }
            set {
                mainFont = value;
            }
        }
        public byte[] GrayCellImageData {
            get {
                return grayCellImageData;
            }
            set {
                grayCellImageData = value;
            }
        }
        public GameAppearence(Game game) {
            this.game = game;
            this.prevDrawMap = new GameMap<DrawCellInfo>(game.MainTable.Width + 1, game.MainTable.Height, true);
            this.randomize = new Random(DateTime.Now.GetHashCode());
        }
        public float GetCellSize(Size screenSize) {
            double rel = (double)game.MainTable.Width / game.MainTable.Height;
            double screenRel = (double)screenSize.Width / screenSize.Height;
            float size;
            if (rel >= screenRel) {
                size = (float)screenSize.Width / game.MainTable.Width;
            } else {
                size = (float)screenSize.Height / game.MainTable.Height;
            }
            return size;
        }
        public Image GetObjectImage(GameObject obj, Color objectColor) {
            if (obj == null || grayCellImageData == null) return null;
            Image objectImage;
            if (!objectImageDict.TryGetValue(obj, out objectImage)) {
                objectImage = CommonHelper.GetColorImage(grayCellImageData, objectColor);
                objectImageDict.Add(obj, objectImage);
            }
            return objectImage;
        }
        public Color GetObjectColor(GameObject obj) {
            if (obj == null) {
                return Color.MidnightBlue;
            }
            Color color;
            if (!objectColorDict.TryGetValue(obj, out color)) {
                color = Color.FromArgb(100 + randomize.Next(155),
                    100 + randomize.Next(155),
                    100 + randomize.Next(155));
                objectColorDict.Add(obj, color);
            }
            return color;
        }
        public bool RemoveObjectFromColorCache(GameObject obj) {
            bool colorResult = objectColorDict.Remove(obj);
            Image image;
            if (objectImageDict.TryGetValue(obj, out image)) {
                image.Dispose();
                return true;
            }
            return colorResult;
        }
        public RectangleF GetCellBounds(DrawCellInfo info) {
            return new RectangleF(
                (tableStart.X + cellSize * info.CellPos.Column),
                (tableStart.Y + cellSize * info.CellPos.Row),
                cellSize, cellSize);
        }
        public void DrawCell(Graphics g, DrawCellInfo info) {
            RectangleF cellBounds = GetCellBounds(info);
            Color cellColor = GetObjectColor(info.Object);
            Image cellImage = GetObjectImage(info.Object, cellColor);
            IStringMapProvider stringMapProvider = info.Object as IStringMapProvider;
            using(Brush brush = new SolidBrush(cellColor)){
                if (cellImage == null) {
                    g.FillRectangle(brush, cellBounds);
                } else {
                    g.DrawImage(cellImage, cellBounds);
                }
                g.DrawRectangle(Pens.Silver, cellBounds.Left, cellBounds.Top, cellBounds.Width, cellBounds.Height);
            }
            if (stringMapProvider == null) return;
            string text = stringMapProvider.StringMap[info.CellPos.Row - info.ObjectPos.Row, info.CellPos.Column - info.ObjectPos.Column];
            text = text.Replace(' ', '_');
            float fontSize = cellSize / 3.55555558f;
            using (Font currentFont = new Font(mainFont.FontFamily, fontSize, mainFont.Style, mainFont.Unit, mainFont.GdiCharSet, mainFont.GdiVerticalFont)) {
                SizeF stringSize = g.MeasureString(text, currentFont);
                if (stringSize.Width > cellSize) stringSize.Width = cellSize;
                if (stringSize.Height > cellSize) stringSize.Height = cellSize;
                PointF stringPos = new PointF(((cellSize - stringSize.Width) / 2) + cellBounds.Left,
                                            ((cellSize - stringSize.Height) / 2) + cellBounds.Top);
                RectangleF stringBounds = new RectangleF(stringPos, stringSize);
                RectangleF stringBoundsShadow = RectangleF.Inflate(stringBounds, 1, 1);
                g.DrawString(text, currentFont, Brushes.Black, stringBoundsShadow);
                g.DrawString(text, currentFont, Brushes.White, stringBounds);
            }
        }

        public void DrawTable(Graphics g) {
            if (cellSize == 0) {
                firstDraw = true;
                return;
            }
            if (firstDraw) {
                g.FillRectangle(Brushes.Snow, new RectangleF(new PointF(0f, 0f), screenSize));
            }
            for (int row = 0; row < game.MainTable.Height; row++) {
                for (int col = 0; col < game.MainTable.Width; col++) {
                    GameCell cell = game.MainTable[row, col];
                    GameCellPos objectPos;
                    DrawCellInfo drawInfo;
                    if (cell.GameObject == null || !game.MainTable.TryGetObjectPosition(cell.GameObject, out objectPos)) {
                        drawInfo = new DrawCellInfo(new GameCellPos(row, col));
                    }else{
                        drawInfo = new DrawCellInfo(new GameCellPos(row, col), objectPos, cell.GameObject);
                    }
                    if (firstDraw || !drawInfo.Equals(prevDrawMap[row, col])) {
                        DrawCell(g, drawInfo);
                        prevDrawMap[row, col] = drawInfo;
                    }
                }
            }
            firstDraw = false;
        }
        public void UpdateScreenSize(Size screenSize) {
            this.screenSize = screenSize;
            this.cellSize = GetCellSize(screenSize);
            this.tableStart = new PointF((screenSize.Width - game.MainTable.Width * this.cellSize) / 2,
                                        (screenSize.Height - game.MainTable.Height * this.cellSize) / 2);
            this.firstDraw = true;
        }
        public void Invalidate() {
            this.firstDraw = true;
        }

        public void ClearCache() {
            foreach (Image image in objectImageDict.Values) {
                image.Dispose();
            }
            objectColorDict.Clear();
            objectImageDict.Clear();
        }
    }

    class DrawCellInfo {
        GameCellPos cellPos;
        GameCellPos objectPos;
        GameObject obj;
        public GameCellPos CellPos {
            get { return cellPos; }
        }
        public GameCellPos ObjectPos {
            get { return objectPos; }
        }
        public GameObject Object {
            get { return obj; }
        }
        public DrawCellInfo(GameCellPos cellPos) {
            this.cellPos = cellPos;
        }
        public DrawCellInfo(GameCellPos cellPos, GameCellPos objectPos, GameObject obj)
            :this(cellPos) {
            this.objectPos = objectPos;
            this.obj = obj;
        }
        public override bool Equals(object obj) {
            DrawCellInfo other = obj as DrawCellInfo;
            if (other == null) return false;
            return this.cellPos.Equals(other.cellPos) 
                && this.objectPos.Equals(other.objectPos) && ReferenceEquals(this.obj, other.obj);
        }
        public override int GetHashCode() {
            return cellPos.GetHashCode() ^ objectPos.GetHashCode() ^ (obj == null ? 0x15467834 : obj.GetHashCode());
        }
    }
}
