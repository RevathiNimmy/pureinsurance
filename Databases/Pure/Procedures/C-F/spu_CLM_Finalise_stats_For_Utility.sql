SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Finalise_stats_For_Utility'
GO

CREATE PROCEDURE spu_CLM_Finalise_stats_For_Utility  
    @claim_id int,  
    @transaction_type_id int,  
    @transaction_type_code varchar(10),  
    @stats_folder_cnt int,  
    @bstatssuppressed  tinyint output,    
    @nIsCloned tinyint =0,   
    @nIsCloned_reversal tinyint =0 ,  
    @is_pt tinyint =0    
AS  
  
--  @transaction_export_folder_cnt int OUTPUT  
/*********************************************************************************************  
 1.1    Updated to create with correct Document Ref.    RWH 06/07/01  
  
 1.2    Increase numeric element of Doc Ref to 8 digits RWH 27/07/01  
 1.3    Pass in @stats_folder_cnt as parameter.    RWH 14/09/01  
  
**********************************************************************************************/  
  
DECLARE  
    @source_id int,  
    @sub_branch_id int, -- PWF 03/07/2002  
    @stats_detail_id int,  
    @transaction_export_folder_cnt int,  
    @transaction_export_detail_id int,  
    @document_prefix char(3),  
    @retrieved_prefix varchar(10),  
    @max_orion_ref varchar(20),  
    @document_ref varchar(25),  
    @posting_period_year int,  
    @posting_period_number smallint,  
    @key_suffix_int int,  
    @transaction_amount numeric(19, 4),  
    @NumberRangeID int,  
    @DocumentRefNumber Varchar(25),  
    @UniqueDocumentRef Integer,
    @user_id int,
	@nPayment_id int,
	@nReceipt_id int  
--Get the real data  
  
-- Get transaction_type_code from stats_folder rather than roadmap.  
SELECT @transaction_type_code = transaction_type_code  
FROM    stats_folder  
WHERE   stats_folder_cnt = @stats_folder_cnt  
  
--  Check the transaction type to set the Document Type.  
SELECT @transaction_type_code = LTRIM(RTRIM(@transaction_type_code))  
  
-- determine if this copy work to claim call should be suppressed  
DECLARE @suppress int  
EXEC spu_CLM_Suppress_Stats @claim_id, @transaction_type_code, @suppress OUTPUT  
IF @suppress = 1  
BEGIN  
 -- remove the suppressed stats details  
 DELETE FROM stats_detail where stats_folder_cnt = @stats_folder_cnt  
 DELETE FROM stats_folder where stats_folder_cnt = @stats_folder_cnt  
 Set @bstatssuppressed = 1  
 RETURN  
END  

  If @nIsCloned =1 
BEGIN
	 Update Stats_Detail   
      SET tax_value = 0,  
      annual_premium=0,  
      this_premium_original =0,  
      this_premium_home=0,  
      lead_commission_value_home=0,  
      sub_commission_value_home=0,  
      sum_insured_home=0,  
      sum_insured_total=0,  
      charges_total=0,  
      taxes_total =0,        
      recoveries_total =0,        
      commission_excluded =0,        
      withholding_tax_excluded =0,    
      this_premium_system =0,        
      lead_commission_value_system =0,        
      sub_commission_value_system =0,        
      sum_insured_system =0  
      Where stats_folder_cnt = @stats_folder_cnt and Stats_detail_type ='GRS'   
END

    
Select @UniqueDocumentRef = ISNull(value,0) 
				From Hidden_options Where branch_id = 1 And option_number= 100
  
SELECT @document_prefix =  
    CASE @transaction_type_code  
        WHEN 'C_CO' THEN 'CLO'  
        WHEN 'C_CP' THEN 'CLP'  
        WHEN 'C_CR' THEN 'CLA'  
        WHEN 'C_SA' THEN 'CLR'  
        WHEN 'C_RV' THEN 'CLR'  
    END  
  
    if @nIsCloned = 1    
  SELECT @document_prefix='CLD'    
    
  if @nIsCloned_reversal = 1  
  SELECT @document_prefix = 'CLC'  
    
    if @is_pt = 1    
  SELECT @document_prefix='CPA'    
    
--We need to check the sign of a payment/receipt to finalise the document type.  
  
-- Adjust the Doc Ref for payment or receipt depending on sign of amount.  
IF (@document_prefix = 'CLR') OR (@document_prefix = 'CLP')  
BEGIN  
  
    SELECT  @transaction_amount = sum_insured_total  
    FROM    stats_detail  
    WHERE   stats_folder_cnt  = @stats_folder_cnt  
    AND  stats_detail_type = 'GRS' -- there should be only one GRS line  
  
    IF @transaction_amount < 0  
    BEGIN  
        IF @document_prefix = 'CLR'  
            SELECT @document_prefix = 'CLP'  
        ELSE  
            SELECT @document_prefix = 'CLR'  
    END  
  
END
IF ISNULL(@nIsCloned_reversal,0) <> 1 
BEGIN 
	IF ISNULL(@nIsCloned,0) = 1
	BEGIN

	SELECT @document_ref=document_ref FROM Stats_Folder 
	WHERE loss_id = @claim_id and document_ref like 'CLC%' and transaction_type_code=@transaction_type_code

	END
END
IF ((ISNULL(@nIsCloned,0) <> 1 OR @document_ref IS NULL) OR (ISNULL(@nIsCloned_reversal,0) = 1 OR @document_ref IS NULL) )
BEGIN  

IF (@UniqueDocumentRef = 1)
    BEGIN
	SELECT  
	        @retrieved_prefix = prefix,  
	        @key_suffix_int = next_number  
	FROM    Next_Orion_Doc_Ref  
	WHERE   prefix = @document_prefix  
	  
	IF @Key_Suffix_Int is NULL  
	BEGIN  
	    SELECT  @max_orion_ref = MAX(document_ref)  
	    FROM    Document  
	    WHERE   document_ref like @document_prefix +'%'  
	    AND     LEN ( LTRIM ( RTRIM ( document_ref ))) - LEN ( @document_prefix ) = 8  
	  
	    IF @max_orion_ref IS NOT NULL  
	        SELECT  @Key_Suffix_Int = SUBSTRING ( @max_orion_ref, LEN ( @document_prefix ) + 1, 8 ) + 1  
	    ELSE  
	        SELECT  @Key_Suffix_Int = 10000001  
	  
	END  
	--Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)  
	  
	--Note:- DocumentRef will be calculated with the following SPs as per the new development "WPR78"  
	EXEC spu_ACT_Get_Number_Range_From_Code @document_prefix,  @NumberRangeID  OUTPUT  
	  
	EXEC spu_ACT_Generate_Next_Unique_Document_Reference @NumberRangeID,1,1,@DocumentRefNumber OUTPUT  
	  
	SELECT  @document_ref = @document_prefix + @DocumentRefNumber  
	  
	--End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)  
	  
	IF (@retrieved_prefix is null) OR (@retrieved_prefix = '')  
	    INSERT INTO Next_Orion_Doc_Ref  
	    VALUES  (@document_prefix, @Key_Suffix_Int + 1)  
	ELSE  
	    UPDATE  Next_Orion_Doc_Ref  
	    SET next_number = @Key_Suffix_Int + 1  
	    WHERE   prefix = @document_prefix  
    END
Else
    BEGIN
	SELECT @source_id = source_id,
		@user_id = created_by_user_id
	FROM   stats_folder  
	WHERE  stats_folder_cnt = @stats_folder_cnt  
	  
	SELECT @NumberRangeID =  
	    CASE @document_prefix  
	        WHEN 'CLO' THEN 40  
	        WHEN 'CLP' THEN 28 
	        WHEN 'CLA' THEN 41
	        WHEN 'CLR' THEN 29 
         WHEN 'CLD' THEN 59    
         WHEN 'CPA' THEN 60  
         WHEN 'CLC' THEN 58   
	    END  
	
	EXEC spe_ACTnumber_add @Key_Suffix_Int OUTPUT , @NumberRangeID, @user_id, @source_id
	SELECT @document_ref = RTRIM(@document_prefix) 
					+ CONVERT(VARCHAR, REPLICATE ( 0 , 10-LEN(@Key_Suffix_Int)))+ CONVERT(VARCHAR,@Key_Suffix_Int)
	
    END  

END
ELSE
BEGIN
IF ISNULL(@nIsCloned_reversal,0) <> 1
BEGIN
	SELECT @document_ref=REPLACE(@document_ref,'CLC','CLD')
END
END
--*************  
-- MEvans : 06-03-2003 : Issue 2728  
DECLARE @ProductOption int  
  
SELECT  @ProductOption = value  
FROM    Hidden_Options  
WHERE   branch_id = 1 and option_number = 16  
--*************  
  
-- PWF 30/07/2002 - get sub branch id  
  
        SELECT  @sub_branch_id = branch_id -- IFIBCR  
 FROM    insurance_file  
        INNER JOIN  
                claim ON insurance_file.insurance_file_cnt = claim.policy_id  
        WHERE   claim.claim_id = @claim_id  
  
--*************  
-- MEvans : 06-03-2003 : Issue 2728  
IF @ProductOption = 1  
    BEGIN  
  
        -- use sub branch ids  
        SELECT  @posting_period_number = current_period_id  
        FROM    ledger  
        WHERE   ledger_short_name = 'SA'  
        AND     sub_branch_id = @sub_branch_id -- PWF 30/07/2002  
  
        SELECT  @posting_period_year = datepart(year, min(period_end_date))  
        FROM    period  
        WHERE   year_name = (  
            SELECT  year_name  
            FROM    period  
            WHERE   Period_id = @posting_period_number)  
        AND     sub_branch_id = @sub_branch_id -- PWF 30/07/2002  
  
    END  
ELSE  
    BEGIN  
  
        -- use default ledger  
        SELECT  @posting_period_number = current_period_id  
                FROM    ledger  
                WHERE   ledger_short_name = 'SA'  
  
        SELECT  @posting_period_year = datepart(year, min(period_end_date))  
        FROM    period  
        WHERE   year_name = (  
            SELECT  year_name  
            FROM    period  
            WHERE   Period_id = @posting_period_number)  
  
    END  
--*************  
  
-- Now for the stats.  Every folder should be the same, so let's use the first one.  
-- Then we just take the details for each of the folders and write it out  

  
BEGIN  
  
    UPDATE Stats_Folder  
    SET   document_ref = @document_ref,  
          posting_period_year = @posting_period_year,  
       posting_period_number = @posting_period_number,  
          transaction_type_id = @transaction_type_id,  
          transaction_type_code = @transaction_type_code  
    WHERE stats_folder_cnt = @stats_folder_cnt  
  
		if (@nIsCloned_reversal = 1 or @nIsCloned=1)
		BEGIN
			select @nPayment_id=payment_id,@nReceipt_id=Receipt_Id from stats_folder where loss_id=@claim_id and document_ref not like 'CLC%' and document_ref not like 'CLD%'
			UPDATE Stats_Folder
			SET   payment_id=@nPayment_id,
				  Receipt_Id=@nReceipt_id
			WHERE stats_folder_cnt = @stats_folder_cnt
		END
  
END  
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
