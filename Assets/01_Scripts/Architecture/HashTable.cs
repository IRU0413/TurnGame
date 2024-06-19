using System.Collections.Generic;

public class HashTable
{
    private class Node
    {
        public string key;
        string value;

        public Node(string key, string value)
        {
            this.key = key;
            this.value = value;
        }

        public string Value()
        {
            return value;
        }

        public void Value(string value)
        {
            this.value = value;
        }
    }

    private List<Node>[] data;
    public HashTable(int size)
    {
        this.data = new List<Node>[size];
    }

    private int GetHashCode(string key)
    {
        int hashCode = 0;
        foreach (char c in key.ToCharArray())
        {
            hashCode += c;
        }
        return hashCode;
    }

    private int ConvertToIndex(int hashCode)
    {
        return hashCode % data.Length;
    }

    private Node SearchKey(List<Node> list, string key)
    {
        if (list == null || list.Count == 0)
            return null;

        foreach (Node node in list)
        {
            if (node.key.Equals(key))
                return node;
        }

        return null;
    }

    public void Put(string key, string value, bool log = false)
    {
        int hashCode = GetHashCode(key);
        int index = ConvertToIndex(hashCode);
        UnityEngine.Debug.Log($"Key = {key}, Value = {value}, Hash Code = {hashCode}, Index = {index}");
        List<Node> list = data[index];
        if (list == null)
        {
            list = new List<Node>();
            data[index] = list;
        }

        Node node = SearchKey(list, key);
        if (node == null)
            data[index].Add(new Node(key, value));
        else
            node.Value(value);
    }

    public string Get(string key)
    {
        int hashCode = GetHashCode(key);
        int index = ConvertToIndex(hashCode);
        List<Node> list = data[index];

        if (list == null)
        {
            return null;
        }

        Node node = SearchKey(list, key);
        return node.Value();
    }
}
