SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Check_MTC_Rating_Rules'
GO

CREATE PROCEDURE spu_Check_MTC_Rating_Rules
   @insurance_file_cnt int

AS

	SELECT isnull(enable_mtc_rating_rule,0) 
	FROM   product 
        WHERE product_id =
                        	(
                     			SELECT product_id 
                     			FROM insurance_file 
                     			WHERE insurance_file_cnt=@insurance_file_cnt
				)
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
