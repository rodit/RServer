using System;
using System.Collections.Generic;
using System.Text;

namespace RUtils.Comparers
{
    public delegate int CompareFunction<T>(T t0, T t1);

    public class FunctionComparer<T> : Comparer<T>
    {
        private readonly CompareFunction<T> _func;

        public FunctionComparer(CompareFunction<T> func)
        {
            _func = func;
        }

        public override int Compare(T x, T y)
        {
            return _func(x, y);
        }
    }
}
