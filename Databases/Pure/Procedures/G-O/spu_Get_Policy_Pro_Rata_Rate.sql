Execute DDLDropProcedure 'spu_Get_Policy_Pro_Rata_Rate'
GO

CREATE PROCEDURE spu_Get_Policy_Pro_Rata_Rate  
@nInsuranceFileCnt INT,  
@sTransactionType  VARCHAR(10),
@crProrataRate NUMERIC(19,8) OUTPUT   
AS
DECLARE @nBaseLength INT
DECLARE @nPeriodLength INT
DECLARE @bIsTrueMonthlyPolicy INT
DECLARE @nMonthCount INT
DECLARE @dtLastDateOfMth  Date
DECLARE @nTotalDaysInMth INT
DECLARE @nPolicyDays INT
DECLARE @dtDate  DATE
DECLARE @dtStartDate  DATE
DECLARE @dtEndDate  DATE
DECLARE @dtTmpDate  DATE
DECLARE @dtTmpDate1  DATE
DECLARE @dtInceptionDate  DATE
DECLARE @crTempProrataRate  NUMERIC(19,8)
DECLARE @crProrataRateFractionVal  NUMERIC(19,8)
DECLARE @crProrata  NUMERIC(19,8)
DECLARE @nProductId INT
DECLARE @bIsMidnightRenewal INT

--GET PRODUCT ID FROM INSURANCE FILE CNT

SELECT @nProductId=product_id,
       @dtStartDate=cover_start_date,
	   @dtEndDate=[expiry_date],
	   @dtInceptionDate =inception_date_tpi   
  FROM insurance_file 
 WHERE insurance_file_cnt = @nInsuranceFileCnt

SELECT @bIsMidnightRenewal=is_midnight_renewal  FROM Product WHERE product_id=@nProductId  
SELECT @bIsTrueMonthlyPolicy=is_true_monthly_policy FROM product WHERE product_id= @nProductId


IF @bIsTrueMonthlyPolicy =1
BEGIN 
	SET @dtDate = @dtStartDate
	SET @crProrata = 0.0
	SET @nMonthCount= DATEDIFF(MONTH ,@dtStartDate,@dtEndDate)  
	    WHILE @dtDate <= @dtEndDate
			BEGIN
                SET @nTotalDaysInMth=(  CASE 
					WHEN Month(@dtDate) IN (1 ,3,5,7,8,10,12) THEN  31
					ELSE  DATEDIFF(DAY, @dtDate, DateAdd(MONTH, 1, @dtDate))
					END) 
                
                SET @dtLastDateOfMth = CONVERT(DATE,(CONVERT(VARCHAR(10),@nTotalDaysInMth) + '/' + CONVERT(VARCHAR(10),Month(@dtDate)) + '/' + CONVERT(VARCHAR(10),Year(@dtDate))),103)
                    
                IF (Month(@dtDate) = Month(@dtEndDate))
					BEGIN 
					  SET  @nPolicyDays = DateDiff(DAY, @dtDate, @dtEndDate) + 1
                    END
                ELSE
					BEGIN
					  SET  @nPolicyDays = DateDiff(DAY, @dtDate, @dtLastDateOfMth) + 1
                    END
                      
                SELECT @crProrata = @crProrata + (CONVERT(NUMERIC(19,8),@nPolicyDays) / CONVERT(NUMERIC(19,8),@nTotalDaysInMth))
                   
                IF (@nMonthCount > 0)
					BEGIN 
                    IF Month(@dtDate) < 12 
                        SET @dtDate = CONVERT(DATE,('01/' + CONVERT(VARCHAR(20),(Month(@dtDate)+ 1)) + '/' + CONVERT(VARCHAR(20),Year(@dtDate))),103)  
                    ELSE
                        SET @dtDate = CONVERT(DATE,('01/01/' + CONVERT(VARCHAR(20),(Year(@dtDate) + 1)) ),103)
                    END
                ELSE
					BEGIN
						BREAK
					END  
            END
                --Loop
            SET @crProrataRate = @crProrata
    END 
ELSE
	BEGIN
            If (@bIsMidnightRenewal = 0) 
              BEGIN
				SET @dtEndDate = DateAdd(DAY, -1, @dtEndDate)
			  END
            
            SET @crTempProrataRate = 0
            SET @crProrataRateFractionVal = 0

    WHILE (1=1)
    BEGIN
	SET  @dtTmpDate = DateAdd(YEAR, 1, @dtStartDate)
	SET @dtTmpDate1 = DateAdd(YEAR, 1, @dtInceptionDate)
    --'DateAdd add completly one year eg. if date is 1.1.08
    --'then result date after adding a year comes 1.1.09
    --'in our case end date must be 31.12.08 so subtraction of a day is necessary
    --'dtTmpDate = DateAdd("d", -1, dtTmpDate)
    --'dtTmpDate1 = DateAdd("d", -1, dtTmpDate1)
	IF @dtEndDate < @dtTmpDate 
	BEGIN
		SET @nPeriodLength = DateDiff(DAY, @dtStartDate, @dtEndDate) + 1
								
		--'Last day cancellation shouldn't generate return premium
		IF @nPeriodLength = 1 And (@sTransactionType = 'MTC' Or @sTransactionType = 'MTR') 
			SET @nPeriodLength = 0
           
		SET @nBaseLength = DateDiff(DAY, DateAdd(YEAR, -1, @dtTmpDate1), @dtTmpDate1)
	                  
		If @nPeriodLength > 0 
			BEGIN
			  SET  @crProrataRateFractionVal = CONVERT(NUMERIC(19,8),@nPeriodLength) / CONVERT(NUMERIC(19,8),@nBaseLength)
			END
		    BREAK	   
			END	   
			--Exit Do
		Else If @dtEndDate = @dtTmpDate
			BEGIN 
				SET @crTempProrataRate = @crTempProrataRate + 1
			    BREAK
			END
			--Exit Do
		Else If @dtEndDate > @dtTmpDate
			BEGIN 
				SET  @crTempProrataRate = @crTempProrataRate + 1
				--'Set the last date to first date after adding a day
				SET @dtStartDate = @dtTmpDate
				SET @dtInceptionDate = DateAdd(DAY, 1, @dtTmpDate1)
			END
	END--Loop
    SET @crProrataRate = @crTempProrataRate + @crProrataRateFractionVal	
	END

	GO

    
