using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace BT
{
    

    public abstract class BehaviorTree
    {
        public virtual RootNode root { get; set; }

       

        public abstract void Init();
        public abstract ReturnType Tick();

        public enum ReturnType
        {
            Success,
            Failure,
            OnRunning
        }

        public abstract class Node
        {
            public abstract ReturnType Invoke(out Node leaf);
        }

        public class RootNode : Node
        {
            public Node child;
            public Node RunningNode { get; set; }

            public override ReturnType Invoke(out Node leaf)
            {
                ReturnType result = child.Invoke(out leaf);
                if(result == ReturnType.OnRunning)
                {
                    RunningNode = leaf;
                }
                else
                {
                    RunningNode = null;
                }
                return  result;
            }

            public void SetChild(Node child) => this.child = child;
        }

        public class Execution : Node
        {
            public Func<ReturnType> function;
            public Execution(Func<ReturnType> function) => this.function = function;

            public override ReturnType Invoke(out Node leaf)
            {
                leaf = this;
                return function.Invoke();
            }
        }

        public abstract class CompositeNode : Node
        {
            public List<Node> children = new List<Node>();
            public CompositeNode Addchild(Node child)
            {
                children.Add(child);
                return this;
            }

            public IEnumerable<Node> GetChildren() => children;
        }

        public class Selector : CompositeNode
        {
            private ReturnType _tmpResult;
            public override ReturnType Invoke(out Node leaf)
            {
                foreach (var child in GetChildren())
                {
                    _tmpResult = child.Invoke(out leaf);
                    if (_tmpResult != ReturnType.Failure)
                    {
                        return _tmpResult;
                    }
                }
                leaf = null;
                return ReturnType.Failure;
            }
        }
        public class RandomSelector : CompositeNode
        {
            private ReturnType _tmpResult;
            public override ReturnType Invoke(out Node leaf)
            {
                foreach (var child in GetChildren().OrderBy(node => Guid.NewGuid()))
                {
                    _tmpResult = child.Invoke(out leaf);
                    if (_tmpResult != ReturnType.Failure)
                    {
                        return _tmpResult;
                    }
                }
                leaf=null;
                return ReturnType.Failure;
            }
        }

        public class Sequence : CompositeNode
        {
            private ReturnType _tmpResult;
            public override ReturnType Invoke(out Node leaf)
            {
                foreach (var child in GetChildren())
                {
                    _tmpResult = child.Invoke(out leaf);
                    if (_tmpResult != ReturnType.Success)
                    {
                        return _tmpResult;
                    }
                }
                leaf = null;
                return ReturnType.Success;
            }
        }

        public class ConditionNode : Node
        {
            public Node child;
            public event Func<bool> condition;

            public ConditionNode(Func<bool> condition) => this.condition = condition;
            public void SetChild(Node child) => this.child = child;

            public override ReturnType Invoke(out Node leaf)
            {
                if (condition.Invoke())
                {
                    return child.Invoke(out leaf);
                }
                leaf = null;
                return ReturnType.Failure;
            }
        }

        public abstract class Decorator : Node
        {
            public Node child;
            public void SetChild(Node child) => this.child = child;
            public override ReturnType Invoke(out Node leaf)
            {
                return Decorate(child.Invoke(out leaf));
            }
            protected abstract ReturnType Decorate(ReturnType childreturn);
        }

        public class Repeater : Decorator
        {
            public event Func<bool> condition;
            protected override ReturnType Decorate(ReturnType childreturn)
            {
                if (condition.Invoke())
                {
                    return ReturnType.OnRunning;
                }
                return childreturn;
            }
        }
        public class Inverter : Decorator
        {
            protected override ReturnType Decorate(ReturnType childreturn)
            {
                switch (childreturn)
                {
                    case ReturnType.Success:
                        return ReturnType.Failure;
                    case ReturnType.Failure:
                        return ReturnType.Success;
                    case ReturnType.OnRunning:
                        return ReturnType.OnRunning;
                    default:
                        throw new Exception("inverter : not retrun type");
                }
            }
        }
    }
    
}