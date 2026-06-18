SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_process_types_docs_ForFunctionalArea'
GO

CREATE PROCEDURE spu_get_process_types_docs_ForFunctionalArea
    @FunctionalArea INT =0
AS
    /*
        Functional Area    
        PMDocLinkpolicy                     = 1
        PMDocLinkPolicyOpenMaintainclaims   = 2 For future expansion
        PMDocLinkAccounts                   = 3
        PMDocLinkPayclaims                  = 4
    */
    IF @FunctionalArea = 0 
    
        SELECT process_types_docs_id,Description
        FROM process_types_docs
    ELSE IF @FunctionalArea = 1 
        SELECT process_types_docs_id,Description
        FROM process_types_docs
        WHERE Code Not Like 'CLM'
    ELSE IF @FunctionalArea >1 
        SELECT process_types_docs_id,Description
        FROM process_types_docs
        WHERE Code Like 'CLM'   
    
GO