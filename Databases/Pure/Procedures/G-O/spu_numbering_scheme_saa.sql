SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_numbering_scheme_saa'
GO

CREATE PROCEDURE spu_numbering_scheme_saa
    @language_id int,
    @numbering_scheme_id int = NULL
AS


    SELECT
        a.numbering_scheme_id,
        a.caption_id,
        a.code,
        a.description,
        a.is_deleted,
        a.effective_date,
        a.numbering_scheme_type_id,
        a.numbering_scheme,
        a.is_generated,     
        a.mask_code,
        a.fixed_code,
        a.next_number,      
        a.highest_number,
        a.step,
        a.is_reuse_abandoned,
        c.caption as 'type_description',
	a.is_read_only,
	isnull(a.party_type_id,0) as party_type_id,
--(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.2)
 0 as is_deleted_temp  ,
 is_reset_daily,
 date_last_generated,
--(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.2)
		--Start - Renuka - (WPR87 Paralleling)
		is_reset_number
		--End - Renuka - (WPR87 Paralleling)
    FROM numbering_scheme a 
	INNER JOIN numbering_scheme_type b ON a.numbering_scheme_type_id = b.numbering_scheme_type_id 
	INNER JOIN PMCaption c ON b.caption_id = c.caption_id	
    WHERE  a.numbering_scheme_id =ISNULL(@numbering_scheme_id,a.numbering_scheme_id)
	--AND a.is_deleted = 0
    ORDER BY numbering_scheme_id ASC


GO
