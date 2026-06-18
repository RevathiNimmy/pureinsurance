EXECUTE DDLDropProcedure 'spu_SAM_IsPending_CloneTransfer'
GO

CREATE PROCEDURE spu_SAM_IsPending_CloneTransfer    
@sPolicyNumber varchar(30),    
@nInsuranceFileCnt INT =0    
AS    
BEGIN    
DECLARE @iInsFileTypeLiveID  INT    
DECLARE @iInsFileTypeQuoteID INT    
DECLARE @iInsFileTypeMTAPID  INT    
DECLARE @iInsFileTypeMTATID  INT    
DECLARE @iInsFileTypeRenewalID INT    
DECLARE @iInsFileTypeMTCID INT    
DECLARE @iInsFileTypeMTRID INT    
DECLARE @iInsFileTypeQMTAPID  INT    
DECLARE @iInsFileTypeQMTATID  INT    
DECLARE @iInsFileTypeQMTRID INT    
DECLARE @transfer_date DATETIME,    
  @year_name INT,    
  @period_end_date DATETIME ,    
  @current_System_Date DATETIME    
    
SELECT @year_name= YEAR(GETDATE())    
SELECT @current_System_Date = GETDATE()    
SELECT top 1 @period_end_date = period_end_date    
FROM Period    
WHERE @year_name = YEAR(period_end_date)    
ORDER BY period_end_date    
    
SELECT @transfer_date =DATEADD(m,DATEDIFF(m,0,@period_end_date),0)    
IF ISNULL(@sPolicyNumber,'')=''    
SELECT @sPolicyNumber = LTRIM(RTRIM(insurance_ref )) FROM Insurance_File WHERE insurance_file_cnt = @nInsuranceFileCnt    
    
SELECT @iInsFileTypeLiveID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'POLICY'    
SELECT @iInsFileTypeQuoteID = insurance_file_type_id FROM insurance_file_type WHERE code = 'QUOTE'    
SELECT @iInsFileTypeMTAPID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTA PERM'    
SELECT @iInsFileTypeMTATID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTA TEMP'    
SELECT @iInsFileTypeRenewalID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'RENEWAL'    
SELECT @iInsFileTypeMTCID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTACAN'    
SELECT @iInsFileTypeMTRID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTAREINS'    
SELECT @iInsFileTypeQMTAPID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTAQUOTE'    
SELECT @iInsFileTypeQMTATID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTAQTETEMP'    
SELECT @iInsFileTypeQMTRID  = insurance_file_type_id FROM insurance_file_type WHERE code = 'MTAQREINS'    
    
IF @sPolicyNumber=''    
SELECT @sPolicyNumber = LTRIM(RTRIM(insurance_ref)) from Insurance_File where insurance_file_cnt =@nInsuranceFileCnt    
    
IF @current_System_Date >= @transfer_date    
SELECT DISTINCT  -- we only want one policy for each risk returned, we'll look thru each risk later    
    ins.insurance_file_cnt    
,   ins.insurance_ref AS 'Policy Number'    
,   ins.insurance_folder_cnt    
,   ins.insurance_file_type_id    
,   par.shortname AS 'Party Shortname'    
,   par.[name] AS 'Party Name'    
,   ins.cover_start_date    
FROM    
    insurance_file AS ins    
INNER JOIN insurance_file_risk_link ifrl ON    
 ins.insurance_file_cnt = ifrl.insurance_file_cnt    
INNER JOIN    
    Risk AS rsk ON ifrl.risk_cnt = rsk.risk_cnt    
INNER JOIN    
    Risk_Type AS rskt ON rskt.risk_type_id = rsk.risk_type_id    
INNER JOIN    
    Party AS par ON par.party_cnt = ins.insured_cnt    
INNER JOIN    
    RI_Arrangement AS ri ON ri.risk_cnt = rsk.risk_cnt    
INNER JOIN    
    RI_Arrangement_Line AS ra ON ri.ri_arrangement_id = ra.ri_arrangement_id    
INNER Join    
 RI_Model As rim ON ri.ri_model_id = rim.ri_model_id    
inner Join    
 RI_Model As xrim ON ri.xol_ri_model_id = xrim.ri_model_id  

WHERE  ins.insurance_file_type_id IN (@iInsFileTypeLiveID, @iInsFileTypeMTAPID,  @iInsFileTypeMTATID, @iInsFileTypeMTCID , @iInsFileTypeMTRID)    
   AND original_flag=0 AND ri.Cloned = 1 
   AND (rim.ri_model_type = 0 AND xrim.ri_model_type = 0)
   AND (ins.insurance_file_status_id IS NULL OR ins.insurance_file_status_id<>309)    
   AND ins.insurance_ref =LTRIM(RTRIM(@sPolicyNumber))    
END    
GO
