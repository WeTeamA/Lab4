using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{

        public abstract class Operation
        {
            public abstract object Calculate(object[] args);
        }

        public class FloatValue : Operation
        {
            public double value;
            public override object Calculate(object[] args)
            {
                return value;
            }
            public FloatValue(double value)
            {
                this.value = value;
            }
        }

        public class BoolValue : Operation
        {
            public bool value;
            public override object Calculate(object[] args)
            {
                return value;
            }
            public BoolValue(bool value)
            {
                this.value = value;
            }
        }

        public delegate double Compare(double a, double b);
        public delegate double FloatOp1(double a);
        public delegate double FloatOp2(double a, double b);
        public delegate double BoolOp1(double a);
        public delegate double BoolOp2(double a, double b);

        public class FloatOperation1 : Operation
        {
            public FloatOp1 operation;
            public FloatOperation1(FloatOp1 operation)
            {
                this.operation = operation;
            }
            public override object Calculate(object[] args)
            {
                return operation((double)args[0]);
            }
        }

        public class FloatOperation2 : Operation
        {
            public FloatOp2 operation;

            public FloatOperation2(FloatOp2 operation)
            {
                this.operation = operation;
            }

            public override object Calculate(object[] args)
            {
                return operation((double)args[0], (double)args[1]);
            }
        }
        public class BoolOperation1 : Operation
        {
            public BoolOp1 operation;
            public BoolOperation1(BoolOp1 operation)
            {
                this.operation = operation;
            }
            public override object Calculate(object[] args)
            {
                return operation((double)args[0]);
            }
        }

        public class BoolOperation2 : Operation
        {
            public BoolOp2 operation;
            public BoolOperation2(BoolOp2 operation)
            {
                this.operation = operation;
            }
            public override object Calculate(object[] args)
            {
                return operation((double)args[0], (double)args[1]);
            }
        }

        public class Comparsion : Operation
        {
            public Compare operation;
            public Comparsion(Compare operation)
            {
                this.operation = operation;
            }
            public override object Calculate(object[] args)
            {
                return operation((double)args[0], (double)args[1]);
            }
        }
    }

