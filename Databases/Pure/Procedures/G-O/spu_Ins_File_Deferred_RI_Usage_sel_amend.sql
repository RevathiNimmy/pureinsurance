-------------------------------------------------------------------------------
--  Author:  AMB
--  Date:    04 Sept 2003
--  Desc:    SFU 1.8.6 Deferred Reinsurance development
-------------------------------------------------------------------------------

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Ins_File_Deferred_RI_Usage_sel_amend'
GO

CREATE PROCEDURE spu_Ins_File_Deferred_RI_Usage_sel_amend
AS 

-- get the records that are marked as 'manual review'
SELECT
    ifd.ins_file_deferred_RI_usage_id
,	ifd.insurance_file_cnt
,   ins.insurance_ref AS 'Policy Number'
,   src.source_id
,   src.[description] AS 'Branch Desc'
,	ifd.deferred_RI_status_type_id
,   dri.[description] AS 'Def RI Status Desc'
,   ins.insured_cnt
,   par.shortname AS 'Party Shortname'
,   par.[name] AS 'Party Name'
,   pro.product_id
,   pro.[description] AS 'Product Desc'
,   ins.insurance_folder_cnt
,   ISNULL((SELECT top 1 insurance_file_status_id FROM insurance_file  
      WHERE insurance_folder_cnt = ins. insurance_folder_cnt
     AND insurance_file_type_id IN (2, 3, 5, 6, 8, 9) order by insurance_file_cnt desc), 0) AS InsStatus

FROM
	Insurance_File_Deferred_RI_Usage AS ifd
INNER JOIN
    Insurance_File AS ins ON ins.insurance_file_cnt = ifd.insurance_file_cnt
INNER JOIN
    Source AS src ON src.source_id = ins.source_id
INNER JOIN
    Deferred_RI_Status_Type AS dri ON dri.deferred_RI_status_type_id = ifd.deferred_RI_status_type_id
INNER JOIN
    Party AS par ON par.party_cnt = ins.insured_cnt
INNER JOIN
    Product AS pro ON pro.product_id = ins.product_id
ORDER BY
    src.source_id ASC,
    ins.insurance_ref ASC


GO
