﻿using System;

namespace ChessSystem.BoardChess {
    class Position {
        public int linha { get; set; }
        public int coluna { get; set; }

        public Position(int linha, int coluna) {
            this.linha = linha;
            this.coluna = coluna;
        }

        public void definirValores(int linha, int coluna) {
            this.linha = linha;
            this.coluna = coluna;
        }

        public override string ToString() {
            return linha
                + ", "
                + coluna;
        }
    }
}

