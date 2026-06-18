
 

--(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.5)

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_reset_numbering_scheme'
GO

CREATE  PROCEDURE spu_reset_numbering_scheme
    @numbering_scheme_id INT
AS

UPDATE 
	numbering_scheme
SET 
	next_number = 1
	 
WHERE   
  numbering_scheme_id = @numbering_scheme_id



GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO       
--(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.5)