SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure spu_PMB_Doc_Temp_Get
GO

CREATE PROCEDURE spu_PMB_Doc_Temp_Get
    @GIS_Scheme_id INT,
    @Agent_Cnt INT,
    @Process_Type_Code VARCHAR(10)
AS

DECLARE @pmb_doc_link_id INT

SELECT @pmb_doc_link_id = NULL

/*Check for document set up for this agent.*/
IF ISNULL(@Agent_Cnt, 0) > 0
BEGIN
    SELECT 
        @pmb_doc_link_id = l.pmb_doc_link_id
    FROM pmb_doc_link l
    JOIN process_type p
        ON p.process_type_id = l.process_type_id
        AND p.is_deleted = 0
        AND RTRIM(p.code) = RTRIM(@Process_Type_Code)
    WHERE l.gis_scheme_id = @GIS_Scheme_id
    AND l.agent_cnt = @Agent_Cnt
END

/*If no document set up for agent, or no agent passed in, find document just set up for this scheme.*/
IF @pmb_doc_link_id IS NULL
BEGIN
    SELECT 
        @pmb_doc_link_id = l.pmb_doc_link_id
    FROM pmb_doc_link l
    JOIN process_type p
        ON p.process_type_id = l.process_type_id
        AND p.is_deleted = 0
        AND RTRIM(p.code) = RTRIM(@Process_Type_Code)
    WHERE l.gis_scheme_id = @GIS_Scheme_id
    AND l.agent_cnt IS NULL
END

/*Select the details of the document.*/
SELECT 
    l.document_template_id,
    l.document_type_id,
    l.spool_document,
    l.auto_archive_document
FROM pmb_doc_link l
WHERE l.pmb_doc_link_id = @pmb_doc_link_id

GO