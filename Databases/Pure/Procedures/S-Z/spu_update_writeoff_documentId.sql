SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO 
EXEC DDLDropProcedure 'spu_update_writeoff_documentId'
GO 

CREATE PROCEDURE  spu_update_writeoff_documentId
@nTransdetail_id INT,
@nDocument_id INT

AS

UPDATE td1 SET document_id=@nDocument_id FROM transdetail td 
			INNER JOIN transdetail td1 ON td.document_id=td1.document_id 
			WHERE td.transdetail_id=@nTransdetail_id AND td1.transdetail_id>=@nTransdetail_id AND td1.spare='WRITEOFF'

UPDATE allocationdetail SET document_ref=d.document_ref FROM allocationdetail ad 
			INNER JOIN transdetail td ON td.transdetail_id=ad.transdetail_id
			INNER JOIN document d ON d.document_id=td.document_id
			WHERE td.transdetail_id=@nTransdetail_id

UPDATE transdetail SET document_sequence=newseq
FROM (SELECT document_sequence,ROW_NUMBER() OVER (ORDER BY transdetail_id) newseq FROM transdetail WHERE document_id=@nDocument_id) temp 
				INNER JOIN transdetail td ON td.document_sequence=temp.document_sequence
				WHERE document_id=@nDocument_id





