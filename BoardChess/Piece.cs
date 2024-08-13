using System;
using System.Runtime.ConstrainedExecution;

namespace ChessSystem.BoardChess {
    class Piece {
            public Position posicao { get; set; }
            public Color cor { get; protected set; }
            public int qteMovimentos { get; protected set; }
            public Board tab { get; protected set; }

            public Piece(Board tab, Color cor) {
                this.posicao = null;
                this.tab = tab;
                this.cor = cor;
                this.qteMovimentos = 0;
            }

            public void incrementarQteMovimentos() {
                qteMovimentos++;
            }

            public void decrementarQteMovimentos() {
                qteMovimentos--;
            }

            public bool existeMovimentosPossiveis() {
                bool[,] mat = movimentosPossiveis();
                for (int i = 0; i<tab.linhas; i++) {
                    for (int j = 0; j<tab.colunas; j++) {
                        if (mat[i, j]) {
                            return true;
                        }
                    }
                }
                return false;
            }

            public bool movimentoPossivel(Position pos) {
                return movimentosPossiveis()[pos.linha, pos.coluna];
            }

            public abstract bool[,] movimentosPossiveis() {

            }
    }
}
