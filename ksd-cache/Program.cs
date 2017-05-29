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

	class NodeNotFoundException : Exception
	{
		public NodeNotFoundException(string name) : base(name)
		{
		}
	}

	class NodeTypeMismatchException : Exception
	{
		public NodeTypeMismatchException(string msg) : base(msg)
		{
		}
	}


    abstract class Node
    {
		public Node(string name, Node parent)
        {
			if (!(parent is TreeNode)) {
				throw new NodeTypeMismatchException("[parent] Expected TreeNode");
			}
            this.name = name;
			this.parent = parent;
        }

        protected string name;
        protected Node parent;
		protected abstract void addNode (Stack<string> names, Node n);
		protected virtual void addNode (string name, Node n)
		{
			addNode (name.Split ("."), n);
		}

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

	class TreeNode : Node
	{
		Dictionary<string, Node> children;

		public TreeNode(string name, Node parent) : base(name, parent)
		{
		}

		protected override void addNode(Stack<string> names, Node n)
		{
			if (names.Length == 1) {
				children [names [0]] = n;
			} else {
				string key = names.Pop ();
				if(!children.ContainsKey(key)) {
					children [key] = new TreeNode(key, this);
				}
				children [key].addNode (names, n);
			}
		}

		public override string toString()
		{
			throw new NotImplementedException();
		}

		public override int toInt()
		{
			throw new NotImplementedException();
		}

		public override double toDouble()
		{
			throw new NotImplementedException();
		}

		public override bool toBool()
		{
			throw new NotImplementedException();
		}

		public override byte[] toBinary()
		{
			throw new NotImplementedException();
		}

		public abstract void setValue(string key, string value)
		{
			addNode (key, new StringNode (key, this, value));
		}

		public abstract void setValue(string key, int value)
		{
			addNode (key, new IntNode (key, this, value));
		}

		public abstract void setValue(string key, double value)
		{
			addNode (key, new DoubleNode (key, this, value));
		}

		public abstract void setValue(string key, bool value)
		{
			addNode (key, new BoolNode (key, this, value));
		}

		public abstract void setValue(string key, byte[] value)
		{
			addNode (key, new BinaryNode (key, this, value));
		}
	}


	class StringNode : Node
	{
		string value;

		public StringNode(string name, Node parent, string value) : base(name,parent)
		{
			this.value = value;
		}

		public override Node get(string key)
		{
			if (key == this.name) {
				return this;
			}

			throw new NodeNotFoundException (key);
		}

		public override string toString()
		{
			return value;
		}

		public override int toInt()
		{
			return Int32.Parse (value);
		}

		public override double toDouble()
		{
			return Double.Parse (value);
		}

		public override bool toBool()
		{
			return bool.Parse (value);
		}

		public override byte[] toBinary()
		{
			return Encoding.ASCII.GetBytes(value); 
		}

		public override void setValue(string key, string value)
		{
			if (key != name) throw new NodeNameMismatchException(key, name);
			this.value = value;
		}

		public override void setValue(string key, int value)
		{
			if (key != name) throw new NodeNameMismatchException(key, name);
			this.parent.addNode (key, new IntNode (name, parent, value));
		}

		public override void setValue(string key, double value)
		{
			if (key != name) throw new NodeNameMismatchException(key, name);
			this.parent.addNode (key, new DoubleNode (name, parent, value));
		}

		public override void setValue(string key, bool value)
		{
			if (key != name) throw new NodeNameMismatchException(key, name);
			this.parent.addNode (key, new BoolNode (name, parent, value));
		}

		public override void setValue(string key, byte[] value)
		{
			if (key != name) throw new NodeNameMismatchException(key, name);
			this.parent.addNode (key, new BinaryNode (name, parent, value));
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