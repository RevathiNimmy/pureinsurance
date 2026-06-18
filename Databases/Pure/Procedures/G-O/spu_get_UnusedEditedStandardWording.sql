SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_UnusedEditedStandardWording'
GO

CREATE PROCEDURE spu_get_UnusedEditedStandardWording
    @created_by_id int
AS
BEGIN

    -- *****************************************************************************
    -- * 354:      Standard Wording Control Enchancements (spu_get_UnusedEditedStandardWording)
    -- * Author:   Ramakant Singh
    -- * Date:     06/05/2005
    -- *****************************************************************************
    
    DECLARE @dmsw varchar(255) -- data model standard wording object name
    DECLARE @sSQL varchar(1000) -- to prepare sql statement

    --select all of the edited document_template (document_type_id=7)
    EXEC DDLDropTable '#temp_document_template'

    SELECT DISTINCT DT.document_template_id
        INTO #temp_document_template 
        FROM document_template DT
        LEFT JOIN policy_standard_wording PSW ON PSW.document_template_id = DT.document_template_id
        WHERE DT.copy_of_original=1
            AND DT.document_type_id=7
            AND DT.created_by_id=@created_by_id 
            AND PSW.document_template_id IS NULL
			    
    IF @@ROWCOUNT>0 BEGIN
	    -- Declare a cursor to get all risk type data models
	    DECLARE DMCursor CURSOR FAST_FORWARD FOR 
	        SELECT  LTRIM(RTRIM(code))  + '_standard_wording'
	            FROM gis_data_model
	        
	    OPEN DMCursor
	    FETCH NEXT FROM DMCursor INTO @dmsw
	        
	    WHILE @@FETCH_STATUS = 0
	    BEGIN
	        IF EXISTS (SELECT name FROM sysobjects WHERE name = @dmsw)
	        BEGIN
	            --Delete the used document_template from temp_document_template
	            SELECT @sSQL = 'DELETE FROM #temp_document_template 
	                WHERE EXISTS(SELECT * FROM ' + @dmsw + ' sw 
	                                WHERE sw.document_template_id=#temp_document_template.document_template_id)'
	            EXECUTE(@sSQL)
	        END
	        FETCH NEXT FROM DMCursor INTO @dmsw
	    END
	    
	    CLOSE DMCursor
	    DEALLOCATE DMCursor
	    
	    --select the unused document_template   
	    SELECT document_template_id from #temp_document_template
 	END
   
    EXEC DDLDropTable '#temp_document_template'

END

GO