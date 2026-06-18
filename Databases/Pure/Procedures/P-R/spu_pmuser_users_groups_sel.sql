SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pmuser_users_groups_sel'
GO

CREATE PROCEDURE spu_pmuser_users_groups_sel
    @user_id INT,
    @effective_date DATETIME,
    @language_id INT,
	@AgentKey INT=0
AS

/* declare local variables */
DECLARE @check_group_id integer

if (convert(datetime, '29/12/1899', 103) = convert(datetime, @effective_date, 103))
	Set @effective_date = null


/* Create the Temporary Group Table */
CREATE TABLE #Temp_Groups 
(
    pmuser_group_id INT NOT NULL,
    is_supervisor TINYINT NOT NULL
)

/* Create the Temporary Sub Group Table */
CREATE TABLE #Temp_Check_Groups 
(
    pmuser_group_id int NOT NULL
)

/* Insert the Groups that the user is a direct member of */
INSERT INTO #Temp_Groups
(
    pmuser_group_id,
    is_supervisor
)
SELECT 
    ugu.pmuser_group_id,
    ugu.is_supervisor
FROM pmuser_group_user ugu
JOIN pmuser_group ug
    ON ug.pmuser_group_id = ugu.pmuser_group_id
WHERE ugu.user_id = @user_id
AND ug.is_deleted = 0
AND CONVERT(date , ug.effective_date,106) <= isnull(CONVERT(date , @effective_date,106), CONVERT(date , ug.effective_date,106))


/* Also Insert them Into the Check Group Temp Table so that */
/* we can check to see if the Groups are themselves a member*/
/* of any other groups */
INSERT INTO #Temp_Check_Groups
(
    pmuser_group_id
)
SELECT 
    ugu.pmuser_group_id
FROM pmuser_group_user ugu
JOIN pmuser_group ug
    ON ug.pmuser_group_id = ugu.pmuser_group_id
WHERE ugu.user_id = @user_id
AND ug.is_deleted = 0
AND CONVERT(date , ug.effective_date,106) <= isnull(CONVERT(date , @effective_date,106), CONVERT(date , ug.effective_date,106))

/* Check Each Group to be Checked*/
SELECT 
    @check_group_id = MIN(pmuser_group_id)
FROM #Temp_Check_Groups

WHILE @check_group_id <> NULL
BEGIN

    /* Insert the Groups they are a member of */
    INSERT INTO #Temp_Groups
    (
        pmuser_group_id,
        is_supervisor
    )
    SELECT
        ugg.pmuser_group_id,
        0
    FROM pmuser_group_group ugg
    JOIN pmuser_group ug
        ON ug.pmuser_group_id = ugg.pmuser_group_id
    WHERE ugg.pmuser_member_group_id = @check_group_id
    AND ug.is_deleted = 0
   AND CONVERT(date , ug.effective_date,106) <= isnull(CONVERT(date , @effective_date,106), CONVERT(date , ug.effective_date,106))
    AND ugg.pmuser_group_id NOT IN
        (
            SELECT 
                pmuser_group_id
            FROM #Temp_Groups
        )

    /* Check the groups they are a member of also */
    INSERT INTO #Temp_Check_Groups
    (
        pmuser_group_id
    )
    SELECT
        ugg.pmuser_group_id
    FROM pmuser_group_group ugg
    JOIN pmuser_group ug
        ON ug.pmuser_group_id = ugg.pmuser_group_id
    WHERE ugg.pmuser_member_group_id = @check_group_id
    AND ug.is_deleted = 0
    AND ug.effective_date <= isnull(@effective_date, ug.effective_date)
    AND ugg.pmuser_group_id NOT IN
        (
            SELECT 
                pmuser_group_id
            FROM #Temp_Check_Groups
        )

    /* Delete the Group we have Just Checked */
    DELETE
    FROM #Temp_Check_Groups
    WHERE pmuser_group_id = @check_group_id

    /* Check Each Sub Group */
    SELECT 
        @check_group_id = MIN(pmuser_group_id)
    FROM #Temp_Check_Groups

END

/* Select the Results */
SELECT DISTINCT
    ug.pmuser_group_id,
    ug.code,
    CONVERT(VARCHAR(255), c.caption),
    tg.is_supervisor
FROM pmuser_group ug
JOIN #Temp_Groups tg
    ON tg.pmuser_group_id = ug.pmuser_group_id
LEFT JOIN pmcaption c
    ON c.caption_id = ug.caption_id
    AND c.language_id = @language_id
LEFT OUTER JOIN PMUser_Group_User pgu ON ug.pmuser_group_id = pgu.pmuser_group_id  
LEFT OUTER JOIN PMUser pu ON pgu.user_id =pu.user_id  
WHERE  (pu.party_cnt=@AgentKey OR @AgentKey=0)  
ORDER BY
    c.caption ASC

/* Drop the Temporary Groups & Sub Groups Table */
DROP TABLE #Temp_Groups
DROP TABLE #Temp_Check_Groups


GO


