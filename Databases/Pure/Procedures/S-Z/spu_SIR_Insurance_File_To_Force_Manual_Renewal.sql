SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Insurance_File_To_Force_Manual_Renewal'
GO

CREATE PROCEDURE spu_SIR_Insurance_File_To_Force_Manual_Renewal

@insurance_file_cnt int

AS

BEGIN

	UPDATE insurance_file
	SET is_referred_at_renewal = 1
	WHERE insurance_file_cnt = @insurance_file_cnt

END


GO
