SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_Report_Reconciled_Items
GO

CREATE PROCEDURE spu_Report_Reconciled_Items
 @bank_code VARCHAR(20)
AS
BEGIN 
     
    SELECT DISTINCT 
        ISNULL(B.bank_name, '')  Bank,  
        ISNULL(B.bank_address1, '')  Address1,  
        ISNULL(B.bank_address2, '')  Address2,  
        ISNULL(B.bank_address3, '')  Address3,  
        ISNULL(B.bank_address4, '')  Address4,  
        ISNULL(B.bank_postal_code, '')  Postal_Code,  
        ISNULL(BA.bank_account_no, '')  Bank_Account_No,  
        ISNULL(BA.bank_account_name, '')  Bank_Account_Name,  
        D.document_ref    Document_Ref,  
        D.document_id    Document_ID,  
        D.document_date   Policy_Doc_Date,  
        ISNULL(P.resolved_name, '')  Account_Name,  
        ISNULL(ACli.short_code, '')  Account_Code,  
        ISNULL(CQ.media_ref, '')   Media_Ref,  
        ISNULL  
            (  
             (  
              SELECT amount  
              FROM Transdetail  
              WHERE document_id = D.document_id  
                  AND account_id = A.account_id  
                  AND D.documenttype_id = 23  
             )  
        , 0)     Payment,  
        ISNULL  
            (  
             (  
              SELECT amount  
              FROM Transdetail  
              WHERE document_id = D.document_id  
                  AND account_id = A.account_id
                  AND D.documenttype_id = 22
             )  
        , 0)     Receipt,  
        C.description    Currency,  
        C.iso_code    Currency_Code,  
        MT.description    Payment_Type  
    FROM 
        Account  A
        INNER JOIN BankAccount BA
            ON A.account_id = BA.account_id
        INNER JOIN Bank  B
            ON B.Bank_id = BA.bank_id  
        INNER JOIN TransDetail TD
            ON TD.account_id = A.account_id    
            AND  TD.spare <> 'RECONCILED'  
        LEFT OUTER JOIN Cheque  CQ    
            ON CQ.transdetail_id = TD.transdetail_id    
        INNER JOIN Document D
            ON D.document_id = TD.document_id  
        INNER JOIN DocumentType DT
            ON DT.documenttype_id = D.documenttype_id
            AND  DT.documenttype_id IN (22,23)   
        INNER JOIN TransMatch TM
            ON TM.transdetail_id = TD.transdetail_id
            AND TM.allocationdetail_id IS NULL  
        INNER JOIN Currency C
            ON C.currency_id = TD.currency_id 
        INNER JOIN TransDetail TDCli
            ON TDCli.transdetail_id =(  
                                      SELECT T2.transdetail_id  
                                      FROM TransDetail  T2  
                                      WHERE Transdetail_id =( 
                                                             SELECT MIN(transdetail_id)  
                                                             FROM 
                                                                 Transdetail T3
                                                                 INNER JOIN Account  A3
                                                                     ON A3.account_id = T3.account_id          
                                                             WHERE T3.document_id = D.document_id  
                                                            )  
                                     )
        INNER JOIN Account  ACli
            ON ACli.Account_id = TDCli.Account_id
        INNER JOIN Party  P
            ON P.shortname = ACli.Short_Code  
        INNER JOIN CashListItem CLI 
            ON CLI.transdetail_id = TDCli.transdetail_id  
        INNER JOIN MediaType MT
            ON MT.mediatype_id = CLI.mediatype_Id
        WHERE 
            BA.Code = @bank_code 
    ORDER BY  
        Currency_Code, 
        Account_Code  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

