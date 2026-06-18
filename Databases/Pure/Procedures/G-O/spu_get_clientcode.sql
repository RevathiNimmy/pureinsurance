SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_get_ClientCode'
GO

--Start( Sriram )Tech Spec - WR8 - Navigator DME Link.doc 

CREATE Procedure spu_get_ClientCode
	@party_cnt integer
As 
BEGIN  
	select 	shortname 
	from 	party 
	where 	party_cnt = @party_cnt
END

--End( Sriram) Tech Spec - WR8 - Navigator DME Link.doc 

GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
