using System;
using System.Collections.Generic;

public class Heap <T> where T : IComparable<T>
{
    T[] items;
    Dictionary<T, int> index = new Dictionary<T, int>();

    public int Count;

    public bool Empty
    {
        get
        {
            return Count == 0;
        }
    }
    
    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];

    }

    public T Pop()
    {
        Count--;
        T head = items[0];
        T tail = items[Count];
        index.Remove(head);
        if (!Empty)
        {
            index[tail] = 0;
            items[0] = items[Count];
        }
        SortDown(items[0]);
        
        return head;
    }

    public void Sort(T item)
    {
        SortUp(item);
        //SortDown(item) for a different scenario
    }

    public bool Contains(T item)
    {
        return index.ContainsKey(item);
    }

    private void SortDown(T item)
    {
        while(!Empty)
        {
            int childIndexLeft  = index[item] * 2 + 1;
            int childIndexRight = index[item] * 2 + 2;
            int smallest = 0;

            //if there is a left child
            if(childIndexLeft < Count)
            {
                smallest = childIndexLeft;
                
                //if there is a right child
                if(childIndexRight < Count)
                {   
                    //chose to swap with the smallest child
                    if(items[childIndexRight].CompareTo(items[childIndexLeft]) < 0)
                    {
                        smallest = childIndexRight;
                    }
                }
                //if the smallest child is smaller than item
                if(items[smallest].CompareTo(item) < 0)
                {
                    //swap them
                    Swap(item, items[smallest]);
                }
                else//you've reached the best index
                {
                    return;
                }
            }
            else//there are no children: you've reached the bottom of the heap
            {
                return;
            }
        }

    }

    public void Add(T item)
    {
        index[item] = Count;
        items[Count] = item;
        Count++;
        SortUp(item);
    }

    private void SortUp(T item)
    {
        int parentIndex;

        //while item is smaller than parentItem
        do
        {
            parentIndex = (index[item] - 1) / 2;
            T parentItem = items[parentIndex];

            if (item.CompareTo(parentItem) < 0)
            { 
                Swap(parentItem, item);
            }
            else
            {
                break;
            }

            
        } while (true) ;
    }

    private void Swap(T itemA, T itemB)
    {
        items[index[itemA]] = itemB;
        items[index[itemB]] = itemA;
        int oldindexA = index[itemA];
        int oldindexB = index[itemB];
        index[itemA] = -1;
        index[itemB] = oldindexA;
        index[itemA] = oldindexB;
    }

}

