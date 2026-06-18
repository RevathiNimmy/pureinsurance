
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_Previous_RiskCnt_ForBackDatedMTA'
GO

Create Procedure spu_get_Previous_RiskCnt_ForBackDatedMTA
	@Insurance_file_cnt int,
	@Risk_cnt int
AS

Declare  @insurance_folder_cnt int , @Cover_start_date DateTime,  @Risk_Folder_cnt int ,
@Insurance_file_cnt_prev int

Select @insurance_folder_cnt = Insurance_folder_cnt, @Cover_start_date = Cover_start_date
 From insurance_file  (NOLOCK) Where Insurance_file_cnt = @Insurance_file_cnt

Select @Insurance_file_cnt_prev = MAX(insurance_file_cnt) from insurance_file (NOLOCK) Where Insurance_folder_cnt = @insurance_folder_cnt
And cover_start_date <= @Cover_start_date and Insurance_file_cnt <> @Insurance_file_cnt

Select @Risk_Folder_cnt = Risk_Folder_cnt From Risk(NOLOCK)  where risk_cnt= @Risk_cnt

Select Insurance_file_risk_link.Risk_cnt From Insurance_file_risk_link  (NOLOCK)
INNER Join Risk (NOLOCK) ON Insurance_file_risk_link.risk_cnt=Risk.risk_cnt
where  insurance_file_cnt = @Insurance_file_cnt_prev  AND Risk.risk_folder_cnt=@Risk_Folder_cnt

