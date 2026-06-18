SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PMB_Doc_Link_upd'
GO

CREATE PROCEDURE spu_PMB_Doc_Link_upd
@Scheme_Desc varchar(255),
@Process_Type varchar(255),
@Document_Type varchar(255),
@Document_Template_ID integer,
@Agent_Desc varchar(255),
@Spool_Document integer,
@Auto_Archive_Document tinyint
AS
BEGIN
	DECLARE @PMB_Doc_Link_Id Int, @Ag_cnt int

	IF  (@Agent_Desc = '(none) ')
		SELECT @Ag_Cnt  = 0
	ELSE
		SELECT @Ag_cnt = ( SELECT party_cnt FROM party WHERE shortname = @Agent_Desc )
	
	Select @PMB_Doc_Link_Id = ISNULL(max(PMB_Doc_Link_Id),0) + 1 from PMB_Doc_Link
	
	UPDATE PMB_Doc_Link 
	
	SET 
	    GIS_Scheme_Id = s.GIS_Scheme_Id,
	    Process_Type_Id = p.Process_Type_Id,
	    Document_Type_Id = dt.Document_Type_Id,
	    Document_Template_Id = t. Document_Template_Id,
	    Agent_Cnt = @Ag_cnt,
	    Spool_Document = @Spool_Document,
	    Auto_Archive_Document = @Auto_Archive_Document
	 FROM   GIS_Scheme s, 
	 	Process_Type p, 
	 	Document_Type dt, 
	 	Document_Template t
	 WHERE  rtrim(s.Scheme_Desc)  = rtrim(@Scheme_Desc)
	 AND	rtrim(p.Description) = rtrim(@Process_Type) 
	 AND	rtrim(dt.Description) = rtrim(@Document_Type)
	 AND 	t.document_template_id = @Document_Template_ID
END

GO