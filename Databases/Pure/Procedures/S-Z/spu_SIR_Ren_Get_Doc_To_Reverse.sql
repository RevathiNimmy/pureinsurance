SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_Ren_Get_Doc_To_Reverse'
GO


CREATE PROCEDURE spu_SIR_Ren_Get_Doc_To_Reverse
    @insurance_folder_cnt int
AS


SELECT document_id
FROM Renewal_Control R,
        Document D
WHERE R.insurance_folder_cnt = @insurance_folder_cnt
AND r.renewal_insurance_file_cnt = D.insurance_file_cnt

GO


