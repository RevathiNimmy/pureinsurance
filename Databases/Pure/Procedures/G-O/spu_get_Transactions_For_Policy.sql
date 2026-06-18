SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_Transactions_For_Policy'
GO


CREATE PROCEDURE spu_get_Transactions_For_Policy
    @insurance_ref varchar(30)
AS

/********************************************************************************************************/
/* Revision         Date            Who         Description of Modification                             */
/* --------         ----            ---         ---------------------------                             */
/* 1.0              11/10/2001      JMK         Created                                                 */
/* 1.1              05/11/2001      JMK         rsa_transfer!                                           */
/********************************************************************************************************/
-- for testing
/*
declare @insurance_ref varchar(30)
select @insurance_ref = 'HOM/   /POL/00294'
*/
SELECT d.company_id,
    a.short_code,
    d.document_ref,
    d.document_date,
    t.amount,
    t.outstanding_amount outstanding ,
    (SELECT dt.description
    FROM documenttype(NOLOCK) dt
    WHERE dt.documenttype_id = d.documenttype_id) document_type,
    source.description  as BranchDescription,
    source.code as BranchCode 
FROM Account(NOLOCK) a
JOIN Ledger(NOLOCK) l       ON a.ledger_id = l.ledger_id
JOIN transdetail(NOLOCK) t  ON a.account_id = t.account_id
JOIN  Document(NOLOCK) d    ON t.document_id = d.document_id
JOIN Source(NOLOCK)         ON 
	d.company_id = source.source_id
WHERE t.insurance_ref = @insurance_ref
AND (l.ledger_name = 'client'
    OR t.spare = 'GROSS')
AND (
    (SELECT max(tef.accounts_export_status)
    FROM transaction_export_folder(NOLOCK) tef
    WHERE tef.document_ref = d.document_ref) = 'c'
    OR
    (SELECT dt.from_sirius
    FROM documenttype(NOLOCK) dt
    WHERE d.documenttype_id = dt.documenttype_id) = 0

	or		(D.document_ref Like'IND%'    OR   D.document_ref Like'IED%' OR D.document_ref ='IRD%' OR D.document_ref Like 'IID%')
    )
GO


