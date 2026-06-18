SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Bank_Reconciliation'
GO


CREATE PROCEDURE spu_Report_Bank_Reconciliation
    @bank_code varchar(20)
AS


SELECT  DISTINCT
    ISNULL(Bank.bank_name, '')      BankName,
    ISNULL(Bank.bank_address1, '')      BankAddress1,
    ISNULL(Bank.bank_address2, '')      BankAddress2,
    ISNULL(Bank.bank_address3, '')      BankAddress3,
    ISNULL(Bank.bank_address4, '')      BankAddress4,
    ISNULL(Bank.bank_postal_code, '')   BankPostalCode,
    D.document_date             Document_Date,
    ISNULL(I.media_ref, '')         Media_Ref,
    I.amount                CashList_Amount,
    ISNULL(P.name, '')          Insurer,
    TM.allocationdetail_id          Allocation_ID,
    DCli.document_ref           Document_Ref,
    DCli.document_id            Document_ID,
    DCli.document_date          Policy_Doc_Date,
    DTCli.description           Document_Type,

    ISNULL(ACli.account_name, '')       Client,
    ISNULL(ACli.short_code, '')     Client_Code,
    ISNULL(TCli.insurance_ref, '')      Policy_Ref,
    TCli.amount +
        ISNULL
        (
            (
            SELECT  SUM(amount)
            FROM    Transdetail
            WHERE   document_id = DCli.document_id
            AND account_id = A.account_id
            AND transdetail_id <> TCli.transdetail_id
            )
        , 0)            Premium,
    TCli.amount         Premium,
    ISNULL(
        (
        SELECT  SUM(amount)
        FROM    Transdetail
        WHERE   document_id = DCli.document_id
        AND spare like  'COMM%'
        AND account_id = A.account_id
        ), 0)           Commission,
    TCli.ref_amount         IPT

FROM    CashList      C,
    CashListItem  I,
    TransDetail   T,
    Document      D,
    Account       A,
    Party                   P,
    AllocationDetail  AL,
    Account       ACli,
    TransDetail   TCli,
    Document      DCli,
    DocumentType  DTCli,
    --sj 31/07/2002 - start
    --Transaction_Export_Folder       TEF,
    --sj 31/07/2002 - end
    Insurance_File              INF,
    TransDetail   T2,
    TransMatch    TM,
    Bank      Bank,
    BankAccount   BA
WHERE   Bank.code = @bank_code
AND     Bank.bank_id = BA.bank_id
AND BA.account_id = T2.account_id
AND I.cashlist_id = C.cashlist_id
AND T.transdetail_id = I.transdetail_id
AND TM.transdetail_id = T.transdetail_id
AND TM.allocationdetail_id IS NULL
AND D.document_id = T.document_id
AND A.account_id = T.account_id
AND P.party_id = A.account_key
AND AL.cashlistitem_id = I.cashlistitem_id
--sj 31/07/2002 - start
AND INF.insurance_file_cnt = D.insurance_file_cnt
--AND TEF.document_ref = DCli.document_ref
--AND TEF.accounts_export_status = 'c'
--AND INF.insurance_file_cnt = TEF.insurance_file_cnt
--sj 31/07/2002 - end
AND TCli.transdetail_id = AL.transdetail_id
AND TCli.amount <> 0
AND DCli.document_id = TCli.document_id
AND DTCli.documenttype_id = DCli.documenttype_id

ORDER BY
    DCli.document_date
GO


