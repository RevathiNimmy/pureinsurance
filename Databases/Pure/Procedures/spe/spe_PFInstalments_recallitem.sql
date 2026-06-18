SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFInstalments_recallitem'
GO
CREATE PROCEDURE spe_PFInstalments_recallitem
    @pfinstalments_id INT,
	@pfinstalmentsreverse_id INT = NULL
AS

IF @pfinstalmentsreverse_id = 0
BEGIN
      SET @pfinstalmentsreverse_id = NULL
END

--Update existing transdetail with pfinstalments_id
DECLARE @ExistingTransaction INT
SELECT 
      @ExistingTransaction = PFTransaction_id
FROM
      PFInstalments 
WHERE
      pfinstalments_id=@pfinstalments_id

UPDATE
    TransDetail
SET
    pfinstalments_id=@pfinstalments_id
WHERE
      transdetail_id=@ExistingTransaction


--Update Instalment with new transaction
UPDATE
    PFInstalments
SET
    PFTransaction_id = @pfinstalmentsreverse_id,
    PostedDate = NULL
WHERE
    pfinstalments_id=@pfinstalments_id
AND PostedDate IS NOT NULL
AND PFTransaction_id IS NOT NULL

-- Update the PFInstalment_id to the transdetail table
UPDATE
    TransDetail
SET
    pfinstalments_id=@pfinstalments_id
WHERE
      transdetail_id=@pfinstalmentsreverse_id
