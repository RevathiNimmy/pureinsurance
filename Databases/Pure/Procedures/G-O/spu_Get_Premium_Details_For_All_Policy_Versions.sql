SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_Premium_Details_For_All_Policy_Versions'
GO
--Start (Prakash Varghese) Modified the sp to get the paid amount for all payment methods.
--Start (Prakash Varghese) Modified the sp to get the paid amount for all payment methods.
CREATE PROCEDURE spu_Get_Premium_Details_For_All_Policy_Versions  
 @nInsurance_folder_cnt INT = NULL,
 @nInsurance_file_cnt INT = NULL
AS  
BEGIN  

IF ISNULL(@nInsurance_folder_cnt,0)=0
	 Select @nInsurance_folder_cnt = (select insurance_folder_cnt from Insurance_File where insurance_file_cnt =@nInsurance_file_cnt)
 --Table variable to store premium details entries  
 DECLARE @PremiumDetails TABLE(  
          Insurance_File_Cnt INT PRIMARY KEY ,  
          PremiumPaid NUMERIC(19,4),  
          Payment_Method VARCHAR(60) NULL ,  
          PlanTransactionID INT NULL,
		   Outstanding_Amount NUMERIC(19,4)
         )  
 --Table variable to store insurance file cnts for and insurance folder cnt  
 DECLARE @PolicyVersions  TABLE (  
         Row_Cnt INT IDENTITY(1,1),  
         Insurance_File_Cnt INT,  
         PaymentMethod VARCHAR(60),  
         Annual_Premium NUMERIC(19,4),  
         This_Premium NUMERIC(19,4),  
         Net_Premium NUMERIC(19,4)  
           )  
 DECLARE @RecordCnt AS INT  
 DECLARE @Index AS INT  
 DECLARE @InsuranceFileCnt AS INT  
 DECLARE @PaymentMethod AS VARCHAR(60)  
 DECLARE @PremiumPaid AS NUMERIC (19,4)  
 DECLARE @PlanTransactionID AS INT  
 DECLARE @OutstandingAmount AS NUMERIC(19,4)
 DECLARE @RiskFeesTotal AS NUMERIC(19,4)
 --Get the Insruance file cnt and Payment method for policy versions  
 INSERT INTO  
  @PolicyVersions (Insurance_File_Cnt,PaymentMethod,Annual_Premium,This_Premium,Net_Premium)  
  SELECT  
    Insurance_File_Cnt,
    (CASE WHEN Payment_Method IS NULL OR Payment_Method='PayNow'THEN 'PayNow'
       WHEN Payment_Method='Invoice' THEN 'Invoice'  
      ELSE 'Instalment'  
    END) AS PaymentMethod,  
    Annual_Premium,  
    This_Premium,  
    Net_Premium  
  FROM  
   Insurance_File IFI
  WHERE  
   Insurance_Folder_Cnt = @nInsurance_folder_cnt
      --AND insurance_file_status_id IS NULL
      -- Exclude insurance files of type MTA Quotation Permanent,
      -- MTA Quotation Temporary and MTA Quotation Reinstatement
      --AND insurance_file_type_id NOT IN (4,7,10)
	   AND insurance_file_type_id IN (2,3,5,6,8,9)
  
 SET @RecordCnt=@@ROWCOUNT  
  
 --Loop through all policy versions and get premium details  
 SET @Index=1  
 WHILE @Index<=@RecordCnt  
 BEGIN  
  
  --Get current insurance file cnt and payment method  
  SELECT  
   @InsuranceFileCnt=Insurance_File_Cnt,  
   @PaymentMethod=PaymentMethod  
  FROM  
   @PolicyVersions  
  WHERE  
   Row_Cnt=@Index  
  
  --Insert the insurance file values to premium details table variable  
  INSERT INTO  
   @PremiumDetails (  
        Insurance_File_Cnt,  
        PremiumPaid,  
        Payment_Method,
		Outstanding_Amount
          )  
  VALUES (  
    @InsuranceFileCnt,  
    0,  
    @PaymentMethod,
	0
      )  
  
  IF @PaymentMethod='Instalment'  
  BEGIN  
  
   --Get the premium paid  
   SELECT  
    @PremiumPaid=ISNULL(SUM(pfi.amount),0)  
    FROM  
    PFPremiumFinance pf  
     INNER JOIN PFInstalments pfi  
      ON pf.pfprem_finance_cnt = pfi.pfprem_finance_cnt  
      AND pf.pfprem_finance_version = pfi.pfprem_finance_version  
    WHERE  
    pf.insurance_file_cnt = @InsuranceFileCnt  
     AND pfi.status = 3  
  
                        --PN: 61749  
                        --Get the amounts of cancelled installment also  
   SELECT  
    @PremiumPaid = @PremiumPaid + ISNULL ((SUM(td.Amount)-SUM(td.Outstanding_Amount)),0),
	@OutstandingAmount = SUM(td.Outstanding_Amount)
   FROM  
    Document d  
    INNER JOIN DocumentType dt  
     ON dt.DocumentType_id=d.DocumentType_id  
     AND dt.Code IN ('SED')  
    INNER JOIN TransDetail td  
      ON td.Document_Id=d.Document_Id  
   WHERE  
    d.Insurance_File_Cnt=@InsuranceFileCnt  
                                AND td.comment = 'Instalment Plan'  
    AND td.Account_ID IN (  
           SELECT  
            ACT.Account_Id  
           FROM  
            Insurance_File IFI  
            INNER JOIN Account ACT  
             ON ACT.Account_Key=IFI.Insured_Cnt  
           WHERE  
           Insurance_File_Cnt=@InsuranceFileCnt  
           UNION  
           SELECT  
              ACT.Account_Id  
           FROM  
            Insurance_File IFI  
            INNER JOIN Party_Agent PAG  
             ON PAG.Party_Cnt=IFI.Lead_Agent_Cnt  
            INNER JOIN Party_Agent_Type PAT  
             ON PAT.Party_Agent_Type_Id=PAG.Party_Agent_Type_Id  
             AND PAT.Code='Broker'  
            INNER JOIN Account ACT  
             ON ACT.Account_Key=PAG.Party_Cnt  
           WHERE Insurance_File_Cnt=@InsuranceFileCnt  
          )  
  
		                       --Get the amounts of reinstatement installment also
   SELECT
    @PremiumPaid = @PremiumPaid - ISNULL ((SUM(td.Amount)-SUM(td.Outstanding_Amount)),0)
   FROM
    Document d
    INNER JOIN DocumentType dt
     ON dt.DocumentType_id=d.DocumentType_id
     AND dt.Code IN ('SID')
    INNER JOIN TransDetail td
      ON td.Document_Id=d.Document_Id
   WHERE
    d.Insurance_File_Cnt=@InsuranceFileCnt
                                AND td.comment = 'Policy Reinstatement'
                                AND   (SELECT  COUNT(ad1.document_ref) FROM AllocationDetail ad INNER JOIN AllocationDetail ad1 ON ad1.allocation_id=ad.allocation_id
    INNER JOIN transdetail t ON t.transdetail_id=ad.transdetail_id INNER JOIN document d ON d.document_id=t.document_id
	WHERE d.Insurance_File_Cnt=@InsuranceFileCnt AND LEFT(ad.document_ref,3)= 'SID' AND ad.document_ref<>ad1.document_ref AND LEFT(ad1.document_ref,3)='INC') = 0
    AND td.Account_ID IN (
           SELECT
            ACT.Account_Id
           FROM
            Insurance_File IFI
            INNER JOIN Account ACT
             ON ACT.Account_Key=IFI.Insured_Cnt
           WHERE
           Insurance_File_Cnt=@InsuranceFileCnt
           UNION
           SELECT
              ACT.Account_Id
           FROM
            Insurance_File IFI
            INNER JOIN Party_Agent PAG
             ON PAG.Party_Cnt=IFI.Lead_Agent_Cnt
            INNER JOIN Party_Agent_Type PAT
             ON PAT.Party_Agent_Type_Id=PAG.Party_Agent_Type_Id
             AND PAT.Code='Broker'
            INNER JOIN Account ACT
             ON ACT.Account_Key=PAG.Party_Cnt
           WHERE Insurance_File_Cnt=@InsuranceFileCnt
          )
  
   --Update the premium details table variable entry with premium paid  
   UPDATE  
    @PremiumDetails  
   SET  
    PremiumPaid=@PremiumPaid,
	outstanding_amount = @OutstandingAmount
   WHERE  
    Insurance_File_Cnt=@InsuranceFileCnt  
  
   --Get TransDetail id of main transaction  
   SELECT  
    @PlanTransactionID=pf.PlanTransaction_id  
    FROM  
    PFPremiumFinance pf  
    WHERE  
    pf.insurance_file_cnt = @InsuranceFileCnt  
	and pf.PlanTransaction_id is NOT NUll
   --Update the premium details table variable entry with the trasdetail id of main transaction  
   UPDATE  
    @PremiumDetails  
   SET  
    PlanTransactionID=@PlanTransactionID  
   WHERE  
    Insurance_File_Cnt=@InsuranceFileCnt  
  END  
  ELSE  
  BEGIN  
  
   --Get the premium paid  
   --Selecting only the transactions whose account_id belongs to the client or the broker  
   SELECT  
    @PremiumPaid=ISNULL ((SUM(td.Amount)-SUM(td.Outstanding_Amount)),0),
	@OutstandingAmount = SUM(td.Outstanding_Amount)
   FROM  
    Document d  
    INNER JOIN DocumentType dt  
     ON dt.DocumentType_id=d.DocumentType_id  
     AND dt.Code IN ('SND','SED','SID','SRD','SEC')  
    INNER JOIN TransDetail td  
      ON td.Document_Id=d.Document_Id  
   WHERE  
    d.Insurance_File_Cnt=@InsuranceFileCnt  
    AND td.Account_ID IN (  
           SELECT  
            ACT.Account_Id  
           FROM  
            Insurance_File IFI  
            INNER JOIN Account ACT  
             ON ACT.Account_Key=IFI.Insured_Cnt  
           WHERE  
           Insurance_File_Cnt=@InsuranceFileCnt  
           UNION  
           SELECT  
              ACT.Account_Id  
           FROM  
            Insurance_File IFI  
            INNER JOIN Party_Agent PAG  
             ON PAG.Party_Cnt=IFI.Lead_Agent_Cnt  
            INNER JOIN Party_Agent_Type PAT  
             ON PAT.Party_Agent_Type_Id=PAG.Party_Agent_Type_Id  
             AND PAT.Code='Broker'  
            INNER JOIN Account ACT  
             ON ACT.Account_Key=PAG.Party_Cnt  
           WHERE Insurance_File_Cnt=@InsuranceFileCnt  
          )  
  
   --Update the premium details table variable entry with premium paid  
   UPDATE  
    @PremiumDetails  
   SET  
    PremiumPaid=@PremiumPaid,
	outstanding_amount = @OutstandingAmount
   WHERE  
    Insurance_File_Cnt=@InsuranceFileCnt  
  
  --If payment method is not instalment, no need to get the mail transaction id (As told by Gautam)  
  END  
  SET @Index= @Index+1  
 CONTINUE  
 END  
  
 SELECT  
  ifi.insurance_file_cnt,  
  ifi.annual_premium,  
  ifi.this_premium,  
  ifi.net_premium,  
  
  /*Total risk level tax, not applied to client*/  
  (SELECT SUM(ISNULL(t.value,0))  
   FROM Tax_Calculation t  
   LEFT JOIN risk r ON r.risk_cnt = t.risk_cnt  
   WHERE t.Insurance_File_Cnt=ifi.Insurance_File_Cnt  
   AND t.risk_cnt IS NOT NULL AND r.is_risk_selected=1  
   AND t.is_not_applied_to_client=1  
  )AS Risk_Non_Client_Tax,  
  
  /*Total risk level tax, applied to client*/  
  (SELECT SUM(ISNULL(t.value,0))  
   FROM Tax_Calculation t  
   LEFT JOIN risk r ON r.risk_cnt = t.risk_cnt  
   WHERE t.Insurance_File_Cnt=ifi.Insurance_File_Cnt  
   AND t.risk_cnt IS NOT NULL AND r.is_risk_selected=1 AND t.is_not_applied_to_client=0  
   AND transtype NOT IN ('TTRITP','TTRITC')  
  )AS Risk_Client_Tax,  
  
  /*Total Fees at Risk Level */  
  (SELECT SUM(ISNULL(pf.currency_amount,0))  
   FROM policy_fee_u pf  
   WHERE pf.insurance_file_cnt = ifi.Insurance_File_Cnt  
   AND NOT pf.risk_cnt IS NULL  
  )AS Risk_Fee_Total,  
  
  /*Total policy level tax, not applied to client*/  
  (SELECT SUM(ISNULL(t.value,0))  
   FROM Tax_Calculation t  
   WHERE t.Insurance_File_Cnt=ifi.Insurance_File_Cnt  
   AND t.risk_cnt IS NULL AND t.is_not_applied_to_client=1  
  )AS Policy_Non_Client_tax,  
  
  /*Total policy level tax, applied to client*/  
  (SELECT SUM(ISNULL(t.value,0))  
   FROM Tax_Calculation t  
   WHERE t.Insurance_File_Cnt=ifi.Insurance_File_Cnt  
   AND t.risk_cnt IS NULL  AND t.is_not_applied_to_client=0  
  )AS Policy_Client_tax,  
  
  /*Total Fee at Policy Level */  
  (SELECT SUM(ISNULL(pf.currency_amount,0))  
   FROM policy_fee_u pf  
   WHERE pf.insurance_file_cnt =ifi.Insurance_File_Cnt  
   AND pf.risk_cnt IS NULL  
  )AS Policy_Fee_Total,  
  
  /* Total amount of paid for each policy version */  
  PD.PremiumPaid,  
  PD.Payment_Method,  
  PD.PlanTransactionId,
  (SELECT SUM(ISNULL(ac.commission_value,0)+ISNULL(ac.tax_amount,0))  
   FROM Agent_commission ac  
    WHERE (ac.insurance_file_cnt =ifi.Insurance_File_Cnt)
  )AS Agent_commission_Total,
   Outstanding_Amount,
   (ifi.this_premium 
   + ISNULL((SELECT SUM(ISNULL(pf.currency_amount,0)) FROM policy_fee_u pf WHERE pf.insurance_file_cnt = ifi.Insurance_File_Cnt AND NOT pf.risk_cnt IS NULL),0)
   + ISNULL((SELECT SUM(ISNULL(t.value,0)) FROM Tax_Calculation t WHERE t.Insurance_File_Cnt=ifi.Insurance_File_Cnt AND t.risk_cnt IS NULL  AND t.is_not_applied_to_client=0),0)
   + ISNULL((SELECT SUM(ISNULL(pf.currency_amount,0))  FROM policy_fee_u pf WHERE pf.insurance_file_cnt =ifi.Insurance_File_Cnt AND pf.risk_cnt IS NULL),0)) As Transaction_Amount     
 FROM  
  @PolicyVersions IFI  
  INNER JOIN @PremiumDetails PD  
  ON PD.Insurance_File_Cnt=IFI.Insurance_File_Cnt  
  WHERE (@nInsurance_file_cnt IS NULL OR IFI.Insurance_File_Cnt = @nInsurance_file_cnt)
END  
--End (Prakash Varghese) Modified the sp to get the paid amount for all payment methods.  
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
