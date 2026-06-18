 
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GetRITypeForTreaty'
GO

--Start( Sriram )Tech Spec - Calliden WR3.2.1.2 (1) - RI Model Line Priority.doc sec(5.2)
CREATE PROCEDURE spu_GetRITypeForTreaty
@treaty_id int
AS
SELECT reinsurance_type_id from Treaty where Treaty_id=@treaty_id
GO
--End( Sriram) Tech Spec - Calliden WR3.2.1.2 (1) - RI Model Line Priority.doc sec(5.2)



SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO