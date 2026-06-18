SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Lapsed_Reason_Sel_From_Code'
GO

CREATE PROCEDURE spu_Lapsed_Reason_Sel_From_Code  
    @code Char(10)='CCNTRL'   
AS  
  
BEGIN  
	SELECT lapsed_reason_id 
	FROM lapsed_reason 
	WHERE code=@code and is_deleted=0   

END  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO