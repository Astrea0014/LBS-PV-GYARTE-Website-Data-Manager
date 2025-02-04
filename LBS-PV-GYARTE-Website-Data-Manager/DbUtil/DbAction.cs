namespace DataManager.DbUtil
{
    internal enum DbActionType
    {
        TABLE,
        DATA
    }
    internal interface IDbAction
    {
        abstract DbActionType Type { get; }
    }

    internal enum DbDataAction
    {
        INSERT,
        UPDATE,
        DELETE
    }
    internal interface IDbDataAction : IDbAction
    {
        DbActionType IDbAction.Type => DbActionType.DATA;
        abstract DbDataAction Action { get; }
        abstract string SqlTableName { get; set; }
    }
    internal interface IDbConditionalDataAction : IDbDataAction
    {
        abstract string SqlCondition { get; set; }
    }
    internal struct DbInsertDataAction : IDbDataAction
    {
        public readonly DbDataAction Action => DbDataAction.INSERT;
        public string SqlTableName { get; set; }
    }
    internal struct DbUpdateDataAction : IDbConditionalDataAction
    {
        public readonly DbDataAction Action => DbDataAction.UPDATE;
        public string SqlTableName { get; set; }
        public string SqlCondition { get; set; }
    }
    internal struct DbDeleteDataAction : IDbConditionalDataAction
    {
        public readonly DbDataAction Action => DbDataAction.DELETE;
        public string SqlTableName { get; set; }
        public string SqlCondition { get; set; }
    }
}
