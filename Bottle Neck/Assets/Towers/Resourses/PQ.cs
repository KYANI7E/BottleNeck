using System.Collections;
using System.Collections.Generic;

//this is just a priority queue for resource manager
public class PQ<T>
{
    private T[] list;
    private int[] prits;

    public int highestPriority;

    public PQ()
    {
        list = new T[20];
        prits = new int[20];
        for (int i = 0; i < prits.Length; i++) {
            prits[i] = -1;
        }
        highestPriority = 0;
    }

    public void Enqueue(T obj, int pri)
    {
        if (pri > highestPriority)
            highestPriority = pri;

        for (int i = 0; i < list.Length; i++) {
            if (prits[i] == -1) {
                list[i] = obj;
                prits[i] = pri;
                return;
            }
        }

        T[] temp = list;
        int[] tamp = prits;
        list = new T[temp.Length * 2];
        prits = new int[tamp.Length * 2];

        for (int i = 0; i < temp.Length; i++) {
            list[i] = temp[i];
            prits[i] = tamp[i];
        }
        list[temp.Length] = obj;
        prits[tamp.Length] = pri;
    }

    public T Dequeue()
    {
        int largest = 0;
        int hold = 0;
        for (int i = 0; i < list.Length; i++) {
            if (prits[i] == -1) continue;
            if (prits[i] > largest) {
                largest = prits[i];
                hold = i;
            }
        }

        T n = list[hold];
        prits[hold] = -1;

        for (int i = 0; i < list.Length; i++) {
            if (prits[i] > largest) {
                largest = prits[i];
            }
        }
        highestPriority = largest;


        return n;
    }

    public int Count()
    {
        int count = 0;
        for (int i = 0; i < list.Length; i++) {
            if (prits[i] != -1)
                count++;
        }
        return count;
    }
}
