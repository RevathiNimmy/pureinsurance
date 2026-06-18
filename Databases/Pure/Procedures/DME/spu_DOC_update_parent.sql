SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_DOC_update_parent'
GO


CREATE PROCEDURE spu_DOC_update_parent
                             @v_iInsuranceFileCnt INTEGER
AS
BEGIN

    DECLARE @iParentFolderNum INTEGER
    DECLARE @sChildFolderName VARCHAR(70)
    
    -- Is Documaster installed
    -- get out of here if doc_folder table does not exist
    IF  NOT EXISTS (SELECT o.name from sysobjects o
                    WHERE  o.name = 'doc_folder'
                      AND  o.type = 'U')
        RETURN 0       -- get out of here


    -- identify the new parent and the child which is to be assigned to it
    SELECT  @iParentFolderNum = df.folder_num, 
            @sChildFolderName = ifi.insurance_ref
    FROM    doc_folder df,
            insurance_file ifi, 
            party p 
    WHERE   df.folder_name = p.shortname 
      AND   p.party_cnt = ifi.insured_cnt
      AND   ifi.insurance_file_cnt = @v_iInsuranceFileCnt
      AND   df.folder_level = 1 
    
    -- reassign the child to the new parent
    UPDATE  doc_folder 
    SET     doc_folder.parent_num = @iParentFolderNum
    FROM    doc_folder df,
            insurance_file ifi
    WHERE   RTRIM(df.folder_name) = RTRIM(@sChildFolderName)
      AND   df.folder_level = 2
        
END
GO