SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PFInstalments_Result_Sel_By_Code'
GO

CREATE PROCEDURE spu_PFInstalments_Result_Sel_By_Code  
  
@code varchar(20), 
@pfinstalments_result_id  int OUTPUT
  
AS  
  
SELECT @pfinstalments_result_id =pfinstalments_result_id   
FROM pfinstalments_result   
WHERE code = @code  
  


GO
