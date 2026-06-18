SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_GIS_Get_Insurance_File_Product_Details'
GO


CREATE PROCEDURE spu_GIS_Get_Insurance_File_Product_Details

@insurance_file_cnt int

AS
	BEGIN		
		SELECT is_true_monthly_policy 
		FROM product 
		WHERE product_id in (	
			SELECT product_id 
			FROM insurance_file 
			WHERE insurance_file_cnt = @insurance_file_cnt)
	END




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
