using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace A
{
    class Poluput : IComparable<Poluput>
    {
        public long value;
        public int lenght;


        public Poluput(Node a)
        {
            value = a.value;
            if (a.right == null || a.left == null)
            {
                lenght = a.height;
            }
            else
            {
                if (a.height - 1 == a.right.height)
                {
                    lenght = a.height + a.left.height + 1;
                }
                else if (a.height - 1 == a.left.height)
                {
                    lenght = a.height + a.right.height + 1;
                }
            }
        }

        public int CompareTo(Poluput a)
        {
            if (lenght.CompareTo(a.lenght) != 0)
            {
                return lenght.CompareTo(a.lenght);
            }
            else
            {
                return -(value.CompareTo(a.value));
            }
        }
    }

    class Node
    {
        public long value;
        public Node left;
        public Node right;
        public Node father;
        public int height;
        public Node(long data)
        {
            value = data;
            left = null;
            right = null;
            father = null;
            height = 0;
        }
        Node(long data, Node father)
        {
            value = data;
            left = null;
            right = null;
            this.father = father;
            height = 0;
        }

        public void add(long data)
        {
            if (this == null || this.value == data)
            {
                return;
            }
            else if (this.value < data)
            {
                if (this.right == null)
                {
                    this.right = new Node(data, this);
                }
                else
                {
                    this.right.add(data);
                }
            }
            else
            {
                {
                    if (this.left == null)
                    {
                        this.left = new Node(data, this);
                    }
                    else
                    {
                        this.left.add(data);
                    }
                }
            }
        }

        public int size(int a)
        {
            a++;
            int l = a, r = a;
            if (left != null)
            {
                l = size(l);
            }

            if (right != null)
            {
                r = size(r);
            }

            if (r > l)
            {
                return r;
            }
            else
            {
                return l;
            }
        }
        public void leftTravel(StreamWriter output)
        {
            output.WriteLine(this.value);

            if (this.left != null)
            {
                this.left.leftTravel(output);
            }

            if (this.right != null)
            {
                this.right.leftTravel(output);
            }
        }

        public Node find(long data)
        {
            if (this.value == data)
            {
                return this;
            }
            else
            {
                if (this.value > data && this.left != null)
                {
                    return this.left.find(data);
                }
                if (this.right != null && data > this.value)
                {
                    return this.right.find(data);
                }
            }

            return null;
        }


        public void delete(long data)
        {
            Node del = this.find(data);
            if (del == null)
            {
                return;
            }
            if (del.right == null && del.left != null)
            {
                del.right = del.left.right;
                del.value = del.left.value;
                del.left = del.left.left;
            }
            else if (del.right == null && del.left == null)
            {
                Node temp = del.father;
                if (temp.value > data)
                {
                    del.father.left = null;
                }
                else
                {
                    del.father.right = null;
                }
            }
            else if (del.left == null && del.right != null)
            {
                del.left = del.right.left;
                del.value = del.right.value;
                del.right = del.right.right;
            }
            else
            {
                if (del.right.left == null)
                {
                    del.value = del.right.value;
                    del.right = del.right.right;
                }
                else
                {
                    Node temp = del.right;
                    while (temp.left != null)
                    {
                        if (temp.left != null)
                        {
                            temp = temp.left;
                        }
                    }

                    del.value = temp.value;
                    if (temp.right == null)
                    {
                        temp.father.left = null;
                    }
                    else
                    {
                        temp.father.left = temp.right;
                    }

                }
            }
        }


        public void revTravel()
        {
            leftTravel(this);
            rightTravel(this);
            setHeight(this);
        }

        private void leftTravel(Node list)
        {
            if (list.left != null)
            {
                list = list.left;
                leftTravel(list);
                rightTravel(list);
                setHeight(list);
            }
        }

        private void rightTravel(Node list)
        {
            if (list.right != null)
            {
                list = list.right;
                leftTravel(list);
                rightTravel(list);
                setHeight(list);
            }
        }

        private void setHeight(Node list)
        {
            if (list.right == null && list.left == null)
            {
                list.height = 0;
            }
            else if (list.right == null && list.left != null)
            {
                list.height = list.left.height + 1;
            }
            else if (list.right != null && list.left == null)
            {
                list.height = list.right.height + 1;
            }
            else
            {
                if (list.right.height > list.left.height)
                {
                    list.height = list.right.height + 1;
                }
                else
                {
                    list.height = list.left.height + 1;
                }
            }
        }
    }




    class Program
    {
        public static List<Poluput> P = new List<Poluput>();

        public static void createP(Node tree)
        {
            Poluput a = new Poluput(tree);
            P.Add(a);
            if (tree.left != null)
            {
                createP(tree.left);
            }

            if (tree.right != null)
            {
                createP(tree.right);
            }
        }

        static void Main(string[] args)
        {
            StreamReader input = new StreamReader(new FileStream("in.txt", FileMode.Open));
            Node MyTree = new Node(long.Parse(input.ReadLine()));
            while (!input.EndOfStream)
            {
                MyTree.add(long.Parse(input.ReadLine()));
            }
            input.Close();
            MyTree.revTravel();
            createP(MyTree);
            P.Sort();
            MyTree.delete(P[P.Count -1].value);
            StreamWriter outpu = new StreamWriter("out.txt");
            MyTree.leftTravel(outpu);
            outpu.Close();
        }
    }
}

