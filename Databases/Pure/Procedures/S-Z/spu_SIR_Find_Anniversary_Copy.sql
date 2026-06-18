SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Find_Anniversary_Copy'
GO


CREATE PROCEDURE spu_SIR_Find_Anniversary_Copy

@insurance_ref varchar(30), 
@cover_start_date datetime

AS

BEGIN

	SELECT COUNT(*) FROM Insurance_File
	WHERE (anniversary_copy =1
	OR CONVERT(VARCHAR(10),cover_start_date,110)=CONVERT(VARCHAR(10),anniversary_date,110))  
	AND cover_start_date >=@cover_start_date
	AND insurance_ref = @insurance_ref

END




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
