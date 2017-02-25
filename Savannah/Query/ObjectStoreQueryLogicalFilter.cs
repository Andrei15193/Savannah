using System;

namespace Savannah.Query
{
    internal class ObjectStoreQueryLogicalFilter
        : ObjectStoreQueryFilter
    {
        private readonly ObjectStoreQueryOperator _operator;
        private readonly ObjectStoreQueryOperator _notOperator;

        internal ObjectStoreQueryLogicalFilter(ObjectStoreQueryFilter leftOperand, ObjectStoreQueryOperator @operator, ObjectStoreQueryFilter rightOperand, ObjectStoreQueryOperator notOperator)
        {
#if DEBUG
            if (leftOperand == null)
                throw new ArgumentNullException(nameof(leftOperand));
            if (rightOperand == null)
                throw new ArgumentNullException(nameof(rightOperand));
#endif
            LeftOperand = leftOperand;
            RightOperand = rightOperand;

            _operator = @operator;
            _notOperator = notOperator;
        }

        internal ObjectStoreQueryFilter LeftOperand { get; }

        internal ObjectStoreQueryFilter RightOperand { get; }

        public override ObjectStoreQueryOperator Operator
            => _operator;

        public override ObjectStoreQueryFilter Not()
            => new ObjectStoreQueryLogicalFilter(LeftOperand.Not(), _notOperator, RightOperand.Not(), _operator);

        public sealed override string ToString()
            => $"({LeftOperand} {Operator} {RightOperand})";
    }
}