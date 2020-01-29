using System.Collections.Generic;

namespace TeamCity.Docker
{
    internal class TreeNode<T>
    {
        public readonly T Value;
        public readonly IList<TreeNode<T>> Children;

        public TreeNode(T value)
        {
            Value = value;
            Children = new List<TreeNode<T>>();
        }

        public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value);

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
