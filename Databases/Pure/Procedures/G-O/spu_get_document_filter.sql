SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_document_filter'
GO
--********************************************************************************************************************************
-- RKS  27/04/2005          selecting the distinct document_filter from Document_Template table
-- RKS  30/05/2005          retricted the blank document_filter
--*********************************************************************************************************************************
CREATE PROCEDURE spu_get_document_filter

AS

SELECT distinct 
    isnull(document_filter,'')
FROM Document_Template where len(document_filter)>0
GO
