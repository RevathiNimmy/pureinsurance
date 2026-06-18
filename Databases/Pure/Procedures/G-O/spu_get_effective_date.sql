SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_effective_date'
GO

CREATE PROCEDURE spu_get_effective_date  
    @insurance_file_cnt int,  
    @risk_cnt int,  
    @effective_date datetime output,  
    @prop_effective_date datetime = null output,
	@transfer_date datetime = null
AS  
  
    DECLARE  
        @original_risk_cnt int,  
        @original_insurance_file_cnt int,  
            @option int,  
            @status int,  
   @insurance_file_type int,  
   @RIRegen varchar(20),  
   @cover_start_date datetime,  
   @RI2007Enabled int  
  
  SELECT @RIRegen=value  FROM Hidden_options WHERE option_number=103  
  
  Select @RI2007Enabled=ISNull(value,0) from hidden_options where option_number=88  
  

        Select @insurance_file_type=insurance_file_type_id,@cover_start_date=cover_start_date from Insurance_file  
        WHERE insurance_file_cnt = @insurance_file_cnt  
  
 -- Get original risk  
 SELECT @original_risk_cnt = original_risk_cnt  
 FROM insurance_file_risk_link  
 WHERE insurance_file_cnt = @insurance_file_cnt  
 AND risk_cnt = @risk_cnt  
  
        -- This must be a renewal or NB  
 IF ISNULL(@original_risk_cnt, 0) = 0 AND @insurance_file_type<=3  
        BEGIN  
            SELECT @effective_date = CASE WHEN @transfer_date is null then cover_start_date  Else @transfer_date end
            FROM insurance_file  
            WHERE insurance_file_cnt = @insurance_file_cnt  
 END  
 ELSE  
        -- This must be an MTA, DRI or PT  
  BEGIN  
    -- Check "Use MTA Date for Reinsurance"  
    SELECT @option = CONVERT(INT, value)  
    FROM system_options  
    WHERE branch_id = 1  
    AND option_number = 1023  
  
 -- We can use this policy (PT and DRI would use this version anyway)  
            IF ISNULL(@option, 0) = 1  
            BEGIN  
                SELECT @effective_date = CASE WHEN @transfer_date is null then cover_start_date  Else @transfer_date end  
                FROM insurance_file  
                WHERE insurance_file_cnt = @insurance_file_cnt  
            END  
            ELSE  
            -- So we need the original policy for the MTA (but not if it's DRI or PT)  
            BEGIN  
  
                    SELECT @original_insurance_file_cnt=max(insurance_file_cnt)  
                    FROM insurance_file  
                    WHERE insurance_folder_cnt IN (  
                        SELECT insurance_folder_cnt  
                        FROM insurance_file  
                        WHERE insurance_file_cnt = @insurance_file_cnt  
                        )  
                 AND insurance_file_type_id =2 and cover_start_date <= @cover_start_date  
  
                -- Get the effective date and status  
                SELECT @effective_date = cover_start_date,  
                       @status = insurance_file_status_id  
                FROM insurance_file  
                WHERE insurance_file_cnt = @original_insurance_file_cnt  
                IF (ISNULL(@original_risk_cnt, 0) = 0 AND @RIRegen='1')  
                    SELECT @effective_date = cover_start_date  
                    FROM insurance_file  
                    WHERE insurance_file_cnt = @insurance_file_cnt  
            END  
    END  
  
 -- We can use this policy (PT and DRI would use this version anyway)  
 IF ISNULL(@option, 0) = 1 OR (ISNULL(@original_risk_cnt, 0) = 0 AND @insurance_file_type<=3 )  
 BEGIN  
  SELECT @effective_date = CASE WHEN @transfer_date is null then cover_start_date  Else @transfer_date end  
  FROM insurance_file  
  WHERE insurance_file_cnt = @insurance_file_cnt  
  END  
  ELSE IF  ISNULL(@original_risk_cnt, 0) = 0  And  ISNULL(@RI2007Enabled, 0) = 0  
 BEGIN  
 SELECT @effective_date = (select ifi.inception_date_tpi from  insurance_file ifi  
     INNER JOIN insurance_file_risk_link ifrl ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt  
     INNER JOIN risk r ON r.risk_cnt = ifrl.risk_cnt  
     WHERE r.risk_cnt = @risk_cnt AND ifi.insurance_file_cnt = @insurance_file_cnt)  
 END  
 ELSE  
 BEGIN  
  SELECT @effective_date =  
   CASE WHEN r.inception_date > ifi.inception_date_tpi THEN r.inception_date  
    ELSE ifi.inception_date_tpi END  
  FROM insurance_file ifi  
  INNER JOIN insurance_file_risk_link ifrl ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt  
  INNER JOIN risk r ON r.risk_cnt = ifrl.risk_cnt  
  WHERE r.risk_cnt = @risk_cnt AND ifi.insurance_file_cnt = @insurance_file_cnt  
 END  
  SELECT @prop_effective_date = @effective_date  
  
  IF ISNULL(@option, 0) = 1  
  BEGIN  
    SELECT @prop_effective_date = CASE WHEN @transfer_date is null then cover_start_date  Else @transfer_date end 
    FROM insurance_file  
    WHERE insurance_file_cnt = @insurance_file_cnt  
  END  
GO
