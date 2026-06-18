SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_RI_Arrangement_Line_saa_RI2007'     
GO

CREATE PROCEDURE spu_RI_Arrangement_Line_saa_RI2007  
    @ri_arrangement_id int  
AS                        

DECLARE @MultiActs	INT

--Retrieve the Number of FX rows in the Ri_Arrangement_Line table
SELECT 	@MultiActs = Count(*) 
FROM 	Ri_Arrangement_Line
WHERE 	ri_arrangement_id = @ri_arrangement_id
AND 	type = 'FX'


	IF ISNULL(@MultiActs,0) = 0 OR @MultiActs <=1 	--IF We have 1 or No FX Row then execute the following SP
		EXEC spu_RI_Arrangement_Line_Select_RI2007 @ri_arrangement_id = @ri_arrangement_id

	ELSE IF @MultiActs>1 	--IF We have 1 or No FX Row then execute the following SP
		EXEC spu_RI_Arrangement_Line_MultiActs_RI2007 @ri_arrangement_id = @ri_arrangement_id

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


    
  
