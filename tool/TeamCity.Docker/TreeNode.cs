using System.Collections.Generic;
using IoC;

namespace TeamCity.Docker
{
    internal class TreeNode<T>
    {
        [CanBeNull] public readonly TreeNode<T> Parent;
        public readonly T Value;
        public readonly IReadOnlyCollection<TreeNode<T>> Children;

        public TreeNode([CanBeNull] TreeNode<T> parent, T value, IReadOnlyCollection<TreeNode<T>> children)
        {
            Parent = parent;
            Value = value;
            Children = children;
        }

        public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value);

        public override string ToString() => Value.ToString();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            var other = (TreeNode<T>) obj;
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }
    }
}
