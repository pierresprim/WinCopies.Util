
        public static unsafe bool Contains<T>(this System.Collections.Generic.IReadOnlyList<T> list, System.Collections.Generic.IReadOnlyList<T> value, EqualityComparison<T> comparison, in int startIndex, in int length, int? lowerBound, int? upperBound)
        {
            if (startIndex < 0 || startIndex >= list.Count)

                throw new ArgumentOutOfRangeException(nameof(startIndex));

            if (length < 0 || length > list.Count - startIndex)

                throw new ArgumentOutOfRangeException(nameof(length));

            if (lowerBound < 0)

                throw new ArgumentOutOfRangeException(nameof(lowerBound));

            if (upperBound < 0)

                throw new ArgumentOutOfRangeException(nameof(upperBound));

            if (lowerBound > upperBound)

                throw new ArgumentException($"{nameof(lowerBound)} must be less than or equal to {nameof(upperBound)}.");

            if (list.Count == 0 || value.Count == 0 || length == 0 || value.Count > length || (upperBound.HasValue && upperBound.Value == 0))

                return false;

            if (lowerBound.HasValue && lowerBound.Value == 0)

                return true;

            int* countPtr = (int*)IntPtr.Zero;

            if (lowerBound.HasValue || upperBound.HasValue)
            {
                countPtr = (int*)Marshal.AllocHGlobal(Marshal.SizeOf<int>());
            }

            Func<bool, bool?> inLoopCondition;
            Func<bool> postLoopResultDelegate;

            bool contains(in int i)
            {
                for (int j = 0; j < value.Count; j++)

                    if (!comparison(list[i + j], value[j]))

                        return false;

                return true;
            }

            if (lowerBound.HasValue)

                if (upperBound.HasValue)
                {
                    inLoopCondition = value =>
                      {
                          if (value && (*countPtr) == upperBound)

                              return false;

                          (*countPtr)++;

                          return null;
                      };

                    postLoopResultDelegate = () =>
                    {
                        Marshal.FreeHGlobal((IntPtr)countPtr);

                        return *countPtr >= lowerBound.Value;
                    };
                }

                else
                {
                    inLoopCondition = value =>
                      {
                          if (value && ++*countPtr == lowerBound)

                              return true;

                          else

                              return null;
                      };

                    postLoopResultDelegate = () =>
                    {
                        Marshal.FreeHGlobal((IntPtr)countPtr);

                        return false;
                    };
                }

            else if (upperBound.HasValue)
            {
                inLoopCondition = value =>
                  {
                      if (value)

                          if ((*countPtr) == upperBound)

                              return false;

                          else

                              (*countPtr)++;

                      return null;
                  };

                postLoopResultDelegate = () =>
                  {
                      Marshal.FreeHGlobal((IntPtr)countPtr);

                      return true;
                  };
            }

            else
            {
                inLoopCondition = value =>
                {
                    if (value)

                        return true;

                    return null;
                };

                postLoopResultDelegate = () => false;
            }

            bool? result;

            for (int i = startIndex; i < length; i++)
            {
                if (value.Count > length - i)

                    return postLoopResultDelegate();

                result = inLoopCondition(contains(i));

                if (result.HasValue)

                    return result.Value;
            }

            return postLoopResultDelegate();
        }

        public static bool Validate(this string s, in string startsWith, in int length, in string value)
        {
            ThrowIfNull(s, nameof(s));
            ThrowIfNull(startsWith, nameof(startsWith));
            ThrowIfNull(value, nameof(value));

            if (startsWith.Length + length + value.Length <= s.Length)
            {
                if (s.Length == 0)

                    return true;

                int i = 0;

                if (startsWith.Length > 0)

                    for (; i < startsWith.Length; i++)
                    {
                        if (s[i] != value[i])

                            return false;
                    }

                if (value.Length > 0)
                {
                    int count = 0;
                    int j;

                    i += length;
                }

                return true;
            }

            else throw new InvalidArgumentException($"The total length of {nameof(startsWith)}, {nameof(length)}, and {nameof(value)} must be less than or equal to the length of {nameof(s)}.");
        }

        public static bool Validate(this string s, in char[] startsWith, in int length, params char[] chars) => s.Validate(new string(startsWith), length, new string(chars));

        public static bool Validate(this string s, in char startsWith, in int length, params char[] chars) => s.Validate(new char[] { startsWith }, length, chars);
