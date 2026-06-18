SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_select_all_pmlock'
GO

CREATE PROCEDURE spu_select_all_pmlock
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 01/07/1997 TF */
/* 1.1 Amended to return the username so that iPMLock works. 22/12/1998 RFC */


--	Amended By		Date		Description
	-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--	George Harris	12 Dec 2018	Changed the select to rather use a join than joini g on the where clause for performance reasons
/********************************************************************************************************/
BEGIN
	DECLARE @enumEnableExclusiveLocking AS INT = 5174
	DECLARE @enumRemoveExclusiveLockAfter AS INT = 5175
	DECLARE @nOptionValueEnableExclusiveLocking AS INT = 0
	DECLARE @nOptionValueRemoveExclusiveLockAfter AS INT = 0

	SELECT Top 1 @nOptionValueEnableExclusiveLocking= value from  System_Options where option_number = @enumEnableExclusiveLocking
	SELECT Top 1 @nOptionValueRemoveExclusiveLockAfter= value from  System_Options where option_number = @enumRemoveExclusiveLockAfter

	IF ISNULL(@nOptionValueEnableExclusiveLocking,0) = 1 AND ISNULL(@nOptionValueRemoveExclusiveLockAfter,0) > 0
	BEGIN
		DECLARE @Time smallDatetime 
		SELECT @Time = DATEADD(MI ,@nOptionValueRemoveExclusiveLockAfter * -1, GetDate())
		DELETE FROM PMLOCK WHERE locked_time < @Time
	END

    SELECT RTRIM(l.lock_name) lock_name,
        l.lock_value,
        u.username,
        l.locked_time,
        l.locked_by_id,
        l.lock2_value,
		l.is_system_lock,		
		l.lock3_value,
		l.is_exclusive_lock
    FROM PMLock l,
        PMUser u
    WHERE l.locked_by_id = u.user_id
    ORDER BY lock_name,
         lock_value

END
GO


