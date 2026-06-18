SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Document_Type_delete'
GO

CREATE PROCEDURE spu_Document_Type_delete
    @document_type_id smallint,
    @ok tinyint output
 
AS

select @ok = 1

select @ok = 0
where   @document_type_id in (select distinct document_type_id from document_template)
 
GO

