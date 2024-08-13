using System;
using ChessSystem.BoardChess;
using ChessSystem.PiecesChess;
using xadrez_console;

namespace ChessSystem {
    class Program {
        static void Main(string[] args) {
            try {
                ChessMatch partida = new ChessMatch();

                while (!partida.terminada) {

                    try {
                        Console.Clear();
                        Screen.imprimirPartida(partida);

                        Console.WriteLine();
                        Console.Write("Origem: ");
                        Position origem = Screen.lerPositionChess().toPosition();
                        partida.validarPositionDeOrigem(origem);

                        bool[,] posicoesPossiveis = partida.tab.peca(origem).movimentosPossiveis();

                        Console.Clear();
                        Screen.imprimirTabuleiro(partida.tab, posicoesPossiveis);

                        Console.WriteLine();
                        Console.Write("Destino: ");
                        Position destino = Screen.lerPositionChess().toPosition();
                        partida.validarPositionDeDestino(origem, destino);

                        partida.realizaJogada(origem, destino);
                    }
                    catch (BoardException e) {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                }
                Console.Clear();
                Screen.imprimirPartida(partida);
            }
            catch (BoardException e) {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
