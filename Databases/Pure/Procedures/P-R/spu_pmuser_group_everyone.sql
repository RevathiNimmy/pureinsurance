SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmuser_group_everyone'
GO


CREATE PROCEDURE spu_pmuser_group_everyone
    @pmuser_group_id INT,
    @effective_date DATETIME
AS

/********************************************************************************************************/
/* sp_pmuser_group_groups_sel selects the groups that the group is a member of. */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 07/10/1998 RFC */
/* 1.1 Added Is Supervisor and Is Sys Admin Group flags 20/11/1998 Tom */
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
        SELECT ugg.pmuser_group_id
    FROM pmuser_group_group ugg,
        pmuser_group ug
    WHERE ugg.pmuser_member_group_id = @pmuser_group_id
    AND ug.pmuser_group_id = ugg.pmuser_group_id
    AND ug.is_deleted = 0
    AND ug.effective_date <= @effective_date

    /* Also Insert them Into the Sub Group Temp Table so that */
    /* we can check to see if they have any sub groups */
    INSERT INTO #Temp_Master_Groups
        (pmuser_group_id)
        SELECT ugg.pmuser_group_id
    FROM pmuser_group_group ugg,
        pmuser_group ug
    WHERE ugg.pmuser_member_group_id = @pmuser_group_id
    AND ug.pmuser_group_id = ugg.pmuser_group_id
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

    select "user" user_or_group,
        u.user_id id,
        0 caption_id,
        u.username code,
        u.username description,
        u.is_deleted is_deleted,
        u.effective_date effective_date,
        1 included,
        ugu.is_supervisor is_supervisor,
        0 is_sys_admin_group
        from PMUser u,
        PMUser_Group_User ugu
    where u.user_id = ugu.user_id
    and ugu.pmuser_group_id = @pmuser_group_id
    and u.is_deleted = 0
    and u.effective_date <= @effective_date
    union
    select "group" user_or_group,
        ug.pmuser_group_id id,
        ug.caption_id caption_id,
        ug.code code,
        ug.description description,
        ug.is_deleted is_deleted,
        ug.effective_date effective_date,
        1 included,
        0 is_supervisor,
        ug.is_sys_admin_group is_sys_admin_group
        from PMUser_Group ug,
        PMUser_Group_Group ugg
    where ug.pmuser_group_id = ugg.pmuser_member_group_id
    and ugg.pmuser_group_id = @pmuser_group_id
    and ug.is_deleted = 0
    and ug.effective_date <= @effective_date
    union
    select "user" user_or_group,
        u.user_id id,
        0 caption_id,
        u.username code,
        u.username description,
        u.is_deleted is_deleted,
        u.effective_date effective_date,
        0 included,
        0 is_supervisor,
        0 is_sys_admin_group
        from PMUser u
    where u.user_id not in (select ugu.user_id
                from PMUser_Group_User ugu
                where ugu.pmuser_group_id = @pmuser_group_id)
    and u.is_deleted = 0
    and u.effective_date <= @effective_date
    union
    select "group" user_or_group,
        ug.pmuser_group_id id,
        ug.caption_id caption_id,
        ug.code code,
        ug.description description,
        ug.is_deleted is_deleted,
        ug.effective_date effective_date,
        0 included,
        0 is_supervisor,
        ug.is_sys_admin_group is_sys_admin_group
        from PMUser_Group ug
    where ug.pmuser_group_id not in (select ugg.pmuser_member_group_id
                     from PMUser_Group_Group ugg
                     where ugg.pmuser_group_id = @pmuser_group_id)
    and ug.pmuser_group_id <> @pmuser_group_id
    and ug.pmuser_group_id not in (select t.pmuser_group_id
                    from #Temp_Groups t)
    and ug.is_deleted = 0
    and ug.effective_date <= @effective_date
    order by code

    /* Drop the Temporary Groups & Sub Groups Table */

    DROP TABLE #Temp_Groups
    DROP TABLE #Temp_Master_Groups
GO


