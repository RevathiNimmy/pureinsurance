SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_insurance_file_risk_li_add'
GO

CREATE PROCEDURE spe_insurance_file_risk_li_add
     @insurance_file_cnt int,  
     @risk_cnt int,  
     @status_flag varchar(1),  
     @original_risk_cnt int,  
     @renewed_risk_cnt int = null,
     @is_manually_changed int = null,
     @is_risk_edited int = null 
 AS  
   
     IF @original_risk_cnt = 0  
         SELECT @original_risk_cnt = NULL  
   
     IF @renewed_risk_cnt = 0  
         SELECT @renewed_risk_cnt = NULL  
   
     INSERT INTO insurance_file_risk_link (  
         insurance_file_cnt,  
         risk_cnt,  
         status_flag,  
         original_risk_cnt,  
         renewed_risk_cnt,
 	is_manually_changed,
        is_risk_edited)  
     VALUES (  
         @insurance_file_cnt,  
         @risk_cnt,  
         @status_flag,  
         @original_risk_cnt,  
         @renewed_risk_cnt,
 	 @is_manually_changed,
         @is_risk_edited)  
   
     INSERT INTO insurance_file_persistent_risk_link (  
         insurance_file_cnt,  
         risk_cnt,  
         status_flag,  
         original_risk_cnt,  
         renewed_risk_cnt,
 	is_manually_changed)  
     VALUES (  
         @insurance_file_cnt,  
         @risk_cnt,  
         @status_flag,  
         @original_risk_cnt,  
         @renewed_risk_cnt,
 	 @is_manually_changed)  
GO


