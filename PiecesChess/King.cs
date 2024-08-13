using System;
using System.Runtime.ConstrainedExecution;
using ChessSystem.BoardChess;

namespace ChessSystem.PiecesChess {
    class King : Piece {
        private ChessMatch partida;

        public King(Board tab, Color cor, ChessMatch partida) : base(tab, cor) {
            this.partida = partida;
        }

        public override string ToString() {
            return "K";
        }

        private bool podeMover(Position pos) {
            Piece p = tab.peca(pos);
            return p == null || p.cor != cor;
        }

        private bool testeTowerParaRoque(Position pos) {
            Piece p = tab.peca(pos);
            return p != null && p is Tower && p.cor == cor && p.qteMovimentos == 0;
        }

        public override bool[,] movimentosPossiveis() {
            bool[,] mat = new bool[tab.linhas, tab.colunas];

            Position pos = new Position(0, 0);

            // acima
            pos.definirValores(posicao.linha - 1, posicao.coluna);
            if (tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            // ne
            pos.definirValores(posicao.linha - 1, posicao.coluna + 1);
            if (tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            // direita
            pos.definirValores(posicao.linha, posicao.coluna + 1);
            if (tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            // se
            pos.definirValores(posicao.linha + 1, posicao.coluna + 1);
            if (tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            // abaixo
            pos.definirValores(posicao.linha + 1, posicao.coluna);
            if (tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            // so
            pos.definirValores(posicao.linha + 1, posicao.coluna - 1);
            if (tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            // esquerda
            pos.definirValores(posicao.linha, posicao.coluna - 1);
            if (tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }
            // no
            pos.definirValores(posicao.linha - 1, posicao.coluna - 1);
            if (tab.posicaoValida(pos) && podeMover(pos)) {
                mat[pos.linha, pos.coluna] = true;
            }

            // #jogadaespecial roque
            if (qteMovimentos==0 && !partida.xeque) {
                // #jogadaespecial roque pequeno
                Position posT1 = new Position(posicao.linha, posicao.coluna + 3);
                if (testeTowerParaRoque(posT1)) {
                    Position p1 = new Position(posicao.linha, posicao.coluna + 1);
                    Position p2 = new Position(posicao.linha, posicao.coluna + 2);
                    if (tab.peca(p1)==null && tab.peca(p2)==null) {
                        mat[posicao.linha, posicao.coluna + 2] = true;
                    }
                }
                // #jogadaespecial roque grande
                Position posT2 = new Position(posicao.linha, posicao.coluna - 4);
                if (testeTowerParaRoque(posT2)) {
                    Position p1 = new Position(posicao.linha, posicao.coluna - 1);
                    Position p2 = new Position(posicao.linha, posicao.coluna - 2);
                    Position p3 = new Position(posicao.linha, posicao.coluna - 3);
                    if (tab.peca(p1) == null && tab.peca(p2) == null && tab.peca(p3) == null) {
                        mat[posicao.linha, posicao.coluna - 2] = true;
                    }
                }
            }


            return mat;
        }

    }
}
