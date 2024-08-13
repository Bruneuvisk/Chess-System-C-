using System;

namespace ChessSystem.BoardChess {
    class Board {
        public int linhas { get; set; }
        public int colunas { get; set; }
        private Piece[,] pecas;

        public Board(int linhas, int colunas) {
            this.linhas = linhas;
            this.colunas = colunas;
            pecas = new Piece[linhas, colunas];
        }

        public Piece peca(int linha, int coluna) {
            return pecas[linha, coluna];
        }

        public Piece peca(Position pos) {
            return pecas[pos.linha, pos.coluna];
        }

        public bool existePeca(Position pos) {
            validarPosicao(pos);
            return peca(pos) != null;
        }

        public void colocarPeca(Piece p, Position pos) {
            if (existePeca(pos)) {
                throw new BoardException("Já existe uma peça nessa posição!");
            }
            pecas[pos.linha, pos.coluna] = p;
            p.posicao = pos;
        }

        public Piece retirarPeca(Position pos) {
            if (peca(pos) == null) {
                return null;
            }
            Piece aux = peca(pos);
            aux.posicao = null;
            pecas[pos.linha, pos.coluna] = null;
            return aux;
        }

        public bool posicaoValida(Position pos) {
            if (pos.linha<0 || pos.linha>=linhas || pos.coluna<0 || pos.coluna>=colunas) {
                return false;
            }
            return true;
        }

        public void validarPosicao(Position pos) {
            if (!posicaoValida(pos)) {
                throw new BoardException("Posição inválida!");
            }
        }
    }
}
