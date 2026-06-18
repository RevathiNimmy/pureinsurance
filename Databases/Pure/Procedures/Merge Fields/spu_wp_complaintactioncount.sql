SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_complaintactioncount'
GO

CREATE PROCEDURE spu_wp_complaintactioncount
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT = NULL,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

    SELECT 	Count(*) AS how_many
    FROM 	fsa_complaint_folder folder
    JOIN    fsa_complaint_file cfile ON folder.fsa_complaint_folder_cnt = cfile.fsa_complaint_folder_cnt
    WHERE 	folder.reference = @DocumentRef
   
go 

