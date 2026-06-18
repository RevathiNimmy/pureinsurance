SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_Marked_Details'
GO

CREATE PROCEDURE spu_ACT_Select_Marked_Details
    @account_id int,
    @Company_id int = null,
    @Currency_id int = null,
    @DocTypeGroup int = null,
    @DocumentTypeID int = null,
    @Period_Id int = null,
    @DateFrom datetime = null,
    @DateTo datetime = null,
    @OperatorName varchar(50)=null,
    @Department varchar(50)=null,
    @Multitreeaccounting smallint=0
AS
SELECT
    d.documenttype_id,
    d.document_ref,
    d.document_date,
    t.document_sequence,
    t.transdetail_id,
    t.currency_id,
    t.amount,                                -- RAW 01/04/2003 : ISS2854 : replaced unrounded
    t.currency_amount,                       -- RAW 01/04/2003 : ISS2854 : replaced unrounded
    t.currency_base_xrate,
    (SELECT SUM(alloc_base_amount) FROM allocationdetail
        WHERE transdetail_id = t.transdetail_id
        AND allocationdetail_id IS NOT NULL
    ),
    (SELECT SUM(alloc_ccy_amount) FROM allocationdetail
        WHERE transdetail_id = t.transdetail_id
        AND allocationdetail_id IS NOT NULL
    ),
    m.base_match_amount,
    m.currency_match_amount,
    m.currency_match_xrate,
    m.transmatch_id,
    t.sub_branch_id, 
    t.base_amount_unrounded,            -- RAW 01/04/2003 : ISS2854 : added
    t.currency_amount_unrounded         -- RAW 01/04/2003 : ISS2854 : added

    FROM TransMatch M,
    Transdetail T,
    Document D,
    DocumentType DT,
    PMUser pmu 

    WHERE t.document_id = d.document_id
    AND t.transdetail_id = m.transdetail_id
    AND t.account_id = @account_id
    AND m.allocationdetail_id IS NULL

    AND t.operator_id = pmu.user_id
    AND dt.documenttype_id=d.documenttype_id
    AND ((t.currency_id=@currency_id) or (@currency_id is null))
    AND ((t.period_id=@period_id) or (@period_id is null))
    AND ((t.accounting_date >= @dateFrom) or (@DateFrom is null))
    AND ((t.accounting_date <= @DateTo) or (@DateTo is null))
    AND ((t.department like @Department) or (@Department is null))
    AND ((@Multitreeaccounting>0 AND t.company_id=@company_id) OR @Multitreeaccounting=0)	--PN4594 & PN22347
    AND ((dt.doctypegroup_id=@DocTypeGroup) or (@DocTypeGroup is null))
    AND ((pmu.username like @OperatorName) or (@OperatorName is null))
    AND ((d.documenttype_id=@documenttypeid) or (@documenttypeid is null))
GO

