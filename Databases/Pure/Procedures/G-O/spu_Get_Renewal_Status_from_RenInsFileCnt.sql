SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Renewal_Status_from_RenInsFileCnt'
GO

CREATE PROCEDURE spu_Get_Renewal_Status_from_RenInsFileCnt
    @ren_ins_file_cnt int
AS  
  
BEGIN  
	SELECT insurance_file_cnt, renewal_status_cnt 
	FROM renewal_status 
	WHERE renewal_insurance_file_cnt=@ren_ins_file_cnt 
END  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO