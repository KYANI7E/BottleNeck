using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PQ<T>
{
    public struct Item
    {
        public T thing;
        public int pri;

        public Item(T thing, int pri)
        {
            this.thing = thing;
            this.pri = pri;
        }
    }

    List<Item> liste;
    public int highestPriority;

    public PQ()
    {
        this.liste = new List<Item>();
    }

    public void Enqueue(T thing, int priority)
    {
        liste.Add(new Item(thing, priority));
        if (priority < highestPriority) highestPriority = priority;
    }

    public void Enqueue(Item t)
    {
        liste.Add(t);
        if (t.pri < highestPriority) highestPriority = t.pri;
    }

    public void EnqueueItems(List<Item> qu)
    {
        foreach (Item t in qu) Enqueue(t);
    }
    public Item Dequeue()
    {
        Item temp = new Item();
        int secSmallest = 0;
        int smallest = 0;
        bool start = true;
        bool sec = false;
        foreach(Item t in liste) {
            if(start || t.pri < smallest) {
                temp = t;
                if (!start) {
                    secSmallest = smallest;
                    sec = true;
                }
                smallest = t.pri;
                start = false;
            }
        }

        if(sec) highestPriority = secSmallest;

        liste.Remove(temp);
        return temp;
    }

    public int Count()
    {
        int num = 0;
        foreach (Item t in liste) num++;
        return num;
    }
}

