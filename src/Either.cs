//
// Copyright (c) 2008 Novell, Inc. (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

namespace Fizzler
{
    #region Imports

    using System;
    using System.Collections.Generic;

    #endregion

    // Adapted from Mono Rocks

    abstract class Either<TA, TB>
            : IEquatable<Either<TA, TB>>
    {
        Either() {}

        public static Either<TA, TB> A(TA value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return new AImpl(value);
        }

        public static Either<TA, TB> B(TB value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return new BImpl(value);
        }

        public abstract override bool Equals(object obj);
        public abstract bool Equals(Either<TA, TB> obj);
        public abstract override int GetHashCode();
        public abstract override string ToString();
        public abstract TResult Fold<TResult>(Func<TA, TResult> a, Func<TB, TResult> b);

        sealed class AImpl : Either<TA, TB>
        {
            readonly TA _value;

            public AImpl(TA value) => _value = value;

            public override int GetHashCode() => _value.GetHashCode();

            public override bool Equals(object obj) => Equals(obj as AImpl);

            public override bool Equals(Either<TA, TB> obj) =>
                obj is AImpl a
                && EqualityComparer<TA>.Default.Equals(_value, a._value);

            public override TResult Fold<TResult>(Func<TA, TResult> a, Func<TB, TResult> b)
            {
                if (a == null) throw new ArgumentNullException(nameof(a));
                if (b == null) throw new ArgumentNullException(nameof(b));
                return a(_value);
            }

            public override string ToString() => _value.ToString();
        }

        sealed class BImpl : Either<TA, TB>
        {
            readonly TB _value;

            public BImpl(TB value) => _value = value;

            public override int GetHashCode() => _value.GetHashCode();

            public override bool Equals(object obj) => Equals(obj as BImpl);

            public override bool Equals(Either<TA, TB> obj) =>
                obj is BImpl b
                && EqualityComparer<TB>.Default.Equals(_value, b._value);

            public override TResult Fold<TResult>(Func<TA, TResult> a, Func<TB, TResult> b)
            {
                if (a == null) throw new ArgumentNullException(nameof(a));
                if (b == null) throw new ArgumentNullException(nameof(b));
                return b(_value);
            }

            public override string ToString() => _value.ToString();
        }
    }
}
