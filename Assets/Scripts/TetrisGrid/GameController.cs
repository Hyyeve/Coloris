using UnityEngine;
using System.Collections;
using TetrisGrid;
using Input;
using System.Collections.Generic;
using DefaultNamespace;

public abstract class GameController : MonoBehaviour
{
    public TetrisGridController tetrisGrid;

    public TetrisPieceDisplayer holdBlockDisplayer;
    public TetrisPieceDisplayer ghostBlockDisplayer;
    public MultiTetrisPieceDisplayer upcomingBlockDisplayer;
    public MultiTetrisPieceDisplayer lineClearRequirementViewer;

    public AudioSource audioPlayer;
    
    public TetrisBlockState holdBlock;

    public List<TetrisBlockState> upcomingBlocks = new List<TetrisBlockState>();

    public LineClearObjective currentObjective;

    public GameObject particlesPrefab;

    
    public bool active;

    public int currentComplexity;

    public int ARR = 5;
    protected int arrCount;

    public int randomGarbageDistri;

    public int DAS = 30;
    protected int dasCount;

    public float rainbowChance = 0.02f;

    public int SDS = 5;
    protected int sdsCount;
    public bool needsNewObjective;
    public int TLD = 30;
    public int RLD = 10;
    protected int tldCount;
    protected int rldCount;
    protected bool hasTouchedGround;
    protected bool hasHeldBlock;
    public bool cheeseGarbage;
    public bool autoLock;
    public bool useClearEffect;

    public bool startImmediately;

    public float blockGravityLevel = 0.1f;
    public Vector2Int spawnPosition;

    protected float amountToDrop = 0;

    protected TetrisBlock currentBlock;
    protected InputComponent input;

    public AudioClip moveSound; 
    public AudioClip rotateSound;
    public AudioClip collideSound;
    public AudioClip clearSound;
    public AudioClip swapSound;
    public AudioClip lockSound;
    public AudioClip clearFailSound;

    public int lineClears;

    public int lineClearsPerLevel;

    public int score;

    private int scoreMult = 1;

    private Shaker shaker;
    private UIMover mover;

    protected bool updateInProgress = false;

    public UIMover gameEndMover;
    
    public void Start()
    {
        shaker = GetComponent<Shaker>();
        mover = GetComponent<UIMover>();

        if (startImmediately) StartGame();
    }

    public void StartGame()
    {
        tetrisGrid = GetComponent<TetrisGridController>();
        input = GetComponent<InputComponent>();
        currentBlock = new TetrisBlock();
        holdBlockDisplayer.ApplyEmptyState();
        active = true;
        switch(SettingsContainer.difficulty)
        {
            case Difficulty.Easy:
                currentComplexity = 1;
                rainbowChance = 0.05f;
                scoreMult = 1;
                break;
            case Difficulty.Normal:
                currentComplexity = 2;
                rainbowChance = 0.02f;
                scoreMult = 2;
                break;
            case Difficulty.Tricky:
                currentComplexity = 3;
                rainbowChance = 0.01f;
                scoreMult = 4;
                break;

        }
        GenerateInitialBlocks();
        StartCoroutine(HandleGameUpdate());
    }

    public void EndGame()
    {
        ResetControl();
        upcomingBlocks.Clear();
        currentObjective = null;
        currentBlock = null;
        holdBlock = null;
        active = false;
        shaker.Shake();
        StartCoroutine(waitandmove());
    }

    private IEnumerator waitandmove()
    {
        yield return new WaitForSeconds(1f);
        mover.TriggerMove();
        gameEndMover.TriggerMove();
    }

    public IEnumerator HandleGameUpdate()
    {
        while (active)
        {
            if (!updateInProgress)
            {
                input.FinaliseInputs();

                if(input.AnyPressed()) DemoModeChecker.ResetDemoTimer();
                
                DebugChecks();

                if (!currentBlock.placed)
                {
                    yield return StartCoroutine(HandleMoveBlock());
                }
                else
                {
                    SpawnNextBlock();
                }

                input.PopInputs();
            }


            yield return new WaitForFixedUpdate();
        }
    }

    public void Update()
    {
        if (active) HandleUpdate();
    }

    public void HandleUpdate()
    {
        input.PushInputs();
    }

    public virtual IEnumerator HandleMoveBlock()
    {
        updateInProgress = true;

        amountToDrop += blockGravityLevel;
        int gravDrop = (int)Mathf.Floor(amountToDrop);

        if(gravDrop > 0)
        {
            if(!currentBlock.TryMoveDistanceY(-gravDrop)) BlockCollisionEffect();
            amountToDrop -= gravDrop;
        }

        if(!currentBlock.IsValidWhenAt(new Vector2Int(0, -1)))
        {
            tldCount++;
            hasTouchedGround = true;
        }
        else if(rldCount < RLD)
        {
            tldCount = 0;
        }

        if(tldCount > TLD && autoLock)
        {
            yield return StartCoroutine(LockBlock());
            updateInProgress = false;
            yield break;
        }

        if(input.Pressed(TetrisInput.HardDrop))
        {
            currentBlock.TryMoveDistanceY(-100);
            yield return StartCoroutine(LockBlock());
            updateInProgress = false;
            yield break;
        }

        if(input.Pressed(TetrisInput.Hold) && !hasHeldBlock)
        {
            SwapHoldBlock();
            hasHeldBlock = true;
        }

        if(input.Pressed(TetrisInput.RotateR))
        {
            if (hasTouchedGround) rldCount++;
            else rldCount = 0;
            if (currentBlock.Rotate(true)) RotateEffect();
            UpdateGhostPiece();
        }
        else if(input.Pressed(TetrisInput.RotateL))
        {
            if (hasTouchedGround) rldCount++;
            else rldCount = 0;
            if (currentBlock.Rotate(false)) RotateEffect();
            UpdateGhostPiece();
        }

        if(input.Down(TetrisInput.SoftDrop))
        {
            if(SDS == 0)
            {
                currentBlock.TryMoveDistanceY(-100);
                BlockCollisionEffect();
            }
            else
            {
                sdsCount++;
                if (sdsCount >= SDS)
                {
                    if (currentBlock.TryMoveDistanceY(-1)) MoveEffect();
                    sdsCount = 0;
                }
            }
        }
        else
        {
            sdsCount = 0;
        }

        if(!input.Down(TetrisInput.Right) && !input.Down(TetrisInput.Left) || input.Down(TetrisInput.Right) && input.Down(TetrisInput.Left))
        {
            dasCount = 0;
        }

        if(input.Pressed(TetrisInput.Right) || input.Down(TetrisInput.Right) && !input.Down(TetrisInput.Left))
        {
            if(input.Pressed(TetrisInput.Right))
            {
                dasCount = 0;
                if(currentBlock.TryMoveDistanceX(1)) MoveEffect();
            }
            else
            {
                dasCount++;
                if(dasCount >= DAS)
                {
                    if(ARR == 0)
                    {
                        currentBlock.TryMoveDistanceX(100);
                        BlockCollisionEffect();
                    }
                    else
                    {
                        arrCount++;
                        if (arrCount > ARR)
                        {
                            if (currentBlock.TryMoveDistanceX(1)) MoveEffect();
                            arrCount = 0;

                        }
                    }
                }
                else
                {
                    arrCount = 0;
                }
            }
            UpdateGhostPiece();

        }


        if (input.Pressed(TetrisInput.Left) || input.Down(TetrisInput.Left) && !input.Down(TetrisInput.Right))
        {
            if (input.Pressed(TetrisInput.Left))
            {
                dasCount = 0;
                if (currentBlock.TryMoveDistanceX(-1)) MoveEffect();
            }
            else
            {
                dasCount++;
                if (dasCount >= DAS)
                {
                    if (ARR == 0)
                    {
                        currentBlock.TryMoveDistanceX(-100);
                        BlockCollisionEffect();
                    }
                    else
                    {
                        arrCount++;
                        if (arrCount > ARR)
                        {
                            if(currentBlock.TryMoveDistanceX(-1)) MoveEffect();
                            arrCount = 0;
                        }
                    }
                }
                else
                {
                    arrCount = 0;
                }
            }
            UpdateGhostPiece();
        }

        updateInProgress = false;
        yield break;
    }

    public virtual IEnumerator LockBlock()
    {
        currentBlock.Place();

        score += 5 * scoreMult;

        yield return StartCoroutine(CheckLineClears());

        LockBlockEffect();

        yield break;
    }

    public virtual void UpdateGhostPiece()
    {
        ghostBlockDisplayer.ApplyNewPieceState(currentBlock.GetState(), currentBlock.GetRotation());
        ghostBlockDisplayer.SetPosition(tetrisGrid.FromGrid(currentBlock.CalculateGhostPlacement()));
    }

    public virtual void ResetControl()
    {
        arrCount = 0;
        tldCount = 0;
        rldCount = 0;
        dasCount = 0;
        sdsCount = 0;
        hasHeldBlock = false;
        hasTouchedGround = false;
        currentBlock.placed = false;
    }

    public virtual void SwapHoldBlock()
    {
        var temp = holdBlock;

        holdBlock = currentBlock.GetState();

        holdBlockDisplayer.ApplyNewPieceState(holdBlock, 0);

        currentBlock.Remove();

        if (temp != null)
        {
            if(!currentBlock.TrySpawn(tetrisGrid, temp, spawnPosition))
            {
                EndGame();
                return;
            }
            SpawnBlockEffect();
            ResetControl();
        }
        else
        {
            SpawnNextBlock();
        }
        
        UpdateGhostPiece();

        SwapBlockEffect();
    }

    public virtual IEnumerator CheckLineClears(int linesInClear = 0)
    {
        int line = -1;
        bool rain = false;
        List<TetrisTileColour> row = new List<TetrisTileColour>();

        for(int y = 0; y < tetrisGrid.gridCells.y; y++)
        {
            bool full = true;
            for (int x = 0; x < tetrisGrid.gridCells.x; x++)
            {
                TetrisTile state = tetrisGrid.GetCell(new Vector2Int(x, y));
                row.Add(state.GetColour());
                if(state.GetColour() == TetrisTileColour.Rainbow)
                {
                    full = true;
                    rain = true;
                    break;
                }
                if (state.GetState() == TetrisTileType.Empty)
                {
                    full = false;
                }
            }
            if(full)
            {
                line = y;
                break;
            }
        }

        if(line != -1)
        {
            if(currentObjective.CheckObjective(row) || rain)
            {
                if(useClearEffect) yield return tetrisGrid.BurstLine(line, true);
                tetrisGrid.ShiftDownFrom(line);
                LineClearedEffect();
                linesInClear++;
                if (!rain) score += 50 * linesInClear * scoreMult;
                else score += 100 * scoreMult;
            }
            else
            {
                score -= 10;
                if (useClearEffect) yield return tetrisGrid.BurstLine(line, false);
                if (cheeseGarbage)
                {
                    tetrisGrid.ShiftUpTo(line);
                    tetrisGrid.FillRandomGarbageRow(0, randomGarbageDistri);
                }
                else
                {
                    tetrisGrid.FillRandomGarbageRow(line, randomGarbageDistri);
                }
                LineFailedEffect();
            }
           
            yield return StartCoroutine(CheckLineClears(linesInClear));

            needsNewObjective = true;
        }

        yield break;
    }

    public virtual void GenerateInitialBlocks()
    {
        for (int i = 0; i < 10; i++) GenerateNextPiece();
        upcomingBlockDisplayer.ApplyNewPieceState(upcomingBlocks);
        currentObjective = TetrisRandomiserSystem.GenerateObjective(upcomingBlocks.GetRange(0, 5 > upcomingBlocks.Count ? upcomingBlocks.Count - 1 : 4), currentComplexity);
        UpdateObjectiveDisplayer();
    }

    public virtual void SpawnNextBlock()
    {
        ResetControl();
        GenerateNextPiece();
        if (!currentBlock.TrySpawn(tetrisGrid, upcomingBlocks[0], spawnPosition))
        {
            EndGame();
            return;
        }
        upcomingBlocks.RemoveAt(0);
        upcomingBlockDisplayer.ApplyNewPieceState(upcomingBlocks);
        UpdateGhostPiece();

        if(needsNewObjective)
        {
            currentObjective = TetrisRandomiserSystem.GenerateObjective(upcomingBlocks.GetRange(0, 5 > upcomingBlocks.Count ? upcomingBlocks.Count - 1 : 4), currentComplexity);
            UpdateObjectiveDisplayer();
            needsNewObjective = false;
        }
    }

    public virtual void GenerateNextPiece()
    {
        TetrisTileType block = TetrisRandomiserSystem.Get35WeightedPoolRandom();
        TetrisTileColour colour = TetrisRandomiserSystem.GetBaisedRandomColour();

        TetrisBlockState newBlock = new TetrisBlockState(block, new[] { colour });

        if(Random.RandomRange(0f,1f) <= rainbowChance)
        {
            newBlock.Rainbowify();
        }

        upcomingBlocks.Add(newBlock);
    }

    public virtual void UpdateObjectiveDisplayer()
    {
        lineClearRequirementViewer.ApplySpecialPieceState(currentObjective.colourRequirements);
    }


    public virtual void MoveEffect()
    {
        if(moveSound != null) audioPlayer.PlayOneShot(moveSound);
    }

    public virtual void RotateEffect()
    {
        if(rotateSound != null) audioPlayer.PlayOneShot(rotateSound);
    }

    public virtual void SwapBlockEffect()
    {
        if(swapSound != null) audioPlayer.PlayOneShot(swapSound);
    }
    public virtual void LockBlockEffect()
    {
        if (lockSound != null) audioPlayer.PlayOneShot(lockSound);
        if(!needsNewObjective) currentBlock.SpawnParticles(particlesPrefab);
    }

    public abstract void SpawnBlockEffect();

    public virtual void BlockCollisionEffect()
    {
        if (collideSound != null) audioPlayer.PlayOneShot(collideSound);
    }

    public virtual void LineClearedEffect()
    {
        if (clearSound != null) audioPlayer.PlayOneShot(clearSound);
    }

    public virtual void LineFailedEffect()
    {
        if (clearFailSound != null) audioPlayer.PlayOneShot(clearFailSound);
    }

    protected virtual void DebugChecks()
    {
        if(input.Pressed(TetrisInput.Debug1))
        {
            DAS = 5;
            SDS = 2;
            ARR = 2;
        }

        if (input.Pressed(TetrisInput.Debug2))
        {
            DAS = 8;
            SDS = 5;
            ARR = 3;
        }

        if (input.Pressed(TetrisInput.Debug3))
        {
            DAS = 12;
            SDS = 8;
            ARR = 6;
        }
    }
}

