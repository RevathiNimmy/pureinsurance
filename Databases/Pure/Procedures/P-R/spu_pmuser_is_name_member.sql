SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmuser_is_name_member'
GO


CREATE PROCEDURE spu_pmuser_is_name_member
    @username varchar(255),
    @group_code char(10)
AS

/********************************************************************************************************/
/* sp_pmuser_is_name_member selects the supplied group if the user is a member of it. */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 01/09/1999 DAK */
/********************************************************************************************************/
BEGIN

SET NOCOUNT ON

/* declare local variables */DECLARE @check_group_id integer
DECLARE @user_id integer

    SELECT @user_id = (SELECT user_id
               FROM PMUser u
               WHERE u.username = @username)

/* Create the Temporary Group Table */
CREATE TABLE #Temp_Groups (
        pmuser_group_id int NOT NULL)

/* Create the Temporary Sub Group Table */
CREATE TABLE #Temp_Check_Groups (
        pmuser_group_id int NOT NULL)

    /* Insert the Groups that the user is a direct member of */
    INSERT INTO #Temp_Groups
        (pmuser_group_id)
    SELECT pmuser_group_id
    FROM pmuser_group_user ugu
    WHERE ugu.user_id = @user_id

    /* Also Insert them Into the Check Group Temp Table so that */
    /* we can check to see if the Groups are themselves a member*/
        /* of any other groups */
    INSERT INTO #Temp_Check_Groups
        (pmuser_group_id)
    SELECT pmuser_group_id
    FROM pmuser_group_user ugu
    WHERE ugu.user_id = @user_id

    /* Check Each Group to be Checked*/
    SELECT @check_group_id = MIN(pmuser_group_id)
    FROM #Temp_Check_Groups

    WHILE (@check_group_id <> NULL)
    BEGIN

        /* Insert the Groups they are a member of */
        INSERT INTO #Temp_Groups
            (pmuser_group_id) SELECT ugg.pmuser_group_id
        FROM pmuser_group_group ugg,
            pmuser_group ug
        WHERE ugg.pmuser_member_group_id = @check_group_id
          AND ugg.pmuser_group_id = ug.pmuser_group_id
          AND ugg.pmuser_group_id NOT IN
            (SELECT pmuser_group_id
             FROM #Temp_Groups)

        /* Check the groups they are a member of also */
        INSERT INTO #Temp_Check_Groups
            (pmuser_group_id) SELECT
            ugg.pmuser_group_id
        FROM pmuser_group_group ugg,
            pmuser_group ug
        WHERE ugg.pmuser_member_group_id = @check_group_id
          AND ugg.pmuser_group_id = ug.pmuser_group_id
          AND ugg.pmuser_group_id NOT IN
            (SELECT pmuser_group_id
             FROM #Temp_Check_Groups)

        /* Delete the Group we have Just Checked */
        DELETE
        FROM #Temp_Check_Groups
        WHERE pmuser_group_id = @check_group_id

        /* Check Each Sub Group */
        SELECT @check_group_id = MIN(pmuser_group_id)
        FROM #Temp_Check_Groups

    END

    /* Select the Results */
    SELECT ug.code
    FROM #Temp_Groups tg,
        pmuser_group ug
    WHERE ug.pmuser_group_id = tg.pmuser_group_id
    AND ug.code = @group_code

    /* Drop the Temporary Groups & Sub Groups Table */
    DROP TABLE #Temp_Groups
    DROP TABLE #Temp_Check_Groups

END
GO


