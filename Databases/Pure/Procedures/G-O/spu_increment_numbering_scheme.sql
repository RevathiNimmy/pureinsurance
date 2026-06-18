SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_increment_numbering_scheme'
GO


CREATE PROCEDURE spu_increment_numbering_scheme
    @numbering_scheme_id INT
AS


UPDATE numbering_scheme
--(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.6)
SET next_number = next_number + step, 
    date_last_generated = GETDATE() 
--(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.6)
WHERE   numbering_scheme_id = @numbering_scheme_id
AND step <> 0

UPDATE numbering_scheme
--(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.6)
SET next_number = next_number + 1, 
date_last_generated = GETDATE() 
--(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.6)
WHERE   numbering_scheme_id = @numbering_scheme_id
AND step = 0

GO


