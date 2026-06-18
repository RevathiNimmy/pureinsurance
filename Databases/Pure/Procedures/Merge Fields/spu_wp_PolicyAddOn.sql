SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_PolicyAddOn'
GO


CREATE PROCEDURE spu_wp_PolicyAddOn
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

DECLARE
    @AddOnName VARCHAR(255), 
    @AddOnIncTax MONEY,
    @AddOnExcTax MONEY,
    @AddOnCommission MONEY,
    @AddOnTaxAmount MONEY

DECLARE PolicyAddOn_Cursor SCROLL CURSOR FOR  
    SELECT 
        p.name,
        pf.Total_Fee,
        pf.Fee_Amount,
        pf.Total_Commission,
        pf.Tax_Amount 
    FROM policy_fee pf 
    JOIN party p 
        ON p.party_cnt = pf.party_cnt
    WHERE insurance_file_cnt = @InsuranceFileCnt  
    ORDER BY policy_fee_id

OPEN PolicyAddOn_Cursor  
  
FETCH ABSOLUTE @Instance1 FROM PolicyAddOn_Cursor INTO  
    @AddOnName,  
    @AddOnIncTax,  
    @AddOnExcTax,  
    @AddOnCommission,  
    @AddOnTaxAmount

CLOSE PolicyAddOn_Cursor  

DEALLOCATE PolicyAddOn_Cursor  
  
SELECT  
    @AddOnName 'AddOnName',  
    @AddOnIncTax 'AddOnIncTax',  
    @AddOnExcTax 'AddOnExcTax',  
    @AddOnCommission 'AddOnCommission', 
    @AddOnTaxAmount 'AddOnTaxAmount'

GO
