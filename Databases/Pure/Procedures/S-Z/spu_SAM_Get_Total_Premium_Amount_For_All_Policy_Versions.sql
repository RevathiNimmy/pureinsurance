SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Total_Premium_Amount_For_All_Policy_Versions'
GO

CREATE PROCEDURE spu_SAM_Get_Total_Premium_Amount_For_All_Policy_Versions

@insurance_ref varchar(30),  
@nFileCnt Int=0 
AS  
  
BEGIN  
 
    CREATE TABLE #tmpPolicyPremiums(
		total_premium_amount money,
		this_premium money,
		insurance_file_tax money,
		risk_tax money,
		policy_fee_amount money,
		risk_fee_amount money,
		fee_tax money,
		commission_value money,
        totalTaxNotAppliedToClient money)

    DECLARE @insurance_file_cnt int
	DECLARE @insurance_folder_cnt int
	SELECT TOP 1 @insurance_folder_cnt=insurance_folder_cnt from insurance_file where insurance_ref=@insurance_ref
    DECLARE PolicyVersion_Cursor Cursor FAST_FORWARD FOR
        SELECT insurance_file_cnt
	FROM insurance_file
	WHERE insurance_folder_cnt = @insurance_folder_cnt and (insurance_file_type_id in (2,5,6,8,9) Or( ISNULL(base_insurance_file_cnt,0)=@nFileCnt and insurance_file_cnt<>@nFileCnt))

    -- Process for lead_commission_Band
    OPEN PolicyVersion_Cursor

    FETCH NEXT FROM PolicyVersion_Cursor INTO @insurance_file_cnt

    WHILE @@Fetch_Status = 0

    BEGIN

    	INSERT INTO #tmpPolicyPremiums
        	EXEC spu_SAM_Get_Total_Premium_Amount @insurance_file_cnt

        -- Fetch the next record
        FETCH NEXT FROM PolicyVersion_Cursor INTO @insurance_file_cnt
    END

    -- Close and Deallocate
    CLOSE PolicyVersion_Cursor
    DEALLOCATE PolicyVersion_Cursor

    SELECT SUM(total_premium_amount) total_premium_amount, SUM(commission_value) total_commission_value,ROUND(SUM(totalTaxNotAppliedToClient),2) total_Tax_Not_Applied_To_Client FROM #tmpPolicyPremiums

    DROP TABLE #tmpPolicyPremiums

END


GO
