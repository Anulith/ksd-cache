using System;
using System.Collections;
using System.Collections.Generic;

namespace ksd_cache
{
    class NodeNameMismatchException : Exception
    {
        public NodeNameMismatchException(string name, string nodeName)
            : base(name + "!=" + nodeName)
        {

        }
    }

    abstract class Node
    {
        public Node(string name)
        {
            this.name = name;
        }

        protected string name;
        protected Node parent;

        public abstract Node get(string key);
        public abstract string toString();
        public abstract int toInt();
        public abstract double toDouble();
        public abstract bool toBool();
        public abstract byte[] toBinary();
        public abstract void setValue(string key, string value);        
        public abstract void setValue(string key, int value);
        public abstract void setValue(string key, double value);
        public abstract void setValue(string key, bool value);
        public abstract void setValue(string key, byte[] value);
    }

    class LeafNode : Node
    {
        public LeafNode(string name, string value) : base(name)
        {
            this.value = value;
        }
        
        string value;

        public override string toString()
        {
            return value;
        }

        public override void setValue(string key, string value)
        {
            if (key != name) throw new NodeNameMismatchException(key, name);
            this.value = value;
        }

        public override void setValue(string key, Dictionary<string, string> values)
        {
            throw new NotImplementedException();
        }
    }

    class TreeNode : Node
    {
        Dictionary<string, Node> children;

        public TreeNode(string name) : base(name)
        {
        }

        public override string getValue(string key)
        {
            throw new NotImplementedException();
        }
    }

    class KsdCache
    {
        Node root = new TreeNode();
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}