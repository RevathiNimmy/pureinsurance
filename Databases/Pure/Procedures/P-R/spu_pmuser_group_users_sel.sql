SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_pmuser_group_users_sel'
GO

CREATE  PROCEDURE spu_pmuser_group_users_sel
    @pmuser_group_id integer,
    @effective_date     DATETIME,
    @source_id          INTEGER=NULL,
    @RestrictUserList   INTEGER=NULL,
    @party_cnt		     INTEGER=NULL
AS  
  

/********************************************************************************************************/
/* sp_pmuser_group_users_sel selects ALL users of the supplied User Group */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 07/10/1998 RFC */
/********************************************************************************************************/

SET NOCOUNT ON

/* declare local variables */
DECLARE @sub_group_id integer

/* Create the Temporary Users Table */
CREATE TABLE #Temp_Users (
        user_id int NOT NULL,
        username varchar(255) NOT NULL)

/* Create the Temporary Group Table */
CREATE TABLE #Temp_Groups (
        pmuser_group_id int NOT NULL)

/* Insert the Users from the Top Level Group */
IF ISNULL(@RestrictUserList, 0) = 1 AND ISNULL(@source_id, 0) > 0
BEGIN
    INSERT INTO #Temp_Users (user_id, username)
        SELECT u.user_id, u.username
        FROM pmuser u
        JOIN pmuser_group_user ug ON u.user_id = ug.user_id
        WHERE ug.pmuser_group_id = @pmuser_group_id
        AND u.is_deleted = 0
        AND u.effective_date <= @effective_date
        AND EXISTS (SELECT NULL FROM source WHERE source_id = @source_id)
        AND u.user_id NOT IN (
            SELECT DISTINCT user_id
            FROM pmuser_source
            WHERE source_id = @source_id)
END
ELSE
BEGIN
    INSERT INTO #Temp_Users (user_id, username)
        SELECT u.user_id, username
        FROM pmuser u
        JOIN pmuser_group_user ug ON u.user_id = ug.user_id
        WHERE ug.pmuser_group_id = @pmuser_group_id
        AND u.is_deleted = 0
        AND u.effective_date <= @effective_date
END

/* Insert the Sub Groups from the Top Level Group */
INSERT INTO #Temp_Groups
    (pmuser_group_id)
SELECT pmuser_member_group_id
FROM pmuser_group_group ugg,
    pmuser_group ug
WHERE ugg.pmuser_group_id = @pmuser_group_id
AND ugg.pmuser_group_id = ug.pmuser_group_id
      AND ug.is_deleted = 0
      AND ug.effective_date <= @effective_date

/* Add the Users from each Sub Group */
SELECT @sub_group_id = MIN(pmuser_group_id)
FROM #Temp_Groups

WHILE (@sub_group_id <> NULL)
BEGIN

    /* Insert the Users from the Sub Group */
    IF ISNULL(@RestrictUserList, 0) = 1 AND ISNULL(@source_id, 0) > 0
    BEGIN
        INSERT INTO #Temp_Users (user_id, username)
            SELECT u.user_id, u.username
            FROM pmuser u
            JOIN pmuser_group_user ug ON u.user_id = ug.user_id
			--Start (Sankar)-(Tech Spec - UIIC WR01 - User Access - Get User Group Users.doc)
			--As told by gaurav, changing the where condition from @pmuser_group_id to @sub_group_id
            WHERE ug.pmuser_group_id = @sub_group_id
			--End (Sankar)-(Tech Spec - UIIC WR01 - User Access - Get User Group Users.doc)
            AND u.is_deleted = 0
            AND u.effective_date <= @effective_date
            AND ug.user_id NOT IN
                (SELECT user_id FROM #Temp_Users)
            AND EXISTS (SELECT NULL FROM source WHERE source_id = @source_id)
            AND u.user_id NOT IN (
                SELECT DISTINCT user_id
                FROM pmuser_source
                WHERE source_id = @source_id)
    END
    ELSE
    BEGIN
        INSERT INTO #Temp_Users (user_id, username)
            SELECT u.user_id, username
            FROM pmuser u,
            pmuser_group_user ug
            WHERE u.user_id = ug.user_id
            AND ug.pmuser_group_id = @sub_group_id
            AND u.is_deleted = 0
            AND u.effective_date <= @effective_date
            AND ug.user_id NOT IN
                (SELECT user_id FROM #Temp_Users)
    END

    /* Insert the Sub Groups from the Sub Group */
    INSERT INTO #Temp_Groups
        (pmuser_group_id)
    SELECT pmuser_member_group_id
      FROM pmuser_group_group ugg,
        pmuser_group ug
    WHERE ugg.pmuser_group_id = @sub_group_id
      AND ugg.pmuser_group_id = ug.pmuser_group_id
          AND ug.is_deleted = 0
          AND ug.effective_date <= @effective_date
              AND ugg.pmuser_group_id NOT IN
        (SELECT pmuser_group_id
         FROM #Temp_Groups)

    /* Delete the Group we have Just Done */
    DELETE
    FROM #Temp_Groups
    WHERE pmuser_group_id = @sub_group_id

    /* Get the Next Sub Group */
    SELECT @sub_group_id = MIN(pmuser_group_id)
    FROM #Temp_Groups

END

  
/* Select the Users from the Temporary Table */  
SELECT t.user_id,  
    t.username,p.email_address  
FROM #Temp_Users t join PMUser p on t.user_id=p.user_id
WHERE p.party_cnt = @party_cnt  or @party_cnt is null
ORDER BY  
    t.username ASC  
  
/* Drop the Temporary Users & Groups Table */
DROP TABLE #Temp_Users
DROP TABLE #Temp_Groups

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

