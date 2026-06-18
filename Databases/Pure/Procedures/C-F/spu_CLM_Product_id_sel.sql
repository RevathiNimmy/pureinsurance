SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_CLM_Product_id_sel'
GO

CREATE PROCEDURE spu_CLM_Product_id_sel
      @lInsuranceFileCnt int
AS
SELECT report_pointer
           FROM product
             WHERE product_id =(SELECT product_id
            FROM insurance_file
          WHERE insurance_file_cnt = @lInsuranceFileCnt)


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
