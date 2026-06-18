SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_claimriskdetails'
GO


CREATE PROCEDURE spu_claimriskdetails
    @Risk INT
AS


DECLARE @SQL VARCHAR(1000)

     /* Create temporary table */
    CREATE TABLE #TempID
        (temp_id int IDENTITY(1,1),  caption varchar(50), type int,claim_lookup_id  int, lookup_table varchar(30), claim_party_type_id int ,  claim_party_type varchar(20), value varchar(30) )

    /* Build SQL statement  that gets all values we are interested in*/
    SELECT @SQL = "INSERT INTO #TempID "
    SELECT @SQL =  @SQL + "SELECT  rdf.caption, "
    SELECT @SQL =  @SQL +  "rdf.type, "
    SELECT @SQL =  @SQL + "rdf.claim_lookup_id, "
    SELECT @SQL =  @SQL +  "cl.lookup_tablename , "
    SELECT @SQL =  @SQL +  "rdf.claim_party_type_id, "
    SELECT @SQL =  @SQL +  "cpt.Description, "
    SELECT @SQL =  @SQL +  "cudrd.Value "

    SELECT @SQL =  @SQL +  "FROM    risk_data_definition rdf "
    SELECT @SQL =  @SQL + "LEFT OUTER JOIN claim_user_defined_risk_data  cudrd ON cudrd.risk_data_defn_id = rdf.risk_data_defn_id "
    SELECT @SQL =  @SQL +  "LEFT OUTER JOIN claim_lookup cl ON cl.claim_lookup_id = rdf.claim_lookup_id "
    SELECT @SQL =  @SQL + "LEFT OUTER JOIN claim_party_type cpt ON cpt.claim_party_type_id = rdf.claim_party_type_id  "

    SELECT @SQL =  @SQL +  "WHERE rdf.risk_type_id =  " + @Risk

    /* Execute SQL statement */
    EXEC(@SQL)

    /* Drop Temporary Table */
    DROP Table #TempID
GO


