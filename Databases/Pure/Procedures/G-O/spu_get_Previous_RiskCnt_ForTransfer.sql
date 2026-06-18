
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_Previous_RiskCnt_ForTransfer'
GO

Create Procedure spu_get_Previous_RiskCnt_ForTransfer  
 @Insurance_file_cnt int,  
 @Risk_cnt int,  
 @processed_Insurance_file_cnt  int  
AS  
Declare  @insurance_folder_cnt int , @Cover_start_date DateTime,  @Risk_Folder_cnt int ,  
@Insurance_file_cnt_prev int  

SELECT	@insurance_folder_cnt = Insurance_folder_cnt, 
		@Cover_start_date = Cover_start_date  
FROM	insurance_file  (NOLOCK) 
WHERE	Insurance_file_cnt = @Insurance_file_cnt  


SELECT	@Insurance_file_cnt_prev = MAX(insurance_file_cnt) 
FROM	insurance_file (NOLOCK) 
WHERE	Insurance_folder_cnt = @insurance_folder_cnt  
AND		cover_start_date <= @Cover_start_date 
AND		Insurance_file_cnt not in (@Insurance_file_cnt  ,@processed_Insurance_file_cnt)  
AND		insurance_file_type_id IN (2,5,8,9)
AND  insurance_file_cnt < @processed_Insurance_file_cnt

SELECT	@Risk_Folder_cnt = Risk_Folder_cnt 
FROM	Risk(NOLOCK)  
where	risk_cnt= @Risk_cnt  

Select Insurance_file_risk_link.Risk_cnt 
FROM	Insurance_file_risk_link  (NOLOCK)  
		INNER Join Risk (NOLOCK) 
			ON Insurance_file_risk_link.risk_cnt=Risk.risk_cnt  
WHERE  insurance_file_cnt = @Insurance_file_cnt_prev  AND Risk.risk_folder_cnt=@Risk_Folder_cnt



