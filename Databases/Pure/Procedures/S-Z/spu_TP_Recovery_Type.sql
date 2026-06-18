SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_TP_Recovery_Type'
GO


CREATE PROCEDURE spu_TP_Recovery_Type
AS


SELECT tn.Recovery_Type_id, cap.caption, tn.code FROM Recovery_Type tn, pmcaption cap WHERE tn.is_Salvage=0 AND tn.caption_id = cap.caption_id and cap.language_id = 1
GO


