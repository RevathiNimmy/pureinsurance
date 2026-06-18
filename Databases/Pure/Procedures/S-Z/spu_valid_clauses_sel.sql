SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_valid_clauses_sel'
GO


CREATE PROCEDURE spu_valid_clauses_sel
    @document_template_id integer
AS


BEGIN
SET NOCOUNT ON
/*declare local variables*/
DECLARE @check_template_id integer

/*Create the temporary tables*/
CREATE TABLE #Temp_Templates(
            document_template_id     int NOT NULL)

CREATE TABLE #Temp_Check(
            document_template_id     int NOT NULL)

/*Insert our parent template*/
INSERT INTO #Temp_Templates
            (document_template_id)
        VALUES (@document_template_id)

/*Insert templates which directly reference the parent*/
INSERT INTO #Temp_Templates
            (document_template_id)
            SELECT
            document_template_id
        FROM    wording_wording_link
        WHERE calls_template_id = @document_template_id

/*Also Insert  into check table so we can check to see what other templates reference these templates*/
INSERT INTO #Temp_Check
            (document_template_id)
            SELECT
            document_template_id
        FROM    wording_wording_link
        WHERE calls_template_id = @document_template_id

SELECT @check_template_id = MIN(document_template_id)
FROM #Temp_Check

/*Check each member of  check table against wwl to find which clauses reference this each. In this way we build up a complet list of all clauses referenced by parent at any level*/

    WHILE (@check_template_id <> NULL)
    BEGIN
        /* Insert the Groups they are a member of */
        INSERT INTO #Temp_templates
            (document_template_id)
            SELECT
            wwl.document_template_id
        FROM    wording_wording_link wwl,
            document_template dt
        WHERE wwl.calls_template_id = @check_template_id
          AND   wwl.document_template_id = dt.document_template_id
          AND   dt.is_deleted = 0
          AND   wwl.document_template_id NOT IN
            (SELECT document_template_id
             FROM   #Temp_templates)
        /* Check the groups they are a member of also */
        INSERT INTO #Temp_Check
            (document_template_id)
            SELECT
            wwl. document_template_id
        FROM    wording_wording_link wwl,
            document_template dt
        WHERE wwl.calls_template_id = @check_template_id
          AND   wwl.document_template_id = dt.document_template_id
                  AND   dt.is_deleted = 0
          AND   wwl.document_template_id NOT IN
            (SELECT document_template_id
             FROM   #Temp_Check)
        /* Delete the Template we have Just Checked */
        DELETE
        FROM    #Temp_Check
        WHERE   document_template_id = @check_template_id
        /* Check Each Sub Group */
        SELECT @check_template_id = MIN(document_template_id)
        FROM    #Temp_Check
    END
    /*Select the results*/
    SELECT  dt.document_template_id,
        dt.code,
        dt.description
    FROM document_template dt
    WHERE dt.document_type_id = 7 and is_deleted = 0
    AND dt.document_template_id NOT IN
            (SELECT document_template_id
            FROM #Temp_Templates)

ORDER BY
    dt.code ASC

DROP TABLE #Temp_Templates
DROP TABLE #Temp_Check
END
GO


