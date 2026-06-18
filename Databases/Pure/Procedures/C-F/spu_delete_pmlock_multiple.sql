SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_delete_pmlock_multiple'
GO


CREATE PROCEDURE spu_delete_pmlock_multiple
	@multiple_lock_details TPMLock READONLY
AS


BEGIN

    DELETE PMLock FROM PMLock PM	
	JOIN @multiple_lock_details TPM on UPPER(RTRIM(LTRIM(TPM.lock_name))) = UPPER(RTRIM(LTRIM(PM.lock_name)))
	AND ISNULL(TPM.lock_value,0) = ISNULL(PM.lock_value,0)
	AND ISNULL(PM.Is_system_lock,0) = 0
	

	
	
END
GO


