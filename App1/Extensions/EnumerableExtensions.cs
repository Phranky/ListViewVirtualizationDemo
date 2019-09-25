using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ItemsRepeaterDemo.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool Merge<T>(this IList<T> source, IEnumerable<T> collection) where T : IMergeable<T>
        {
            var changed = false;

            if (collection != source)
            {
                if (!collection.Any())
                {
                    if (source.Any())
                    {
                        // Remove all items
                        source.Clear();
                        changed = true;
                    }
                }
                else if (!source.Any())
                {
                    if (collection.Any())
                    {
                        // Add all items
                        source.AddRange(collection);
                        changed = true;
                    }
                }
                else
                {
                    var i = 0;

                    foreach (var item in collection)
                    {
                        var j = source.Skip(i).ToList().FindIndex(s => s.Equals(item));

                        if (j == -1)
                        {
                            // Insert new item
                            source.Insert(i, item);
                            changed = true;
                        }
                        else if (j > 0)
                        {
                            // Move existing item
                            source.Move(i + j, i);
                            source[i].Update(item);
                            changed = true;
                        }
                        else
                        {
                            // Update existing item
                            if (source[i].Update(item))
                            {
                                changed = true;
                            }
                        }

                        i++;
                    }

                    // Remove remaining items
                    if (source.Count > i)
                    {
                        source.RemoveRange(i, source.Count - i);
                        changed = true;
                    }
                }
            }

            return changed;
        }

        public static void Move<T>(this IList<T> source, int oldIndex, int newIndex)
        {
            switch (source)
            {
                case ObservableCollection<T> collection:
                    collection.Move(oldIndex, newIndex);
                    break;

                default:
                    var itemToMove = source[oldIndex];
                    source.RemoveAt(oldIndex);
                    source.Insert(newIndex, itemToMove);
                    break;
            }
        }

        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> collection)
        {
            switch (source)
            {
                case List<T> list:
                    list.AddRange(collection);
                    break;

                default:
                    foreach (var itemToAdd in collection) source.Add(itemToAdd);
                    break;
            }
        }

        public static void RemoveRange<T>(this ICollection<T> source, int index, int count)
        {
            switch (source)
            {
                case List<T> list:
                    list.RemoveRange(index, count);
                    break;

                case IList<T> list:
                    foreach (var indexToRemove in Enumerable.Range(index, count).Reverse()) list.RemoveAt(indexToRemove);
                    break;

                default:
                    foreach (var itemToRemove in source.Skip(index).Take(count).Reverse()) source.Remove(itemToRemove);
                    break;
            }
        }

        public interface IMergeable<T>
        {
            bool Equals(T other);
            bool Update(T other);
        }

    }
}
