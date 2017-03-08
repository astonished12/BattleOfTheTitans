using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Heap
{
    private Node[] heapArray;
    private int maxSize;
    private int currentSize;
    public Heap(int maxHeapSize)
    {
        maxSize = maxHeapSize;
        currentSize = 0;
        heapArray = new Node[maxSize];
    }


    public bool IsEmpty()
    {
        return currentSize == 0;
    }
    public bool Insert(Node node)
    {
        if (currentSize == maxSize)
            return false;
        node.heapIndex = ++currentSize;
        heapArray[currentSize] = node;
        GoUp(currentSize);
        return true;
    }

    public Node GetRoot()
    {
        Node node = heapArray[1];
        Swap(1, currentSize--);
        if (!IsEmpty())
            GoDown(1);
        return node;
    }

    public bool Contains(Node n)
    {
        return Equals(n, heapArray[n.heapIndex]);
    }
    private void GoDown(int p)
    {
        int fiust = 2 * p, fiudr = 2 * p + 1, bun = p;

        if (fiust <= currentSize && heapArray[fiust].CompareTo(heapArray[bun])<0)
            bun = fiust;
        // determin fiul "minim"
        if (fiudr <= currentSize && heapArray[fiudr].CompareTo(heapArray[bun])<0)
            bun = fiudr;

        if (bun != p)
        {
            Swap(p, bun);
            GoDown(bun);
        }
    }

    private void GoUp(int p)
    {
        while (p > 1 && heapArray[p].CompareTo(heapArray[p / 2])<0)
        {
            Swap(p, p / 2); // schimb fiu cu tata
            p /= 2;
        }
    }
    private void Swap(int p, int q)
    {
        Node aux = heapArray[p];
        heapArray[p] = heapArray[q];
        heapArray[q] = aux;
        heapArray[p].heapIndex = p;
        heapArray[q].heapIndex = q;
    }
}