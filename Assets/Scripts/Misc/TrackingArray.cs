public class TrackingArray<T>
{
    private T[] _array;

    private int _count;
    public int Count
    {
        get { return _count; }
    }

    private int _populatedCount;
    public int PopulatedCount
    {
        get { return _populatedCount; }
    }

    public TrackingArray(int size)
    {
        _array = new T[size];
        _count = size;
        _populatedCount = 0;
    }

    public TrackingArray(T[] arrayToCopy, bool setPopulatedCount=true)
    {
        _array = arrayToCopy;
        _count = arrayToCopy.Length;

        if (setPopulatedCount)
            _populatedCount = arrayToCopy.Length;
    }

    public void Add(T item)
    {
        _array[_populatedCount] = item;
        _populatedCount += 1;
    }

    public void Add(int index, T item)
    {
        _array[index] = item;
        _populatedCount += 1;
    }

    public T Get(int index)
    {
        return _array[index];
    }

    public ref T[] ToArray()
    {
        return ref this._array;
    }
}
