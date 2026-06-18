SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PMB_Doc_Link_Add'
GO

CREATE PROCEDURE spu_PMB_Doc_Link_Add
    @GIS_Scheme_ID integer, 
    @Process_Type_ID integer,
    @Agent_ID integer = NULL, 
    @Document_Type_ID integer,
    @Document_Template_ID integer,
    @Spool_Document integer,
    @Scheme_Ver integer,
    @Auto_Archive_Document tinyint

AS
BEGIN
	
	INSERT INTO PMB_Doc_Link (
        GIS_Scheme_Id, 
        Process_Type_Id, 
        Document_Type_Id, 
        Document_Template_Id, 
        Agent_Cnt,
        Spool_Document, 
        Auto_Archive_Document)
    VALUES (
	    @GIS_Scheme_ID,
	    @Process_Type_ID,
	    @Document_Type_ID,
	    @Document_Template_ID,
	    @Agent_ID,
	    @Spool_Document,
	    @Auto_Archive_Document)

END

GO