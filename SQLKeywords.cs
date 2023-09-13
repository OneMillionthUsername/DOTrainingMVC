using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTrainingMVC
{
    public enum SQLBlueKeywords
    {
        SELECT, 
        ADD, 
        CONSTRAINT, 
        GROUP_BY, 
        ORDER_BY, 
        IDENTITY, 
        INT, 
        FLOAT, 
        RETURN, 
        RETURNS, 
        BEGIN, 
        END, 
        GO, 
        AS, 
        ON, 
        BY,
        INSERT, 
        ALTER, 
        DELETE, 
        CREATE, 
        DROP, 
        INTO,
        FROM,
        IF,
        SET,
        DATABASE,
        DECLARE,
        NVARCHAR,
        PRIMARY,
        KEY,
        CLUSTERED,
        INDEX,
        REFERENCES,
        CHECK,
        VALUES,
        AFTER,
        BIT,
        VARCHAR,
        CHAR,
        NCHAR,
        DATETIME,
        MONEY,
        UNION,
        TOP,
        UNIQUE,
        DEFAULT,
        VIEW,
        ASC,
        ASCENDING,
        DESC,
        DESCENDING,
        PROCEDURE,
        DISTINCT,
        EXEC,
        HAVING,
        GROUP,
        ORDER,
        FUNCTION,
        WHERE
    }

    public enum SQLGreyKeywords
    {
        NOT,
        NULL,
        BETWEEN,
        AND,
        OR,
        LEFT,
        RIGHT,
        JOIN,
        INNER,
        OUTER,
        FULL,
        CROSS,
        LIKE
    }

    public enum SQLFunctions
    {
        AVG,
        CAST,
        SUBSTRING,
        LOWER,
        CHARINDEX,
        CONVERT,
        DATENAME,
        YEAR,
        MAX,
        UPDATE,
        GETDATE,
        CONCAT,
        SUM,
        COUNT
    }
}
