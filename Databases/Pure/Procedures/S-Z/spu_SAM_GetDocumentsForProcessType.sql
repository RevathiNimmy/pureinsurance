SET QUOTED_IDENTIFIER OFF

GO

SET ANSI_NULLS ON

GO

EXECUTE DDLDROPPROCEDURE
  'spu_SAM_GetDocumentsForProcessType'

GO

CREATE PROCEDURE spu_SAM_getdocumentsforprocesstype @nProcessType      INT,
                                                    @nInsuranceFileKey INT,
                                                    @bIsQuotation      TINYINT = 0
AS
  BEGIN
      DECLARE @nProductId INT = 0      
      DECLARE @nProcessTypeDocId INT = 0
      DECLARE @nPartyKey INT = 0      
      DECLARE @nInsuranceFolderKey INT = 0
      	
      SELECT @nProductId = product_id,
				@nPartyKey = insured_cnt,
				@nInsuranceFolderKey = insurance_folder_cnt
      FROM   Insurance_File
      WHERE  insurance_file_cnt = @nInsuranceFileKey

      SET @nProcessTypeDocId = 1
      IF @bIsQuotation = 1
        BEGIN

            SELECT DT.code,
                   DT.description,
                   @nPartyKey AS PartyKey,
                   @nInsuranceFolderKey AS InsuranceFolderKey
            FROM   Document_Template DT
                   JOIN PMB_Doc_Link PDL
                     ON dt.document_template_id = pdl.Document_Template_Id                   
            WHERE  PDL.product_id = @nProductId
                   AND pdl.Process_Type_Id = @nProcessType
                   AND pdl.process_types_docs_id = @nProcessTypeDocId
        END
      ELSE
        BEGIN

            SELECT DT.code,
                   DT.description,
                   @nPartyKey AS PartyKey,
                   @nInsuranceFolderKey AS InsuranceFolderKey
            FROM   Document_Template DT
                   JOIN PMB_Doc_Link PDL
                     ON dt.document_template_id = pdl.Document_Template_Id                   
            WHERE  PDL.product_id = @nProductId
                   AND pdl.Process_Type_Id = @nProcessType
                   AND pdl.process_types_docs_id <> @nProcessTypeDocId
        END
  END

GO 
