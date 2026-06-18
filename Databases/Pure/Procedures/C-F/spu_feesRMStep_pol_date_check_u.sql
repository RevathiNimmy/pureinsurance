EXECUTE DDLDropProcedure 'spu_feesRMStep_pol_date_check_u'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE spu_feesRMStep_pol_date_check_u
(
	@InsID	INT
)

AS

SELECT
	cover_start_date,
	this_premium,
	expiry_date,
	annual_premium
FROM Insurance_File
WHERE insurance_file_cnt = @InsID

GO

