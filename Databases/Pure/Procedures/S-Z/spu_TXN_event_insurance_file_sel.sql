SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_TXN_event_insurance_file_sel'
GO

CREATE PROCEDURE spu_TXN_event_insurance_file_sel
(
@document_ref varchar(20),
@source_id int
)
AS

SELECT
EIF.insurance_file_cnt
FROM
transaction_export_folder TEF
JOIN
event_log EL ON EL.event_cnt=TEF.event_log_id
JOIN
event_insurance_file EIF ON EIF.insurance_folder_cnt=EL.event_cnt
WHERE
TEF.document_ref=@document_ref
AND TEF.source_id=@source_id
AND TEF.accounts_export_status='c'

GO

