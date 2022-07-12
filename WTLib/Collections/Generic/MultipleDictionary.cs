using System.Collections.Generic;

namespace WTLib.Collections.Generic
{
    public class MultipleDictionary<TK1, TK2, TV> : Dictionary<(TK1, TK2), TV>
    {
        public IEnumerable<TV> this[TK1 k1]
        {
            get
            {
                if (k1 == null)
                    yield return default;

                foreach (var item in this)
                {
                    if (item.Key.Item1.Equals(k1))
                    {
                        yield return item.Value;
                    }
                }
            }
        }

        public TV this[TK1 k1, TK2 k2]
        {
            get => this[(k1, k2)];
            set => this[(k1, k2)] = value;
        }

        public void Add(TK1 k1, TK2 k2, TV v)
        {
            Add((k1, k2), v);
        }

        public void Remove(TK1 k1, TK2 k2)
        {
            Remove((k1, k2));
        }

        public bool ContainsKey(TK1 k1, TK2 k2)
        {
            return ContainsKey((k1, k2));
        }
    }

    public class MultipleDictionary<TK1, TK2, TK3, TV> : Dictionary<(TK1, TK2, TK3), TV>
    {
        public IEnumerable<TV> this[TK1 k1]
        {
            get
            {
                if (k1 == null)
                    yield return default;

                foreach (var item in this)
                {
                    if (item.Key.Item1.Equals(k1))
                    {
                        yield return item.Value;
                    }
                }
            }
        }

        public TV this[TK1 k1, TK2 k2, TK3 k3]
        {
            get => this[(k1, k2, k3)];
            set => this[(k1, k2, k3)] = value;
        }

        public void Add(TK1 k1, TK2 k2, TK3 k3, TV v)
        {
            Add((k1, k2, k3), v);
        }

        public void Remove(TK1 k1, TK2 k2, TK3 k3)
        {
            Remove((k1, k2, k3));
        }

        public bool ContainsKey(TK1 k1, TK2 k2, TK3 k3)
        {
            return ContainsKey((k1, k2, k3));
        }
    }

    public class MultipleDictionary<TK1, TK2, TK3, TK4, TV> : Dictionary<(TK1, TK2, TK3, TK4), TV>
    {
        public IEnumerable<TV> this[TK1 k1]
        {
            get
            {
                if (k1 == null)
                    yield return default;

                foreach (var item in this)
                {
                    if (item.Key.Item1.Equals(k1))
                    {
                        yield return item.Value;
                    }
                }
            }
        }

        public TV this[TK1 k1, TK2 k2, TK3 k3, TK4 k4]
        {
            get => this[(k1, k2, k3, k4)];
            set => this[(k1, k2, k3, k4)] = value;
        }

        public void Add(TK1 k1, TK2 k2, TK3 k3, TK4 k4, TV v)
        {
            Add((k1, k2, k3, k4), v);
        }

        public void Remove(TK1 k1, TK2 k2, TK3 k3, TK4 k4)
        {
            Remove((k1, k2, k3, k4));
        }

        public bool ContainsKey(TK1 k1, TK2 k2, TK3 k3, TK4 k4)
        {
            return ContainsKey((k1, k2, k3, k4));
        }
    }

    public class MultipleDictionary<TK1, TK2, TK3, TK4, TK5, TV> : Dictionary<(TK1, TK2, TK3, TK4, TK5), TV>
    {
        public IEnumerable<TV> this[TK1 k1]
        {
            get
            {
                if (k1 == null)
                    yield return default;

                foreach (var item in this)
                {
                    if (item.Key.Item1.Equals(k1))
                    {
                        yield return item.Value;
                    }
                }
            }
        }

        public TV this[TK1 k1, TK2 k2, TK3 k3, TK4 k4, TK5 k5]
        {
            get => this[(k1, k2, k3, k4, k5)];
            set => this[(k1, k2, k3, k4, k5)] = value;
        }

        public void Add(TK1 k1, TK2 k2, TK3 k3, TK4 k4, TK5 k5, TV v)
        {
            Add((k1, k2, k3, k4, k5), v);
        }

        public void Remove(TK1 k1, TK2 k2, TK3 k3, TK4 k4, TK5 k5)
        {
            Remove((k1, k2, k3, k4, k5));
        }

        public bool ContainsKey(TK1 k1, TK2 k2, TK3 k3, TK4 k4, TK5 k5)
        {
            return ContainsKey((k1, k2, k3, k4, k5));
        }
    }

    public class MultipleDictionary<TK1, TK2, TK3, TK4, TK5, TK6, TV> : Dictionary<(TK1, TK2, TK3, TK4, TK5, TK6), TV>
    {
        public IEnumerable<TV> this[TK1 k1]
        {
            get
            {
                if (k1 == null)
                    yield return default;

                foreach (var item in this)
                {
                    if (item.Key.Item1.Equals(k1))
                    {
                        yield return item.Value;
                    }
                }
            }
        }

        public TV this[TK1 k1, TK2 k2, TK3 k3, TK4 k4, TK5 k5, TK6 k6]
        {
            get => this[(k1, k2, k3, k4, k5, k6)];
            set => this[(k1, k2, k3, k4, k5, k6)] = value;
        }

        public void Add(TK1 k1, TK2 k2, TK3 k3, TK4 k4, TK5 k5, TK6 k6, TV v)
        {
            Add((k1, k2, k3, k4, k5, k6), v);
        }

        public void Remove(TK1 k1, TK2 k2, TK3 k3, TK4 k4, TK5 k5, TK6 k6)
        {
            Remove((k1, k2, k3, k4, k5, k6));
        }

        public bool ContainsKey(TK1 k1, TK2 k2, TK3 k3, TK4 k4, TK5 k5, TK6 k6)
        {
            return ContainsKey((k1, k2, k3, k4, k5, k6));
        }
    }
}
