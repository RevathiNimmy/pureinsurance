SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_SIR_Get_Risks_Billed_Premium'
GO
CREATE PROCEDURE spu_SIR_Get_Risks_Billed_Premium  
	@risk_cnt int = null  
AS  
BEGIN  
DECLARE @TotalAmount Numeric(19,4),  
	@Amount Numeric(19,4),  
	@LeviTax numeric(19,4),  
	@original_risk_cnt int,  
	@TempTotalAmount Numeric(19,4), 
	@insurance_folder_cnt int,
	@First_Original_Risk_cnt int,
	@First_Policy_Version_no int

set @TempTotalAmount=0
set @insurance_folder_cnt=0
set @First_Original_Risk_cnt = 0
set @First_Policy_Version_no = 0
SET @TotalAmount = 0  
SET @Amount = 0  
SELECT @original_risk_cnt = ISNULL(original_risk_cnt, 0)  
    FROM insurance_file_risk_link WHERE risk_cnt = @risk_cnt AND (status_flag = 'C' OR status_flag = 'D')
IF (@original_risk_cnt > 0)  
BEGIN  
	SET @First_Original_Risk_cnt = @original_risk_cnt
	SELECT @TotalAmount = ROUND(ISNULL(SUM(p.this_premium), 0), 4) FROM 
			Peril p INNER JOIN risk r  ON p.risk_cnt = r.risk_cnt
			INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = r.risk_cnt
			WHERE r.risk_cnt = @original_risk_cnt   
			AND r.is_risk_selected = 1  
			AND ifrl.status_flag IN ('C','D')	
			AND ISNULL(is_levy_tax,0)<>1

	SELECT @LeviTax = ROUND(ISNULL(SUM(this_premium), 0), 4) FROM peril where risk_cnt=@original_risk_cnt and ISNULL(is_levy_tax,0)=1  
	SET @TotalAmount = @TotalAmount + @LeviTax 
	
	WHILE (@original_risk_cnt > 0)  
	BEGIN  
		SELECT @original_risk_cnt = ISNULL(original_risk_cnt, 0)  
		FROM insurance_file_risk_link WHERE risk_cnt = @original_risk_cnt AND (status_flag = 'C' OR status_flag = 'D')

		IF (@original_risk_cnt > 0)  
		BEGIN  
		SET @First_Original_Risk_cnt = @original_risk_cnt

		SELECT @Amount = ROUND(ISNULL(SUM(p.this_premium), 0), 4) FROM 
				Peril p INNER JOIN risk r  ON p.risk_cnt = r.risk_cnt
    			INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = r.risk_cnt
  				WHERE r.risk_cnt = @original_risk_cnt   
   				AND r.is_risk_selected = 1  
   				AND ifrl.status_flag IN ('C','D') 
   				AND ISNULL(is_levy_tax,0)<>1
		SELECT @LeviTax=ROUND(ISNULL(SUM(this_premium), 0), 4) FROM peril where risk_cnt=@original_risk_cnt and ISNULL(is_levy_tax,0)=1  
		SET @TotalAmount = @TotalAmount + @Amount + @LeviTax  
	END  
ELSE  
	BREAK  
END  
END
DECLARE @Inception_date_tpi datetime
SELECT @Inception_date_tpi = inception_date_tpi FROM insurance_file i 
INNER JOIN insurance_file_risk_link ifrl ON i.insurance_file_cnt = ifrl.insurance_file_cnt
WHERE ifrl.risk_cnt = @risk_cnt


select @First_Policy_Version_no=i.policy_version from insurance_file i inner join insurance_file_risk_link ifrl
on ifrl.insurance_file_cnt =i.insurance_file_cnt where ifrl.risk_cnt = @First_Original_Risk_cnt

SET @Amount = 0  
	select @insurance_folder_cnt=i.insurance_folder_cnt from insurance_file i inner join insurance_file_risk_link ifrl
	on ifrl.insurance_file_cnt =i.insurance_file_cnt where ifrl.risk_cnt = @risk_cnt
	DECLARE MY_CURSOR Cursor FAST_FORWARD
	For
		select risk_cnt from insurance_file_risk_link ifrl inner join insurance_file i  
		on i.insurance_file_cnt=ifrl.insurance_file_cnt 
		where (i.insurance_file_type_id=6 ) and i.policy_version>@First_Policy_Version_no
			and i.insurance_folder_cnt=@insurance_folder_cnt and ifrl.risk_cnt <> @risk_cnt
			and i.inception_date_tpi = @Inception_date_tpi
	Open MY_CURSOR
	DECLARE @NewRisk_cnt int
	Fetch NEXT FROM MY_Cursor INTO @NewRisk_cnt
	While (@@FETCH_STATUS <> -1)
	BEGIN
	IF (@@FETCH_STATUS <> -2)
		SELECT @Amount = ROUND(ISNULL(SUM(p.this_premium), 0), 4) FROM 
				Peril p INNER JOIN risk r  ON p.risk_cnt = r.risk_cnt
    			INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = r.risk_cnt
  				WHERE r.risk_cnt = @NewRisk_cnt   
   				AND r.is_risk_selected = 1  
   				AND ifrl.status_flag IN ('C','D') 
 
	SELECT @LeviTax=ROUND(ISNULL(SUM(this_premium), 0), 4) FROM peril where risk_cnt= @NewRisk_cnt and ISNULL(is_levy_tax,0)=1 
	SET @TempTotalAmount = @TempTotalAmount + @Amount + @LeviTax  -- sum of the premium of Temporary MTA's
	FETCH NEXT FROM MY_CURSOR INTO @NewRisk_cnt
	END
	CLOSE MY_CURSOR
	DEALLOCATE MY_CURSOR
	SELECT round(@TotalAmount,2),round(@TempTotalAmount,2)
END