SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_numbering_scheme_saa'
GO

CREATE PROCEDURE spe_numbering_scheme_saa

AS

SELECT
    numbering_scheme_id,
    caption_id,
    code,
    description,
    is_deleted,
    effective_date,
    numbering_scheme_type_id,
    numbering_scheme,
    is_generated,
    mask_code,
    fixed_code,
    next_number,
    highest_number,
    step,
--(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.2)
    is_reuse_abandoned,
    is_reset_daily
--(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.2)
 FROM numbering_scheme

ORDER BY numbering_scheme_id ASC

GO

