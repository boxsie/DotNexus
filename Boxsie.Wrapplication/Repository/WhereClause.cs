namespace Boxsie.Wrapplication.Repository
{
    public struct WhereClause
    {
        public string PropertyName { get; }
        public string Op { get; }
        public object Val { get; }
        public string AndOr { get; }

        public WhereClause(string propertyName, string op, object val, string andOr = null)
        {
            PropertyName = propertyName;
            Op = op;
            Val = val;
            AndOr = andOr ?? "";
        }
    }
}