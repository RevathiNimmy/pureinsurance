
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PMB_Doc_Link_sel'
GO

CREATE PROCEDURE spu_PMB_Doc_Link_sel
AS
	SELECT 
		l.PMB_Doc_Link_Id,
	 	gs.scheme_desc,
	    pt.Description,
	    dt.Description,
	    t.Description,
	    0,	/* to set default is-deleted flag to  off */
	    a.shortname,
	 	l.spool_document,
	    t.document_template_id,
	 	gs.scheme_ver, 
	 	l.Auto_Archive_Document,
	    gs.gis_scheme_id, 
        pt.process_type_id, 
        dt.document_type_id, 
        a.party_cnt 
	
	FROM PMB_DOC_Link l
    JOIN gis_scheme gs ON l.gis_scheme_id = gs.gis_scheme_id 
    JOIN process_type pt ON l.process_type_id = pt.process_type_id
    JOIN document_type dt ON l.document_type_id = dt.document_type_id
    JOIN document_template t ON l.document_template_id = t.document_template_id
    LEFT JOIN party a ON l.agent_cnt = a.party_cnt

    ORDER BY 
        gs.scheme_desc, 
        pt.Description, 
        a.shortname, 
        dt.Description, 
        t.Description

GO


