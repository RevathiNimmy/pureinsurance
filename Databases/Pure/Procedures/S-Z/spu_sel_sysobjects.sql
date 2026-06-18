SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_sel_sysobjects'
GO


CREATE PROCEDURE spu_sel_sysobjects
    @Search varchar(30)
AS


begin
 select * from sysobjects where name like '%' + @Search + '%' and type = 'U'
end
GO


