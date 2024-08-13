using ChessSystem.PiecesChess;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using ChessSystem.BoardChess;


namespace ChessSystem.PiecesChess {
    class ChessMatch {

        public Board tab { get; private set; }
        public int turno { get; private set; }
        public Color jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Piece> Pieces;
        private HashSet<Piece> capturadas;
        public bool xeque { get; private set; }
        public Piece vulneravelEnPassant { get; private set; }

        public ChessMatch() {
            tab = new Board(8, 8);
            turno = 1;
            jogadorAtual = Color.Branca;
            terminada = false;
            xeque = false;
            vulneravelEnPassant = null;
            Pieces = new HashSet<Piece>();
            capturadas = new HashSet<Piece>();
            colocarPieces();
        }

        public Piece executaMovimento(Position origem, Position destino) {
            Piece p = tab.retirarPiece(origem);
            p.incrementarQteMovimentos();
            Piece PieceCapturada = tab.retirarPiece(destino);
            tab.colocarPiece(p, destino);
            if (PieceCapturada != null) {
                capturadas.Add(PieceCapturada);
            }

            // #jogadaespecial roque pequeno
            if (p is King && destino.coluna == origem.coluna + 2) {
                Position origemT = new Position(origem.linha, origem.coluna + 3);
                Position destinoT = new Position(origem.linha, origem.coluna + 1);
                Piece T = tab.retirarPiece(origemT);
                T.incrementarQteMovimentos();
                tab.colocarPiece(T, destinoT);
            }

            // #jogadaespecial roque grande
            if (p is King && destino.coluna == origem.coluna - 2) {
                Position origemT = new Position(origem.linha, origem.coluna - 4);
                Position destinoT = new Position(origem.linha, origem.coluna - 1);
                Piece T = tab.retirarPiece(origemT);
                T.incrementarQteMovimentos();
                tab.colocarPiece(T, destinoT);
            }

            // #jogadaespecial en passant
            if (p is Pawn) {
                if (origem.coluna != destino.coluna && PieceCapturada == null) {
                    Position posP;
                    if (p.cor == Color.Branca) {
                        posP = new Position(destino.linha + 1, destino.coluna);
                    }
                    else {
                        posP = new Position(destino.linha - 1, destino.coluna);
                    }
                    PieceCapturada = tab.retirarPiece(posP);
                    capturadas.Add(PieceCapturada);
                }
            }

            return PieceCapturada;
        }

        public void desfazMovimento(Position origem, Position destino, Piece PieceCapturada) {
            Piece p = tab.retirarPiece(destino);
            p.decrementarQteMovimentos();
            if (PieceCapturada != null) {
                tab.colocarPiece(PieceCapturada, destino);
                capturadas.Remove(PieceCapturada);
            }
            tab.colocarPiece(p, origem);

            // #jogadaespecial roque pequeno
            if (p is King && destino.coluna == origem.coluna + 2) {
                Position origemT = new Position(origem.linha, origem.coluna + 3);
                Position destinoT = new Position(origem.linha, origem.coluna + 1);
                Piece T = tab.retirarPiece(destinoT);
                T.decrementarQteMovimentos();
                tab.colocarPiece(T, origemT);
            }

            // #jogadaespecial roque grande
            if (p is King && destino.coluna == origem.coluna - 2) {
                Position origemT = new Position(origem.linha, origem.coluna - 4);
                Position destinoT = new Position(origem.linha, origem.coluna - 1);
                Piece T = tab.retirarPiece(destinoT);
                T.decrementarQteMovimentos();
                tab.colocarPiece(T, origemT);
            }

            // #jogadaespecial en passant
            if (p is Pawn) {
                if (origem.coluna != destino.coluna && PieceCapturada == vulneravelEnPassant) {
                    Piece Pawn = tab.retirarPiece(destino);
                    Position posP;
                    if (p.cor == Color.Branca) {
                        posP = new Position(3, destino.coluna);
                    }
                    else {
                        posP = new Position(4, destino.coluna);
                    }
                    tab.colocarPiece(Pawn, posP);
                }
            }
        }

        public void realizaJogada(Position origem, Position destino) {
            Piece PieceCapturada = executaMovimento(origem, destino);

            if (estaEmXeque(jogadorAtual)) {
                desfazMovimento(origem, destino, PieceCapturada);
                throw new BoardException("Você não pode se colocar em xeque!");
            }

            Piece p = tab.peca(destino);

            // #jogadaespecial promocao
            if (p is Pawn) {
                if ((p.cor == Color.Branca && destino.linha == 0) || (p.cor == Color.Preta && destino.linha == 7)) {
                    p = tab.retirarPiece(destino);
                    Pieces.Remove(p);
                    Piece dama = new Dama(tab, p.cor);
                    tab.colocarPiece(dama, destino);
                    Pieces.Add(dama);
                }
            }

            if (estaEmXeque(adversaria(jogadorAtual))) {
                xeque = true;
            }
            else {
                xeque = false;
            }

            if (testeXequemate(adversaria(jogadorAtual))) {
                terminada = true;
            }
            else {
                turno++;
                mudaJogador();
            }

            // #jogadaespecial en passant
            if (p is Pawn && (destino.linha == origem.linha - 2 || destino.linha == origem.linha + 2)) {
                vulneravelEnPassant = p;
            }
            else {
                vulneravelEnPassant = null;
            }

        }

        public void validarPositionDeOrigem(Position pos) {
            if (tab.peca(pos) == null) {
                throw new BoardException("Não existe peça na posição de origem escolhida!");
            }
            if (jogadorAtual != tab.peca(pos).cor) {
                throw new BoardException("A peça de origem escolhida não é sua!");
            }
            if (!tab.peca(pos).existeMovimentosPossiveis()) {
                throw new BoardException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void validarPositionDeDestino(Position origem, Position destino) {
            if (!tab.peca(origem).movimentoPossivel(destino)) {
                throw new BoardException("Posição de destino inválida!");
            }
        }

        private void mudaJogador() {
            if (jogadorAtual == Color.Branca) {
                jogadorAtual = Color.Preta;
            }
            else {
                jogadorAtual = Color.Branca;
            }
        }

        public HashSet<Piece> PiecesCapturadas(Color Color) {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece x in capturadas) {
                if (x.cor == Color) {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Piece> PiecesEmJogo(Color Color) {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece x in Pieces) {
                if (x.cor == Color) {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PiecesCapturadas(Color));
            return aux;
        }

        private Color adversaria(Color Color) {
            if (Color == Color.Branca) {
                return Color.Preta;
            }
            else {
                return Color.Branca;
            }
        }

        private Piece King(Color Color) {
            foreach (Piece x in PiecesEmJogo(Color)) {
                if (x is King) {
                    return x;
                }
            }
            return null;
        }

        public bool estaEmXeque(Color Color) {
            Piece R = King(Color);
            if (R == null) {
                throw new BoardException("Não tem King da Color " + Color + " no Board!");
            }
            foreach (Piece x in PiecesEmJogo(adversaria(Color))) {
                bool[,] mat = x.movimentosPossiveis();
                if (mat[R.posicao.linha, R.posicao.coluna]) {
                    return true;
                }
            }
            return false;
        }

        public bool testeXequemate(Color Color) {
            if (!estaEmXeque(Color)) {
                return false;
            }
            foreach (Piece x in PiecesEmJogo(Color)) {
                bool[,] mat = x.movimentosPossiveis();
                for (int i = 0; i<tab.linhas; i++) {
                    for (int j = 0; j<tab.colunas; j++) {
                        if (mat[i, j]) {
                            Position origem = x.posicao;
                            Position destino = new Position(i, j);
                            Piece PieceCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(Color);
                            desfazMovimento(origem, destino, PieceCapturada);
                            if (!testeXeque) {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void colocarNovaPiece(char coluna, int linha, Piece Piece) {
            tab.colocarPiece(Piece, new PositionChess(coluna, linha).toPosition());
            Pieces.Add(Piece);
        }

        private void colocarPieces() {
            colocarNovaPiece('a', 1, new Tower(tab, Color.Branca));
            colocarNovaPiece('b', 1, new Horse(tab, Color.Branca));
            colocarNovaPiece('c', 1, new Bishop(tab, Color.Branca));
            colocarNovaPiece('d', 1, new Dama(tab, Color.Branca));
            colocarNovaPiece('e', 1, new King(tab, Color.Branca, this));
            colocarNovaPiece('f', 1, new Bishop(tab, Color.Branca));
            colocarNovaPiece('g', 1, new Horse(tab, Color.Branca));
            colocarNovaPiece('h', 1, new Tower(tab, Color.Branca));
            colocarNovaPiece('a', 2, new Pawn(tab, Color.Branca, this));
            colocarNovaPiece('b', 2, new Pawn(tab, Color.Branca, this));
            colocarNovaPiece('c', 2, new Pawn(tab, Color.Branca, this));
            colocarNovaPiece('d', 2, new Pawn(tab, Color.Branca, this));
            colocarNovaPiece('e', 2, new Pawn(tab, Color.Branca, this));
            colocarNovaPiece('f', 2, new Pawn(tab, Color.Branca, this));
            colocarNovaPiece('g', 2, new Pawn(tab, Color.Branca, this));
            colocarNovaPiece('h', 2, new Pawn(tab, Color.Branca, this));

            colocarNovaPiece('a', 8, new Tower(tab, Color.Preta));
            colocarNovaPiece('b', 8, new Horse(tab, Color.Preta));
            colocarNovaPiece('c', 8, new Bishop(tab, Color.Preta));
            colocarNovaPiece('d', 8, new Dama(tab, Color.Preta));
            colocarNovaPiece('e', 8, new King(tab, Color.Preta, this));
            colocarNovaPiece('f', 8, new Bishop(tab, Color.Preta));
            colocarNovaPiece('g', 8, new Horse(tab, Color.Preta));
            colocarNovaPiece('h', 8, new Tower(tab, Color.Preta));
            colocarNovaPiece('a', 7, new Pawn(tab, Color.Preta, this));
            colocarNovaPiece('b', 7, new Pawn(tab, Color.Preta, this));
            colocarNovaPiece('c', 7, new Pawn(tab, Color.Preta, this));
            colocarNovaPiece('d', 7, new Pawn(tab, Color.Preta, this));
            colocarNovaPiece('e', 7, new Pawn(tab, Color.Preta, this));
            colocarNovaPiece('f', 7, new Pawn(tab, Color.Preta, this));
            colocarNovaPiece('g', 7, new Pawn(tab, Color.Preta, this));
            colocarNovaPiece('h', 7, new Pawn(tab, Color.Preta, this));
        }
    }
}
