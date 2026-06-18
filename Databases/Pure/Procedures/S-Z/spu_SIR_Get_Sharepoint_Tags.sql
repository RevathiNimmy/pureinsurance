EXEC DDLDropProcedure 'spu_SIR_Get_Sharepoint_Tags'
GO

CREATE PROCEDURE spu_SIR_Get_Sharepoint_Tags    
 @document_template_id INT,    
 @template_group_id INT,    
 @template_sub_group_id INT,    
 @party_cnt INT,    
 @insurance_file_cnt INT,    
 @claim_id INT,    
 @background_job_id INT= NULL  
    
AS    
    
DECLARE @document_template_description VARCHAR(255)    
DECLARE @document_group VARCHAR(255)    
DECLARE @document_sub_group VARCHAR(255)    
DECLARE @party_shortname VARCHAR(255)    
DECLARE @party_resolved_name VARCHAR(255)    
DECLARE @policy_number VARCHAR(255)    
DECLARE @product_code VARCHAR(255)    
DECLARE @agent_shortname VARCHAR(255)    
DECLARE @agent_resolved_name VARCHAR(255)    
DECLARE @cover_start_date DATETIME    
DECLARE @cover_expiry_date DATETIME    
DECLARE @claim_number VARCHAR(255)    
DECLARE @claim_status VARCHAR(255)    
DECLARE @claim_loss_date DATETIME    
DECLARE @claim_payment_date DATETIME    
DECLARE @claim_primary_cause VARCHAR(255)    
DECLARE @claim_paid_amount MONEY    
DECLARE @claim_received_amount MONEY    
DECLARE @UserName VARCHAR(255)=Null    
DECLARE @Quote_Ref VARCHAR(255)=Null  
DECLARE @Insurance_File_Type_Id INT
DECLARE @sBackGroundJob NVarchar(MAX)
    
SELECT  @document_template_description=T.description,    
  @document_group = G.description,    
  @document_sub_group = S.description    
FROM Document_Template T WITH (NOLOCK)    
LEFT JOIN Document_Template_Group G WITH (NOLOCK) ON G.document_template_group_id=T.document_template_group_id    
LEFT JOIN Document_Template_Sub_Group S WITH (NOLOCK) ON S.document_template_sub_group_id=T.document_template_sub_group_id    
WHERE   T.document_template_id=@document_template_id    
    
SELECT @document_group = G.description,    
  @document_sub_group = S.description    
FROM Document_Template_Group G WITH (NOLOCK)    
INNER JOIN Document_Template_Sub_Group S WITH (NOLOCK) ON S.document_template_group_id=G.document_template_group_id    
WHERE G.document_template_group_id=@template_group_id    
AND  S.document_template_sub_group_id=@template_sub_group_id    
    
IF ISNULL(@party_cnt,0)=0 BEGIN    
 IF ISNULL(@insurance_file_cnt,0)<>0    
  SELECT @party_cnt=insured_cnt    
  FROM Insurance_File WITH (NOLOCK)    
  WHERE insurance_file_cnt = @insurance_file_cnt    
 ELSE IF ISNULL(@claim_id,0)<>0    
  SELECT @party_cnt=Client_id    
  FROM Claim WITH (NOLOCK)    
  WHERE claim_id = @claim_id    
END    
    
IF ISNULL(@insurance_file_cnt,0)=0 AND ISNULL(@claim_id,0)<>0 BEGIN    
 SELECT @insurance_file_cnt=policy_id    
 FROM Claim    
 WHERE Claim_id = @claim_id    
END    
    
SELECT @party_shortname = shortname,    
  @party_resolved_name = resolved_name    
FROM Party WITH (NOLOCK)    
WHERE party_cnt = @party_cnt    
    
SELECT @policy_number = I.insurance_ref,    
  @product_code = P.code,    
  @agent_shortname = A.shortname,    
  @agent_resolved_name = A.resolved_name,    
  @cover_start_date = I.cover_start_date,    
  @cover_expiry_date = I.expiry_date,
  @Quote_Ref = IFD.Quote_insurance_ref,  
  @Insurance_File_Type_Id = ISNULL(I.Insurance_File_Type_Id,0)
FROM Insurance_File I WITH (NOLOCK)    
LEFT JOIN Party A WITH (NOLOCK) ON A.party_cnt = I.lead_agent_cnt    
INNER JOIN Product P WITH (NOLOCK) ON P.product_id = I.product_id    
INNER JOIN Insurance_Folder IFD WITH (NOLOCK) ON IFD.insurance_folder_cnt = I.insurance_folder_cnt  
WHERE I.insurance_file_cnt=@insurance_file_cnt    
    
SELECT @claim_number = C.Claim_Number,    
  @claim_loss_date = C.Loss_from_date,    
  @claim_primary_cause = P.description,    
  @claim_status = CS.description    
FROM Claim C WITH (NOLOCK)    
LEFT JOIN primary_cause P WITH (NOLOCK) ON P.primary_cause_id=C.Primary_Cause_id    
LEFT JOIN progress_status CS WITH (NOLOCK) ON CS.progress_status_id=C.Progress_Status_id    
WHERE C.claim_id = @claim_id    
    
SELECT @claim_payment_date = MIN(CP.date_of_payment)    
FROM Claim_Payment CP WITH (NOLOCK)    
WHERE CP.claim_id = @claim_id    
    
SELECT @claim_paid_amount=SUM(R.Paid_to_date)    
FROM Reserve R WITH (NOLOCK)    
INNER JOIN Claim_Peril CP ON CP.Claim_Peril_id=R.claim_Peril_id    
WHERE CP.Claim_id=@claim_id    
    
SELECT @claim_received_amount=SUM(R.received_to_date)    
FROM Recovery R WITH (NOLOCK)    
INNER JOIN Claim_Peril CP ON CP.Claim_Peril_id=R.claim_Peril_id    
WHERE CP.Claim_id=@claim_id    
    
SET @claim_paid_amount=ISNULL(@claim_paid_amount,0)    
SET @claim_received_amount=ISNULL(@claim_received_amount,0)   
  
IF ISNULL(@background_job_id,0)<>0  
BEGIN  
 SELECT  @UserName = U.USERNAME    
  FROM PMUser u with (nolock) LEFT JOIN  
   Background_Job on Background_Job.job_user_id=u.user_id  
  WHERE background_job_id = @background_job_id    
END   
    
	IF ISNULL(@background_job_id,0)<>0
BEGIN
 SELECT  @sBackGroundJob = job_xml From
    Background_Job
  WHERE background_job_id = @background_job_id
END


SELECT ISNULL(@document_template_description,'') [document_template_description],    
  ISNULL(@document_group,'') [document_group],    
  ISNULL(@document_sub_group,'') [document_sub_group],    
  ISNULL(@party_shortname,'') [party_shortname],    
  ISNULL(@party_resolved_name,'') [party_resolved_name],    
  ISNULL(@policy_number,'') [policy_number],    
  ISNULL(@product_code,'') [product_code],    
  ISNULL(@agent_shortname,'') [agent_shortname],    
  ISNULL(@agent_resolved_name,'') [agent_resolved_name],    
  @cover_start_date [cover_start_date],    
  @cover_expiry_date [cover_expiry_date],    
  ISNULL(@claim_number,'') [claim_number],    
  @claim_loss_date [loss_date],    
  ISNULL(@claim_status,'') [claim_status_description],    
  @claim_payment_date [claim_payment_date],    
  ISNULL(@claim_primary_cause,'') [claim_primary_cause_description],    
  @claim_paid_amount - @claim_received_amount [claim_incurred_amount],  
  @UserName [USER_NAME],
  ISNULL(@Quote_Ref,'') [QUOTE_REF],  
  ISNULL(@Insurance_File_Type_Id,0) [Insurance_File_Type_Id],
  ISNULL(@sBackGroundJob,'') As BackGroundJobXML    
  
  GO