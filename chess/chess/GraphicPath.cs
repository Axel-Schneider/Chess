using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    /// <summary>
    /// Path of the Image
    /// </summary>
    internal struct GraphicPath
    {
        /// <summary>
        /// Actual directory
        /// </summary>
        public static string ActualDirectory => Directory.GetCurrentDirectory();
        /// <summary>
        /// Image Directory
        /// </summary>
        public static string ImagePath => ActualDirectory + @"\Images\";
        /// <summary>
        /// Chess pieces
        /// </summary>
        public struct Pieces
        {
            public static string PiecesPath => ImagePath + @"Pieces\";
            /// <summary>
            /// Dark pawn path
            /// </summary>
            public static string DarkPawnImg => PiecesPath + @"DarkPawn.png";
            /// <summary>
            /// Light pawn path
            /// </summary>
            public static string LightPawnImg => PiecesPath + @"LightPawn.png";
            /// <summary>
            /// Dark Bishop path
            /// </summary>
            public static string DarkBishopImg => PiecesPath + @"\DarkBishop.png";
            /// <summary>
            /// Light Bishop path
            /// </summary>
            public static string LightBishopImg => PiecesPath + @"LightBishop.png";
            /// <summary>
            /// Dark Knight path
            /// </summary>
            public static string DarkKnightImg => PiecesPath + @"DarkKnight.png";
            /// <summary>
            /// Light Knight path
            /// </summary>
            public static string LightKnightImg => PiecesPath + @"LightKnight.png";
            /// <summary>
            /// Dark Rook path
            /// </summary>
            public static string DarkRookImg => PiecesPath + @"DarkRook.png";
            /// <summary>
            /// Light Rook path
            /// </summary>
            public static string LightRookImg => PiecesPath + @"LightRook.png";
            /// <summary>
            /// Dark Queen path
            /// </summary>
            public static string DarkQueenImg => PiecesPath + @"DarkQueen.png";
            /// <summary>
            /// Light Queen path
            /// </summary>
            public static string LightQueenImg => PiecesPath + @"LightQueen.png";
            /// <summary>
            /// Dark King path
            /// </summary>
            public static string DarkKingImg => PiecesPath + @"DarkKing.png";
            /// <summary>
            /// Light King path
            /// </summary>
            public static string LightKingImg => PiecesPath + @"LightKing.png";
            /// <summary>
            /// Get King path
            /// </summary>
            /// <param name="color">Color</param>
            /// <returns>King path</returns>
            public static string King(bool color) => (color) ? LightKingImg : DarkKingImg;
            /// <summary>
            /// Get Pawn path
            /// </summary>
            /// <param name="color">Color</param>
            /// <returns>Pawn path</returns>
            public static string Pawn(bool color) => (color) ? LightPawnImg : DarkPawnImg;
            /// <summary>
            /// Get Bishop path
            /// </summary>
            /// <param name="color">Color</param>
            /// <returns>Bishop path</returns>
            public static string Bishop(bool color) => (color) ? LightBishopImg : DarkBishopImg;
            /// <summary>
            /// Get Knight path
            /// </summary>
            /// <param name="color">Color</param>
            /// <returns>Knight path</returns>
            public static string Knight(bool color) => (color) ? LightKnightImg : DarkKnightImg;
            /// <summary>
            /// Get Rook path
            /// </summary>
            /// <param name="color">Color</param>
            /// <returns>Rook path</returns>
            public static string Rook(bool color) => (color) ? LightRookImg : DarkRookImg;
            /// <summary>
            /// Get Queen path
            /// </summary>
            /// <param name="color">Color</param>
            /// <returns>Queen path</returns>
            public static string Queen(bool color) => (color) ? LightQueenImg : DarkQueenImg;

        }
        /// <summary>
        /// Chess cases
        /// </summary>
        public struct Cases
        {
            /// <summary>
            /// Case directory
            /// </summary>
            public static string CasesPath => ImagePath + @"Cases\";
            /// <summary>
            /// Dark case path
            /// </summary>
            public static string DarkCase => CasesPath + @"DarkCase.png";
            /// <summary>
            /// Light case path
            /// </summary>
            public static string LightCase => CasesPath + @"LightCase.png";
            /// <summary>
            /// Case Path
            /// </summary>
            /// <param name="color">Case color</param>
            /// <returns>Case image path</returns>
            public static string GetCase(bool color) => (color) ? LightCase : DarkCase;
        }

        public struct Show
        {
            public static string ShowPath => ImagePath + @"Show\";
            public static string move => ShowPath + @"move.png";
            public static string Background => ShowPath + @"Background.png";
            public static string BackgroundObject => ShowPath + @"BackgroundObject.jpg";
        }
        public TwoPath Two => new TwoPath("", "");

        

        internal class TwoPath
        {
            public string Dark { get; private set; }
            public string Light { get; private set; }
            public TwoPath(string dark, string light)
            {
                this.Dark = dark;
                this.Light = Light;
            }
            public string GetColor(bool color)
            {
                return (color) ? Dark : Light;
            }
        }
    }
}
