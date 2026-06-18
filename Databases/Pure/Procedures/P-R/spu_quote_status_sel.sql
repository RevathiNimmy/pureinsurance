SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_quote_status_sel'
GO

CREATE PROCEDURE spu_quote_status_sel  
    @insurance_file_cnt int  
AS  
  
 
 SELECT  
     IsNull(base_insurance_folder_cnt,0) as 'base_insurance_folder_cnt',
     IsNull(quote_version,0) as 'quote_version',  
     IsNull(quote_status_id,0)  as 'quote_status_id'
  
 FROM  
 Insurance_File  IFI
 WHERE IFI.insurance_file_cnt = @insurance_file_cnt
    
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 