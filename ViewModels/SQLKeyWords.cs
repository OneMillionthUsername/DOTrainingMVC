using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTrainingMVC.ViewModels
{
    public enum SQLKeyWordsDML
    {

        SELECT,
        DELETE,
        UPDATE,
        INSERT,
        INTO,
        FROM,
        VALUES
    }
    public enum SQLKeyWordsDDL
    {
        CREATE, ALTER, DROP, RENAME, DATABASE, TABLE
    }

    public enum SQLKeyWordsFunctions
    {
        WHERE, DISTINCT, ORDER, BY, ASCENDING, DESCENDING, GROUP, ON, JOIN, LEFT_JOIN, RIGHT_JOIN, INNER_JOIN, FULL_JOIN, CROSS_JOIN, UNION
    }
}
