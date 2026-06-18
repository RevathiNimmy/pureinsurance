--SMJB CQ2189 - 13/08/2003: Change of logic for Sirius 1.9 SR35 means that Credit Control items are not deleted
--Therefore we now have to just set the is_deleted flag of the CCI to 0
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_ReAdd_Credit_Control_Item'
GO
CREATE PROCEDURE spu_ACT_ReAdd_Credit_Control_Item 
    @allocationdetail_id INT,
    @transdetail_id INT = 0 
AS

DECLARE @credit_control_item_id INT

SELECT @credit_control_item_id=0


	IF @transdetail_id > 0
	        SELECT  @credit_control_item_id = CCI.credit_control_item_id
			FROM credit_control_item CCI
				INNER JOIN document DOC
			ON CCI.insurance_file_cnt =  DOC.insurance_file_cnt
				INNER JOIN transdetail TRD
			ON TRD.document_id = DOC.document_id
				INNER JOIN allocationdetail ALD
			ON ALD.transdetail_id = TRD.transdetail_id
				INNER JOIN PFInstalments PFI
			ON PFI.pfinstalments_id  = CCI.PFInstalments_id
		WHERE ALD.allocationdetail_id = @allocationdetail_id AND PFI.PFTransaction_id = @transdetail_id
	ELSE
		SELECT @credit_control_item_id = CCI.credit_control_item_id
			FROM credit_control_item CCI
				INNER JOIN document DOC
			ON CCI.insurance_file_cnt =  DOC.insurance_file_cnt
				INNER JOIN transdetail TRD
			ON TRD.document_id = DOC.document_id
				INNER JOIN allocationdetail ALD
			ON ALD.transdetail_id = TRD.transdetail_id
		WHERE ALD.allocationdetail_id = @allocationdetail_id

    UPDATE credit_control_item WITH (ROWLOCK) SET is_deleted=0 WHERE credit_control_item_id = @credit_control_item_id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
