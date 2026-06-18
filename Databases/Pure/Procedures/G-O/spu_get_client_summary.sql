-- sj 30/07/2002 - Remove transaction_export_folder

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_get_client_summary'
GO


CREATE PROCEDURE spu_get_client_summary
    @PartyCnt INT
AS


DECLARE @Home   VARCHAR(255),
    @Work   VARCHAR(255),
    @Mobile VARCHAR(255),
    @Email  VARCHAR(255),
    @Web    VARCHAR(255),
    @Temp   VARCHAR(255),
    @Many   INT
DECLARE @address1   VARCHAR(60),
    @address2   VARCHAR(60),
    @address3   VARCHAR(60),
    @address4   VARCHAR(60),
    @postal_code    VARCHAR(20)
DECLARE @BalanceOutstanding DECIMAL(19,4),
    @GrossPremiumThisYear   DECIMAL(19,4),
    @GrossPremiumLastYear   DECIMAL(19,4),
    @CommissionThisYear DECIMAL(19,4),
    @CommissionLastYear DECIMAL(19,4),
    @FeesThisYear       DECIMAL(19,4),
    @FeesLastYear       DECIMAL(19,4)
SELECT @Temp = Null

exec spu_get_contact @PartyCnt, "3131 XCO", "TELEPHONE", @Temp OUTPUT
SELECT @Home = @Temp
SELECT @Temp = Null

exec spu_get_contact @PartyCnt, "3131 XCO", "MOBILE", @Temp OUTPUT
SELECT @Mobile = @Temp
SELECT @Temp = Null

exec spu_get_contact @PartyCnt, "3131 XCO", "E-MAIL", @Temp OUTPUT
SELECT @Email = @Temp
SELECT @Temp = Null

exec spu_get_contact @PartyCnt, "3131 XCO", "WEB", @Temp OUTPUT
SELECT @Web = @Temp
SELECT @Temp = Null

exec spu_get_contact @PartyCnt, "3131 002", "TELEPHONE", @Temp OUTPUT
SELECT @Work = @Temp
exec spu_wp_get_address @PartyCnt,
            0,
            0,
            "3131 XCO",
            @address1 OUTPUT,
            @address2 OUTPUT,
            @address3 OUTPUT,
            @address4 OUTPUT,
            @postal_code OUTPUT
SELECT  DISTINCT
    Party.party_cnt party_cnt,
    Party.shortname Client_Code,
    Party.name Client_Name,
    address1 = @address1,
    address2 = @address2,
    address3 = @address3,
    address4 = @address4,
    postal_code = @postal_code,
    country = "UK",
    Home_Phone_Number = @Home,
    Work_Phone_Number = @Work,
    Mobile_Phone_Number = @Mobile,
    EMail_Address = @Email,
    Website_Address = @Web,
        Balance_Outstanding = @BalanceOutstanding,
        GrossPremium_Current = @GrossPremiumThisYear,
        GrossPremium_Last = @GrossPremiumLastYear,
        Commission_Current = @CommissionThisYear,
        Commission_Last = @CommissionLastYear,
        Fees_Current = @FeesThisYear,
        Fees_Last = @FeesLastYear,
    TransDetail.TransDetail_id,
    TransDetail.Accounting_Date,
    Insurance_File.insurance_ref,
    TransDetail.Amount
FROM
    Party Party,
    Party_Type,
    Insurance_File Insurance_File,
    --Transaction_Export_Folder Transaction_Export_Folder,
    Account Account,
    TransDetail TransDetail,
    Ledger Ledger,
    LedgerType LedgerType,
    Document Document,
    DocumentType DocumentType

WHERE
    Party.party_id  = Account.account_key
    AND Account.account_id = TransDetail.account_id
    AND Account.ledger_id = Ledger.ledger_id
    -- sj 30/07/2002 - Start
    AND Insurance_File.insurance_file_cnt = Document.insurance_file_cnt
    --AND Insurance_File.insurance_file_cnt = Transaction_Export_Folder.insurance_file_cnt
    --AND Transaction_Export_Folder.transaction_type_id = DocumentType.documenttype_id
    --AND Document.document_ref = Transaction_Export_Folder.document_ref
    -- sj 30/07/2002 - End    
AND party.party_cnt = @PartyCnt
IF @@rowcount = 0
BEGIN
SELECT  DISTINCT
    Party.party_cnt party_cnt,
    Party.shortname Client_Code,
    Party.name Client_Name,
    address1 = @address1,
    address2 = @address2,
    address3 = @address3,
    address4 = @address4,
    postal_code = @postal_code,
    country = "UK",
    Home_Phone_Number = @Home,
    Work_Phone_Number = @Work,
    Mobile_Phone_Number = @Mobile,
    EMail_Address = @Email,
    Website_Address = @Web,
        Balance_Outstanding = @BalanceOutstanding,
        GrossPremium_Current = @GrossPremiumThisYear,
        GrossPremium_Last = @GrossPremiumLastYear,
        Commission_Current = @CommissionThisYear,
        Commission_Last = @CommissionLastYear,
        Fees_Current = @FeesThisYear,
        Fees_Last = @FeesLastYear,
    TransDetail_id = Null,
    Accounting_Date = Null,
    insurance_ref = Null,
    Amount = Null
FROM
    Party Party
WHERE
    party.party_cnt = @PartyCnt
END
GO


