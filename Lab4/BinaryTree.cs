using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class BinaryTree<T>
    {
        public class BinaryNode
        {
            public T Data;
            public BinaryNode Parent;
            public readonly List<BinaryNode> Children = new List<BinaryNode>();
            //public BinaryNode Top, Bot;


            public BinaryNode()
            { }


            public BinaryNode(BinaryNode parent, T data)
            {
                Parent = parent;
                Data = data;
            }


            public BinaryNode(T data)
            {
                Data = data;
            }


            public void AddChild(T data)
            {
                if (this.Children.Count < 2)
                    this.Children.Add(new BinaryNode(this, data));
                else
                    throw new Exception("Бинарное дерево, больше нельзя добавлять");
            }


            /// <summary>
            /// Удаление полностью
            /// </summary>
            public void Delete_Child()
            {
                this.Parent.Children.Remove(this);
            }


            public IEnumerable<BinaryNode> EnumerationTree()
            {

                yield return this; //Собираем в стек родителей 
                foreach (BinaryNode Child in this.Children)
                {
                    foreach (BinaryNode Child2 in Child.EnumerationTree())
                    {
                        yield return Child2; //Обращение к первому yield return 
                    }
                }
            }
        }
            private BinaryNode root;
            public BinaryNode Root
            { get { return root; } }

            /// <summary>
            /// добавление элемента к известному родителю
            /// </summary>
            /// <param name="parent"></param>
            /// <param name="item"></param>
            public void AddNode(BinaryNode parent, T item)
            {
                if (parent == null)                     //Если пустой, создаем рут
                    if (root == null)
                    {
                        root = new BinaryNode(parent, item);
                    }
                    else
                        throw new Exception("tree already has a root"); //Если родитель уже имеет наследников
                else
                {
                    parent.AddChild(item);
                }
        }

            /// <summary>
            /// удаление узла и всех детей
            /// </summary>
            /// <param name="parent"></param>
            public void Delete_All(BinaryNode parent)
            {
                if (parent == null)
                    root = null;
                else
                    parent.Delete_Child();
            }


            /// <summary>
            /// Замена элемента
            /// </summary>
            /// <param name="parent"></param>
            /// <param name="Data"></param>
            public void StandinNode(BinaryNode parent, T Data)
            {
                parent.Data = Data;
            }
        
    }
}

