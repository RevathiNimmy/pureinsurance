


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_renewal_automatic_accept_failure_Add'
GO

CREATE PROCEDURE spe_renewal_automatic_accept_failure_Add
	@insurance_file_cnt int,
	@Failure_Reason varchar(255) 
    
AS
BEGIN
    INSERT INTO renewal_automatic_accept_failure VALUES(@insurance_file_cnt, @Failure_Reason  )
END

GO
