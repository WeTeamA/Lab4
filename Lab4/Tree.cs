using System;
using System.Collections.Generic;
using System.Collections;

namespace Lab4
{
    /// <summary>
    /// Класс, описывающий структуру дерево
    /// Внутри содержится класс Node, описывающий узел дерева
    /// </summary>
    public class Tree<T> : IEnumerable
    {
        public class Node : IEnumerable<Node>
        {
            /// <summary>Родительский элемент</summary>
            public Node Parent;

            /// <summary>Значение </summary>
            public T Value;

            /// <summary>Дочерние элементы </summary>
            public readonly List<Node> Children = new List<Node>();

            public Node() { }

            public Node(Node parent, T value)
            {
                Parent = parent;
                Value = value;
            }


            /// <summary>
            /// Добавление узла с указанным значением в дерево
            /// </summary>
            /// <param name="value">Значение узла</param>
            public void AddChild(T value)
            {
                this.Children.Add(new Node(this, value));
            }

            /// <summary>
            /// Добавление узла с указанным значением в дерево
            /// </summary>
            /// <param name="node">Узел</param>
            public void AddChild(Node node)
            {
                this.Children.Add(node);
            }

            /// <summary>Удаляет узел и его дочерние</summary>
            public void RemoveAll()
            {
                this.Parent.Children.Remove(this);
            }


            /// <summary>
            /// Удаляет узел c переносом дочерних элементов
            /// в родительский данного узла
            /// </summary>>
            public void Remove()
            {
                if (this.Children.Count > 0)
                {
                    foreach (var n in this.Children)
                    {
                        n.Parent = this.Parent;
                        this.Parent.Children.Add(n);
                    }
                }

                this.RemoveAll();
            }

            /// <summary>
            /// Добавляет дочерний элемент к текущему
            /// </summary>
            /// <param name="node">Дочерний элемент</param>
            public void AdoptNode(Node child)
            {
                child.Parent = this;
                this.Children.Add(child);
            }


            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<Node> GetEnumerator()
            {
                if (this == null)
                    yield break;
                yield return this;
                foreach (var nextLevelNode in this.Children)
                    foreach (var node in nextLevelNode)
                        yield return node;
            }
        }


        private Node _root;

        /// <summary>Корень дерева</summary>
        public Node Root { get { return _root; } }

        /// <summary>
        /// Добавление узла в дерево
        /// (Если родитель = null, значение записывается в корень)
        /// </summary>
        /// <param name="parent">Значение родительского элемента</param>
        /// <param name="value">Значение узла</param>

        public void AddNode(Node parent, T value)
        {
            if (parent == null)
            {
                if (_root == null)
                    _root = new Node(parent, value);
                else
                    throw new Exception("Корень уже есть");
            }
            else
                parent.AddChild(value);
        }

        /// <summary>
        /// Удаление узла из дереваc переносом дочерних элементов
        /// в родительский данного узла
        /// </summary>
        /// <param name="node">Удаляемый узел</param>
        public void RemoveNodes(Node node)
        {
            if (node.Parent == null)
                _root = null;
            else
                node.RemoveAll();
        }

        /// <summary>Удаление узла из дерева</summary>
        /// <param name="node">Удаляемый узел</param>
        public void RemoveNode(Node node)
        {
            if (node.Parent == null)
                _root = null;
            else
                node.Remove();
        }

        /// <summary>Смена родительского элемента</summary>
        /// <param name="parent">Новый родитель</param>
        /// <param name="child">Ребенок</param>
        public void AdoptNode(Node parent, Node child)
        {
            child.RemoveAll();
            parent.AdoptNode(child);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Node> GetEnumerator()
        {
            return _root.GetEnumerator();
        }
    }
}
