using System;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;
    public int Count => currentItemCount;
    public Heap(int maxHeapSize) => items = new T[maxHeapSize];

    public void Add(T item)
    {
        if (currentItemCount >= items.Length)
        {
            ResizeHeap();
        }
        item.HeapIndex = currentItemCount;
        items[currentItemCount++] = item;
        SortUp(item);
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        items[0] = items[--currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }
    public bool Contains(T item) => Equals(items[item.HeapIndex], item);

    public void UpdateItem(T item) => SortUp(item);
    public void ResetIndexes()
    {
        foreach (T item in items)
        {
            if (item != null) item.HeapIndex = 0;
        }
    }
    void ResizeHeap()
    {
        T[] newItems = new T[items.Length + 100];
        for (int i = 0; i < items.Length; i++)
        {
            newItems[i] = items[i];
        }
        items = newItems;
    }


    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;
            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;
                if (childIndexRight < currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                if (item.CompareTo(items[swapIndex]) < 0) Swap(item, items[swapIndex]);
                else return;
            }
            else return;
        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0) Swap(item, parentItem);
            else break;
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}