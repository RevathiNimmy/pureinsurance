SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_gis_policy_link_sel'
GO

CREATE PROCEDURE spu_gis_policy_link_sel  
    @gis_policy_link_id INTEGER = NULL,  
    @insurance_file_cnt INTEGER = NULL,  
    @quote_ref CHAR(11) = NULL,  
    @risk_id INTEGER = NULL,  
    @claim_id INTEGER = NULL,  
    @party_cnt INTEGER = NULL,
    @case_id INTEGER = NUll
  
AS  

DECLARE @ClaimID INT, @PartyCnt INT, @InsuranceFileCnt INT, @RiskID INT
SELECT @ClaimID  = @claim_id , @PartyCnt = @party_cnt , @InsuranceFileCnt = @insurance_file_cnt, @RiskID = @risk_id

BEGIN  
DECLARE @ret_data_model_code  char(10) ,  
    @ret_data_model_id        int ,  
    @ret_policy_link_id       int ,  
    @ret_insurance_file_cnt   int ,  
    @ret_risk_id              int ,  
    @ret_quote_ref            char(11) ,  
    @ret_quote_ref_password   varchar(30) ,  
    @ret_gteed_quote_date     datetime ,  
    @ret_claim_id             int ,  
    @ret_party_cnt            int ,  
    @ret_insurance_folder_cnt int , 
    @ret_case_id              int
  
/* Have ANY of the Keys been supplied */  
/* Note: RiskID and ClaimPerilID can only be used in conjunction */  
/* with InsuranceFolderCnt and Claim_id respectively */  
IF (@InsuranceFileCnt IS NULL)  
AND (@gis_policy_link_id IS NULL)  
AND (@quote_ref IS NULL)  
AND (@ClaimID IS NULL)  
AND (@PartyCnt IS NULL) 
AND (@case_id IS NULL)
BEGIN  
    SELECT  gis_policy_link_id ,  
        insurance_file_cnt ,  
        risk_id ,  
        gis_data_model_id ,  
        '' gis_data_model_code ,  
        quote_ref ,  
        quote_ref_password ,  
        guaranteed_quote_date  
    FROM    gis_policy_link WITH (NOLOCK) 
    WHERE   gis_policy_link_id = -100  
    RETURN  
END  
  
IF (@gis_policy_link_id IS NOT NULL)  
    SELECT @ret_policy_link_id = gis_policy_link_id ,  
           @ret_insurance_file_cnt = insurance_file_cnt ,  
           @ret_risk_id = risk_id ,  
           @ret_data_model_id = gis_data_model_id ,  
           @ret_quote_ref = quote_ref ,  
           @ret_quote_ref_password = quote_ref_password ,  
           @ret_gteed_quote_date = guaranteed_quote_date ,  
           @ret_claim_id = claim_id ,  
           @ret_party_cnt = party_cnt,
           @ret_case_id = case_id
    FROM   gis_policy_link  WITH (NOLOCK) 
    WHERE  gis_policy_link_id = @gis_policy_link_id  
  
ELSE IF @ClaimID IS NOT NULL  
        -- claim_id  
        SELECT @ret_policy_link_id = gis_policy_link_id ,  
               @ret_insurance_file_cnt = insurance_file_cnt ,  
               @ret_risk_id = risk_id ,  
               @ret_data_model_id = gis_data_model_id ,  
               @ret_quote_ref = quote_ref ,  
               @ret_quote_ref_password = quote_ref_password ,  
               @ret_gteed_quote_date = guaranteed_quote_date ,  
               @ret_claim_id = claim_id ,  
               @ret_party_cnt = party_cnt,
               @ret_case_id = case_id   
        FROM   gis_policy_link  WITH (NOLOCK) 
        WHERE  claim_id = @ClaimID  
 
ELSE IF @PartyCnt IS NOT NULL  
        -- party_cnt  
        SELECT @ret_policy_link_id = gis_policy_link_id ,  
               @ret_insurance_file_cnt = insurance_file_cnt ,  
               @ret_risk_id = risk_id ,  
               @ret_data_model_id = gis_data_model_id ,  
               @ret_quote_ref = quote_ref ,  
               @ret_quote_ref_password = quote_ref_password ,  
               @ret_gteed_quote_date = guaranteed_quote_date ,  
               @ret_claim_id = claim_id ,  
               @ret_party_cnt = party_cnt,
               @ret_case_id = case_id
        FROM   gis_policy_link  WITH (NOLOCK) 
        WHERE  party_cnt = @PartyCnt  
  
ELSE IF @InsuranceFileCnt IS NOT NULL  
        IF @RiskID IS NOT NULL  
  --insurance_file_cnt  
         SELECT @ret_policy_link_id = gis_policy_link_id ,  
                @ret_insurance_file_cnt = insurance_file_cnt ,  
                @ret_risk_id = risk_id ,  
                @ret_data_model_id = gis_data_model_id ,  
                @ret_quote_ref = quote_ref ,  
                @ret_quote_ref_password = quote_ref_password ,  
                @ret_gteed_quote_date = guaranteed_quote_date ,  
                @ret_claim_id = claim_id ,  
                @ret_party_cnt = party_cnt,
                @ret_case_id = case_id
         FROM   gis_policy_link  WITH (NOLOCK) 
         WHERE  insurance_file_cnt = @InsuranceFileCnt  
  AND    risk_Id = @RiskID  
  
 ELSE  
  --insurance_file_cnt  
         SELECT @ret_policy_link_id = gis_policy_link_id ,  
                @ret_insurance_file_cnt = insurance_file_cnt ,  
                @ret_risk_id = risk_id ,  
                @ret_data_model_id = gis_data_model_id ,  
                @ret_quote_ref = quote_ref ,  
                @ret_quote_ref_password = quote_ref_password ,  
                @ret_gteed_quote_date = guaranteed_quote_date ,  
                @ret_claim_id = claim_id ,  
                @ret_party_cnt = party_cnt   
         FROM   gis_policy_link  WITH (NOLOCK) 
         WHERE  insurance_file_cnt = @InsuranceFileCnt 
ELSE IF @case_id IS NOT NULL  
        -- case_id  
        SELECT @ret_policy_link_id = gis_policy_link_id ,  
               @ret_insurance_file_cnt = insurance_file_cnt ,  
               @ret_risk_id = risk_id ,  
               @ret_data_model_id = gis_data_model_id ,  
               @ret_quote_ref = quote_ref ,  
               @ret_quote_ref_password = quote_ref_password ,  
               @ret_gteed_quote_date = guaranteed_quote_date ,  
               @ret_claim_id = claim_id ,  
               @ret_party_cnt = party_cnt,
               @ret_case_id = case_id
        FROM   gis_policy_link  WITH (NOLOCK) 
        WHERE  case_id = @case_id  
ELSE  
        SELECT @ret_policy_link_id = gis_policy_link_id ,  
               @ret_insurance_file_cnt = insurance_file_cnt ,  
               @ret_risk_id = risk_id ,  
               @ret_data_model_id = gis_data_model_id ,  
               @ret_quote_ref = quote_ref ,  
               @ret_quote_ref_password = quote_ref_password ,  
               @ret_gteed_quote_date = guaranteed_quote_date ,  
               @ret_claim_id = claim_id ,  
               @ret_party_cnt = party_cnt,
               @ret_case_id = case_id   
        FROM   gis_policy_link  WITH (NOLOCK) 
        WHERE  quote_ref = @quote_ref  
  
SELECT  @ret_data_model_code = code  
FROM    gis_data_model  WITH (NOLOCK) 
WHERE   gis_data_model_id = @ret_data_model_id  
  
SELECT  @ret_policy_link_id gis_policy_link_id ,  
    @ret_insurance_file_cnt insurance_file_cnt ,  
    @ret_risk_id risk_id ,  
    @ret_data_model_id gis_data_model_id ,  
    @ret_data_model_code gis_data_model_code ,  
    @ret_quote_ref quote_ref ,  
    @ret_quote_ref_password quote_ref_password ,  
    @ret_gteed_quote_date guaranteed_quote_date ,  
    @ret_claim_id claim_id ,  
    @ret_party_cnt party_cnt,
    @ret_case_id case_id
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
