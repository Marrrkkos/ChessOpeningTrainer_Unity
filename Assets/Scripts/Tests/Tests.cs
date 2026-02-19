using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static Game;
using static UnityEngine.Rendering.DebugUI;

public class Tests : MonoBehaviour
{
    public SliderController sliderController;
    long currentFillStand;

    public Board board;

    public Game game;
    public Player player1;
    public Player player2;

    private Piece piece;

    private bool isTesting = true;
    private bool oneTimer = true;
    private int currentTest = 0;
    private int result = 0;

    public bool withPreview = false;
    public bool withAnimation = false;
    public bool testBoardUtil = false;

    long[] results = new long[] {20, 400, 8902, 197281, 4865609,119060324, 3195901860, 84998978956, 2439530234167, 69352859712417, 2097651003696806, 62854969236701747};

    void Start() {

        piece = board.fields[0].piece;
    }
    void Update()
    {
        sliderController.setFillStand(currentFillStand);

        if (isTesting && oneTimer)
        {
            if (currentTest == 0)
            {
                oneTimer = false;
                najdorf();

            }
            else if (currentTest == 1)
            {
                oneTimer = false;
                weirdPosition1();
            }
            else if (currentTest == 2)
            {
                oneTimer = false;
                normal();
            }
            else if (currentTest == 3)
            {
                oneTimer = false;
                friedLiver();
            }
            else if (currentTest == 4)
            {
                oneTimer = false;
                kingsGambit();
            }

        }

    }
    private void normal() {
        Thread normal = new Thread(() =>
        {
            Piece piece = board.fields[0].piece;
            long result;

            for (int i = 0; i < 5; i++)
            {
                result = results[i];
                sliderController.setMax(result);
                testCurrentPos(i + 1, result, "normal");
            }
            currentFillStand = 0;
            currentTest++;
            oneTimer = true;
        });
        normal.IsBackground = true;
        normal.Start();
    }
    private void weirdPosition1() {

        //Weird Position 1
        //BoardUtil.SANToMove(board, "e4", true)
        board.doMove(0, "d2", "d4", withPreview, false);
        board.doMove(0, "g8", "f6", withPreview, false);

        board.doMove(0, "e2", "e4", withPreview, false);
        board.doMove(0, "f6", "e4", withPreview, false);

        board.doMove(0, "f1", "c4", withPreview, false);
        board.doMove(0, "e4", "f2", withPreview, false);

        board.doMove(0, "d4", "d5", withPreview, false);
        board.doMove(0, "e7", "e6", withPreview, false);

        board.doMove(0, "d5", "e6", withPreview, false);
        board.doMove(0, "f8", "e7", withPreview, false);

        board.doMove(0, "e6", "d7", withPreview, false);
        board.doMove(0, "e8", "f8", withPreview, false);

        board.doMove(0, "g1", "e2", withPreview, false);
        board.doMove(0, "c7", "c6", withPreview, false);

        Thread weirdPosition1 = new Thread(() =>
        {
            result = 89941194;
            sliderController.setMax(result);
            testCurrentPos(5, result, "weird position 1");
            currentFillStand = 0;

            currentTest++;
            undoPosition(14);
            oneTimer = true;
        });
        weirdPosition1.IsBackground = true;
        weirdPosition1.Start();
    }
    private void najdorf() {

        // Najdorf
        board.doMove(0, "e2", "e4", withPreview, false);
        board.doMove(0, "c7", "c5", withPreview, false);

        board.doMove(0, "g1", "f3", withPreview, false);
        board.doMove(0, "d7", "d6", withPreview, false);

        board.doMove(0, "d2", "d4", withPreview, false);
        board.doMove(0, "c5", "d4", withPreview, false);

        board.doMove(0, "f3", "d4", withPreview, false);

        Thread najdorf = new Thread(() =>
        {
            result = 38114769;
            sliderController.setMax(result);
            testCurrentPos(5, result, "Najdorf");
            currentFillStand = 0;

            currentTest++;
            undoPosition(7);
            oneTimer = true;
        });
        najdorf.IsBackground = true;
        najdorf.Start();
    }
    private void friedLiver() {

        // Fried Liver
        board.doMove(0, "e2", "e4", withPreview, false);
        board.doMove(0, "e7", "e5", withPreview, false);

        board.doMove(0, "g1", "f3", withPreview, false);
        board.doMove(0, "b8", "c6", withPreview, false);

        board.doMove(0, "f1", "c4", withPreview, false);
        board.doMove(0, "g8", "f6", withPreview, false);

        board.doMove(0, "f3", "g5", withPreview, false);

        Thread friedLiver = new Thread(() =>
        {
            result = 34062181;
            sliderController.setMax(result);
            testCurrentPos(5, result, "Fried Liver");
            currentFillStand = 0;

            currentTest++;
            undoPosition(7);
            oneTimer = true;
        });
        friedLiver.IsBackground = true;
        friedLiver.Start();
    }
   
    private void kingsGambit() {
        // KönigsGamit
        board.doMove(0, "e2", "e4", withPreview, false);
        board.doMove(0, "e7", "e5", withPreview, false);

        board.doMove(0, "f2", "f4", withPreview, false);
        board.doMove(0, "e5", "f4", withPreview, false);

        board.doMove(0, "g1", "f3", withPreview, false);

        Thread kingsGambit = new Thread(() =>
        {
            result = 22095269;
            sliderController.setMax(result);
            testCurrentPos(5, result, "KönigsGambit");
            currentFillStand = 0;

            currentTest++;
            undoPosition(5);
            oneTimer = true;
            Debug.Log("END");
        });
        kingsGambit.IsBackground = true;
        kingsGambit.Start();
    }
    private void undoPosition(int count)
    {
        for (int i = 0; i < count; i++)
        {
            board.undoMove(false);
        }
    }

    private void testCurrentPos(int depth, long result, string name)
    {


        int x = board.currentGame.playedMoves.Count;

        //Debug.Log(name + "\n");
        //System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
        long moveCount = breadth_first_search(depth, x);
        //sw.Stop();
        //Debug.Log($"Tiefe {depth} dauerte {sw.ElapsedMilliseconds}ms");

        if (moveCount == result)
        {
            Debug.Log(depth + " ✓_________MOVECOUNT: " + moveCount);
        }
        else
        {
            Debug.Log(depth + " X_________MOVECOUNT: " + moveCount + "  SHOULD BE: " + result);
        }
        Debug.Log("\n\n");
    }

    void OnDestroy()
    {
        isTesting = false;
    }
    private long breadth_first_search(int depth, int currentGameDepth)
    {
        if (!isTesting) return 0;
        if (depth == 0)
        {
            currentFillStand++;
            return 1;
        }

        long nodes = 0;

        bool isWhiteTurn = (currentGameDepth % 2) == 0;
        List<Piece> activePieces = isWhiteTurn ? board.whitePieces : board.blackPieces;

        List<Piece> piecesToProcess = new List<Piece>(activePieces);

        foreach (Piece piece in piecesToProcess)
        {
            List<Vector2Int> moves = piece.getPossibleMoves();
            moves = GameRules.removeChecks(piece.position, moves, board);

            foreach (Vector2Int move in moves)
            {
                 piece.doMove(move.x, move.y, null, false);

                nodes += breadth_first_search(depth - 1, currentGameDepth - 1);

                piece.undoMove(false);
            }
        }

        return nodes;
    }
}
