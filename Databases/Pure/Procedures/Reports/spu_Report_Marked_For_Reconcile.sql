SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Marked_For_Reconcile'
GO
/* DC200502 use contact_name instead of using party */
/* MKW180203 1.6.9 to 1.8.6 Catchup PN 1517 (F51856 + F64893) */
/* MKW160602 Changed to use @bank_id instead of @bank_code */
/* JMK250903 PN6853 - include instalment credit transactions as payments */
/* PN 25738 In case of Receipts as batch total don't include the amt of */
/* entry which is already reconciled */
-- 1= JN Journal
--10= RVJ Ad hoc Reversing Journal
--12= RCJ Ad hoc Recurring Journal
--22= SRP Receipt
--23= SPY Payment
--39= ICA Instalment Cash
--43= INC Instalment NB Credit
--45= IEC Instalment Endorsement Credit
--47= IRC Instalment Renewal Credit

CREATE PROCEDURE spu_Report_Marked_For_Reconcile
    @bank_id integer
AS

SELECT DISTINCT

    ISNULL(B.bank_name, '') Bank,
    ISNULL(B.bank_address1, '') Address1,
    ISNULL(B.bank_address2, '') Address2,
    ISNULL(B.bank_address3, '') Address3,
    ISNULL(B.bank_address4, '') Address4,
    ISNULL(B.bank_postal_code, '') Postal_Code,
    ISNULL(BA.bank_account_no, '') Bank_Account_No,
    ISNULL(BA.bank_account_name, '') Bank_Account_Name,
    D.document_ref Document_Ref,
    D.document_id Document_ID,
    D.document_date Policy_Doc_Date,
    ISNULL(ACli.contact_name, '') Account_Name,
    ISNULL(ACli.short_code, '') Account_Code,
    ISNULL(CLI.media_ref, '') Media_Ref,
    ISNULL
    (
        (
        SELECT sum(amount)
        FROM Transdetail
        WHERE document_id = D.document_id
        AND account_id = A.account_id
        AND D.documenttype_id IN (23,1,10,12,20)
        --AND spare LIKE '%RECONCILED%'
        HAVING  (sum(amount) < 0 AND D.documenttype_id in (1,10,12,20))
            OR D.documenttype_id = 23
        )
    , 0) Payment,
    ISNULL
    (
        (
        SELECT sum(amount)
        FROM Transdetail
        WHERE document_id = D.document_id
        AND account_id = A.account_id
        AND D.documenttype_id IN (22,1,10,12, 39, 43, 45, 47,20)
		--AND transdetail_id=TM.transdetail_id
        --AND spare LIKE '%RECONCILED%'
        HAVING  (sum(amount) > 0 AND D.documenttype_id in (1,10,12,20))
        OR D.documenttype_id IN (22, 39, 43, 45, 47)
        )
    , 0) Receipt,
    C.description Currency,
    C.iso_code Currency_Code,
    MT.description Payment_Type

FROM BankAccount        BA
JOIN Bank           B
ON B.Bank_id=BA.Bank_id
JOIN Account            A
ON A.account_id = BA.account_id
JOIN TransDetail        TD
ON TD.account_id = A.account_id
JOIN Document           D
ON D.document_id = TD.document_id
JOIN DocumentType       DT
ON DT.DocumentType_id = D.DocumentType_id
JOIN TransMatch         TM
ON TM.transdetail_id = TD.transdetail_id
JOIN Currency           C
ON C.Currency_id = TD.Currency_id
JOIN TransDetail        TDCli
ON TDCli.Transdetail_id =
    (
        Select  T2.transdetail_id
        FROM    TransDetail T2
        WHERE   transdetail_id =
        (   SELECT  MIN(transdetail_id)
            FROM    transdetail T3,
                Account     A3
            WHERE   T3.document_id = D.Document_id
            AND A3.account_id = T3.account_id
        )
    )
JOIN Account            ACli
ON ACli.Account_id = TDCli.Account_id
LEFT OUTER JOIN CashListItem    CLI
ON CLI.transdetail_id = TDCli.transdetail_id
LEFT OUTER JOIN MediaType MT
ON MT.mediatype_id = CLI.mediatype_id

WHERE
    BA.BankAccount_Id = @Bank_id
AND ((TD.spare LIKE '%RECONCILED%' AND TD.bank_reconciliation_date >= DATEADD(ss, -10, SYSDATETIME())) OR TD.spare NOT LIKE '%RECONCILED%')
AND TM.allocationdetail_id IS NULL
ORDER BY
    Currency_Code, Document_Ref

GO

