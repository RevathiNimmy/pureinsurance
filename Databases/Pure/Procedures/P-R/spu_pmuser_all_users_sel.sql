SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmuser_all_users_sel'
GO


CREATE PROCEDURE spu_pmuser_all_users_sel
    @effective_date     DATETIME,
    @source_id          INTEGER=NULL,
    @RestrictUserList   INTEGER=NULL,
	@AgentKey INT=0
AS

/********************************************************************************************************/
/* sp_pmuser_all_users_sel selects ALL effective Users. */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 07/10/1998 RFC */
/********************************************************************************************************/

SET NOCOUNT ON

/* Select All Effective Users */
IF ISNULL(@RestrictUserList, 0) = 1 AND ISNULL(@source_id, 0) > 0
BEGIN
    SELECT u.user_id, u.username, u.email_address
    FROM pmuser u
    WHERE u.is_deleted = 0
    AND u.effective_date <= @effective_date
    AND EXISTS (SELECT NULL FROM source WHERE source_id = @source_id)
    AND u.user_id NOT IN (
        SELECT DISTINCT user_id
        FROM pmuser_source
        WHERE source_id = @source_id)
	AND (u.party_cnt=@AgentKey OR @AgentKey=0)  
     ORDER BY u.username ASC
END
ELSE
BEGIN
SELECT user_id, username,email_address
    FROM pmuser
    WHERE is_deleted = 0
    AND effective_date <= @effective_date
	AND (party_cnt=@AgentKey OR @AgentKey=0)  
    ORDER BY username ASC

END
GO


