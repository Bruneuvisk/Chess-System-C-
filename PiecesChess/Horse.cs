using System;
using ChessSystem.BoardChess;

namespace ChessSystem.PiecesChess {
    class Horse : Piece {
        public Horse(Board tab, Color cor) : base(tab, cor) {
        }

        public override string ToString() {
            return "C";
        }

        private bool podeMover(Position pos) {
            Piece p = tab.peca(pos);
            return p == null || p.cor != cor;
        }

        public override bool[,] movimentosPossiveis() {
            bool[,] mat = new bool[tab.linhas, tab.colunas];

            Position pos = new Position(0, 0);

            pos.definirValores(pos.linha - 1, pos.coluna - 2);
            if (tab.PositionValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            pos.definirValores(pos.linha - 2, pos.coluna - 1);
            if (tab.PositionValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            pos.definirValores(pos.linha - 2, pos.coluna + 1);
            if (tab.PositionValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            pos.definirValores(pos.linha - 1, pos.coluna + 2);
            if (tab.PositionValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            pos.definirValores(pos.linha + 1, pos.coluna + 2);
            if (tab.PositionValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            pos.definirValores(pos.linha + 2, pos.coluna + 1);
            if (tab.PositionValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            pos.definirValores(pos.linha + 2, pos.coluna - 1);
            if (tab.PositionValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            pos.definirValores(pos.linha + 1, pos.coluna - 2);
            if (tab.PositionValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }

            return mat;
        }
    }
}
