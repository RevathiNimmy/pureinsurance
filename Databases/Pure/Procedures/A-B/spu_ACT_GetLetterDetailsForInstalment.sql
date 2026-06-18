
EXECUTE DDLDropProcedure 'spu_ACT_GetLetterDetailsForInstalment'
GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_ACT_GetLetterDetailsForInstalment
    @cashlistitem_id int

AS BEGIN

    SELECT p.party_cnt,p.shortname, d.document_ref, d.insurance_file_cnt 
    FROM
        party p ,document d, cashlistitem_instalments clii, pfInstalments pfi, TransDetail t, account a
    WHERE
        clii.cashlistitem_id = @cashlistitem_id
    AND clii.pfInstalments_id = pfi.pfInstalments_id
    AND pfi.PFTransaction_id = t.TransDetail_id
    AND t.document_id = d.document_id
    AND a.account_key = P.party_cnt
    AND t.account_id = a.account_id 

END

GO
