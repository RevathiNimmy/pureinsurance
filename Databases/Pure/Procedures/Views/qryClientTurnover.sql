SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropView 'qryClientTurnover'
GO

CREATE VIEW qryClientTurnover AS 

SELECT
    party_cnt 'ClientID',
    (
        SELECT 
            ISNULL(SUM(td.outstanding_account_amount), 0)
        FROM transdetail td
        JOIN account a 
            ON a.account_id = td.account_id
        WHERE a.account_key = ppp.party_cnt
        AND td.postingstatus_id = 3
    ) 'AccountBalance',
    (
        SELECT 
            ISNULL(SUM(T.account_amount), 0) 
        FROM Transdetail T
        JOIN Account A ON A.account_id = T.account_id
        JOIN Document D ON D.document_id = T.document_id
        JOIN Period P ON P.period_id = T.period_id
        WHERE D.documenttype_id in (4,5,15,16,17,18,30,35,36)
        AND A.account_key = ppp.party_cnt
        AND P.year_name = 
            (
                SELECT 
                    year_name 
                FROM period
                WHERE period_id = 
                    (
                        SELECT 
                            MAX(period_id) 
                        FROM period
                        WHERE company_id = 1
                        AND period_end_date <
                            (
                                SELECT 
                                    MIN(period_end_date) 
                                FROM period
                                WHERE year_name = 
                                    (
                                        SELECT 
                                            year_name 
                                        FROM period 
                                        WHERE period_id = 
                                            (
                                                SELECT MIN(period_id) 
                                                FROM period
                                                WHERE period_end_complete = 0
                                                AND company_id = 1
                                            )
                                    )
                                AND company_id = 1
                            )
                    )

            )
    ) 'LastYearTurnover',
    (
        /*Client Year To Date*/
        SELECT 
            CASE
                WHEN EXISTS
                    (
                        SELECT
                            NULL
                        FROM system_options
                        WHERE option_number = 5007 /*include_tax_on_ytd_turnover*/
                        AND branch_id = 1
                        AND value = 1
                    ) 
                THEN
                    ISNULL(SUM(td.account_amount), 0)     
                ELSE
                    ISNULL(SUM(td.account_amount) - SUM(td.ref_amount), 0)
            END        
        FROM transdetail td
        JOIN account a 
            ON a.account_id = td.account_id
        JOIN ledger l
            ON l.ledger_id = a.ledger_id
            AND l.ledger_short_name = 'SA'
        JOIN document d 
            ON d.document_id = td.document_id
        JOIN documenttype dt 
            ON dt.documenttype_id = d.documenttype_id
            AND dt.code IN ('SND', 'SNC', 'SRD', 'SRC', 'SED', 'SEC', 'SHD', 'SHC', 'TRD', 'TRC', 'FEE')
        JOIN period p 
            ON p.period_id = td.period_id
        WHERE p.year_name = 
            (
                SELECT 
                    year_name 
                FROM period 
                WHERE period_id = 
                    (
                        SELECT MIN(period_id) 
                        FROM period
                        WHERE period_end_complete = 0
                        AND company_id = 1
                    )
            )
        AND a.account_key = ppp.party_cnt
    ) 'YearToDateTurnover'
FROM party ppp
JOIN party_type pt
    ON pt.party_type_id = ppp.party_type_id
WHERE pt.code IN ('PC', 'CC', 'GC')


GO