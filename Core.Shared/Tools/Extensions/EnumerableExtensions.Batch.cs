using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Core.Shared.Tools.Extensions
{
    public static partial class EnumerableExtensions
    {
        /// <summary>
        /// https://github.com/morelinq/MoreLINQ/blob/dbad4803bedfef69f6a27cd897d0a4b2a66ce491/MoreLinq/Batch.cs#L65-L102
        /// </summary>
        /// <summary>
        /// Batches the source sequence into sized buckets.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of buckets.</param>
        /// <returns>A sequence of equally sized buckets containing elements of the source collection.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results (buckets and bucket content). 
        /// </remarks>
        public static IEnumerable<IEnumerable<TSource>> Batch<TSource>(this IEnumerable<TSource> source, int size)
        {
            return Batch(source, size, x => x);
        }

        /// <summary>
        /// https://stackoverflow.com/a/13710023/6098146
        /// Batches the source sequence into sized buckets and applies a projection to each bucket.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of result returned by <paramref name="resultSelector"/>.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="size">Size of buckets.</param>
        /// <param name="resultSelector">The projection to apply to each bucket.</param>
        /// <returns>A sequence of projections on equally sized buckets containing elements of the source collection.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results (buckets and bucket content).
        /// </remarks>
        public static IEnumerable<TResult> Batch<TSource, TResult>(
            this IEnumerable<TSource> source,
            int size,
            Func<IEnumerable<TSource>, TResult> resultSelector
        )
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return BatchImpl(source, size, resultSelector);
        }

        private static IEnumerable<TResult> BatchImpl<TSource, TResult>(
            this IEnumerable<TSource> source,
            int size,
            Func<IEnumerable<TSource>, TResult> resultSelector
        )
        {
            Debug.Assert(source != null);
            Debug.Assert(size > 0);
            Debug.Assert(resultSelector != null);

            TSource[] bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                {
                    bucket = new TSource[size];
                }

                bucket[count++] = item;

                // The bucket is fully buffered before it's yielded
                if (count != size)
                {
                    continue;
                }

                // Select is necessary so bucket contents are streamed too
                yield return resultSelector(bucket);

                bucket = null;
                count = 0;
            }

            // Return the last bucket with all remaining elements
            if (bucket != null && count > 0)
            {
                Array.Resize(ref bucket, count);
                yield return resultSelector(bucket);
            }
        }
    }
}
