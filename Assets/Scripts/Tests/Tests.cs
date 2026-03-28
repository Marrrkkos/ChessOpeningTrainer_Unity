using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static Game;
using static UnityEngine.Rendering.DebugUI;

public class Tests : MonoBehaviour
{
    public SliderControllerFillStand sliderController;
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
        sliderController.SetFillStand(currentFillStand);

        if (isTesting && oneTimer)
        {
            if (currentTest == 0)
            {
                oneTimer = false;
                Najdorf();

            }
            else if (currentTest == 1)
            {
                oneTimer = false;
                WeirdPosition1();
            }
            else if (currentTest == 2)
            {
                oneTimer = false;
                Normal();
            }
            else if (currentTest == 3)
            {
                oneTimer = false;
                FriedLiver();
            }
            else if (currentTest == 4)
            {
                oneTimer = false;
                KingsGambit();
            }

        }

    }
    private void Normal() {
        Thread Normal = new Thread(() =>
        {
            Piece piece = board.fields[0].piece;
            long result;

            for (int i = 0; i < 5; i++)
            {
                result = results[i];
                sliderController.SetMinMax(0, result);
                TestCurrentPos(i + 1, result, "normal");
            }
            currentFillStand = 0;
            currentTest++;
            oneTimer = true;
        });
        Normal.IsBackground = true;
        Normal.Start();
    }
    private void WeirdPosition1() {

        //Weird Position 1
        //BoardUtil.SANToMove(board, "e4", true)
        board.DoMove(0, "d2", "d4", withPreview, false);
        board.DoMove(0, "g8", "f6", withPreview, false);

        board.DoMove(0, "e2", "e4", withPreview, false);
        board.DoMove(0, "f6", "e4", withPreview, false);

        board.DoMove(0, "f1", "c4", withPreview, false);
        board.DoMove(0, "e4", "f2", withPreview,false);

        board.DoMove(0, "d4", "d5", withPreview, false);
        board.DoMove(0, "e7", "e6", withPreview, false);

        board.DoMove(0, "d5", "e6", withPreview, false);
        board.DoMove(0, "f8", "e7", withPreview, false);

        board.DoMove(0, "e6", "d7", withPreview, false);
        board.DoMove(0, "e8", "f8", withPreview,false);

        board.DoMove(0, "g1", "e2", withPreview, false);
        board.DoMove(0, "c7", "c6", withPreview,false);

        Thread WeirdPosition1 = new Thread(() =>
        {
            result = 89941194;
            sliderController.SetMinMax(0, result);
            TestCurrentPos(5, result, "weird position 1");
            currentFillStand = 0;

            currentTest++;
            UndoPosition(14);
            oneTimer = true;
        });
        WeirdPosition1.IsBackground = true;
        WeirdPosition1.Start();
    }
    private void Najdorf() {

        // Najdorf
        board.DoMove(0, "e2", "e4", withPreview, false);
        board.DoMove(0, "c7", "c5", withPreview, false);

        board.DoMove(0, "g1", "f3", withPreview, false);
        board.DoMove(0, "d7", "d6", withPreview,false);

        board.DoMove(0, "d2", "d4", withPreview,false);
        board.DoMove(0, "c5", "d4", withPreview,false);

        board.DoMove(0, "f3", "d4", withPreview,false);

        Thread Najdorf = new Thread(() =>
        {
            result = 38114769;
            sliderController.SetMinMax(0, result);
            TestCurrentPos(5, result, "Najdorf");
            currentFillStand = 0;

            currentTest++;
            UndoPosition(7);
            oneTimer = true;
        });
        Najdorf.IsBackground = true;
        Najdorf.Start();
    }
    private void FriedLiver() {

        // Fried Liver
        board.DoMove(0, "e2", "e4", withPreview,false);
        board.DoMove(0, "e7", "e5", withPreview,false);

        board.DoMove(0, "g1", "f3", withPreview,false);
        board.DoMove(0, "b8", "c6", withPreview,false);

        board.DoMove(0, "f1", "c4", withPreview,false);
        board.DoMove(0, "g8", "f6", withPreview,false);

        board.DoMove(0, "f3", "g5", withPreview,false);

        Thread FriedLiver = new Thread(() =>
        {
            result = 34062181;
            sliderController.SetMinMax(0, result);
            TestCurrentPos(5, result, "Fried Liver");
            currentFillStand = 0;

            currentTest++;
            UndoPosition(7);
            oneTimer = true;
        });
        FriedLiver.IsBackground = true;
        FriedLiver.Start();
    }
   
    private void KingsGambit() {
        // KönigsGamit
        board.DoMove(0, "e2", "e4", withPreview,false);
        board.DoMove(0, "e7", "e5", withPreview,false);

        board.DoMove(0, "f2", "f4", withPreview,false);
        board.DoMove(0, "e5", "f4", withPreview, false);

        board.DoMove(0, "g1", "f3", withPreview, false);

        Thread KingsGambit = new Thread(() =>
        {
            result = 22095269;
            sliderController.SetMinMax(0, result);
            TestCurrentPos(5, result, "KönigsGambit");
            currentFillStand = 0;

            currentTest++;
            UndoPosition(5);
            oneTimer = true;
            Debug.Log("END");
        });
        KingsGambit.IsBackground = true;
        KingsGambit.Start();
    }
    private void UndoPosition(int count)
    {
        for (int i = 0; i < count; i++)
        {
            board.UndoMove(false,false);
        }
    }

    private void TestCurrentPos(int depth, long result, string name)
    {


        int x = board.currentGame.playedMoves.Count;

        //Debug.Log(name + "\n");
        //System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
        long moveCount = Breadth_first_search(depth, x);
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
    private long Breadth_first_search(int depth, int currentGameDepth)
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
                 piece.doMove(move.x, move.y, null,false, false);

                nodes += Breadth_first_search(depth - 1, currentGameDepth - 1);

                piece.undoMove(false,false);
            }
        }

        return nodes;
    }
}
