
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_PaymentLedgers'
GO

CREATE PROCEDURE spu_ACT_PaymentLedgers 
	(@source_id int)
AS BEGIN
    DECLARE @InsurerLedgerID INT
    DECLARE @AgentLedgerID INT
    DECLARE @PremiumFinanceLedgerID INT
    DECLARE @OtherReceivableID INT
    DECLARE @OtherPayableID INT
    DECLARE @CommissionID INT
    DECLARE @SubAgentLedgerID INT  

	DECLARE @MultiCompany VARCHAR(20)

	SELECT @MultiCompany=''
	SELECT @MultiCompany=RTRIM(ISNULL(value,'')) FROM Hidden_Options WHERE option_number=16

	IF @MultiCompany<>'1'
		SELECT @source_id=1

    SELECT @InsurerLedgerID = Ledger_id
    FROM Ledger
    WHERE RTRIM(Ledger_Short_Name) = 'IN'
        AND company_id=@source_id

    SELECT @AgentLedgerID = Ledger_id
    FROM Ledger
    WHERE RTRIM(Ledger_Short_Name) = 'AG'
        AND company_id=@source_id

    SELECT @PremiumFinanceLedgerID = Ledger_id
    FROM Ledger
    WHERE RTRIM(Ledger_Short_Name) = 'RF'
        AND company_id=@source_id

    SELECT @OtherReceivableID = Ledger_id
    FROM Ledger
    WHERE RTRIM(Ledger_Short_Name) = 'OR'
        AND company_id=@source_id

    SELECT @OtherPayableID = Ledger_id
    FROM Ledger
    WHERE RTRIM(Ledger_Short_Name) = 'OP'
        AND company_id=@source_id

    SELECT @CommissionID = Ledger_id   
    FROM Ledger  
    WHERE RTRIM(Ledger_Short_Name) = 'CO'  
        AND company_id=@source_id 

    SELECT @SubAgentLedgerID = Ledger_id  
    FROM Ledger  
    WHERE RTRIM(Ledger_Short_Name) = 'UB'  
        AND company_id=@source_id

  SELECT  
        @InsurerLedgerID AS InsurerLedgerID,  
        @AgentLedgerID AS AgentLedgerID,  
        @PremiumFinanceLedgerID AS PremiumFinanceLedgerID,  
  	@OtherReceivableID AS OtherReceivableID,  
  	@OtherPayableID AS OtherPayableID,  
  	@CommissionID AS CommissionID,
  	@SubAgentLedgerID AS SubAgentLedgerID  
END
GO
