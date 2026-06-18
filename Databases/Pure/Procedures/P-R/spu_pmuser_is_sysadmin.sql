SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmuser_is_sysadmin'
GO


CREATE PROCEDURE spu_pmuser_is_sysadmin
    @user_id SMALLINT,
    @effective_date DATETIME
AS

/********************************************************************************************************/
/* sp_pmuser_is_sysadmin checks if the user is a system administrator. */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 24/11/1998 Tom */
/********************************************************************************************************/
SET NOCOUNT ON

/* declare local variables */
DECLARE @master_group_id integer

/* Create the Temporary Group Table */
CREATE TABLE #Temp_Groups (
        pmuser_group_id int NOT NULL)

/* Create the Temporary Master Group Table */
CREATE TABLE #Temp_Master_Groups (
        pmuser_group_id int NOT NULL)

    /* Insert the Groups that the user is a direct member of */
    INSERT INTO #Temp_Groups
        (pmuser_group_id)
        SELECT ugu.pmuser_group_id
    FROM pmuser_group_user ugu,
        pmuser_group ug
    WHERE ugu.user_id = @user_id
    AND ug.pmuser_group_id = ugu.pmuser_group_id
    AND ug.is_deleted = 0
    AND ug.effective_date <= @effective_date

    /* Also Insert them Into the Master Group Temp Table so that */
    /* we can check to see if they have any master groups */
    INSERT INTO #Temp_Master_Groups
        (pmuser_group_id)
        SELECT ugu.pmuser_group_id
    FROM pmuser_group_user ugu,
        pmuser_group ug
    WHERE ugu.user_id = @user_id
    AND ug.pmuser_group_id = ugu.pmuser_group_id
    AND ug.is_deleted = 0
    AND ug.effective_date <= @effective_date

    /* Check Each Master Group */
    SELECT @master_group_id = MIN(pmuser_group_id)
    FROM #Temp_Master_Groups

    WHILE (@master_group_id <> NULL)
    BEGIN

        /* Insert the Master Groups */
        INSERT INTO #Temp_Groups
            (pmuser_group_id)
            SELECT ugg.pmuser_group_id
        FROM pmuser_group_group ugg,
            pmuser_group ug
        WHERE ugg.pmuser_member_group_id = @master_group_id
          AND ug.pmuser_group_id = ugg.pmuser_group_id
          AND ug.is_deleted = 0
          AND ug.effective_date <= @effective_date
          AND ugg.pmuser_group_id NOT IN
            (SELECT pmuser_group_id
             FROM #Temp_Groups)

        /* Insert each Master Group so that it may also */
        /* be checked for the Master Groups */
        INSERT INTO #Temp_Master_Groups
            (pmuser_group_id)
            SELECT ugg.pmuser_group_id
        FROM pmuser_group_group ugg,
            pmuser_group ug
        WHERE ugg.pmuser_member_group_id = @master_group_id
          AND ug.pmuser_group_id = ugg.pmuser_group_id
          AND ug.is_deleted = 0
          AND ug.effective_date <= @effective_date
          AND ugg.pmuser_group_id NOT IN
            (SELECT pmuser_group_id
             FROM #Temp_Groups)

        /* Delete the Master Group we have just done */
        DELETE
        FROM #Temp_Master_Groups
        WHERE pmuser_group_id = @master_group_id

        /* Check Each Master Group */
        SELECT @master_group_id = MIN(pmuser_group_id)
        FROM #Temp_Master_Groups

    END

    select count('x') how_many
    from PMUser_Group ug,
        #Temp_Groups t
    where ug.pmuser_group_id = t.pmuser_group_id
    and ug.is_sys_admin_group = 1

    /* Drop the Temporary Groups & Sub Groups Table */

    DROP TABLE #Temp_Groups
    DROP TABLE #Temp_Master_Groups
GO


