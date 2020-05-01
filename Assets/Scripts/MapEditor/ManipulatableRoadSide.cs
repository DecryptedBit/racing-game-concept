public class ManipulatableRoadSide
{
    private int _startingVertex;
    public int StartingVertex
    {
        get { return _startingVertex; }
    }

    private int _endingVertex;
    public int EndingVertex
    {
        get { return _endingVertex; }
    }

    private int _pointerIncrementIndex;
    public int PointerIncrementIndex
    {
        get { return _pointerIncrementIndex; }
    }

    public ManipulatableRoadSide(int startingVertex, int endingVertex, int pointerIncrementIndex=1)
    {
        _startingVertex = startingVertex;
        _endingVertex = endingVertex;
        _pointerIncrementIndex = pointerIncrementIndex;
    }
}
