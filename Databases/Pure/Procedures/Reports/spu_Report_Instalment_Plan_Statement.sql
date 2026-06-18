SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Instalment_Plan_Statement'
GO

CREATE PROCEDURE spu_Report_Instalment_Plan_Statement    
        @branch_id  int,
		@AgentShortName varchar (40),
		@End_date   nvarchar(50),
		@Basis      varchar (40),            
        @Underwriting_Year varchar(40),  
        @Start_Date 		nvarchar(50),            
        @TypeOfCurrency Varchar(40),            
        @GroupByCode    Varchar(40),            
        @IncludeBalanceAccount as varchar(10),            
        @TransactionType   Varchar(30),  
        @ProductCode varchar(255),                     
        @AgeAndUnalloc Varchar(10)
AS 
         
DECLARE @AgentCnt Int          
DECLARE @AgentGroupCnt Int      
         
	SELECT @Start_Date= CONVERT(DATETIME, @Start_Date, 103),    
    @End_date = CONVERT(DATETIME, @End_date, 103) 
	
IF @AgentShortName <> 'ALL'          
SET @AgentCnt=(SELECT party_cnt FROM Party WHERE shortname=@AgentShortName)          

 /*Get System Currency Details*/          
declare @SystemCurrencyCode varchar(10)          
declare @SystemCurrencyDesc varchar(255)          
SELECT          
    @SystemCurrencyCode = c.iso_code,          
    @SystemCurrencyDesc = c.description          
FROM PMSystem pms          
JOIN currency c          
    ON c.currency_id = pms.currency_id          
WHERE pms.system_id = 1          
/*end  Get System Currency*/         
        
DECLARE @product_id int          
SELECT @product_id = product_id          
FROM Product          
WHERE description = @productcode          
        
          
IF ISNULL(@Underwriting_Year, '') = ''            
    SET @Underwriting_Year = 'ALL'            
          
IF ISNULL(@product_id, '') = ''            
    SET @product_id =0          
          
CREATE TABLE #tempParameter            
(            
    PremiumFinanceCnt       Int         NULL,            
    PremiumFinanceVersion   Int             NULL,            
    InstalmentDueDate       Datetime        NULL            
)            
          
IF @TransactionType<>'Claim Transaction Only'    
BEGIN        
IF @AgentShortName = 'ALL'          
BEGIN      
    BEGIN            
        INSERT INTO #tempParameter              
        SELECT pfi.pfprem_finance_cnt,          
           Max(pfi.pfprem_finance_version),          
           Max(pfi.DueDate)        
        FROM PFInstalments pfi          
        INNER JOIN PFPremiumFinance pf ON pf.pfprem_finance_cnt=pfi.pfprem_finance_cnt AND pf.agent_cnt is NOT Null        
        WHERE  pfi.DueDate>=@start_date AND pfi.DueDate<=@end_date AND pf.StatusInd IN ('040','140') and pfi.InstalmentNumber<>0           
        GROUP BY Day(pfi.DueDate),Month(pfi.DueDate),Year(pfi.DueDate),pfi.pfprem_finance_cnt          
        ORDER BY pfi.pfprem_finance_cnt      
     END
END          
ELSE          
BEGIN        
    BEGIN         
        INSERT INTO #tempParameter        
        SELECT pfi.pfprem_finance_cnt,          
              Max(pfi.pfprem_finance_version),          
              Max(pfi.DueDate)       
        FROM PFInstalments pfi          
        INNER JOIN PFPremiumFinance pf ON pf.pfprem_finance_cnt=pfi.pfprem_finance_cnt AND pf.agent_cnt=@AgentCnt          
        WHERE  pfi.DueDate>=@start_date AND pfi.DueDate<=@end_date AND pf.StatusInd IN ('040','140') and pfi.InstalmentNumber<>0           
        GROUP BY Day(pfi.DueDate),Month(pfi.DueDate),Year(pfi.DueDate),pfi.pfprem_finance_cnt        
        ORDER BY pfi.pfprem_finance_cnt     
    END
END        
END            
CREATE TABLE #tempStat            
(            
    InstalmentDueDate       Datetime        NULL,          
    InstalmentPlanNo        Int             NULL,          
    TransactionType         varchar(40)     NULL,          
    TransactionRef          varchar(100)    NULL,          
    Amount                  Numeric(8,2)    NULL,          
    Tax                     Numeric(8,2)    NULL,          
    NetAmount               Numeric(8,2)    NULL,          
    Company                 varchar(255)    NULL,            
    CompanyAddress1         varchar(255)     NULL,            
    CompanyAddress2         varchar(255)     NULL,            
    CompanyAddress3         varchar(255)     NULL,            
    CompanyAddress4         varchar(255)     NULL,            
    CompanyPostCode         varchar(40)     NULL,            
    PhoneAreaCode           varchar(10)     NULL,            
    PhoneNumber  varchar(15)     NULL,            
    PhoneExtension          varchar(6)      NULL,            
    FaxAreaCode             varchar(10)     NULL,            
    FaxNumber               varchar(15)     NULL,            
    AgentResolvedName       varchar(255)    NULL,             
    AccountID               int             NULL,            
    AccountCode             varchar(255)     NULL,            
    AccountAddress1         varchar(255)     NULL,            
    AccountAddress2         varchar(255)     NULL,            
    AccountAddress3         varchar(255)     NULL,            
    AccountAddress4         varchar(255)     NULL,            
    AccountPostCode         varchar(40)  NULL,           
    CurrencyCode            Varchar(30)     Null,          
    CurrencyDesc            Varchar(255)    NULL,        
    Band_1_Start            int             NULL,      
    Band_2_Start            int             NULL,      
    Band_3_Start            int             NULL,      
    Band_4_Start            int             NULL,      
    Band_5_Start            int             NULL,      
    Band_1_End              int             NULL,      
    Band_2_End              int             NULL,      
    Band_3_End              int             NULL,      
    Band_4_End              int             NULL,  
    PFTransaction_Id        int             Null,
    InstalmentStatus        int             NULL
)          
           
          
DECLARE @PremiumFinanceCnt int           
DECLARE @PremiumFinanceVersion int          
DECLARE @InstalmentDueDate Datetime          
          
DECLARE tempStat_cursor CURSOR FOR           
SELECT PremiumFinanceCnt, PremiumFinanceVersion,InstalmentDueDate          
FROM #tempParameter          
ORDER BY PremiumFinanceCnt          
          
OPEN tempStat_cursor          
          
FETCH NEXT FROM tempStat_cursor           
INTO @PremiumFinanceCnt, @PremiumFinanceVersion,@InstalmentDueDate          
          
WHILE @@FETCH_STATUS = 0          
BEGIN          
IF @branch_id=0           
  BEGIN          
     INSERT INTO #tempStat          
       SELECT           
          pfi.DueDate,          
          pf.AutoGeneratedPlanRef,
          --pfi.pfprem_finance_cnt,          
          --pf.TransType,      
          CASE pf.TransType      
               WHEN 'NB'  THEN 'New business'          
               WHEN 'MTA' THEN 'Mid term adjustment'          
               WHEN 'REN' THEN 'Renewal'      
               WHEN 'MTACAN' THEN 'Cancellation'       
             END,         
          (select top 1 d.document_ref from document d where d.insurance_file_cnt=pf.insurance_file_cnt and d.comment like 'Instalment%'),                    
	  --((pfi.Amount*100)/(100+ISNULL((SELECT TOP 1 tc.percentage FROM tax_calculation tc WHERE tc.insurance_file_cnt=pf.insurance_file_cnt and tc.transtype='TTR'),0))),    
	    ((pfi.Amount*100)/(100+ISNULL((SELECT TOP 1 tc.percentage FROM tax_calculation tc WHERE tc.insurance_file_cnt IN (SELECT Insurance_File_Cnt FROM PFPremiumFinance WHERE PFPrem_Finance_Cnt=pf.PFPrem_Finance_Cnt) and tc.transtype='TTR'),0))),    
          --pfi.Tax,            
          --ISNULL((((pfi.Amount*100)/(100+ISNULL((SELECT TOP 1 tc.percentage FROM tax_calculation tc WHERE tc.insurance_file_cnt=pf.insurance_file_cnt and tc.transtype='TTR'),0)))*(SELECT TOP 1 tc.percentage FROM tax_calculation tc WHERE tc.insurance_file_cnt=pf.insurance_file_cnt and tc.transtype='TTR')/100),0),  
			ISNULL((((pfi.Amount*100)/(100+ISNULL((SELECT TOP 1 tc.percentage FROM tax_calculation tc WHERE tc.insurance_file_cnt IN (SELECT Insurance_File_Cnt FROM PFPremiumFinance WHERE PFPrem_Finance_Cnt=pf.PFPrem_Finance_Cnt) and tc.transtype='TTR'),0)))*(SELECT TOP 1 tc.percentage FROM tax_calculation tc WHERE tc.insurance_file_cnt IN (SELECT Insurance_File_Cnt FROM PFPremiumFinance WHERE PFPrem_Finance_Cnt=pf.PFPrem_Finance_Cnt) and tc.transtype='TTR')/100),0),  		  
		  pfi.Amount,
          NULL,          
          NULL,          
          NULL,          
          NULL,          
          NULL,          
          NULL,          
          NULL,          
          NULL,          
          NULL,          
          NULL,          
          NULL,          
          p.resolved_name,          
          ac.account_id,          
          ac.short_code,          
          ac.address1,          
          ac.address2,          
          ac.address3,          
          ac.address4,          
          ac.postal_code,        
          Case @TypeOfCurrency          
                WHEN 'Base'  THEN CB.Code          
                WHEN 'System' THEN @SystemcurrencyCode          
                WHEN 'Account' THEN CA.Code          
   WHEN 'Transaction' THEN CT.Code          
             END,          
          Case @TypeOfCurrency          
                 WHEN 'Base'  THEN CB.Description          
                 WHEN 'System' THEN @SystemcurrencyDesc          
                 WHEN 'Account' THEN CA.Description          
                 WHEN 'Transaction' THEN CT.Description          
            END,      
          Null,      
          Null,      
          Null,      
          Null,      
          Null,      
          Null,      
          Null,      
          Null,      
          Null,  
          ISNULL(pfi.PFTransaction_Id,0),
          ISNULL(pfi.Status,0)   
      FROM PFInstalments pfi          
      INNER JOIN PFPremiumFinance pf ON pf.pfprem_finance_cnt=pfi.pfprem_finance_cnt AND pf.pfprem_finance_version=pfi.pfprem_finance_version          
      LEFT JOIN party p ON p.party_cnt=pf.agent_cnt          
      LEFT JOIN account ac ON ac.short_code=p.shortname          
      LEFT JOIN insurance_file ifl ON ifl.insurance_file_cnt=pf.insurance_file_cnt             
      LEFT JOIN underwriting_year  ON underwriting_year.underwriting_year_id = ifl.underwriting_year_id            
      JOIN    Company CO                      ON CO.company_id= ifl.branch_id          
      JOIN    Currency CB                     ON CB.currency_id = CO.base_currency          
      JOIN    Currency CA                     ON CA.currency_id = ac.currency_id          
      JOIN    Currency CT                     ON CT.Currency_id = ifl.currency_id          
      WHERE pfi.pfprem_finance_cnt=@PremiumFinanceCnt           
      AND  pfi.pfprem_finance_version=@PremiumFinanceVersion
      AND ((pfi.status=3 and @IncludeBalanceAccount='No') OR (@IncludeBalanceAccount='Yes'and pfi.status<>3))          
      AND  pfi.DueDate=@InstalmentDueDate          
      AND (underwriting_year.code = @Underwriting_Year OR rtrim(@Underwriting_Year) = 'ALL')           
      AND (ifl.product_id = @product_id OR @product_id = 0)
      AND pfi.InstalmentNumber<>0  
      AND pf.StatusInd IN ('040','140')         
  END          
ELSE          
  BEGIN          
     INSERT INTO #tempStat          
        SELECT          
        pfi.DueDate,          
        pf.AutoGeneratedPlanRef,
        --pfi.pfprem_finance_cnt,          
        --pf.TransType,      
        CASE pf.TransType      
               WHEN 'NB'  THEN 'New business'          
               WHEN 'MTA' THEN 'Mid term adjustment'          
               WHEN 'REN' THEN 'Renewal'      
               WHEN 'MTACAN' THEN 'Cancellation'      
             END,             
        (select top 1 d.document_ref from document d where d.insurance_file_cnt=pf.insurance_file_cnt and d.comment like 'Instalment%'),            
        --((pfi.Amount*100)/(100+ISNULL((SELECT TOP 1 tc.percentage FROM tax_calculation tc WHERE tc.insurance_file_cnt=pf.insurance_file_cnt and tc.transtype='TTR'),0))),    
		((pfi.Amount*100)/(100+ISNULL((SELECT TOP 1 tc.percentage FROM tax_calculation tc WHERE tc.insurance_file_cnt IN (SELECT Insurance_File_Cnt FROM PFPremiumFinance WHERE PFPrem_Finance_Cnt=pf.PFPrem_Finance_Cnt) and tc.transtype='TTR'),0))),    
        --pfi.Tax,          
        --ISNULL((((pfi.Amount*100)/(100+ISNULL((SELECT TOP 1 tc.percentage FROM tax_calculation tc WHERE tc.insurance_file_cnt=pf.insurance_file_cnt and tc.transtype='TTR'),0)))*(SELECT TOP 1 tc.percentage FROM tax_calculation tc WHERE tc.insurance_file_cnt=pf.insurance_file_cnt and tc.transtype='TTR')/100),0),  
		ISNULL((((pfi.Amount*100)/(100+ISNULL((SELECT TOP 1 tc.percentage FROM tax_calculation tc WHERE tc.insurance_file_cnt IN (SELECT Insurance_File_Cnt FROM PFPremiumFinance WHERE PFPrem_Finance_Cnt=pf.PFPrem_Finance_Cnt) and tc.transtype='TTR'),0)))*(SELECT TOP 1 tc.percentage FROM tax_calculation tc WHERE tc.insurance_file_cnt IN (SELECT Insurance_File_Cnt FROM PFPremiumFinance WHERE PFPrem_Finance_Cnt=pf.PFPrem_Finance_Cnt) and tc.transtype='TTR')/100),0),  
	    pfi.Amount,
        NULL,          
        NULL,          
        NULL,          
        NULL,          
        NULL,          
        NULL,          
        NULL,          
        NULL,          
        NULL,          
        NULL,          
        NULL,          
        p.resolved_name,          
        ac.account_id,          
        ac.short_code,          
        ac.address1,          
        ac.address2,          
        ac.address3,          
        ac.address4,          
        ac.postal_code,   
        Case @TypeOfCurrency          
                WHEN 'Base'  THEN CB.Code          
                WHEN 'System' THEN @SystemcurrencyCode          
                WHEN 'Account' THEN CA.Code          
                WHEN 'Transaction' THEN CT.Code          
             END,          
         Case @TypeOfCurrency          
                 WHEN 'Base'  THEN CB.Description          
                 WHEN 'System' THEN @SystemcurrencyDesc          
                 WHEN 'Account' THEN CA.Description          
                 WHEN 'Transaction' THEN CT.Description          
            END,      
          Null,      
          Null,      
          Null,      
          Null,      
          Null,      
          Null,      
          Null,      
      	Null,      
          Null,  
          ISNULL(pfi.PFTransaction_Id,0),
          ISNULL(pfi.Status,0)      
    FROM PFInstalments pfi          
    INNER JOIN PFPremiumFinance pf ON pf.pfprem_finance_cnt=pfi.pfprem_finance_cnt AND pf.pfprem_finance_version=pfi.pfprem_finance_version          
    LEFT JOIN party p ON p.party_cnt=pf.agent_cnt          
    LEFT JOIN account ac ON ac.short_code=p.shortname          
    LEFT JOIN insurance_file ifl ON ifl.insurance_file_cnt=pf.insurance_file_cnt          
    LEFT JOIN underwriting_year  ON underwriting_year.underwriting_year_id = ifl.underwriting_year_id            
    JOIN    Company CO                      ON CO.company_id= ifl.branch_id          
    JOIN    Currency CB                     ON CB.currency_id = CO.base_currency          
    JOIN    Currency CA                     ON CA.currency_id = ac.currency_id          
    JOIN    Currency CT                     ON CT.Currency_id = ifl.currency_id          
    WHERE pfi.pfprem_finance_cnt=@PremiumFinanceCnt           
    AND  pfi.pfprem_finance_version=@PremiumFinanceVersion
    AND ((pfi.status=3 and @IncludeBalanceAccount='No') OR (@IncludeBalanceAccount='Yes'and pfi.status<>3))        
    AND  pfi.DueDate=@InstalmentDueDate          
    AND  pf.companyNo=@branch_id          
           AND (underwriting_year.code = @Underwriting_Year OR rtrim(@Underwriting_Year) = 'ALL')          
           AND (ifl.product_id = @product_id OR @product_id = 0)
    AND pfi.InstalmentNumber<>0
    AND pf.StatusInd IN ('040','140')	            
  END          
          
FETCH NEXT FROM tempStat_cursor           
INTO @PremiumFinanceCnt, @PremiumFinanceVersion,@InstalmentDueDate          
END           
CLOSE tempStat_cursor          
DEALLOCATE tempStat_cursor          
IF @branch_id=0          
   BEGIN          
 UPDATE #tempStat            
     SET     Company = s.Description,            
             CompanyAddress1 = s.Address1,            
             CompanyAddress2 = s.Address2,            
             CompanyAddress3 = s.Address3,            
             CompanyAddress4 = s.Address4,            
             CompanyPostCode = s.postal_code,            
             PhoneAreaCode = s.Phone_Area_Code,            
             PhoneNumber = s.Phone_Number,            
             PhoneExtension = s.Phone_Extension,            
             FaxAreaCode = s.Fax_Area_Code,            
             FaxNumber = s.Fax_Number            
     FROM    Source s            
     WHERE   s.Source_Id = 1          
   END          
ELSE          
   BEGIN          
 UPDATE #tempStat            
     SET     Company = s.Description,            
             CompanyAddress1 = s.Address1,            
             CompanyAddress2 = s.Address2,            
             CompanyAddress3 = s.Address3,            
             CompanyAddress4 = s.Address4,            
             CompanyPostCode = s.postal_code,            
             PhoneAreaCode = s.Phone_Area_Code,            
             PhoneNumber = s.Phone_Number,            
             PhoneExtension = s.Phone_Extension,            
             FaxAreaCode = s.Fax_Area_Code,            
             FaxNumber = s.Fax_Number            
     FROM    Source s            
     WHERE   s.Source_Id = @branch_id          
   END         
      
-- Let's get the Aging bands for this report      
Declare      @Band_1_Start     int,      
                @Band_2_Start     int,      
                @Band_3_Start     int,      
                @Band_4_Start     int,      
                @Band_5_Start     int,      
                @Band_1_End       int,      
                @Band_2_End       int,      
                @Band_3_End       int,      
                @Band_4_End       int,      
                @Band_Branch      int      
      
--Set the Band Branch must be = 1 at least      
IF @branch_id IS NULL OR @branch_id = 0      
    SELECT @Band_Branch = 1      
ELSE      
    SELECT @Band_Branch = @branch_id      
      
--Get the Start      
Execute spu_get_Aging_band 3001, @Band_Branch, @Band_1_Start Output      
Execute spu_get_Aging_band 3003, @Band_Branch, @Band_2_Start Output      
Execute spu_get_Aging_band 3005, @Band_Branch, @Band_3_Start Output      
Execute spu_get_Aging_band 3007, @Band_Branch, @Band_4_Start Output      
Execute spu_get_Aging_band 3009, @Band_Branch, @Band_5_Start Output      
      
--Get the End      
Execute spu_get_Aging_band 3002, @Band_Branch, @Band_1_End Output      
Execute spu_get_Aging_band 3004, @Band_Branch, @Band_2_End Output      
Execute spu_get_Aging_band 3006, @Band_Branch, @Band_3_End Output      
Execute spu_get_Aging_band 3008, @Band_Branch, @Band_4_End Output      
      
UPDATE #tempStat      
    Set     Band_1_Start = @Band_1_Start      
UPDATE #tempStat      
    Set     Band_2_Start = @Band_2_Start      
UPDATE #tempStat      
    Set     Band_3_Start = @Band_3_Start      
UPDATE #tempStat      
    Set     Band_4_Start = @Band_4_Start      
UPDATE #tempStat      
    Set     Band_5_Start = @Band_5_Start      
      
UPDATE #tempStat      
    Set     Band_1_End = @Band_1_End      
UPDATE #tempStat      
    Set     Band_2_End = @Band_2_End      
UPDATE #tempStat      
    Set     Band_3_End = @Band_3_End      
UPDATE #tempStat      
    Set     Band_4_End = @Band_4_End      
      
SELECT InstalmentDueDate,         
    InstalmentPlanNo,        
    TransactionType,        
    TransactionRef,        
    Amount,        
    Tax,        
    NetAmount,       
    Company,         
    CompanyAddress1,        
    CompanyAddress2,         
    CompanyAddress3,          
    CompanyAddress4,           
    CompanyPostCode,            
    PhoneAreaCode,          
    PhoneNumber,        
    PhoneExtension,        
    FaxAreaCode,        
    FaxNumber,          
    AgentResolvedName,            
    AccountID,          
    AccountCode,        
    AccountAddress1,        
    AccountAddress2,        
    AccountAddress3,           
    AccountAddress4,          
    AccountPostCode,        
    CurrencyCode,                    
    CurrencyDesc,      
    Band_1_Start,      
    Band_2_Start,      
    Band_3_Start,      
    Band_4_Start,      
    Band_5_Start,      
    Band_1_End,      
    Band_2_End,      
    Band_3_End,      
    Band_4_End,  
    PFTransaction_Id,
    InstalmentStatus                
FROM  #tempStat        
ORDER BY AccountId,InstalmentPlanNo,InstalmentDueDate         
          
DROP TABLE #tempParameter          
DROP TABLE #tempStat 
  
Go
